using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードスロットの状態を管理し、配置などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureCardSlotManager", menuName = "CardsAndDices/Combats/Managers/CreatureCards/CreatureCardSlotManager")]
    public class CreatureCardSlotManager : ScriptableObject, IDisposable, ICreatureCardSlotPosition, ICreatureCardSlotInstanceRepository, ICreatureCardlocation, IIdentifiableManager
    {
        [Header("Components")]
        [SerializeField] private List<CreatureCardSlotPositionEntity> _creatureCardSlotPositionEntitys;
        [SerializeField] private CompositeObjectIdTypeEntity _objectType;
        [SerializeField] private CompositeObjectIdTypeEntity _acceptableTargetObjectType;
        private readonly List<CreatureCardSlotInstance> _creatureCardSlotInstances = new();
        private readonly List<CreatureCardSlotController> _creatureCardSlotControllers = new();
        private readonly List<CreatureCardSlotPresenter> _creatureCardSlotPresenters = new();
        private GameEventBus _eventBus;
        private CompositeObjectIdManager _compositeObjectIdManager;
        private IdentifiableViewRegistry _viewRegistry;
        private ReflowService _reflowService;

        [Inject]
        public void Initialize(GameEventBus eventBus, CompositeObjectIdManager compositeObjectIdManager, IdentifiableViewRegistry viewRegistry)
        {
            DisposeInstances();
            _eventBus = eventBus;
            _compositeObjectIdManager = compositeObjectIdManager;
            _viewRegistry = viewRegistry;
            _eventBus.On<InstanceSetUpedEvent>(OnInstanceSetUped);
            _eventBus.On<IdentifiableStateDragedHoverEvent>(OnIdentifiableStateDragedHover);
            _eventBus.On<SceneScreenSetUpEvent>(OnSceneScreenSetUp);
            _eventBus.On<PlacedHandSlotEvent>(OnPlacedHandSlot);
            _eventBus.On<CombatPhaseCardFrontLoadMovementEvent>(OnCombatPhaseCardFrontLoadMovement);
            _eventBus.On<IdentifiableStateDropEvent>(OnIdentifiableStateDrop);
            _eventBus.On<PlacedPpecifiedSlotEvent>(OnPlacedPpecifiedSlot);
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);

            _reflowService = new ReflowService(this);

        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            DisposePresenters();
            _eventBus.Off<InstanceSetUpedEvent>(OnInstanceSetUped);
            _eventBus.Off<IdentifiableStateDragedHoverEvent>(OnIdentifiableStateDragedHover);
            _eventBus.Off<SceneScreenSetUpEvent>(OnSceneScreenSetUp);
            _eventBus.Off<PlacedHandSlotEvent>(OnPlacedHandSlot);
            _eventBus.Off<CombatPhaseCardFrontLoadMovementEvent>(OnCombatPhaseCardFrontLoadMovement);
            _eventBus.Off<IdentifiableStateDropEvent>(OnIdentifiableStateDrop);
            _eventBus.Off<PlacedPpecifiedSlotEvent>(OnPlacedPpecifiedSlot);
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
        }

        /// <summary>
        /// クリーチャーカードのリフローを行うイベント
        /// </summary>
        private void OnIdentifiableStateDragedHover(IdentifiableStateDragedHoverEvent evt)
        {
            // 受け入れ対象外がドラッグされている場合は無視する
            if (evt.DragedObjectId.ObjectType != _acceptableTargetObjectType) return;

            // リフロー
            CreatureCardReflow(evt.DragedObjectId, evt.ExecutedObjectId);
        }

        /// <summary>
        /// クリーチャーカードのリフローを行う
        /// </summary>
        private void CreatureCardReflow(CompositeObjectId moveCardId, CompositeObjectId toSlotId)
        {
            // 移動するカードの現在の配置場所
            var draggedSlot = GetInstanceInReflowPlaced(moveCardId);

            // 移動先
            var targetSlot = GetInstance(toSlotId);

            // 現在の配置場所と移動先が一致した場合はリフローを行わない
            if (draggedSlot != targetSlot)
            {
                // リフロー
                _reflowService.CalculateReflowMovements(draggedSlot, targetSlot, moveCardId);
            }

            // ドラッグしてたカードが所定位置に移動する必要があるので、移動イベントを通知
            _eventBus.Emit(new MoveToAnimationReflowCreatureCardSlotEvent(moveCardId));
        }

        /// <summary>
        /// ドロップされたカードを配置
        /// </summary>
        private async void OnIdentifiableStateDrop(IdentifiableStateDropEvent evt)
        {
            // ドラッグ対象が受け入れ対象
            if (evt.ExecutedObjectId.ObjectType != _acceptableTargetObjectType) return;

            // リフロー
            CreatureCardReflow(evt.ExecutedObjectId, evt.TargetObjectId);

            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

            // 前詰め処理
            _reflowService.CalculateFrontLoadMovements();
            _eventBus.Emit(new MoveToAnimationReflowCreatureCardSlotEvent(null));

            // reset
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            _eventBus.Emit(new ResetUIStatusEvent());
        }

        /// <summary>
        /// クリーチャーカードの前詰め処理を行います
        /// </summary>
        private void OnCombatPhaseCardFrontLoadMovement(CombatPhaseCardFrontLoadMovementEvent evt)
        {
            _reflowService.CalculateFrontLoadMovements();
        }

        /// <summary>
        /// クリーチャーカードをハンドスロットに配置します
        /// </summary>
        private void OnPlacedHandSlot(PlacedHandSlotEvent evt)
        {
            var instance = GetFirstEmptyHandSlotId();
            if (instance is null)
            {
                Debug.LogWarning("インスタンスがとれない");
                return;
            }
            instance.PlacedCard(evt.CreatureCardId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(evt.CreatureCardId, instance.CreatureCardSlotPosition));
        }

        /// <summary>
        /// クリーチャーカードを指定のスロットに配置します
        /// </summary>
        private void OnPlacedPpecifiedSlot(PlacedPpecifiedSlotEvent evt)
        {
            var instance = GetInstance(evt.Team, evt.LinePosition, evt.SlotLocation);
            if (instance is null)
            {
                Debug.LogWarning("インスタンスがとれない");
                return;
            }
            instance.PlacedCard(evt.CreatureCardId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(evt.CreatureCardId, instance.CreatureCardSlotPosition));
        }

        /// <summary>
        /// クリーチャーカードスロットを所定の場所に配置します
        /// </summary>
        private void OnSceneScreenSetUp(SceneScreenSetUpEvent evt)
        {
            // クリーチャーカードスロットビューを所定の場所に移動
            foreach (var creatureCardSlotPresenter in _creatureCardSlotPresenters)
            {
                // Statusを変更しViewに反映
                _eventBus.Emit(new SetCurrentHomeStatusEvent(creatureCardSlotPresenter.CompositeObjectId, IdentifiableStatus.Inactive));
                _eventBus.Emit(new ChangeViewStatusEvent(creatureCardSlotPresenter.CompositeObjectId, IdentifiableStatus.Inactive));
                _eventBus.Emit(new DisplayStatusViewEvent(creatureCardSlotPresenter.CompositeObjectId));

                // HomePositionを設定し、そこに移動
                _eventBus.Emit(new ChangeHomePositionStatusViewEvent(creatureCardSlotPresenter.CompositeObjectId, creatureCardSlotPresenter.HomePosition));
                _eventBus.Emit(new ReturnHomePositionEvent(creatureCardSlotPresenter.CompositeObjectId));
            }
        }

        /// <summary>
        /// 指定したIdのインスタンスを返します
        /// </summary>
        public CreatureCardSlotInstance GetInstanceInReflowPlaced(CompositeObjectId id)
        {
            var emptyHandSlot = _creatureCardSlotInstances
                .Where(slot => slot.ReflowPlacedCardId == id)
                .FirstOrDefault();

            return emptyHandSlot;
        }

        /// <summary>
        /// 指定したIdのインスタンスを返します
        /// </summary>
        public CreatureCardSlotInstance GetInstance(CompositeObjectId id)
        {
            var emptyHandSlot = _creatureCardSlotInstances
                .Where(slot => slot.CompositeObjectId == id)
                .FirstOrDefault();

            return emptyHandSlot;
        }

        /// <summary>
        /// クリーチャーカードスロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnInstanceSetUped(InstanceSetUpedEvent evt)
        {
            foreach (var creatureCardSlotPositionEntity in _creatureCardSlotPositionEntitys)
            {
                if (creatureCardSlotPositionEntity.Team == Team.Enemy)
                {
                    CreateEnemySlot(creatureCardSlotPositionEntity);
                }
                else
                {
                    CreatePlayerSlot(creatureCardSlotPositionEntity);
                }
            }
        }

        /// <summary>
        /// エネミーカード用のカードスロットを生成します
        /// </summary>
        private void CreateEnemySlot(CreatureCardSlotPositionEntity creatureCardSlotPositionEntity)
        {
            var instance = new CreatureCardSlotInstance(_compositeObjectIdManager.CreateId(_objectType, null), creatureCardSlotPositionEntity);
            _creatureCardSlotInstances.Add(instance);
            var controller = new CreatureCardSlotController(instance, _eventBus, _objectType);
            _creatureCardSlotControllers.Add(controller);
        }

        /// <summary>
        /// プレイヤーカード用のカードスロットを生成します
        /// </summary>
        private void CreatePlayerSlot(CreatureCardSlotPositionEntity creatureCardSlotPositionEntity)
        {
            var view = _viewRegistry.GetNonBoundView<CreatureCardSlotView>();
            if (view is null)
            {
                Debug.LogWarning("viewが取得できない");
                return;
            }
            var instance = new CreatureCardSlotInstance(view.CompositeObjectId, creatureCardSlotPositionEntity);
            _creatureCardSlotInstances.Add(instance);
            var controller = new CreatureCardSlotController(instance, _eventBus, _acceptableTargetObjectType);
            var Presenter = new CreatureCardSlotPresenter(instance, view, _eventBus, _acceptableTargetObjectType);
            _creatureCardSlotControllers.Add(controller);
            _creatureCardSlotPresenters.Add(Presenter);
            view.SetBoundState(true);
            _eventBus.Emit(new SetCurrentHomeStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new ChangeViewStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new DisplayStatusViewEvent(instance.CompositeObjectId));
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _creatureCardSlotInstances)
            {
                instance.Dispose();
            }
            _creatureCardSlotInstances.Clear();
        }
        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var controller in _creatureCardSlotControllers)
            {
                controller.Dispose();
            }
            _creatureCardSlotControllers.Clear();
        }

        /// <summary>
        /// プレゼンターをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var Presenter in _creatureCardSlotPresenters)
            {
                Presenter.Dispose();
            }
            _creatureCardSlotPresenters.Clear();
        }

        /// <summary>
        /// クリーチャーカードをスロットに配置します
        /// </summary>
        public void PlacedCreatureCard(CompositeObjectId cardId, CompositeObjectId targetSlot)
        {
            // 元からカードを除去
            var originalInstance = GetInstanceInReflowPlacedCardId(cardId);
            originalInstance.ReflowPlacedCard(null);

            //
            var targetInstance = GetInstance(targetSlot);
            targetInstance.ReflowPlacedCard(cardId);
        }

        /// <summary>
        /// 配置されたカードのHomePositionを返します
        /// </summary>
        public Vector3 GetCreatureCardHomePosition(CompositeObjectId cardId)
        {
            var placedSlots = _creatureCardSlotInstances.Where(s => s.ReflowPlacedCardId == cardId).ToList();
            if (placedSlots.Count == 0)
            {
                return Vector3.zero;
            }
            return placedSlots[0].CreatureCardSlotPosition;
        }

        /// <summary>
        /// 手札の空いているハンドスロットのうち、最も若い番号のスロットIDを取得します。
        /// </summary>
        /// <returns>空き手札スロットのCompositeObjectId。見つからない場合はnull。</returns>
        public CreatureCardSlotInstance GetFirstEmptyHandSlotId()
        {
            var emptyHandSlot = _creatureCardSlotInstances
                .Where(slot => slot.LinePosition == LinePosition.Hand && !slot.IsOccupied)
                .OrderBy(slot => slot.Location)
                .FirstOrDefault();

            return emptyHandSlot;
        }

        /// <summary>
        /// インスタンスのリストを返します。
        /// </summary>
        public List<CreatureCardSlotInstance> GetInstanceList()
        {
            return _creatureCardSlotInstances;
        }

        /// <summary>
        /// ハンドスロット以外のカードリストを返します
        /// </summary>
        public List<CreatureCardSlotInstance> GetNonHandInstanceList()
        {
            var list = _creatureCardSlotInstances
                .Where(slot => slot.LinePosition != LinePosition.Hand)
                .ToList();
            return list;
        }

        /// <summary>
        /// 指定したカードがリフローに配置されたインスタンスを返します
        /// </summary>
        public CreatureCardSlotInstance GetInstanceInReflowPlacedCardId(CompositeObjectId cardId)
        {
            var emptyHandSlot = _creatureCardSlotInstances
                .Where(slot => slot.ReflowPlacedCardId == cardId)
                .FirstOrDefault();

            return emptyHandSlot;
        }

        /// <summary>
        /// 指定した配置場所のインスタンスを返します
        /// </summary>
        public CreatureCardSlotInstance GetInstance(Team team, LinePosition linePosition, SlotLocation slotLocation)
        {
            var emptyHandSlot = _creatureCardSlotInstances
                .Where(slot => slot.Team == team && slot.LinePosition == linePosition && slot.Location == slotLocation)
                .FirstOrDefault();

            return emptyHandSlot;
        }

        /// <summary>
        /// 配置されたカードのteamを取得します
        /// </summary>
        public Team GetTeam(CompositeObjectId cardId)
        {
            var placedSlots = _creatureCardSlotInstances.Where(s => s.ReflowPlacedCardId == cardId).ToList();
            if (placedSlots.Count == 0)
            {
                Debug.LogWarning("スロットとれない");
                return Team.Player;
            }
            return placedSlots[0].Team;
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeするイベント
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            DisposeByCompositeObjectId(evt.CompositeObjectId);
        }

        /// <summary>
        /// 配置されたカードのlocationを返します
        /// </summary>
        public SlotLocation GetSlotLocation(CompositeObjectId cardId)
        {
            var placedSlots = _creatureCardSlotInstances.Where(s => s.ReflowPlacedCardId == cardId).ToList();
            if (placedSlots.Count == 0)
            {
                Debug.LogWarning("スロットとれない");
                return SlotLocation.Vanguard;
            }
            return placedSlots[0].Location;
        }

        /// <summary>
        /// 配置されたカードのlocationを返します
        /// </summary>
        public LinePosition GetLinePosition(CompositeObjectId cardId)
        {
            var placedSlots = _creatureCardSlotInstances.Where(s => s.ReflowPlacedCardId == cardId).ToList();
            if (placedSlots.Count == 0)
            {
                Debug.LogWarning("スロットとれない");
                return LinePosition.TopLine;
            }
            return placedSlots[0].LinePosition;
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeします
        /// </summary>
        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
            var instance = _creatureCardSlotInstances.Where(i => i.CompositeObjectId == compositeObjectId).FirstOrDefault();
            if (instance is null)
            {
                return;
            }
            instance.Dispose();
            _creatureCardSlotInstances.Remove(instance);
            var presenter = _creatureCardSlotPresenters.Where(p => p.CompositeObjectId == compositeObjectId).FirstOrDefault();
            presenter.Dispose();
            _creatureCardSlotPresenters.Remove(presenter);
            var controller = _creatureCardSlotControllers.Where(c => c.InstanceId == compositeObjectId).FirstOrDefault();
            controller.Dispose();
            _creatureCardSlotControllers.Remove(controller);
        }
    }
}

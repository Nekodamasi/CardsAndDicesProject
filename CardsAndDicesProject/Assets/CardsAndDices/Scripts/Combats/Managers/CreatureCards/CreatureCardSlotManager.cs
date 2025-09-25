using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードスロットの状態を管理し、配置などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureCardSlotManager", menuName = "CardsAndDices/Combats/Managers/CreatureCards/CreatureCardSlotManager")]
    public class CreatureCardSlotManager : ScriptableObject, IDisposable, ICreatureCardSlotPosition, ICreatureCardSlotInstanceRepository
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
            _eventBus.On<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.On<CombatPhaseReflowCardEvent>(OnCombatPhaseReflowCard);
            _eventBus.On<SceneScreenSetUpEvent>(OnSceneScreenSetUp);
            _eventBus.On<PlacedHandSlotEvent>(OnPlacedHandSlot);
            _eventBus.On<CombatPhaseCardFrontLoadMovementEvent>(OnCombatPhaseCardFrontLoadMovement);
            
            _reflowService = new ReflowService(this);

        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            DisposePresenters();
            _eventBus.Off<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.Off<CombatPhaseReflowCardEvent>(OnCombatPhaseReflowCard);
            _eventBus.Off<SceneScreenSetUpEvent>(OnSceneScreenSetUp);
            _eventBus.Off<PlacedHandSlotEvent>(OnPlacedHandSlot);
            _eventBus.Off<CombatPhaseCardFrontLoadMovementEvent>(OnCombatPhaseCardFrontLoadMovement);
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
        /// クリーチャーカードスロットを所定の場所に配置します
        /// </summary>
        private void OnSceneScreenSetUp(SceneScreenSetUpEvent evt)
        {
            // クリーチャーカードスロットビューを所定の場所に移動
            foreach (var creatureCardSlotPresenter in _creatureCardSlotPresenters)
            {
                // HomePositionを設定し、そこに移動
                _eventBus.Emit(new ChangeHomePositionStatusViewEvent(creatureCardSlotPresenter.CompositeObjectId, creatureCardSlotPresenter.HomePosition));
                _eventBus.Emit(new ReturnHomePositionEvent(creatureCardSlotPresenter.CompositeObjectId));
            }
        }

        /// <summary>
        /// クリーチャーカードのリフローを行う
        /// </summary>
        private void OnCombatPhaseReflowCard(CombatPhaseReflowCardEvent evt)
        {
            var draggedSlot = GetInstanceInReflowPlaced(evt.DraggedCardId);
            var targetSlot = GetInstance(evt.TargetSlotId);
            _reflowService.CalculateReflowMovements(targetSlot, targetSlot, evt.DraggedCardId);
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
        private void OnSceneLoaded(SceneLoadedEvent evt)
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
            var controller = new CreatureCardSlotController(instance, _eventBus);
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
            view.SetBoundState(true);
            var instance = new CreatureCardSlotInstance(view.CompositeObjectId, creatureCardSlotPositionEntity);
            _creatureCardSlotInstances.Add(instance);
            var controller = new CreatureCardSlotController(instance, _eventBus);
            var Presenter = new CreatureCardSlotPresenter(instance, view, _eventBus, _acceptableTargetObjectType);
            _creatureCardSlotControllers.Add(controller);
            _creatureCardSlotPresenters.Add(Presenter);
            view.SetBoundState(true);
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
        public void PlacedCreatureCard(CompositeObjectId id)
        {
            /*
                        var sortedSlots = _diceSlotInstances.OrderBy(s => s.DiceSlotLocation).ToList();
                        var occupiedSlots = sortedSlots.Where(s => s.IsOccupied == false).ToList();
                        // Debug.Log("ダイススロット配置ー＞" + sortedSlots.Count + "/" + occupiedSlots.Count);
                        _eventBus.Emit(new PlacedDiceEvent(occupiedSlots[0].CompositeObjectId, DiceId));
                        //            _eventBus.Emit(new ReturnHomePositionEvent(DiceId));
            */
        }

        /// <summary>
        /// 配置されたダイスのHomePositionを返します
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
        /// 手札の空いているスロットのうち、最も若い番号のスロットIDを取得します。
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
        /// 指定した配置場所のインスタンスを返します
        /// </summary>
        public CreatureCardSlotInstance GetInstance(Team team, LinePosition linePosition, SlotLocation slotLocation)
        {
            var emptyHandSlot = _creatureCardSlotInstances
                .Where(slot => slot.Team == team && slot.LinePosition == linePosition && slot.Location == slotLocation)
                .FirstOrDefault();

            return emptyHandSlot;
        }
    }
}

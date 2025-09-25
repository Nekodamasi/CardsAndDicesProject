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
    public class CreatureCardSlotManager : ScriptableObject, IDisposable, ICreatureCardSlotPosition
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

        [Inject]
        public void Initialize(GameEventBus eventBus, CompositeObjectIdManager compositeObjectIdManager, IdentifiableViewRegistry viewRegistry)
        {
            DisposeInstances();
            _eventBus = eventBus;
            _compositeObjectIdManager = compositeObjectIdManager;
            _viewRegistry = viewRegistry;
            _eventBus.On<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.On<CombatPhaseReflowCardEvent>(OnCombatPhaseReflowCard);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            DisposePresenters();
            _eventBus.Off<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.Off<CombatPhaseReflowCardEvent>(OnCombatPhaseReflowCard);
        }

        /// <summary>
        /// クリーチャーカードスロットを前詰めでリフローします。
        /// </summary>
        public void ReflowDCreatureCards()
        {
/*
            var sortedSlots = _diceSlotInstances.OrderBy(s => s.DiceSlotLocation).ToList();
            var occupiedSlots = sortedSlots.Where(s => s.IsOccupied).ToList();
            var diceIds = occupiedSlots.Select(s => s.PlacedDiceId).ToList();

            // すべてのサイコロを削除するコマンドを発行する
            foreach (var slot in occupiedSlots)
            {
                _eventBus.Emit(new RemoveDiceEvent(slot.DiceSlotLocation, slot.PlacedDiceId));
            }

            // サイコロを前積み順に並べるコマンドを発行する
            for (int i = 0; i < diceIds.Count; i++)
            {
                _eventBus.Emit(new PlacedDiceEvent(sortedSlots[i].CompositeObjectId, diceIds[i]));
                _eventBus.Emit(new MoveToAnimationReflowDiceEvent(sortedSlots[i].CompositeObjectId));   
            }
*/
        }

        /// <summary>
        /// クリーチャーカードのリフローを行う
        /// </summary>
        private void OnCombatPhaseReflowCard(CombatPhaseReflowCardEvent evt)
        {            
            ReflowDCreatureCards();
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

            // クリーチャーカードスロットビューを所定の場所に移動
            foreach (var creatureCardSlotPresenter in _creatureCardSlotPresenters)
            {
                Debug.Log("ほげほげほげほげ");
                // HomePositionを設定し、そこに移動
                _eventBus.Emit(new ChangeHomePositionStatusViewEvent(creatureCardSlotPresenter.CompositeObjectId, creatureCardSlotPresenter.HomePosition));
                _eventBus.Emit(new ReturnHomePositionEvent(creatureCardSlotPresenter.CompositeObjectId));
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
        public void PlacedCreatureCard(CompositeObjectId DiceId)
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
/*
            var placedSlots = _diceSlotInstances.Where(s => s.ReflowPlacedDiceId == DiceId).ToList();
            if (placedSlots.Count == 0)
            {
                return Vector3.zero;
            }
            return placedSlots[0].DiceSlotPosition;
*/
            return Vector3.zero;
        }
    }
}

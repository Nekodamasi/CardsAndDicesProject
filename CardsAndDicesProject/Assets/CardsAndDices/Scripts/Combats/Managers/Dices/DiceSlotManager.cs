using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイススロットの状態を管理し、ダイスの配置などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceSlotManager", menuName = "CardsAndDices/Combats/Managers/Dices/DiceSlotManager")]
    public class DiceSlotManager : ScriptableObject, IDisposable, IDiceSlotPosition
    {
        [Header("Components")]
        [SerializeField] private List<DiceSlotPositionEntity> _diceSlotPositionEntities;
        [SerializeField] private CompositeObjectIdTypeEntity _objectType;
        private readonly List<DiceSlotInstance> _diceSlotInstances = new();
        private readonly List<DiceSlotController> _iceSlotControllers = new();
        private GameEventBus _eventBus;
        private CompositeObjectIdManager _compositeObjectIdManager;

        [Inject]
        public void Initialize(GameEventBus identifiableCommandBus, CompositeObjectIdManager compositeObjectIdManager)
        {
            Dispose();
            _eventBus = identifiableCommandBus;
            _compositeObjectIdManager = compositeObjectIdManager;
            _eventBus.On<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.On<CombatPhaseReflowDiceEvent>(OnReflowDiceSlots);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            _eventBus.Off<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.Off<CombatPhaseReflowDiceEvent>(OnReflowDiceSlots);
        }

        /// <summary>
        /// ダイススロットを前詰めでリフローします。
        /// </summary>
        public void ReflowDiceSlots()
        {
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
        }

        /// <summary>
        /// ダイスのリフローを行う
        /// </summary>
        private void OnReflowDiceSlots(CombatPhaseReflowDiceEvent evt)
        {            
            ReflowDiceSlots();
        }

        /// <summary>
        /// ダイススロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedEvent evt)
        {
            foreach (var diceSlotPositionEntity in _diceSlotPositionEntities)
            {
                var instance = new DiceSlotInstance(_compositeObjectIdManager.CreateId(_objectType, null), diceSlotPositionEntity);
                _diceSlotInstances.Add(instance);
                _iceSlotControllers.Add(new DiceSlotController(instance, _eventBus));
            }
        }
        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _diceSlotInstances)
            {
                instance.Dispose();
            }
            _diceSlotInstances.Clear();
        }
        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var controller in _iceSlotControllers)
            {
                controller.Dispose();
            }
            _iceSlotControllers.Clear();
        }
        /// <summary>
        /// ダイスをスロットに配置します
        /// </summary>
        public void PlacedDice(CompositeObjectId DiceId)
        {
            var sortedSlots = _diceSlotInstances.OrderBy(s => s.DiceSlotLocation).ToList();
            var occupiedSlots = sortedSlots.Where(s => s.IsOccupied == false).ToList();
            // Debug.Log("ダイススロット配置ー＞" + sortedSlots.Count + "/" + occupiedSlots.Count);
            _eventBus.Emit(new PlacedDiceEvent(occupiedSlots[0].CompositeObjectId, DiceId));
//            _eventBus.Emit(new ReturnHomePositionEvent(DiceId));
        }

        /// <summary>
        /// 配置されたダイスのHomePositionを返します
        /// </summary>
        public Vector3 GetDiceHomePosition(CompositeObjectId DiceId)
        {
            var placedSlots = _diceSlotInstances.Where(s => s.ReflowPlacedDiceId == DiceId).ToList();
            if (placedSlots.Count == 0)
            {
                return Vector3.zero;
            }
            return placedSlots[0].DiceSlotPosition;
        }
    }
}

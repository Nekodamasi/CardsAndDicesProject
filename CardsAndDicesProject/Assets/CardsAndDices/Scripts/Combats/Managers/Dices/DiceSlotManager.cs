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
    public class DiceSlotManager : ScriptableObject, IDisposable
    {
        [Header("Components")]
        [SerializeField] private List<DiceSlotPositionEntity> _diceSlotPositionEntities;
        [SerializeField] private CompositeObjectIdTypeEntity _objectType;
        private readonly List<DiceSlotInstance> _diceSlotInstances = new();
        private readonly List<DiceSlotController> _iceSlotControllers = new();
        private GameEventBus _identifiableCommandBus;
        private CompositeObjectIdManager _compositeObjectIdManager;

        [Inject]
        public void Initialize(GameEventBus identifiableCommandBus, CompositeObjectIdManager compositeObjectIdManager)
        {
            _identifiableCommandBus = identifiableCommandBus;
            _compositeObjectIdManager = compositeObjectIdManager;
            _identifiableCommandBus.On<SceneLoadedCommand>(OnSceneLoaded);
            _identifiableCommandBus.On<ReflowDiceSlotsCommand>(OnReflowDiceSlots);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            _identifiableCommandBus.Off<SceneLoadedCommand>(OnSceneLoaded);
            _identifiableCommandBus.Off<ReflowDiceSlotsCommand>(OnReflowDiceSlots);
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
                _identifiableCommandBus.Emit(new RemoveDiceCommand(slot.DiceSlotLocation, slot.PlacedDiceId));
            }

            // サイコロを前積み順に並べるコマンドを発行する
            for (int i = 0; i < diceIds.Count; i++)
            {
                _identifiableCommandBus.Emit(new PlacedDiceCommand(sortedSlots[i].DiceSlotLocation, diceIds[i]));
                _identifiableCommandBus.Emit(new MoveToAnimationReflowDiceCommand(sortedSlots[i].DiceSlotLocation));   
            }
        }

        /// <summary>
        /// ダイスのリフローを行う
        /// </summary>
        private void OnReflowDiceSlots(ReflowDiceSlotsCommand cmd)
        {            
            ReflowDiceSlots();
        }

        /// <summary>
        /// ダイススロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedCommand cmd)
        {
            foreach (var diceSlotPositionEntity in _diceSlotPositionEntities)
            {
                var instance = new DiceSlotInstance(_compositeObjectIdManager.CreateId(_objectType, null), diceSlotPositionEntity, _identifiableCommandBus);
                _diceSlotInstances.Add(instance);
                _iceSlotControllers.Add(new DiceSlotController(instance, _identifiableCommandBus));
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
    }
}

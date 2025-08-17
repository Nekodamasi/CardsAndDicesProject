using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// UIからのスロット関連のインタラクションを処理するクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceSlotInteractionHandler", menuName = "CardsAndDices/Managers/DiceSlotInteractionHandler")]
    public class DiceSlotInteractionHandler : ScriptableObject
    {
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private DiceSlotStateRepository _repository;
        [SerializeField] private ReflowService _reflowService;
        [SerializeField] private DicePlacementService _placementService;

        private bool _isDiceFull = false;

        [Inject]
        public void Initialize(SpriteCommandBus commandBus, DiceSlotStateRepository repository, ReflowService reflowService, DicePlacementService placementService)
        {
            _commandBus = commandBus;
            _repository = repository;
            _reflowService = reflowService;
            _placementService = placementService;
        }

        public void OnDiceDroppedOnSlot(CompositeObjectId diceId, CompositeObjectId slotId)
        {
            Debug.Log("OnDiceDroppedOnSlot->" + diceId + "-->" + slotId);
            // このメソッドが呼ばれる場合、カードは全てスロットに配置ずみであり、新たな配置コマンドは必要ない
            //_placementService.PlaceCard(cardId, slotId);

            //リフローの状態を確定する
            ReflowConfirm();

            //確定配置でリフローを行う
            ReflowDicesCurrentValue();
//            CheckAndNotifyPlayerZoneState();
        }

        public void OnDiceHoveredOnSlot(CompositeObjectId diceId, CompositeObjectId slotId)
        {
            ReflowDicesIfNeeded(diceId, slotId);
        }

        public void OnDropFailed()
        {
            Debug.Log("OnDropFailed");
            foreach (var slotData in _repository.GetAllSlots())
            {
                slotData.ReflowPlacedDiceId = slotData.PlacedDiceId;
            }
            ReflowDicesCurrentValue();
        }

        private void ReflowConfirm()
        {
            foreach (var slotData in _repository.GetAllSlots())
            {
                slotData.PlacedDiceId = slotData.ReflowPlacedDiceId;
            }
        }

        public void SystemReflowDicesCurrentValue()
        {
            Debug.Log("ほげほげほげほげ");
            Dictionary<CompositeObjectId, Vector3> diceMovements = new Dictionary<CompositeObjectId, Vector3>();
            foreach (var slotData in _repository.GetAllSlots())
            {
                if (slotData.IsOccupied)
                {
                    diceMovements.Add(slotData.PlacedDiceId, slotData.Position);
                }
            }
            _commandBus.Emit(new SystemDiceReflowCommand(diceMovements, null));
        }
        private void ReflowDicesCurrentValue()
        {
            Dictionary<CompositeObjectId, Vector3> diceMovements = new Dictionary<CompositeObjectId, Vector3>();
            foreach (var slotData in _repository.GetAllSlots())
            {
                if (slotData.IsOccupied)
                {
                    diceMovements.Add(slotData.PlacedDiceId, slotData.Position);
                }
            }
            _commandBus.Emit(new DragReflowCompletedCommand(diceMovements));
        }

        private void ReflowDicesIfNeeded(CompositeObjectId droppedDiceId, CompositeObjectId droppedSlotId)
        {
            DiceSlotData draggedSlotData = _repository.GetSlotDataByReflowPlacedDiceId(droppedDiceId);
            DiceSlotData targetSlotData = _repository.GetSlotData(droppedSlotId);
            
//            Dictionary<CompositeObjectId, Vector3> diceMovements = _reflowService.CalculateReflowMovements(draggedSlotData, targetSlotData, droppedDiceId);
//            _commandBus.Emit(new ReflowCompletedCommand(diceMovements));
        }

        private void CheckAndNotifyDiceState()
        {
            bool isFull = _repository.IsDiceFull();
            if (_isDiceFull != isFull)
            {
                _isDiceFull = isFull;
                _commandBus.Emit(new PlayerZoneStateChangedCommand(isFull));
            }
        }
    }
}

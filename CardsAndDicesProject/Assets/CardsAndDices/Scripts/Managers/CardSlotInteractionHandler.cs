using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// UIからのスロット関連のインタラクションを処理するクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CardSlotInteractionHandler", menuName = "CardsAndDices/Managers/CardSlotInteractionHandler")]
    public class CardSlotInteractionHandler : ScriptableObject
    {
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private CardSlotStateRepository _repository;
        [SerializeField] private ReflowService _reflowService;
        [SerializeField] private CardPlacementService _placementService;

        private bool _isPlayerZoneFull = false;

        [Inject]
        public void Initialize(SpriteCommandBus commandBus, CardSlotStateRepository repository, ReflowService reflowService, CardPlacementService placementService)
        {
            _commandBus = commandBus;
            _repository = repository;
            _reflowService = reflowService;
            _placementService = placementService;
        }

        public void OnCardDroppedOnSlot(CompositeObjectId cardId, CompositeObjectId slotId)
        {
            Debug.Log("OnCardDroppedOnSlot->" + cardId + "-->" + slotId);
            // このメソッドが呼ばれる場合、カードは全てスロットに配置ずみであり、新たな配置コマンドは必要ない
            //_placementService.PlaceCard(cardId, slotId);

            //リフローの状態を確定する
            ReflowConfirm();

            //確定配置でリフローを行う
            ReflowCardsCurrentValue();
//            CheckAndNotifyPlayerZoneState();
        }

        public void OnCardHoveredOnSlot(CompositeObjectId cardId, CompositeObjectId slotId)
        {
            ReflowCardsIfNeeded(cardId, slotId);
        }

        public void OnDropFailed()
        {
            Debug.Log("OnDropFailed");
            foreach (var slotData in _repository.GetAllSlots())
            {
                slotData.ReflowPlacedCardId = slotData.PlacedCardId;
            }
            ReflowCardsCurrentValue();
        }

        private void ReflowConfirm()
        {
            foreach (var slotData in _repository.GetAllSlots())
            {
                slotData.PlacedCardId = slotData.ReflowPlacedCardId;
            }
        }

        public void SystemReflowCardsCurrentValue()
        {
            Dictionary<CompositeObjectId, Vector3> cardMovements = new Dictionary<CompositeObjectId, Vector3>();
            foreach (var slotData in _repository.GetAllSlots())
            {
                if (slotData.IsOccupied)
                {
                    cardMovements.Add(slotData.PlacedCardId, slotData.Position);
                }
            }
            _commandBus.Emit(new SystemReflowCommand(cardMovements, null));
        }
        private void ReflowCardsCurrentValue()
        {
            Dictionary<CompositeObjectId, Vector3> cardMovements = new Dictionary<CompositeObjectId, Vector3>();
            foreach (var slotData in _repository.GetAllSlots())
            {
                if (slotData.IsOccupied)
                {
                    cardMovements.Add(slotData.PlacedCardId, slotData.Position);
                }
            }
            _commandBus.Emit(new DragReflowCompletedCommand(cardMovements));
        }

        private void ReflowCardsIfNeeded(CompositeObjectId droppedCardId, CompositeObjectId droppedSlotId)
        {
            CardSlotData draggedSlotData = _repository.GetSlotDataByReflowPlacedCardId(droppedCardId);
            CardSlotData targetSlotData = _repository.GetSlotData(droppedSlotId);
            
            Dictionary<CompositeObjectId, Vector3> cardMovements = _reflowService.CalculateReflowMovements(draggedSlotData, targetSlotData, droppedCardId);
            _commandBus.Emit(new ReflowCompletedCommand(cardMovements));
        }

        private void CheckAndNotifyPlayerZoneState()
        {
            bool isFull = _repository.IsPlayerZoneFull();
            if (_isPlayerZoneFull != isFull)
            {
                _isPlayerZoneFull = isFull;
                _commandBus.Emit(new PlayerZoneStateChangedCommand(isFull));
            }
        }
    }
}

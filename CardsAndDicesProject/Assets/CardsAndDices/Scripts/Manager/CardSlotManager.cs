using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのカードスロット関連の処理の窓口となるファサードクラス。
    /// 内部で各責務を持つクラスを呼び出します。
    /// </summary>
    [CreateAssetMenu(fileName = "CardSlotManager", menuName = "CardsAndDices/Systems/CardSlotManager")]
    public class CardSlotManager : ScriptableObject
    {
        [Header("System Components")]
        [SerializeField] private CardSlotStateRepository _repository;
        [SerializeField] private CardPlacementService _placementService;
        [SerializeField] private CardSlotInteractionHandler _interactionHandler;
        [SerializeField] private CardSlotDebug _debug;

        [Inject]
        public void Initialize(CardSlotStateRepository repository, CardPlacementService placementService, CardSlotInteractionHandler interactionHandler, CardSlotDebug debug)
        {
            _repository = repository;
            _placementService = placementService;
            _interactionHandler = interactionHandler;
            _debug = debug;
        }

        // --- Public API ---

        public void RegisterSlot(CardSlotData slotData) => _repository.RegisterSlot(slotData);

        public void OnCardDroppedOnSlot(CompositeObjectId cardId, CompositeObjectId slotId) => _interactionHandler.OnCardDroppedOnSlot(cardId, slotId);

        public void OnCardHoveredOnSlot(CompositeObjectId cardId, CompositeObjectId slotId) => _interactionHandler.OnCardHoveredOnSlot(cardId, slotId);

        public void OnDropFailed() => _interactionHandler.OnDropFailed();

        public void PlaceCardAsSystem(CompositeObjectId cardId, CompositeObjectId slotId, bool triggerReflow = true)
        {
            Debug.Log("<color=Yellow>PlaceCardAsSystem</color>");
            _placementService.UnplaceCard(cardId);
            _placementService.PlaceCard(cardId, slotId);
            if (triggerReflow)
            {
                _interactionHandler.SystemReflowCardsCurrentValue();
            }
        }

        public CardSlotData GetSlotData(CompositeObjectId slotId) => _repository.GetSlotData(slotId);

        public CardSlotData GetNextEmptyHandSlot() => _repository.GetNextEmptyHandSlot();

        public CardSlotData GetSlotDataByReflowPlacedCardId(CompositeObjectId reflowPlacedCardId) => _repository.GetSlotDataByReflowPlacedCardId(reflowPlacedCardId);

        public List<CardSlotData> FindSlotsByLocation(LinePosition line, SlotLocation location) => _repository.FindSlotsByLocation(line, location);

        public bool IsPlayerZoneFull() => _repository.IsPlayerZoneFull();

        // --- Debug ---

        public List<SlotDebugInfo> GetDebugSlotData() => _debug.GetDebugSlotData();

        public void DebugSlotData(string callText) => _debug.DebugSlotData(callText);
    }
}

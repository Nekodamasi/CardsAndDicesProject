using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// カードの配置ロジックを担当するサービスクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CardPlacementService", menuName = "CardsAndDices/Systems/CardPlacementService")]
    public class CardPlacementService : ScriptableObject
    {
        [SerializeField] private CardSlotStateRepository _repository;

        [Inject]
        public void Initialize(CardSlotStateRepository repository)
        {
            _repository = repository;
        }

        public void PlaceCard(CompositeObjectId cardId, CompositeObjectId slotId)
        {
            // カードが既にいずれかのスロットに配置されている場合、そのスロットからカードを削除
            UnplaceCard(cardId);

            var targetSlot = _repository.GetSlotData(slotId);
            if (targetSlot != null)
            {
                targetSlot.PlacedCardId = cardId;
                targetSlot.ReflowPlacedCardId = cardId;
            }
            else
            {
                Debug.LogError($"Slot with ID {slotId} not found.");
            }
        }

        public void UnplaceCard(CompositeObjectId cardId)
        {
            foreach (var slot in _repository.GetAllSlots())
            {
                if (slot.PlacedCardId != null && slot.PlacedCardId.Equals(cardId))
                {
                    slot.PlacedCardId = null;
                }
                if (slot.ReflowPlacedCardId != null && slot.ReflowPlacedCardId.Equals(cardId))
                {
                    slot.ReflowPlacedCardId = null;
                }
            }
        }
    }
}

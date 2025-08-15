using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// カードの配置ロジックを担当するサービスクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DicePlacementService", menuName = "CardsAndDices/Systems/DicePlacementService")]
    public class DicePlacementService : ScriptableObject
    {
        [SerializeField] private DiceSlotStateRepository _repository;

        [Inject]
        public void Initialize(DiceSlotStateRepository repository)
        {
            _repository = repository;
        }

        public void PlaceDice(CompositeObjectId diceId, CompositeObjectId slotId)
        {
            // カードが既にいずれかのスロットに配置されている場合、そのスロットからカードを削除
            UnplaceDice(diceId);

            var targetSlot = _repository.GetSlotData(slotId);
            if (targetSlot != null)
            {
                targetSlot.PlacedDiceId = diceId;
                targetSlot.ReflowPlacedDiceId = diceId;
            }
            else
            {
                Debug.LogError($"Slot with ID {slotId} not found.");
            }
        }

        public void UnplaceDice(CompositeObjectId diceId)
        {
            foreach (var slot in _repository.GetAllSlots())
            {
                if (slot.PlacedDiceId != null && slot.PlacedDiceId.Equals(diceId))
                {
                    slot.PlacedDiceId = null;
                }
                if (slot.ReflowPlacedDiceId != null && slot.ReflowPlacedDiceId.Equals(diceId))
                {
                    slot.ReflowPlacedDiceId = null;
                }
            }
        }
    }
}

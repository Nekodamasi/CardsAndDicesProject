using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDice
{
    /// <summary>
    /// デバッグ用にスロットの情報を保持する構造体。
    /// </summary>
    [System.Serializable]
    public struct SlotDebugInfo
    {
        public string SlotId;
        public LinePosition Line;
        public SlotLocation Location;
        public string PlacedCardId;
        public string ReflowPlacedCardId;
    }

    /// <summary>
    /// スロット関連のデバッグ機能を提供するクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CardSlotDebug", menuName = "CardsAndDice/Systems/CardSlotDebug")]
    public class CardSlotDebug : ScriptableObject
    {
        [SerializeField] private CardSlotStateRepository _repository;

        [Inject]
        public void InInitialize(CardSlotStateRepository repository)
        {
            _repository = repository;
        }

        public List<SlotDebugInfo> GetDebugSlotData()
        {
            var duplicateCardIds = _repository.GetAllSlots()
                .Where(slot => slot.PlacedCardId != null)
                .GroupBy(slot => slot.PlacedCardId)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key);

            foreach (var cardId in duplicateCardIds)
            {
                var slotInfo = _repository.GetAllSlots()
                    .Where(s => s.PlacedCardId != null && s.PlacedCardId.Equals(cardId))
                    .Select(s => $"Slot(Line:{s.Line}, Loc:{s.Location})");
                
                Debug.Log($"<color=red>DUPLICATE DETECTED! PlacedCardId: {cardId} is in multiple slots: {string.Join(", ", slotInfo)}</color>");
            }

            var debugList = new List<SlotDebugInfo>();
            foreach (var slotData in _repository.GetAllSlots())
            {
                debugList.Add(new SlotDebugInfo
                {
                    SlotId = slotData.SlotId?.ToString() ?? "NULL",
                    Line = slotData.Line,
                    Location = slotData.Location,
                    PlacedCardId = slotData.PlacedCardId?.ToString() ?? "NULL",
                    ReflowPlacedCardId = slotData.ReflowPlacedCardId?.ToString() ?? "NULL"
                });
            }
            return debugList;
        }

        public void DebugSlotData(string callText)
        {
            Debug.Log("<Color=red>debugログコール＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝</Color>" + callText);
            foreach (var slotData in _repository.GetAllSlots())
            {
                Debug.Log("<Color=red>debugログ</Color>" + slotData.Line + "_" + slotData.Location + " CID:" + slotData.PlacedCardId + "/" + slotData.ReflowPlacedCardId);

                if (slotData.PlacedCardId != null && slotData.PlacedCardId.ObjectType != "Card")
                {
                    Debug.Log("<Color=red>スロットが配置されている</Color>" + slotData.Line + "_" + slotData.Location + " CID:" + slotData.PlacedCardId);
                }
                if (slotData.ReflowPlacedCardId != null && slotData.ReflowPlacedCardId.ObjectType != "Card")
                {
                    Debug.Log("<Color=red>スロットが配置されている</Color>" + slotData.Line + "_" + slotData.Location + " CID:" + slotData.ReflowPlacedCardId);
                }
            }
            GetDebugSlotData();
        }
    }
}

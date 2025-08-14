using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
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
    [CreateAssetMenu(fileName = "CardSlotDebug", menuName = "CardsAndDices/Systems/CardSlotDebug")]
    public class CardSlotDebug : ScriptableObject
    {
        [SerializeField] private CardSlotStateRepository _repository;
        private ViewRegistry _viewRegistry;

        [Inject]
        public void Initialize(CardSlotStateRepository repository, ViewRegistry viewRegistry)
        {
            _repository = repository;
            _viewRegistry = viewRegistry;
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
            LogSlotDifferences();
        }

        /// <summary>
        /// PlacedCardIdとReflowPlacedCardIdの整合性をチェックし、ログ出力します。
        /// </summary>
        public void LogSlotDifferences()
        {
            var allCreatureCardIds = _viewRegistry.GetAllCreatureCardViews()
                                                .Select(v => v.GetObjectId())
                                                .Where(id => id != null)
                                                .ToList();

            var placedCardIds = _repository.GetAllSlots()
                                         .Where(s => s.PlacedCardId != null)
                                         .Select(s => s.PlacedCardId)
                                         .ToList();

            var reflowPlacedCardIds = _repository.GetAllSlots()
                                               .Where(s => s.ReflowPlacedCardId != null)
                                               .Select(s => s.ReflowPlacedCardId)
                                               .ToList();

            // PlacedCardIdのチェック
            CheckCardIdConsistency(allCreatureCardIds, placedCardIds, "PlacedCardId");

            // ReflowPlacedCardIdのチェック
            CheckCardIdConsistency(allCreatureCardIds, reflowPlacedCardIds, "ReflowPlacedCardId");

            // 詳細なスロット情報ログ
            Debug.LogWarning($"[CardSlotDebug] Current Slot State:");
            foreach (var slot in _repository.GetAllSlots())
            {
                Debug.LogWarning($"  Slot: {slot.SlotId?.ToString() ?? "NULL"}, Line: {slot.Line}, Location: {slot.Location}, PlacedCardId: {slot.PlacedCardId?.ToString() ?? "NULL"}, ReflowPlacedCardId: {slot.ReflowPlacedCardId?.ToString() ?? "NULL"}");
            }
        }

        private void CheckCardIdConsistency(List<CompositeObjectId> expectedIds, List<CompositeObjectId> actualIds, string idType)
        {
            // 期待されるIDがすべて存在するかチェック
            foreach (var expectedId in expectedIds)
            {
                if (!actualIds.Contains(expectedId))
                {
                    Debug.LogWarning($"[CardSlotDebug] Consistency Error: Expected {idType} {expectedId} not found in any slot.");
                }
            }

            // 余分なIDが存在しないかチェック
            foreach (var actualId in actualIds)
            {
                if (!expectedIds.Contains(actualId))
                {
                    Debug.LogWarning($"[CardSlotDebug] Consistency Error: Unexpected {idType} {actualId} found in a slot.");
                }
            }

            // カウントの不一致をチェック
            if (expectedIds.Count != actualIds.Count)
            {
                Debug.LogWarning($"[CardSlotDebug] Mismatch in {idType} counts. Expected: {expectedIds.Count}, Actual: {actualIds.Count}");
            }
        }
    }
}

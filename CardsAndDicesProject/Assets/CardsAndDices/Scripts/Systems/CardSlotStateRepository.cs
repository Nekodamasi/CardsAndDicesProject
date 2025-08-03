using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDice
{
    /// <summary>
    /// 全てのカードスロットの状態を管理するリポジトリクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CardSlotStateRepository", menuName = "CardsAndDice/Systems/CardSlotStateRepository")]
    public class CardSlotStateRepository : ScriptableObject
    {
        private readonly Dictionary<CompositeObjectId, CardSlotData> _slotDataMap = new();

        [Inject]
        public void Initialize()
        {
            _slotDataMap.Clear();
        }

        public void RegisterSlot(CardSlotData slotData)
        {
            if (!_slotDataMap.ContainsKey(slotData.SlotId))
            {
                _slotDataMap[slotData.SlotId] = slotData;
            }
            else
            {
                Debug.LogWarning($"Slot with ID {slotData.SlotId} is already registered.");
            }
        }

        public CardSlotData GetSlotData(CompositeObjectId slotId)
        {
            _slotDataMap.TryGetValue(slotId, out CardSlotData slotData);
            return slotData;
        }

        public CardSlotData GetSlotDataByLocation(LinePosition line, SlotLocation location)
        {
            return _slotDataMap.Values.FirstOrDefault(s => s.Line == line && s.Location == location);
        }
        
        public CardSlotData GetSlotDataByReflowPlacedCardId(CompositeObjectId reflowPlacedCardId)
        {
            return _slotDataMap.Values.FirstOrDefault(s => s.ReflowPlacedCardId == reflowPlacedCardId);
        }

        public List<CardSlotData> FindSlotsByLocation(LinePosition line, SlotLocation location)
        {
            return _slotDataMap.Values
                .Where(slot => slot.Line == line && slot.Location == location)
                .ToList();
        }

        public CardSlotData GetNextEmptyHandSlot()
        {
            return _slotDataMap.Values
                .Where(slot => slot.Line == LinePosition.Hand && !slot.IsOccupied)
                .OrderBy(slot => slot.Location)
                .FirstOrDefault();
        }
        
        public IEnumerable<CardSlotData> GetAllSlots()
        {
            return _slotDataMap.Values;
        }
        
        public bool IsPlayerZoneFull()
        {
            return _slotDataMap.Values.Count(s => s.Line != LinePosition.Hand && s.IsOccupied) >= 6;
        }

        /// <summary>
        /// ReflowPlacedCardIdとPlacedCardIdで配置されているカードの数が異なる場合に、
        /// 全てのスロットの中身をログ出力します。
        /// デバッグ用途のメソッドです。
        /// </summary>
        public void LogSlotDifferences()
        {
            int reflowPlacedCount = _slotDataMap.Values.Count(s => s.ReflowPlacedCardId != null);
            int placedCount = _slotDataMap.Values.Count(s => s.PlacedCardId != null);

            if (reflowPlacedCount != placedCount)
            {
                Debug.LogWarning($"[CardSlotStateRepository] Mismatch in card counts. ReflowPlaced: {reflowPlacedCount}, Placed: {placedCount}");
                foreach (var slot in _slotDataMap.Values)
                {
                    Debug.LogWarning($"  Slot: {slot.SlotId?.ToString() ?? "NULL"}, Line: {slot.Line}, Location: {slot.Location}, PlacedCardId: {slot.PlacedCardId?.ToString() ?? "NULL"}, ReflowPlacedCardId: {slot.ReflowPlacedCardId?.ToString() ?? "NULL"}");
                }
            }
        }
    }
}
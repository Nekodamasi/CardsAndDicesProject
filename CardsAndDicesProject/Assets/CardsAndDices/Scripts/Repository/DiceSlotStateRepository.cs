using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのカードスロットの状態を管理するリポジトリクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceSlotStateRepository", menuName = "CardsAndDices/Repositories/DiceSlotStateRepository")]
    public class DiceSlotStateRepository : ScriptableObject
    {
        private readonly Dictionary<CompositeObjectId, DiceSlotData> _slotDataMap = new();

        [Inject]
        public void Initialize()
        {
            _slotDataMap.Clear();
        }

        public void RegisterSlot(DiceSlotData slotData)
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

        public DiceSlotData GetSlotData(CompositeObjectId slotId)
        {
            _slotDataMap.TryGetValue(slotId, out DiceSlotData slotData);
            return slotData;
        }

        public DiceSlotData GetSlotDataByLocation(DiceSlotLocation location)
        {
            return _slotDataMap.Values.FirstOrDefault(s => s.Location == location);
        }
        
        public DiceSlotData GetSlotDataByReflowPlacedCardId(CompositeObjectId reflowPlacedDiceId)
        {
            return _slotDataMap.Values.FirstOrDefault(s => s.ReflowPlacedDiceId == reflowPlacedDiceId);
        }

        public List<DiceSlotData> FindSlotsByLocation(DiceSlotLocation location)
        {
            return _slotDataMap.Values
                .Where(slot => slot.Location == location)
                .ToList();
        }

        public DiceSlotData GetNextEmptyHandSlot()
        {
            return _slotDataMap.Values
                .Where(slot => !slot.IsOccupied)
                .OrderBy(slot => slot.Location)
                .FirstOrDefault();
        }
        
        public IEnumerable<DiceSlotData> GetAllSlots()
        {
            return _slotDataMap.Values;
        }
        
        public bool IsDiceFull()
        {
            return _slotDataMap.Values.Count(s => s.IsOccupied) >= 6;
        }
        public DiceSlotData GetSlotDataByReflowPlacedDiceId(CompositeObjectId reflowPlacedDiceId)
        {
            return _slotDataMap.Values.FirstOrDefault(s => s.ReflowPlacedDiceId == reflowPlacedDiceId);
        }        
    }
}
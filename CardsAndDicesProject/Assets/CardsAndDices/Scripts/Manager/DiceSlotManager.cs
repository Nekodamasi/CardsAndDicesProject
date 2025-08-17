using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイススロットの状態を管理し、ダイスの配置などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceSlotManager", menuName = "CardsAndDices/Managers/DiceSlotManager")]
    public class DiceSlotManager : ScriptableObject
    {
        [Header("System Components")]
        [SerializeField] private DiceSlotStateRepository _repository;
        [SerializeField] private DicePlacementService _placementService;
        [SerializeField] private DiceSlotInteractionHandler _interactionHandler;

        private readonly List<DiceSlotData> _slotList = new();

        [Inject]
        public void Initialize(DiceSlotStateRepository repository, DicePlacementService placementService, DiceSlotInteractionHandler interactionHandler)
        {
            _repository = repository;
            _placementService = placementService;
            _interactionHandler = interactionHandler;
        }

        private void Awake()
        {
        }

        public List<DiceSlotData> FindSlotsByLocation(DiceSlotLocation location) => _repository.FindSlotsByLocation(location);
        public void RegisterSlot(DiceSlotData slotData) => _repository.RegisterSlot(slotData);

        public void OnDiceDroppedOnSlot(CompositeObjectId diceId, CompositeObjectId slotId) => _interactionHandler.OnDiceDroppedOnSlot(diceId, slotId);

        public void OnDiceHoveredOnSlot(CompositeObjectId diceId, CompositeObjectId slotId) => _interactionHandler.OnDiceHoveredOnSlot(diceId, slotId);

        public void OnDropFailed() => _interactionHandler.OnDropFailed();

        public void PlaceDiceAsSystem(CompositeObjectId diceId, CompositeObjectId slotId, bool triggerReflow = true)
        {
            Debug.Log("<color=Yellow>PlaceDiceAsSystem</color>");
            _placementService.UnplaceDice(diceId);
            _placementService.PlaceDice(diceId, slotId);
            if (triggerReflow)
            {
                _interactionHandler.SystemReflowDicesCurrentValue();
            }
        }

        /// <summary>
        /// 指定されたIDのスロットデータを取得します。
        /// </summary>
        /// <param name="id">取得するスロットのID。</param>
        /// <returns>見つかったスロットのデータ。見つからない場合はnull。</returns>
        public DiceSlotData GetSlotData(CompositeObjectId id)
        {
            return _slotList.FirstOrDefault(s => s.SlotId == id);

        }
    }
}

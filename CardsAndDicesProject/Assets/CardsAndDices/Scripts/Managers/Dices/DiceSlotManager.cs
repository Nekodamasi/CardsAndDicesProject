using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイススロットの状態を管理し、ダイスの配置などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceSlotManager", menuName = "CardsAndDices/Managers/Dices/DiceSlotManager")]
    public class DiceSlotManager : ScriptableObject
    {
        [Header("System Components")]
        [SerializeField] private List<DiceSlotPositionEntity> _diceSlotPositionEntities;
        private readonly List<DiceSlotInstance> _diceSlotInstances = new();
        private IdentifiableCommandBus _identifiableCommandBus;

        [Inject]
        public void Initialize(IdentifiableCommandBus identifiableCommandBus)
        {
            _identifiableCommandBus = identifiableCommandBus;
            _identifiableCommandBus.On<SceneLoadedCommand>(OnSceneLoaded);
        }

        /// <summary>
        /// ダイススロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedCommand cmd)
        {
            DisposeInstances();
            foreach (var diceSlotPositionEntity in _diceSlotPositionEntities)
            {
                _diceSlotInstances.Add(new DiceSlotInstance(diceSlotPositionEntity, _identifiableCommandBus));
            }
        }
        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _diceSlotInstances)
            {
                instance.Dispose();
            }
            _diceSlotInstances.Clear();
        }
        public void Dispose()
        {
            DisposeInstances();
        }
    }
}

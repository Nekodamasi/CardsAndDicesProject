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
        private readonly List<DiceSlotController> _iceSlotControllers = new();
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
            Dispose();
            foreach (var diceSlotPositionEntity in _diceSlotPositionEntities)
            {
                var instance = new DiceSlotInstance(diceSlotPositionEntity, _identifiableCommandBus);
                _diceSlotInstances.Add(instance);
                _iceSlotControllers.Add(new DiceSlotController(instance, _identifiableCommandBus));
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
        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var controller in _iceSlotControllers)
            {
                controller.Dispose();
            }
            _iceSlotControllers.Clear();
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            _identifiableCommandBus.Off<SceneLoadedCommand>(OnSceneLoaded);
        }
    }
}

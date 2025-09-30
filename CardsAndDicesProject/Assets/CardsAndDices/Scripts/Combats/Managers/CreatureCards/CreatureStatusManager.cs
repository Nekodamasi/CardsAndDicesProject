using System.Collections.Generic;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードの状態管理などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureStatusManager", menuName = "CardsAndDices/Combats/Managers/CreatureCards/CreatureStatusManager")]
    public class CreatureStatusManager : ScriptableObject, IDisposable
    {
        [Header("Components")]

        [SerializeField] private List<CreatureStatusIconData> _reatureStatusIconDataList = new();
        private readonly List<CreatureStatusInstance> _creatureStatusInstances = new();
        private readonly List<CreatureStatusPresenter> _creatureStatusPresenters = new();
        private readonly List<CreatureCardStatusIconController> _creatureCardStatusIconControllers = new();
        private GameEventBus _eventBus;
        private CreatureCardSlotManager _creatureCardSlotManager;
        private IdentifiableViewRegistry _viewRegistry;
        private IEffectValue _iEffectValue;

        [Inject]
        public void Initialize(GameEventBus eventBus, CreatureCardSlotManager creatureCardSlotManager, IdentifiableViewRegistry viewRegistry, IEffectValue iEffectValue)
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus = eventBus;
            _creatureCardSlotManager = creatureCardSlotManager;
            _viewRegistry = viewRegistry;
            _iEffectValue = iEffectValue;
            _eventBus.On<CreateCreatureEvent>(OnCreateCreature);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            DisposeControllers();
            _eventBus.Off<CreateCreatureEvent>(OnCreateCreature);
        }

        /// <summary>
        /// クリーチャーの生成イベント
        /// </summary>
        private void OnCreateCreature(CreateCreatureEvent evt)
        {
            var instance = new CreatureStatusInstance(evt.CreatureCardId, evt.CardInitializationData.CreatureData, _iEffectValue);
            _creatureStatusInstances.Add(instance);
            var controller = new CreatureCardStatusIconController(instance, _eventBus, _reatureStatusIconDataList);
            _creatureCardStatusIconControllers.Add(controller);

            foreach (var ability in evt.CardInitializationData.CreatureData.Abilities)
            {
                _eventBus.Emit(new CreateAbilityEvent(instance.CompositeObjectId, ability, null, instance));
            }
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _creatureStatusInstances)
            {
                instance.Dispose();
            }
            _creatureStatusInstances.Clear();
        }
        /// <summary>
        /// PresenterをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var presenter in _creatureStatusPresenters)
            {
                presenter.Dispose();
            }
            _creatureStatusPresenters.Clear();
        }

        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var iconController in _creatureCardStatusIconControllers)
            {
                iconController.Dispose();
            }
            _creatureCardStatusIconControllers.Clear();
        }
   }
}

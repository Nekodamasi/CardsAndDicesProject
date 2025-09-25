using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードの状態管理などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureCardManager", menuName = "CardsAndDices/Combats/Managers/CreatureCards/CreatureCardManager")]
    public class CreatureCardManager : ScriptableObject, IDisposable
    {
        [Header("Components")]
        private readonly List<CreatureCardInstance> _creatureCardInstances = new();
        private readonly List<CreatureCardPresenter> _creatureCardPresenters = new();
        private GameEventBus _eventBus;
        private CreatureCardSlotManager _creatureCardSlotManager;
        private IdentifiableViewRegistry _viewRegistry;

        [Inject]
        public void Initialize(GameEventBus eventBus, CreatureCardSlotManager creatureCardSlotManager, IdentifiableViewRegistry viewRegistry)
        {
            DisposeInstances();
            _eventBus = eventBus;
            _creatureCardSlotManager = creatureCardSlotManager;
            _viewRegistry = viewRegistry;
            _eventBus.On<CreateCreatureEvent>(OnCreateCreature);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus.Off<CreateCreatureEvent>(OnCreateCreature);
        }

        /// <summary>
        /// クリーチャーの生成イベント
        /// </summary>
        private void OnCreateCreature(CreateCreatureEvent evt)
        {
            var instance = new CreatureCardInstance(evt.CreatureCardId, _creatureCardSlotManager);
            _creatureCardInstances.Add(instance);
            var view = _viewRegistry.GetView<CreatureCardView>(evt.CreatureCardId);
            view.SetBoundState(true);
            var presenter = new CreatureCardPresenter(instance, view, _eventBus);
            _creatureCardPresenters.Add(presenter);
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _creatureCardInstances)
            {
                instance.Dispose();
            }
            _creatureCardInstances.Clear();
        }
        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var presenter in _creatureCardPresenters)
            {
                presenter.Dispose();
            }
            _creatureCardPresenters.Clear();
        }
   }
}

using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// Manages all ICreature instances in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureManager", menuName = "CardsAndDices/Managers/CreatureManager")]
    public class CreatureManager : ScriptableObject
    {
        private  CreatureFactory _creatureFactory;
        private ViewRegistry _viewRegistry;
        private SpriteCommandBus _commandBus;
        private AbilityManager _abilityManager; // 追加
        private readonly Dictionary<CompositeObjectId, ICreature> _creatures = new();
        private readonly Dictionary<CompositeObjectId, CreatureCardPresenter> _presenters = new();

        /// <summary>
        /// CombatManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(ViewRegistry viewRegistry, SpriteCommandBus commandBus, EffectManager effectManager, AbilityManager abilityManager)
        {
            ClearCollections();
            _creatureFactory = new CreatureFactory(effectManager, commandBus);
            _viewRegistry = viewRegistry;
            _commandBus = commandBus;
            _abilityManager = abilityManager; // 初期化
        }

        private void ClearCollections()
        {
            _creatures.Clear();
            foreach (var presenter in _presenters.Values)
            {
                presenter.Dispose();
            }
            _presenters.Clear();
        }

        /// <summary>
        /// Spawns a new creature, creates its presenter, and links it to its view.
        /// </summary>
        /// <param name="id">The ID for the new creature.</param>
        /// <param name="baseData">The base data for the creature.</param>
        /// <param name="view">The ID of the view component for this creature.</param>
        /// <returns>The newly created creature instance.</returns>
        public ICreature SpawnCreature(CreatureData baseData, CreatureCardView view)
        {
            CompositeObjectId viewId = view.GetObjectId();
            if (_creatures.ContainsKey(viewId))
            {
                Debug.LogError("Handle error: creature with this ID already exists");
                return null;
            }

            ICreature newCreature = _creatureFactory.Create(viewId, baseData);
            _creatures.Add(viewId, newCreature);

            var presenter = new CreatureCardPresenter(newCreature, view, _commandBus);
            _presenters.Add(viewId, presenter);
            return newCreature;
        }

        /// <summary>
        /// Retrieves a creature by its ID.
        /// </summary>
        /// <param name="id">The ID of the creature to retrieve.</param>
        /// <returns>The creature instance, or null if not found.</returns>
        public ICreature GetCreature(CompositeObjectId id)
        {
            _creatures.TryGetValue(id, out var creature);
            return creature;
        }

        /// <summary>
        /// Removes a creature from the game.
        /// </summary>
        /// <param name="id">The ID of the creature to remove.</param>
        public void RemoveCreature(CompositeObjectId id)
        {
            if (_presenters.TryGetValue(id, out var presenter))
            {
                presenter.Dispose();
                _presenters.Remove(id);
            }
            _creatures.Remove(id);
        }
    }
}

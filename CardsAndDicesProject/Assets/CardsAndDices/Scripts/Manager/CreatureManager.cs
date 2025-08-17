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
        private readonly Dictionary<CompositeObjectId, ICreature> _creatures = new();
        private readonly Dictionary<CompositeObjectId, CreatureCardPresenter> _presenters = new();

        /// <summary>
        /// CombatManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(ViewRegistry viewRegistry, SpriteCommandBus commandBus, EffectManager effectManager)
        {
            _creatureFactory = new CreatureFactory(effectManager, commandBus);
            _viewRegistry = viewRegistry;
            _commandBus = commandBus;
        }

        /// <summary>
        /// Spawns a new creature, creates its presenter, and links it to its view.
        /// </summary>
        /// <param name="id">The ID for the new creature.</param>
        /// <param name="baseData">The base data for the creature.</param>
        /// <param name="viewId">The ID of the view component for this creature.</param>
        /// <returns>The newly created creature instance.</returns>
        public ICreature SpawnCreature(CompositeObjectId id, CreatureData baseData, CompositeObjectId viewId)
        {
            if (_creatures.ContainsKey(id))
            {
                // Handle error: creature with this ID already exists
                return null;
            }

            ICreature newCreature = _creatureFactory.Create(id, baseData);
            _creatures.Add(id, newCreature);

            var view = _viewRegistry.GetView<CreatureCardView>(viewId);
            if (view != null)
            {
                var presenter = new CreatureCardPresenter(newCreature, view, _commandBus);
                _presenters.Add(id, presenter);
            }
            else
            {
                // Handle error: view not found
            }

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

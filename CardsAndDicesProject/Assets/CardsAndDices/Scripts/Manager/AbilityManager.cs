using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// Manages all active AbilityInstances in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "AbilityManager", menuName = "CardsAndDices/Managers/AbilityManager")]
    public class AbilityManager : ScriptableObject
    {
        private SpriteCommandBus _commandBus;
        private CompositeObjectIdManager _idManager;

        private readonly List<AbilityInstance> _abilities = new();

        [Inject]
        public void Initialize(SpriteCommandBus commandBus, CompositeObjectIdManager idManager)
        {
            _commandBus = commandBus;
            _idManager = idManager;
            // Subscribe to all commands. A more optimized approach might use a dedicated event type.
            _commandBus.On<ICommand>(OnCommandDispatched);
        }

        private void OnDisable()
        {
            _commandBus.Off<ICommand>(OnCommandDispatched);
        }

        /// <summary>
        /// Creates and registers a new ability instance for a creature.
        /// </summary>
        public void RegisterAbility(BaseAbilityDataSO abilityData, ICreature owner)
        {
            var id = _idManager.CreateId(abilityData.Id, owner.Id);
            var instance = new AbilityInstance(id, abilityData, owner);
            _abilities.Add(instance);
        }

        /// <summary>
        /// Unregisters all abilities associated with a specific owner.
        /// </summary>
        public void UnregisterAbilitiesForOwner(CompositeObjectId ownerId)
        {
            _abilities.RemoveAll(instance => instance.OwnerCreature.Id == ownerId);
        }

        private void OnCommandDispatched(ICommand command)
        {
            // Handle ability triggering
            foreach (var instance in _abilities)
            {
                if (instance.IsSuppressed || instance.Data.TriggerCondition == null) continue;

                if (instance.Data.TriggerCondition.Check(command, instance))
                {
                    // TODO: Check for cooldown and usage limits from instance.Data.Duration
                    var context = new BaseAbilityEffectDefinitionSO.AbilityContext
                    {
                        SourceId = instance.OwnerCreature.Id
                        // TODO: Populate TargetId and other context from the command if available
                    };
                    instance.Data.EffectDefinition?.Execute(context, _commandBus);
                    // TODO: Update duration state (e.g., decrement uses, set cooldown)
                }
            }

            // Handle duration updates
            foreach (var instance in _abilities)
            {
                instance.Data.Duration?.OnEvent(instance, command);
            }
        }
    }
}

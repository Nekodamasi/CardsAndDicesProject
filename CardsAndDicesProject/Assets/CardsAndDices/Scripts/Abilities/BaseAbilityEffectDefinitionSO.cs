using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Abstract base class for all ability effect definitions.
    /// </summary>
    public abstract class BaseAbilityEffectDefinitionSO : ScriptableObject
    {
        /// <summary>
        /// Contains the context for an ability's execution, such as the source and target.
        /// </summary>
        public class AbilityContext
        {
            public CompositeObjectId SourceId;
            public CompositeObjectId TargetId;
            public int DiceValue;
            // Add other context-specific data as needed
        }

        /// <summary>
        /// Executes the ability's effect.
        /// </summary>
        /// <param name="context">The context of the ability execution.</param>
        /// <param name="commandBus">The command bus to dispatch new commands if needed.</param>
        public abstract void Execute(AbilityContext context, SpriteCommandBus commandBus);
    }
}

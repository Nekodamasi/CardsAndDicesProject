using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Abstract base class for all ability trigger conditions.
    /// </summary>
    public abstract class BaseAbilityTriggerConditionSO : ScriptableObject
    {
        /// <summary>
        /// Checks if the condition is met based on the dispatched command and the state of the ability's owner.
        /// </summary>
        /// <param name="command">The command that was dispatched on the event bus.</param>
        /// <param name="abilityInstance">The instance of the ability being checked.</param>
        /// <returns>True if the condition is met, false otherwise.</returns>
        public abstract bool Check(ICommand command, AbilityInstance abilityInstance);
    }
}

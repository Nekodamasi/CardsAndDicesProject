using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Abstract base class for all ability duration, cooldown, and usage limit logic.
    /// </summary>
    public abstract class BaseAbilityDurationSO : ScriptableObject
    {
        [Tooltip("The initial value for the duration (e.g., cooldown turns, number of uses)._")]
        public int InitialValue;

        [Tooltip("The value to reset to after the duration expires (if applicable)._")]
        public int ResetValue;

        /// <summary>
        /// Called on a game event (like TurnEnd) to update the duration state of an ability instance.
        /// </summary>
        /// <param name="instance">The ability instance to update.</param>
        /// <param name="command">The command that triggered the event.</param>
        public abstract void OnEvent(AbilityInstance instance, ICommand command);

        /// <summary>
        /// Resets the duration state of an ability instance to its initial values.
        /// </summary>
        /// <param name="instance">The ability instance to reset.</param>
        public virtual void Reset(AbilityInstance instance)
        {
            instance.RemainingUsages = InitialValue;
            instance.CurrentCooldown = 0;
        }
    }
}

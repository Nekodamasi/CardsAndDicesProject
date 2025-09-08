using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Abstract base class for all ability trigger conditions.
    /// </summary>
    public abstract class BaseAbilityTriggerConditionSO : ScriptableObject
    {
        [Tooltip("アクティブタイミング")]
        public TriggerTiming ActivationTiming;

        /// <summary>
        /// Checks if the trigger condition is met.
        /// This method first checks the activation timing and then calls the specific condition check.
        /// </summary>
        public bool Check(CompositeObjectId ownerId, TriggerTiming activationTiming, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager)
        {
            if (ActivationTiming != activationTiming)
            {
                return false;
            }
            return CheckCondition(ownerId, creatureManager, diceManager, abilityManager);
        }

        /// <summary>
        /// When overridden in a derived class, checks the specific conditions for the trigger.
        /// </summary>
        /// <returns>True if the specific conditions are met, false otherwise.</returns>
        protected abstract bool CheckCondition(CompositeObjectId ownerId, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager);
    }
}

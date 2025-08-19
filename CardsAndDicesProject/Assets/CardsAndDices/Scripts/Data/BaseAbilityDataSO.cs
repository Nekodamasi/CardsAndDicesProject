using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Abstract base class for all ability definition ScriptableObjects.
    /// It holds references to the specific condition, effect, and duration logic.
    /// </summary>
    public abstract class BaseAbilityDataSO : ScriptableObject
    {
        [Tooltip("Unique identifier for this ability.")]
        public string Id;

        [Tooltip("Display name of the ability.")]
        public string Name;

        [Tooltip("Description of what the ability does.")]
        [TextArea]
        public string Description;

        [Tooltip("The condition that triggers this ability.")]
        public BaseAbilityTriggerConditionSO TriggerCondition;

        [Tooltip("The effect that is executed when the ability is triggered.")]
        public BaseAbilityEffectDefinitionSO EffectDefinition;

        [Tooltip("The duration, cooldown, or usage limits of the ability.")]
        public BaseAbilityDurationSO Duration;
    }
}

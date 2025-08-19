using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Abstract base class for all ability definition ScriptableObjects.
    /// It holds references to the specific condition, effect, and duration logic.
    /// </summary>
    public abstract class BaseAbilityDataSO : ScriptableObject
    {
        [Tooltip("この機能の一意の識別子")]
        public string Id;

        [Tooltip("この能力を発動させる条件")]
        public BaseAbilityTriggerConditionSO TriggerCondition;

        [Tooltip("能力が発動されたときに実行される効果")]
        public BaseAbilityEffectDefinitionSO EffectDefinition;

        [Tooltip("能力の持続時間、クールダウン、または使用制限")]
        public BaseAbilityDurationSO Duration;
    }
}

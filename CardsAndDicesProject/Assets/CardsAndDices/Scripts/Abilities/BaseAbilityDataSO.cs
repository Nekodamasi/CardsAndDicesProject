using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// すべてのアビリティ定義 ScriptableObjects の抽象基本クラス
    /// 特定の条件、効果、効果範囲、および期間のロジックへの参照を保持します。
    /// </summary>
    public abstract class BaseAbilityDataSO : ScriptableObject
    {
        [Tooltip("この機能の一意の識別子")]
        public string Id;

        [Tooltip("この能力の効果範囲")]
        public BaseAbilityTargetSelectorSO TargetSelector;

        [Tooltip("この能力を発動させる条件")]
        public BaseAbilityTriggerConditionSO TriggerCondition;

        [Tooltip("能力が発動されたときに実行される効果")]
        public BaseAbilityEffectDefinitionSO EffectDefinition;

        [Tooltip("能力の持続時間、クールダウン、または使用制限")]
        public BaseAbilityDurationSO Duration;
    }
}

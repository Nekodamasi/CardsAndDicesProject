using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// すべての能力効果の発動判定定義の抽象基本クラス。
    /// </summary>
    public abstract class BaseAbilityTriggerConditionSO : ScriptableObject
    {
        [Tooltip("アクティブタイミング")]
        [SerializeField] private ActivationTiming _activationTiming;

        /// <summary>
        /// 発動条件チェックを行う
        /// </summary>
        public ActivationTiming ActivationTiming => _activationTiming;

        /// <summary>
        /// 発動条件チェックを行う
        /// </summary>
        public abstract bool CheckCondition(AbilityContext abilityContext);
    }
}

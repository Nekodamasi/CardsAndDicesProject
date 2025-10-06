using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// トリガーとして常にtrueを返す
    /// </summary>
    [CreateAssetMenu(fileName = "ActivationTimingOnlyTrigger", menuName = "CardsAndDices/Combats/Data/Abilities/Triggers/ActivationTimingOnlyTrigger")]
    public class ActivationTimingOnlyTriggerConditionSO : BaseAbilityTriggerConditionSO
    {
        /// <summary>
        /// ActivationTiming以外のチェックを行わず、トリガーは常にtrueを返す
        /// </summary>
        public override bool CheckCondition(AbilityContext abilityContext)
        {
            return true;
        }
    }
}

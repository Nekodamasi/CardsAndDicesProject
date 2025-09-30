using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ActivationTimingの判定Only
    /// </summary>
    [CreateAssetMenu(fileName = "ActivationTimingOnlyTrigger", menuName = "CardsAndDices/Abilities/Triggers/ActivationTimingOnlyTrigger")]
    public class ActivationTimingOnlyTriggerConditionSO : BaseAbilityTriggerConditionSO
    {
        /// <summary>
        /// Checks if the received command is a CreatureAttackedCommand targeting the owner.
        /// </summary>
        public override bool CheckCondition(AbilityContext abilityContext)
        {
            return false;
        }
    }
}

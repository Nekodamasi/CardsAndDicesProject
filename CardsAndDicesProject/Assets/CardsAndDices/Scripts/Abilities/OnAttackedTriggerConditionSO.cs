using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// A trigger condition that is met when the owner creature is attacked.
    /// </summary>
    [CreateAssetMenu(fileName = "OnAttackedTrigger", menuName = "CardsAndDices/Abilities/Triggers/On Attacked")]
    public class OnAttackedTriggerConditionSO : BaseAbilityTriggerConditionSO
    {
        /// <summary>
        /// Checks if the received command is a CreatureAttackedCommand targeting the owner.
        /// </summary>
        public override bool Check(ICommand command, AbilityInstance abilityInstance)
        {
            if (command is CreatureAttackedCommand attackedCommand)
            {
                return attackedCommand.TargetId == abilityInstance.OwnerCreature.Id;
            }
            return false;
        }
    }
}

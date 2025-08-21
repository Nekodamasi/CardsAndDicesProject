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
        public override bool Check(CompositeObjectId ownerId, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager)
        {
            return false;
        }
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// A trigger condition that is met when the owner creature is attacked.
    /// </summary>
    [CreateAssetMenu(fileName = "OnPlacementCardTrigger", menuName = "CardsAndDices/Abilities/Triggers/OnPlacementCardTriggerCondition")]
    public class OnPlacementCardTriggerConditionSO : BaseAbilityTriggerConditionSO
    {
        /// <summary>
        /// 条件を満たしているかチェックします
        /// </summary>
        public override bool Check(CompositeObjectId ownerId, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager)
        {
            var creature = creatureManager.GetCreature(ownerId);
            Debug.Log("ここはどうかなーーーーー：" + creature.Location);
            if (creature.Location == SlotLocation.Vanguard)
            {
                return true;
            }
            return false;
        }
    }
}

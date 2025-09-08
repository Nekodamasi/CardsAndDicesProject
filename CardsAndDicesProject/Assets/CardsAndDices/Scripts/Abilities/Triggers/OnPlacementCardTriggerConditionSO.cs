using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 前衛に配置された場合
    /// </summary>
    [CreateAssetMenu(fileName = "OnPlacementCardTrigger", menuName = "CardsAndDices/Abilities/Triggers/OnPlacementCardTriggerCondition")]
    public class OnPlacementCardTriggerConditionSO : BaseAbilityTriggerConditionSO
    {
        /// <summary>
        /// 条件を満たしているかチェックします
        /// </summary>
        protected override bool CheckCondition(CompositeObjectId ownerId, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager)
        {
            var creature = creatureManager.GetCreature(ownerId);
            if (creature.Location == SlotLocation.Vanguard)
            {
                return true;
            }
            return false;
        }
    }
}

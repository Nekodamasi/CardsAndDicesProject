using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 指定した配置場所（Location）にカードが置かれた場合をTrigger条件とします。
    /// </summary>
    [CreateAssetMenu(fileName = "OnPlacementCardTrigger", menuName = "CardsAndDices/Combats/Data/Abilities/Triggers/OnPlacementCardTriggerCondition")]
    public class OnPlacementCardTriggerConditionSO : BaseAbilityTriggerConditionSO
    {
        [SerializeField] private SlotLocation _slotLocation = SlotLocation.Vanguard;
        /// <summary>
        /// 条件を満たしているかチェックします
        /// </summary>
        public override bool CheckCondition(AbilityContext abilityContext)
        {
            if (abilityContext.ICreatureCardlocation.GetSlotLocation(abilityContext.CreatureStatusInstance.CompositeObjectId) != _slotLocation) return false;
            return true;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// Selects the owner of the ability as the target.
    /// </summary>
    [CreateAssetMenu(fileName = "SelfTargetSelector", menuName = "CardsAndDices/Abilities/Targets/Self Target")]
    public class SelfTargetSelectorSO : BaseAbilityTargetSelectorSO
    {
        public override List<CompositeObjectId> SelectTarget(CompositeObjectId SourceId, CreatureManager creatureManager, DiceManager diceManager)
        {
            List<CompositeObjectId> list = new List<CompositeObjectId>();
            list.Add(SourceId);
            return list;
        }
    }
}
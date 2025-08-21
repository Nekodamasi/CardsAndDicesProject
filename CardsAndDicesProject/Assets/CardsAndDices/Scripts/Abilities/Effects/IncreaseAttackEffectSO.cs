using UnityEngine;
using VContainer; // CreatureManagerをDIで受け取るため

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "IncreaseAttackEffect", menuName = "CardsAndDices/Abilities/Effects/Increase Attack")]
    public class IncreaseAttackEffectSO : BaseAbilityEffectDefinitionSO
    {
        public override void Execute(AbilityContext context, SpriteCommandBus commandBus)
        {
        }
    }
}

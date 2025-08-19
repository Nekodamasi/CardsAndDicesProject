using UnityEngine;
using VContainer; // CreatureManagerをDIで受け取るため

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "IncreaseAttackEffect", menuName = "CardsAndDices/Abilities/Effects/Increase Attack")]
    public class IncreaseAttackEffectSO : BaseAbilityEffectDefinitionSO
    {
        [SerializeField] private int _attackAmount = 2; // 攻撃力増加量

        public override void Execute(AbilityContext context, SpriteCommandBus commandBus)
        {
            if (context.TargetId == null)
            {
                Debug.LogWarning("IncreaseAttackEffectSO: TargetId is null. Cannot apply effect.");
                return;
            }
        }
    }
}

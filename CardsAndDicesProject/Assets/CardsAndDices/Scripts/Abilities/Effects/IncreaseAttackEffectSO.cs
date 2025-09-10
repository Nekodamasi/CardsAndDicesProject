using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "IncreaseAttackEffect", menuName = "CardsAndDices/Abilities/Effects/Increase Attack")]
    public class IncreaseAttackEffectSO : BaseAbilityEffectDefinitionSO
    {
        public override async UniTask Execute(AbilityContext context, SpriteCommandBus commandBus, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager, EffectManager effectManager)
        {
            Debug.Log("じっこう！！！！！！！！！！！！！！！！！！！！！！！！！！！！！");
            await UniTask.CompletedTask;
        }
    }
}

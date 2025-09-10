using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using VContainer; // CreatureManagerをDIで受け取るため
using Cysharp.Threading.Tasks;
using System;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "CurrentValueChangeEffectSO", menuName = "CardsAndDices/Abilities/Effects/CurrentValueChangeEffectSO")]
    public class CurrentValueChangeEffectSO : BaseAbilityEffectDefinitionSO
    {
        [SerializeField] private EffectData _effectData;
        [SerializeField] private List<CurrentValueChangeContext> _currentValueChangeContexts = new List<CurrentValueChangeContext>();

        /// <summary>
        /// ソースやターゲットなど、アビリティ実行のコンテキストが含まれます。
        /// </summary>
        [System.Serializable]
        public class CurrentValueChangeContext
        {
            public EffectTargetType EffectTargetType;
            public int Value;
        }
        public override async UniTask Execute(AbilityContext context, SpriteCommandBus commandBus, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager, EffectManager effectManager)
        {
            Debug.Log("<color=red>CurrentValueChangeEffectSO：</color>");
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            foreach (var targetId in context.TargetIds)
            {
                var creature = creatureManager.GetCreature(targetId);
                foreach (var currentValueChange in _currentValueChangeContexts)
                {
                    switch (currentValueChange.EffectTargetType)
                    {
                        case EffectTargetType.Health:
                            commandBus.Emit(new CreatureHealthChangedCommand(creature.Id, creature.CurrentHealth + currentValueChange.Value, creature.BaseHealth));
                            break;
                        case EffectTargetType.Shield:
                            commandBus.Emit(new CreatureShieldChangedCommand(creature.Id, creature.CurrentShield + currentValueChange.Value, creature.BaseShield));
                            break;
                        case EffectTargetType.Cooldown:
                            Debug.Log("こことおおったーーーーーーーーーーーーーーーーーーーーーーーーー？");
                            commandBus.Emit(new CreatureCooldownChangedCommand(creature.Id, creature.CurrentCooldown + currentValueChange.Value, creature.BaseCooldown));
                            break;
                    }
                }
                if (VfxDefinition != null)
                {
                    commandBus.Emit(new CreatureBUffEffectedCommand(creature.Id, VfxDefinition));
                    await UniTask.Delay(TimeSpan.FromSeconds(1.6f));
                }
            }
            commandBus.Emit(new CooldownZeroAttacksCommand(new ProcessAllCreaturesCooldownCommand()));
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

        }
    }
}

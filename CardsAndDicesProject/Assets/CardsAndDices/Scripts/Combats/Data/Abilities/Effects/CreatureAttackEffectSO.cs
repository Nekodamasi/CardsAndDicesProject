using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using VContainer; // CreatureManagerをDIで受け取るため
using Cysharp.Threading.Tasks;
using System;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "CreatureAttackEffectSO", menuName = "CardsAndDices/Combats/Data/Abilities/Effects/CreatureAttackEffectSO")]
    public class CreatureAttackEffectSO : BaseAbilityEffectDefinitionSO
    {
        [SerializeField] private CreatureAttackData _creatureAttackData;


        public override void Execute(AbilityContext context, GameEventBus eventBus)
        {
            Debug.Log("くりーちゃーあたっく");
            CreatureAttackContext creatureAttackContext = new CreatureAttackContext(
                                                                                context.CreatureStatusInstance.CompositeObjectId,
                                                                                _creatureAttackData.HitsPerAttack,
                                                                                _creatureAttackData.AreaOfEffect,
                                                                                _creatureAttackData.AttackEffectTargetType,
                                                                                _creatureAttackData.AddAttackPoint);



            eventBus.Emit(new CreatureAttackEvent(creatureAttackContext));
        }
    }
}

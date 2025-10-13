using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using VContainer; // CreatureManagerをDIで受け取るため
using Cysharp.Threading.Tasks;
using System;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "CurrentValueChangeEffectSO", menuName = "CardsAndDices/Combats/Data/Abilities/Effects/CurrentValueChangeEffectSO")]
    public class CurrentValueChangeEffectSO : BaseAbilityEffectDefinitionSO
    {
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
        public override async void Execute(AbilityContext context, GameEventBus eventBus)
        {
            foreach (var targetId in context.TargetIds)
            {
                foreach (var changeValue in _currentValueChangeContexts)
                {
                    Debug.Log("<color=Green>アプライCHANGEValue：</color>" + targetId + "_");
                    eventBus.Emit(new ChangeCreatureCurrentValueEvent(targetId, changeValue.EffectTargetType, changeValue.Value));
                    eventBus.Emit(new DisplayCreatureBuffEvent(targetId, VfxDefinition));

                }
            }
            await UniTask.Delay(TimeSpan.FromSeconds(2.2f));
            eventBus.Emit(new BuffDebuffEffectEndActionEvent());
        }
    }
}

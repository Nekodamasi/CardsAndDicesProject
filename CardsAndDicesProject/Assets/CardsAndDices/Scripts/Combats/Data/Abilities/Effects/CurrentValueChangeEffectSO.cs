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
        public override void Execute(AbilityContext context, GameEventBus eventBus)
        {
        }
    }
}

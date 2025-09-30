using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.Search;
using UnityEngine;
using VContainer; // CreatureManagerをDIで受け取るため

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "BuffDebuffEffect", menuName = "CardsAndDices/Combats/Data/Abilities/Effects/BuffDebuffEffect")]
    public class BuffDebuffEffectSO : BaseAbilityEffectDefinitionSO
    {
        [SerializeField] private EffectData _effectData;
        [SerializeField] private List<BuffDebuffContext> _buffDebuffContexts = new List<BuffDebuffContext>();

        /// <summary>
        /// ソースやターゲットなど、アビリティ実行のコンテキストが含まれます。
        /// </summary>
        [System.Serializable]
        public class BuffDebuffContext
        {
            public EffectTargetType EffectTargetType;
            public int Value;
        }
        public override void Execute(AbilityContext context, GameEventBus eventBus)
        {
            foreach (var targetId in context.TargetIds)
            {
                foreach (var buffDebuff in _buffDebuffContexts)
                {
                    Debug.Log("<color=Green>アプライエフェクト：</color>" + targetId + "_");
                    eventBus.Emit(new ApplyEffectCommand(targetId, _effectData, buffDebuff.EffectTargetType, buffDebuff.Value));
                }
            }
        }
    }
}

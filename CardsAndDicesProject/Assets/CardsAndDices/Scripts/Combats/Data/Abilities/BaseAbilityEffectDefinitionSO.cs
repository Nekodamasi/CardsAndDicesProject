using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// すべての能力効果の定義の抽象基本クラス。
    /// </summary>
    public abstract class BaseAbilityEffectDefinitionSO : ScriptableObject
    {
        [Header("VFX Settings")]
        [Tooltip("再生するパーティクルのVfxDefinition")]
        [SerializeField] private VfxDefinition VfxDefinition;

        /// <summary>
        /// 能力の効果を実行します。
        /// </summary>
        /// <param name="context">The context of the ability execution.</param>
        /// <param name="commandBus">The command bus to dispatch new commands if needed.</param>
        public abstract void Execute(AbilityContext context, GameEventBus eventBus);
    }
}

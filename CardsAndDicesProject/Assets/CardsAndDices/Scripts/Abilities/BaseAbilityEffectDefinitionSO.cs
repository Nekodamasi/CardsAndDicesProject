using UnityEngine;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// すべての能力効果の定義の抽象基本クラス。
    /// </summary>
    public abstract class BaseAbilityEffectDefinitionSO : ScriptableObject
    {
        /// <summary>
        /// ソースやターゲットなど、アビリティ実行のコンテキストが含まれます。
        /// </summary>
        public class AbilityContext
        {
            public CompositeObjectId SourceId;
            public List<CompositeObjectId> TargetIds;
            public int DiceValue;
            // Add other context-specific data as needed
        }

        /// <summary>
        /// 能力の効果を実行します。
        /// </summary>
        /// <param name="context">The context of the ability execution.</param>
        /// <param name="commandBus">The command bus to dispatch new commands if needed.</param>
        public abstract void Execute(AbilityContext context, SpriteCommandBus commandBus, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager, EffectManager effectManager);
    }
}

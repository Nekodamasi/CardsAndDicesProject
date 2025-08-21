using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// アビリティの効果範囲（ターゲット条件）の基底クラスです
    /// </summary>
    public abstract class BaseAbilityTargetSelectorSO : ScriptableObject
    {
        /// <summary>
        /// ソースやターゲットなど、アビリティ実行のコンテキストが含まれます。
        /// </summary>
        public class AbilityContext
        {
            public CompositeObjectId SourceId;
            public CompositeObjectId TargetId;
            public int DiceValue;
            // Add other context-specific data as needed
        }

        /// <summary>
        /// 能力の効果を実行します。
        /// </summary>
        /// <param name="context">The context of the ability execution.</param>
        /// <param name="commandBus">The command bus to dispatch new commands if needed.</param>
        public abstract List<CompositeObjectId> SelectTarget(CompositeObjectId SourceId, CreatureManager creatureManager, DiceManager diceManager);
    }
}

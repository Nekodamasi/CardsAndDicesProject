using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// ソースやターゲットなど、アビリティ実行のコンテキストが含まれます。
    /// </summary>
    public class AbilityContext
    {
        public  CreatureStatusInstance CreatureStatusInstance;
        public List<CompositeObjectId> TargetIds;
        public int DiceValue;
        public ICreatureCardlocation ICreatureCardlocation;
    }
}

using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーアタックコンテキスト
    /// </summary>
    public class CreatureAttackContext
    {
        public CompositeObjectId AttackerId;
        public int HitsPerAttack;
        public AreaOfEffect AreaOfEffect;
        public EffectTargetType AttackEffectTargetType;
        public int AddAttackPoint;
        /// <summary>
        /// クリーチャーアタックコンテキスト
        /// </summary>
        public CreatureAttackContext(CompositeObjectId attackerId, int hitsPerAttack, AreaOfEffect areaOfEffect, EffectTargetType attackEffectTargetType, int addAttackPoint)
        {
            AttackerId = attackerId;
            HitsPerAttack = hitsPerAttack;
            AreaOfEffect = areaOfEffect;
            AttackEffectTargetType = attackEffectTargetType;
            AddAttackPoint = addAttackPoint;
        }
    }
}

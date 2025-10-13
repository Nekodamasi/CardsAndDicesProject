using UnityEngine;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// バフ／デバフ効果の内容を定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureAttackData", menuName = "CardsAndDices/Combats/Data/Abilities/CreateAttack/CreatureAttackData")]
    public class CreatureAttackData : ScriptableObject
    {
        [Tooltip("攻撃回数")]
        public int HitsPerAttack;

        [Tooltip("攻撃範囲")]
        public AreaOfEffect AreaOfEffect;

        [Tooltip("攻撃力に使用する能力値")]
        public EffectTargetType AttackEffectTargetType;

        [Tooltip("追加攻撃ポイント")]
        public int AddAttackPoint;
    }
}

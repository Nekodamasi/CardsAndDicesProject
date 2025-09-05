using System.Collections.Generic; // Listを使用するため追加
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーの基本データを定義します。
    /// </summary>
    [System.Serializable] // Unityエディタで表示・編集可能にするため
    public class CreatureData
    {
        /// <summary>
        /// クリーチャーを一意に識別するID。
        /// </summary>
        public CreatureIdEntity CreatureId;

        /// <summary>
        /// クリーチャーの攻撃力。
        /// </summary>
        public int Attack;

        /// <summary>
        /// クリーチャーの体力。
        /// </summary>
        public int Health;

        /// <summary>
        /// クリーチャーのシールド値。
        /// </summary>
        public int Shield;

        /// <summary>
        /// クリーチャーのクールダウンダイス数。
        /// </summary>
        public int Cooldown;

        /// <summary>
        /// クリーチャーの特殊な能力値（エネルギーなど）。
        /// </summary>
        public int Energy;

        /// <summary>
        /// クリーチャーが持つ固有能力のリスト。
        /// </summary>
        public List<BaseAbilityDataSO> Abilities; // 仮の型。適切なAbilityData型に置き換える必要があるかもしれません。

        /// <summary>
        /// 標準攻撃で使用する能力値のタイプ。
        /// </summary>
        public EffectTargetType MainAttackScoresType;

        /// <summary>
        /// 標準攻撃の攻撃回数。
        /// </summary>
        public int HitsPerMainAttack;

        /// <summary>
        /// 標準攻撃の効果範囲
        /// </summary>
        public AreaOfEffect MainAttackAoE;

        /// <summary>
        /// CreatureDataの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="creatureId">クリーチャーを一意に識別するID。</param>
        /// <param name="creatureName">クリーチャーの名前。</param>
        /// <param name="attack">攻撃力。</param>
        /// <param name="health">体力。</param>
        /// <param name="shield">シールド値。</param>
        /// <param name="cooldown">クールダウンダイス数。</param>
        /// <param name="energy">特殊な能力値（エネルギーなど）。</param>
        /// <param name="abilities">固有能力のリスト。</param>
        public CreatureData(CreatureIdEntity creatureId, int attack, int health, int shield, int cooldown, int energy, List<BaseAbilityDataSO> abilities, EffectTargetType mainAttackScoresType, int hitsPerMainAttack, AreaOfEffect mainAttackAoE)
        {
            CreatureId = creatureId;
            Attack = attack;
            Health = health;
            Shield = shield;
            Cooldown = cooldown;
            Energy = energy;
            Abilities = abilities;
            MainAttackScoresType = mainAttackScoresType;
            HitsPerMainAttack = hitsPerMainAttack;
            MainAttackAoE = mainAttackAoE;
        }
    }
}

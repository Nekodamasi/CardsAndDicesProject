using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// １回分の攻撃を行う。
    /// </summary>
    public struct PerformAttackCommand : ICommand
    {
        /// <summary>
        /// 攻撃対象
        /// </summary>
        public CompositeObjectId AttackerId { get; }

        /// <summary>
        /// 攻撃範囲
        /// </summary>
        public AreaOfEffect AttackAoE { get; }

        /// <summary>
        /// 攻撃ポイント
        /// </summary>
        public int AttackPoint { get; }

        /// <summary>
        /// 攻撃回数
        /// </summary>
        public int HitsPerAttack { get; }

        /// <summary>
        /// 攻撃後に実行されるコマンド
        /// </summary>
        public ICommand PostAttackCommand { get; }

        public PerformAttackCommand(CompositeObjectId attackerId, AreaOfEffect attackAoE, int attackPoint, int hitsPerAttack, ICommand postAttackCommand = null)
        {
            AttackerId = attackerId;
            AttackAoE = attackAoE;
            AttackPoint = attackPoint;
            HitsPerAttack = hitsPerAttack;
            PostAttackCommand = postAttackCommand;
        }
       public void Execute() { }
        public void Undo() { }
     }
}

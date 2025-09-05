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
        public CompositeObjectId AttackerId;

        /// <summary>
        /// 攻撃範囲
        /// </summary>
        public AreaOfEffect AttackAoE;

        /// <summary>
        /// 攻撃ポイント
        /// </summary>
        public int AttackPoint;
        public PerformAttackCommand(CompositeObjectId attackerId, AreaOfEffect attackAoE, int attackPoint)
        {
            AttackerId = attackerId;
            AttackAoE = attackAoE;
            AttackPoint = attackPoint;
        }
       public void Execute() { }
        public void Undo() { }
     }
}

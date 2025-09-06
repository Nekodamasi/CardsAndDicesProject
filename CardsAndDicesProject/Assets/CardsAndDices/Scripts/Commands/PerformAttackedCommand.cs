using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// １回分の攻撃を行う。
    /// </summary>
    public struct PerformAttackedCommand : ICommand
    {
        /// <summary>
        /// 攻撃対象
        /// </summary>
        public CompositeObjectId AttackerId { get; }

        public PerformAttackedCommand(CompositeObjectId attackerId)
        {
            AttackerId = attackerId;
        }
       public void Execute() { }
        public void Undo() { }
     }
}

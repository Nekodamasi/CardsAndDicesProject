using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// １回分の攻撃を行う。
    /// </summary>
    public struct CooldownZeroAttacksCommand : IEvent
    {
        /// <summary>
        /// 終了後に実行されるコマンド
        /// </summary>
        public IEvent PostCommand { get; }

        public CooldownZeroAttacksCommand(IEvent postCommand = null)
        {
            PostCommand = postCommand;
        }
       public void Execute() { }
        public void Undo() { }
     }
}

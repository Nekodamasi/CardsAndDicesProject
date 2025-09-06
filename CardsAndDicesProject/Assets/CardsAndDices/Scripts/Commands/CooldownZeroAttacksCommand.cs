using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// １回分の攻撃を行う。
    /// </summary>
    public struct CooldownZeroAttacksCommand : ICommand
    {
        /// <summary>
        /// 終了後に実行されるコマンド
        /// </summary>
        public ICommand PostCommand { get; }

        public CooldownZeroAttacksCommand(ICommand postCommand = null)
        {
            PostCommand = postCommand;
        }
       public void Execute() { }
        public void Undo() { }
     }
}

using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// システム起因のカードリフローを実行するコマンド。
    /// ユーザー操作を介さず、カードを指定された位置へ移動させます。
    /// </summary>
    public class SystemDiceReflowCommand : ICommand
    {
        /// <summary>
        /// 移動が必要なダイスのIDと、そのダイスが移動すべき最終的なワールド座標のマップ。
        /// </summary>
        public Dictionary<CompositeObjectId, Vector3> DiceMovements { get; }

        /// <summary>
        /// 全てのリフローアニメーション完了後に発行されるコマンド。不要な場合はnull。
        /// </summary>
        public ICommand NextCommand { get; }

        /// <summary>
        /// SystemDiceReflowCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="diceMovements">移動が必要なダイスのIDと最終座標のマップ。</param>
        /// <param name="nextCommand">リフロー完了後に発行するコマンド。</param>
        public
        
        SystemDiceReflowCommand(Dictionary<CompositeObjectId, Vector3> diceMovements, ICommand nextCommand = null)
        {
            DiceMovements = diceMovements;
            NextCommand = nextCommand;
        }

        public void Execute() { }
        public void Undo() { }
    }
}

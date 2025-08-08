using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// システム起因のカードリフローを実行するコマンド。
    /// ユーザー操作を介さず、カードを指定された位置へ移動させます。
    /// </summary>
    public class SystemReflowCommand : ICommand
    {
        /// <summary>
        /// 移動が必要なカードのIDと、そのカードが移動すべき最終的なワールド座標のマップ。
        /// </summary>
        public Dictionary<CompositeObjectId, Vector3> CardMovements { get; }

        /// <summary>
        /// 全てのリフローアニメーション完了後に発行されるコマンド。不要な場合はnull。
        /// </summary>
        public ICommand NextCommand { get; }

        /// <summary>
        /// SystemReflowCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="cardMovements">移動が必要なカードのIDと最終座標のマップ。</param>
        /// <param name="nextCommand">リフロー完了後に発行するコマンド。</param>
        public
        
        SystemReflowCommand(Dictionary<CompositeObjectId, Vector3> cardMovements, ICommand nextCommand = null)
        {
            CardMovements = cardMovements;
            NextCommand = nextCommand;
        }

        public void Execute() { }
        public void Undo() { }
    }
}

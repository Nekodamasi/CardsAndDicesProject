
using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ドラッグ操作に起因するカードのリフローが完了し、カードが新しい位置へ移動する必要があることを通知するコマンド。
    /// </summary>
    public class DragReflowCompletedCommand : ICommand
    {
        /// <summary>
        /// 移動が必要なカードのIDと、そのカードが移動すべき最終的なワールド座標のマップ。
        /// </summary>
        public Dictionary<CompositeObjectId, Vector3> Movements { get; private set; }

        /// <summary>
        /// DragReflowCompletedCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="cardMovements">移動が必要なカードのIDと、そのカードが移動すべき最終的なワールド座標のマップ。</param>
        public DragReflowCompletedCommand(Dictionary<CompositeObjectId, Vector3> cardMovements)
        {
            Movements = cardMovements;
        }

        public void Execute() { }
        public void Undo() { }
    }
}

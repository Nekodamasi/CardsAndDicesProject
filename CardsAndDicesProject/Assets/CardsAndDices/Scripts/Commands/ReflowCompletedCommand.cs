using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// カードのリフローが完了し、カードが新しい位置へ移動する必要があることを通知するコマンド。
    /// </summary>
    public class ReflowCompletedCommand : ICommand
    {
        /// <summary>
        /// 移動が必要なカードのIDと、そのカードが移動すべき最終的なワールド座標のマップ。
        /// </summary>
        public Dictionary<CompositeObjectId, Vector3> CardMovements { get; private set; }

        /// <summary>
        /// ReflowCompletedCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="cardMovements">移動が必要なカードのIDと、そのカードが移動すべき最終的なワールド座標のマップ。</param>
        public ReflowCompletedCommand(Dictionary<CompositeObjectId, Vector3> cardMovements)
        {
            CardMovements = cardMovements;
        }

        /// <summary>
        /// コマンドを実行します。（通知用のため、具体的なロジックは購読側で処理されます）
        /// </summary>
        public void Execute()
        {
            // 実装なし
        }

        /// <summary>
        /// コマンドを元に戻します。（通知用のため、具体的なロジックは購読側で処理されます）
        /// </summary>
        public void Undo()
        {
            // 実装なし
        }
    }
}

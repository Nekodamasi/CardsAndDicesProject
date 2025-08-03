using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// マウスカーソルがSpriteUI要素上に入った時のコマンド。
    /// </summary>
    public class SpriteHoverCommand : ICommand
    {
        /// <summary>
        /// ホバーされたオブジェクトのCompositeObjectId。
        /// </summary>
        public CompositeObjectId TargetObjectId { get; private set; }

        /// <summary>
        /// SpriteHoverCommandを初期化します。
        /// </summary>
        /// <param name="targetObjectId">ホバーイベントが発生したCompositeObjectId</param>
        public SpriteHoverCommand(CompositeObjectId targetObjectId)
        {
            TargetObjectId = targetObjectId;
        }

        /// <summary>
        /// ホバー効果を実行します。
        /// BaseSpriteViewによって実装される予定の処理です。
        /// </summary>
        public void Execute()
        {
            // BaseSpriteViewで実装
        }

        /// <summary>
        /// ホバー効果を元に戻します。
        /// BaseSpriteViewによって実装される予定の処理です。
        /// </summary>
        public void Undo()
        {
            // BaseSpriteViewで実装
        }
    }
}
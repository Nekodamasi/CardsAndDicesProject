using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// マウスカーソルがSpriteUI要素から離れた時のコマンド。
    /// </summary>
    public class SpriteUnhoverCommand : ICommand
    {
        /// <summary>
        /// アンホバーされたオブジェクトのCompositeObjectId。
        /// </summary>
        public CompositeObjectId TargetObjectId { get; private set; }

        /// <summary>
        /// SpriteUnhoverCommandを初期化します。
        /// </summary>
        /// <param name="targetObjectId">アンホバーイベントが発生したCompositeObjectId。</param>
        public SpriteUnhoverCommand(CompositeObjectId targetObjectId)
        {
            TargetObjectId = targetObjectId;
        }

        /// <summary>
        /// アンホバー効果を実行します。
        /// BaseSpriteViewによって実装される予定の処理です。
        /// </summary>
        public void Execute()
        {
            // BaseSpriteViewで実装
        }

        /// <summary>
        /// アンホバー効果を元に戻します。
        /// BaseSpriteViewによって実装される予定の処理です。
        /// </summary>
        public void Undo()
        {
            // BaseSpriteViewで実装
        }
    }
}
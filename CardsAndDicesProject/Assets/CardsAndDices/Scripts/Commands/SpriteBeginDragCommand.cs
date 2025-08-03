using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// SpriteUI要素のドラッグ操作が開始された時のコマンド。
    /// </summary>
    public class SpriteBeginDragCommand : ICommand
    {
        private readonly CompositeObjectId _targetObjectId;

        /// <summary>
        /// SpriteBeginDragCommandを初期化します。
        /// </summary>
        /// <param name="targetObjectId">ドラッグ開始イベントが発生したCompositeObjectId</param>
        public SpriteBeginDragCommand(CompositeObjectId targetObjectId)
        {
            _targetObjectId = targetObjectId;
        }

        /// <summary>
        /// ドラッグ開始されたGameObjectを取得します。
        /// </summary>
        public CompositeObjectId TargetObjectId => _targetObjectId;

        /// <summary>
        /// ドラッグ開始効果を実行します。
        /// BaseSpriteViewによって実装される予定の処理です。
        /// </summary>
        public void Execute()
        {
            // BaseSpriteViewで実装
        }

        /// <summary>
        /// ドラッグ開始効果を元に戻します。
        /// BaseSpriteViewによって実装される予定の処理です。
        /// </summary>
        public void Undo()
        {
            // BaseSpriteViewで実装
        }
    }
} 
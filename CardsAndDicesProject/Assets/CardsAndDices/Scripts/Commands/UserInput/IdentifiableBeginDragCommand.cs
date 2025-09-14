using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// SpriteUI要素のドラッグ操作が開始された時のコマンド。
    /// </summary>
    public class IdentifiableBeginDragCommand : ICommand
    {
        private readonly CompositeObjectId _executedObjectId;

        /// <summary>
        /// IdentifiableBeginDragCommandを初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableBeginDragCommand(CompositeObjectId executedObjectId)
        {
            _executedObjectId = executedObjectId;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// 効果を実行します。
        /// </summary>
        public void Execute()
        {
            // BaseSpriteViewで実装
        }

        /// <summary>
        /// 効果を元に戻します。
        /// </summary>
        public void Undo()
        {
            // BaseSpriteViewで実装
        }
    }
} 
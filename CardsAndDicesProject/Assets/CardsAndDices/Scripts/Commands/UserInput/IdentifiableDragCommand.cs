using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// SpriteUI要素のドラッグ操作が開始された時のコマンド。
    /// </summary>
    public class IdentifiableDragCommand : ICommand
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly Vector3 _newPosition;

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableDragCommand(CompositeObjectId executedObjectId, Vector3 newPosition)
        {
            _executedObjectId = executedObjectId;
            _newPosition = newPosition;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// 新しいPositionを取得します
        /// </summary>
        public Vector3 NewPosition => _newPosition;

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
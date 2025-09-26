using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ムーブアニメーションを実行するコマンドです
    /// </summary>
    public class SetCurrentHomeStatusEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly IdentifiableStatus _currentHomeStatus;

        /// <summary>
        /// IdentifiableBeginDragCommandを初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public SetCurrentHomeStatusEvent(CompositeObjectId executedObjectId, IdentifiableStatus currentHomeStatus)
        {
            _executedObjectId = executedObjectId;
            _currentHomeStatus = currentHomeStatus;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// 移動先のPositionを取得します
        /// </summary>
        public IdentifiableStatus CurrentHomeStatus => _currentHomeStatus;

        /// => _executedObjectId;

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
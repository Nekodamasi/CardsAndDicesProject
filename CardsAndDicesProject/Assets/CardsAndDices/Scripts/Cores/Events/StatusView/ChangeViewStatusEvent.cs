using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ステータス変更イベント
    /// </summary>
    public class ChangeViewStatusEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly IdentifiableStatus _newStatus;

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public ChangeViewStatusEvent(CompositeObjectId executedObjectId, IdentifiableStatus newStatus)
        {
            _executedObjectId = executedObjectId;
            _newStatus = newStatus;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// 変更先のステータスを取得します。
        /// </summary>
        public IdentifiableStatus NewStatus => _newStatus;

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
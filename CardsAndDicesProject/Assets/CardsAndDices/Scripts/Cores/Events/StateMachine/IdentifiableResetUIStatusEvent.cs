using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リセットステータスイベント
    /// </summary>OnResetUIStatus
    public class IdentifiableResetUIStatusEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableResetUIStatusEvent(CompositeObjectId executedObjectId)
        {
            _executedObjectId = executedObjectId;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;
    }
} 
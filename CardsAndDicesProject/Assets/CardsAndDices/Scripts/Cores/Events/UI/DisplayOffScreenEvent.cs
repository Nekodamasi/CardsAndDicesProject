using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 画面から外すイベント
    /// </summary>
    public class DisplayOffScreenEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public DisplayOffScreenEvent(CompositeObjectId executedObjectId)
        {
            _executedObjectId = executedObjectId;
        }
        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;
    }
} 
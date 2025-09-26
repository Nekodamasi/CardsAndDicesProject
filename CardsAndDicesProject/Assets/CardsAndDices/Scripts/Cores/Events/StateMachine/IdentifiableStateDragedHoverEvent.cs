using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// IdentifiableUIStateMachineが発行するDragedHoverイベント。
    /// </summary>
    public class IdentifiableStateDragedHoverEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly CompositeObjectId _dragedObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableStateDragedHoverEvent(CompositeObjectId executedObjectId, CompositeObjectId dragedObjectId)
        {
            _executedObjectId = executedObjectId;
            _dragedObjectId = dragedObjectId;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// ドラックされたオブジェクトのIDを取得します。
        /// </summary>
        public CompositeObjectId DragedObjectId => _dragedObjectId;
    }
} 
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// IdentifiableUIStateMachineが発行するClickイベント。
    /// </summary>
    public class IdentifiableStateClickEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableStateClickEvent(CompositeObjectId executedObjectId)
        {
            _executedObjectId = executedObjectId;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;
    }
} 
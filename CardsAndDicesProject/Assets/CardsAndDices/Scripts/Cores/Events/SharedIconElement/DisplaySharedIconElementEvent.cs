using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// SharedIconElementを現在の状態に更新します
    /// </summary>
    public class DisplaySharedIconElementEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly SharedIconElementTypeEntity _sharedIconElementTypeEntity;
        private readonly int _numberValue;

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public DisplaySharedIconElementEvent(CompositeObjectId executedObjectId, SharedIconElementTypeEntity sharedIconElementTypeEntity, int numberValue)
        {
            _executedObjectId = executedObjectId;
            _sharedIconElementTypeEntity = sharedIconElementTypeEntity;
            _numberValue = numberValue;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// 対象のSharedIconElementTypeEntity
        /// </summary>
        public SharedIconElementTypeEntity SharedIconElementTypeEntity => _sharedIconElementTypeEntity;

        /// <summary>
        /// 更新後の数値
        /// </summary>
        public int NumberValue => _numberValue;
    }
} 
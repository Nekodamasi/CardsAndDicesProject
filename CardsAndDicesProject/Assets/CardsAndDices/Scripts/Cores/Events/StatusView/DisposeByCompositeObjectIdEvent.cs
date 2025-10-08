using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ホームポジション変更コマンド
    /// </summary>
    public class DisposeByCompositeObjectIdEvent : IEvent
    {
        private readonly CompositeObjectId _compositeObjectId;

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="compositeObjectId">イベントの発生源のCompositeObjectId</param>
        public DisposeByCompositeObjectIdEvent(CompositeObjectId compositeObjectId)
        {
            _compositeObjectId = compositeObjectId;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;
    }
} 
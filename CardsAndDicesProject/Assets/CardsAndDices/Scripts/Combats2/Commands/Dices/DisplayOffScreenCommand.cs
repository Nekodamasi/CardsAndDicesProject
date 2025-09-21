using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスロールコマンド
    /// </summary>
    public class DisplayOffScreenCommand : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public DisplayOffScreenCommand(CompositeObjectId executedObjectId)
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
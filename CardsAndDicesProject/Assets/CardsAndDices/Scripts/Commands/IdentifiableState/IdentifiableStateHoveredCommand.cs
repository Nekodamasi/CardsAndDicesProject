using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// マウスホバーが完了した時のコマンド。
    /// </summary>
    public class IdentifiableStateHoveredCommand : ICommand
    {
        private readonly CompositeObjectId _executedObjectId;

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableStateHoveredCommand(CompositeObjectId executedObjectId)
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
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ドロップ完了時のStatecommand
    /// </summary>
    public class IdentifiableStateDropedCommand : ICommand
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly CompositeObjectId _targetObjectId;

        /// <summary>
        /// SpriteBeginDragCommandを初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントが発生したCompositeObjectId</param>
        /// <param name="targetObjectId">イベントの対象となったCompositeObjectId</param>
        public IdentifiableStateDropedCommand(CompositeObjectId executedObjectId, CompositeObjectId targetObjectId)
        {
            _executedObjectId = executedObjectId;
            _targetObjectId = targetObjectId;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// イベント対象のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId TargetObjectId => _targetObjectId;

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
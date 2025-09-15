using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// SpriteUI要素のドラッグ操作が開始された時のコマンド。
    /// </summary>
    public class IdentifiableStateDragCommand : ICommand
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly Vector3 _targetPosition;

        /// <summary>
        /// 初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public IdentifiableStateDragCommand(CompositeObjectId executedObjectId, Vector3 targetPosition)
        {
            _executedObjectId = executedObjectId;
            _targetPosition = targetPosition;
        }

        /// <summary>
        /// イベントの発生源のCompositeObjectIdを取得します。
        /// </summary>
        public CompositeObjectId ExecutedObjectId => _executedObjectId;

        /// <summary>
        /// 移動先のPositionを取得します
        /// </summary>
        public Vector3 TargetPosition => _targetPosition;

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
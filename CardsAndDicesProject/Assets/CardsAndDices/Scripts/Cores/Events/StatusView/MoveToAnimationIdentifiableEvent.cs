using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ムーブアニメーションを実行するコマンドです
    /// </summary>
    public class MoveToAnimationIdentifiableEvent : IEvent
    {
        private readonly CompositeObjectId _executedObjectId;
        private readonly Vector3 _targetPosition;

        /// <summary>
        /// IdentifiableBeginDragCommandを初期化します。
        /// </summary>
        /// <param name="executedObjectId">イベントの発生源のCompositeObjectId</param>
        public MoveToAnimationIdentifiableEvent(CompositeObjectId executedObjectId, Vector3 targetPosition)
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

        /// => _executedObjectId;

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
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフロー配置へのアニメーション移動移動
    /// </summary>
    public class MoveToAnimationReflowCreatureCardSlotEvent : IEvent
    {
        private readonly CompositeObjectId _cragedCreatureCardId;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="cragedCreatureCardId">ドラッグされたクリーチャーカードID</param>
        public MoveToAnimationReflowCreatureCardSlotEvent(CompositeObjectId cragedCreatureCardId)
        {
            _cragedCreatureCardId = cragedCreatureCardId;
        }

        /// <summary>
        /// ドラッグされたクリーチャーカードIDを取得します
        /// </summary>
        public CompositeObjectId CragedCreatureCardId => _cragedCreatureCardId;
    }
} 
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフロー配置へのアニメーション移動移動
    /// </summary>
    public class MoveToAnimationReflowCreatureCardSlotEvent : IEvent
    {
        private readonly CompositeObjectId _creatureCardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="creatureCardId">対象のダイスID</param>
        public MoveToAnimationReflowCreatureCardSlotEvent(CompositeObjectId creatureCardId)
        {
            _creatureCardId = creatureCardId;
        }

        /// <summary>
        /// ダイススロットポジションを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;
   }
} 
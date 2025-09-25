using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフロークリーチャーカードの配置イベント
    /// </summary>
    public class ReflowPlacedCreatureCardSlotEvent : IEvent
    {
        private readonly CompositeObjectId _creatureSlotCard;
        private readonly CompositeObjectId _creatureCard;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="diceId">対象のダイスID</param>
        public ReflowPlacedCreatureCardSlotEvent(CompositeObjectId creatureSlotCard, CompositeObjectId creatureCard)
        {
            _creatureSlotCard = creatureSlotCard;
            _creatureCard = creatureCard;
        }

        /// <summary>
        /// クリーチャーカードスロットIdを取得します
        /// </summary>
        public CompositeObjectId CreatureCardSlotId => _creatureSlotCard;

        /// <summary>
        /// クリーチャーカードIDを取得します
        /// </summary>
        public CompositeObjectId CreatureCard => _creatureCard;
    }
} 
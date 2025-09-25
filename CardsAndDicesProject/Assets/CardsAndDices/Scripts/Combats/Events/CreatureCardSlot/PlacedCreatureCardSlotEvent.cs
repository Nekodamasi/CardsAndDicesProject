using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスの配置イベント
    /// </summary>
    public class PlacedCreatureCardSlotEvent : IEvent
    {
        private readonly CompositeObjectId _creatureCardSlotId;
        private readonly CompositeObjectId _creatureCardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="diceId">対象のダイスID</param>
        public PlacedCreatureCardSlotEvent(CompositeObjectId creatureCardslotId, CompositeObjectId creatureCardId)
        {
            _creatureCardSlotId = creatureCardslotId;
            _creatureCardId = creatureCardId;
        }

        /// <summary>
        /// クリーチャーカードスロットIDを取得します
        /// </summary>
        public CompositeObjectId CreatureCardSlotId => _creatureCardSlotId;

        /// <summary>
        /// クリーチャーカードIDを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;
    }
} 
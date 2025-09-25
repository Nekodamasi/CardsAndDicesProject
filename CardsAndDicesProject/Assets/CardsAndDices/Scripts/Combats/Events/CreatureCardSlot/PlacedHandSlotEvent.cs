using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 空いてるハンドスロットに配置
    /// </summary>
    public class PlacedHandSlotEvent : IEvent
    {
        private readonly CompositeObjectId _creatureCardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="creatureCardId">配置するクリーチャーカードID</param>
        public PlacedHandSlotEvent(CompositeObjectId creatureCardId)
        {
            _creatureCardId = creatureCardId;
        }

        /// <summary>
        /// クリーチャーカードIDを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;
    }
} 
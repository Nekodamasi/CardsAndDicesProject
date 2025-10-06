using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 空いてるハンドスロットに配置
    /// </summary>
    public class PlacedPpecifiedSlotEvent : IEvent
    {
        private readonly CompositeObjectId _creatureCardId;
        private readonly Team _team;
        private readonly LinePosition _linePosition;
        private readonly SlotLocation _slotLocation;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="creatureCardId">配置するクリーチャーカードID</param>
        public PlacedPpecifiedSlotEvent(CompositeObjectId creatureCardId, Team team, LinePosition linePosition, SlotLocation slotLocation)
        {
            _creatureCardId = creatureCardId;
            _team = team;
            _linePosition = linePosition;
            _slotLocation = slotLocation;
        }

        /// <summary>
        /// クリーチャーカードIDを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;

        /// <summary>
        /// チームを取得します
        /// </summary>
        public Team Team => _team;

        /// <summary>
        /// ラインポジションを取得します
        /// </summary>
        public LinePosition LinePosition => _linePosition;

        /// <summary>
        /// スロットロケーションを取得します
        /// </summary>
        public SlotLocation SlotLocation => _slotLocation;
    }
} 
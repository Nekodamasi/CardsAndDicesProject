using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// カードスロットの状態を保持するデータクラス（Model）。
    /// </summary>
    public class CardSlotData
    {
        /// <summary>
        /// このデータに対応するView（GameObject）のID。
        /// </summary>
        public CompositeObjectId SlotId { get; private set; }

        /// <summary>
        /// スロットのワールド座標
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// スロットが存在するライン。
        /// </summary>
        public LinePosition Line { get; private set; }

        /// <summary>
        /// スロットが所属するチーム。
        /// </summary>
        public Team Team { get; private set; }

        /// <summary>
        /// ライン内でのスロットの役割や位置。
        /// </summary>
        public SlotLocation Location { get; private set; }

        /// <summary>
        /// このスロットにカードが配置されているかどうか。
        /// </summary>
        public bool IsOccupied => PlacedCardId != null;

        /// <summary>
        /// このスロットに配置されているカードのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId PlacedCardId { get; set; }
        /// <summary>
        /// リフロー時に、このスロットに配置されているカードのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId ReflowPlacedCardId { get; set; }

        /// <summary>
        /// CardSlotDataの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="slotId">スロットのID</param>
        /// <param name="position">スロットのワールド座標</param>
        /// <param name="line">スロットのライン</param>
        /// <param name="location">スロットの場所</param>
        public CardSlotData(CompositeObjectId slotId, Vector3 position, LinePosition line, SlotLocation location, Team team)
        {
            SlotId = slotId;
            Position = position;
            Line = line;
            Location = location;
            Team = team;
            PlacedCardId = null;
        }
    }
}
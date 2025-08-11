using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットのデータを保持するクラス。
    /// </summary>
    public class DiceSlotData
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
        /// ダイススロットの役割や位置。
        /// </summary>
        public DiceSlotLocation Location { get; private set; }

        /// <summary>
        /// このスロットに配置されているダイスのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId PlacedDiceId { get; set; }
        /// <summary>
        /// リフロー時に、このスロットに配置されているダイスのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId ReflowPlacedDiceId { get; set; }

        /// <summary>
        /// このスロットにダイスが配置されているかどうか。
        /// </summary>
        public bool IsOccupied => PlacedDiceId != null;

        /// <summary>
        /// CardSlotDataの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="slotId">スロットのID</param>
        /// <param name="position">スロットのワールド座標</param>
        /// <param name="line">スロットのライン</param>
        /// <param name="location">スロットの場所</param>
        public DiceSlotData(CompositeObjectId slotId, Vector3 position, DiceSlotLocation location)
        {
            SlotId = slotId;
            Position = position;
            Location = location;
            PlacedDiceId = null;
            ReflowPlacedDiceId = null;
        }
    }
}

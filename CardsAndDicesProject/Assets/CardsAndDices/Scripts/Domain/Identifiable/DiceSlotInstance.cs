using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットを管理するインスタンス。
    /// </summary>
    public class DiceSlotInstance
    {
        private DiceSlotPositionEntity _diceSlotPositionEntity;
        private IdentifiableCommandBus _identifiableCommandBus;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiceSlotInstance(DiceSlotPositionEntity diceSlotPositionEntity, IdentifiableCommandBus identifiableCommandBus)
        {
            _diceSlotPositionEntity = diceSlotPositionEntity;
            _identifiableCommandBus = identifiableCommandBus;
        }

        public void Dispose()
        {
        }

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
    }
}

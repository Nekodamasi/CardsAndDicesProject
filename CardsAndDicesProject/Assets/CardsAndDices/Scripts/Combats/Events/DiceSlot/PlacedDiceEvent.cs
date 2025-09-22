using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスの配置イベント
    /// </summary>
    public class PlacedDiceEvent : IEvent
    {
        private readonly CompositeObjectId _diceSlotId;
        private readonly CompositeObjectId _diceId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="diceId">対象のダイスID</param>
        public PlacedDiceEvent(CompositeObjectId diceSlotId, CompositeObjectId diceId)
        {
            _diceSlotId = diceSlotId;
            _diceId = diceId;
        }

        /// <summary>
        /// ダイススロットポジションを取得します
        /// </summary>
        public CompositeObjectId DiceSlotId => _diceSlotId;

        /// <summary>
        /// ダイスIDを取得します
        /// </summary>
        public CompositeObjectId DiceId => _diceId;

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
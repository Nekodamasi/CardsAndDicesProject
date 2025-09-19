using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフローダイスの配置コマンド
    /// </summary>
    public class ReflowPlacedDiceCommand : IEvent
    {
        private readonly DiceSlotLocation _diceSlotLocation;
        private readonly CompositeObjectId _diceId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="diceId">対象のダイスID</param>
        public ReflowPlacedDiceCommand(DiceSlotLocation diceSlotLocation, CompositeObjectId diceId)
        {
            _diceSlotLocation = diceSlotLocation;
            _diceId = diceId;
        }

        /// <summary>
        /// ダイススロットポジションを取得します
        /// </summary>
        public DiceSlotLocation DiceSlotLocation => _diceSlotLocation;

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
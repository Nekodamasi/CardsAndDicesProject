using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフロー配置へのアニメーション移動コマンド
    /// </summary>
    public class MoveToAnimationReflowDiceCommand : IEvent
    {
        private readonly DiceSlotLocation _diceSlotLocation;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="diceId">対象のダイスID</param>
        public MoveToAnimationReflowDiceCommand(DiceSlotLocation diceSlotLocation)
        {
            _diceSlotLocation = diceSlotLocation;
        }

        /// <summary>
        /// ダイススロットポジションを取得します
        /// </summary>
        public DiceSlotLocation DiceSlotLocation => _diceSlotLocation;

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
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフロー配置へのアニメーション移動移動
    /// </summary>
    public class MoveToAnimationReflowDiceEvent : IEvent
    {
        private readonly CompositeObjectId _diceSlotId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="diceId">対象のダイスID</param>
        public MoveToAnimationReflowDiceEvent(CompositeObjectId diceSlotId)
        {
            _diceSlotId = diceSlotId;
        }

        /// <summary>
        /// ダイススロットポジションを取得します
        /// </summary>
        public CompositeObjectId DiceSlotId => _diceSlotId;

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
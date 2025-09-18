using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスロールコマンド
    /// </summary>
    public class DiceRollCommand : ICommand
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiceRollCommand()
        {
        }

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
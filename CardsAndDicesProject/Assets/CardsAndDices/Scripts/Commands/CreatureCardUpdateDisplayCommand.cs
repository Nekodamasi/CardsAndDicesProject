using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// カードの値を最新に更新する
    /// </summary>
    public class CreatureCardUpdateDisplayCommand : ICommand
    {
        public CreatureCardUpdateDisplayCommand()
        {
        }

        /// <summary>
        /// コマンドを実行します。（通知用のため、具体的なロジジックは購読側で処理されます）
        /// </summary>
        public void Execute()
        {
            // 実装なし
        }

        /// <summary>
        /// コマンドを元に戻します。（通知用のため、具体的なロジックは購読側で処理されます）
        /// </summary>
        public void Undo()
        {
            // 実装なし
        }
    }
}

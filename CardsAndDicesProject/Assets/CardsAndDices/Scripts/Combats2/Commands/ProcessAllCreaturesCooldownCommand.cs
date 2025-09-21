
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全クリーチャーのクールダウン処理を開始するきっかけとなるイベントを通知します。
    /// </summary>
    public struct ProcessAllCreaturesCooldownCommand : IEvent
    {
        /// <summary>
        /// コマンドを実行します。（通知用のため、具体的なロジジックは購読側で処理されます）
        /// </summary>
        public void Execute()
        {
            // 実装なし
        }

        /// <summary>
        /// コマンドを元に戻します。（通知用のため、具体的なロジジックは購読側で処理されます）
        /// </summary>
        public void Undo()
        {
            // 実装なし
        }
    }
}

using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// ドラッグ操作が完全に終了し、その結果（成功/失敗）に関わらず、
    /// コライダーを有効に戻すなどの後処理を行うためのコマンド。
    /// </summary>
    public class SpriteDragOperationCompletedCommand : ICommand
    {
        /// <summary>
        /// ドラッグ操作が完了したオブジェクトのCompositeObjectId。
        /// </summary>
//        public CompositeObjectId TargetObjectId { get; private set; }

        /// <summary>
        /// ドロップが成功したかどうか。
        /// </summary>
//        public bool IsDropSuccessful { get; private set; }

        /// <summary>
        /// SpriteDragOperationCompletedCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="targetObjectId">ドラッグ操作が完了したオブジェクトのCompositeObjectId。</param>
        /// <param name="isDropSuccessful">ドロップが成功したかどうか。</param>
//        public SpriteDragOperationCompletedCommand(CompositeObjectId targetObjectId, bool isDropSuccessful)
        public SpriteDragOperationCompletedCommand()
        {
//            TargetObjectId = targetObjectId;
//            IsDropSuccessful = isDropSuccessful;
        }

        /// <summary>
        /// コマンドを実行します。（通知用のため、具体的なロジックは購読側で処理されます）
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

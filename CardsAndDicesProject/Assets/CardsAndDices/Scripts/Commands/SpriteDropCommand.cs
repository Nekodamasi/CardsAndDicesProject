namespace CardsAndDice
{
    /// <summary>
    /// SpriteUI要素がスロットに正常に配置されたことを通知するコマンド。
    /// または、ドラッグ操作が終了し、ドロップされたことを通知するコマンド。
    /// </summary>
    public class SpriteDropCommand : ICommand
    {
        /// <summary>
        /// ドロップされたSpriteUI要素のCompositeObjectId。
        /// </summary>
        public CompositeObjectId DroppedObjectId { get; private set; }

        /// <summary>
        /// 要素を受け入れたスロットのCompositeObjectId。
        /// ドロップターゲットがない場合はnull。
        /// </summary>
        public CompositeObjectId TargetSlotObjectId { get; private set; }

        /// <summary>
        /// SpriteDropCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="droppedObjectId">ドロップされたSpriteUI要素のCompositeObjectId。</param>
        /// <param name="targetSlotObjectId">要素を受け入れたスロットのCompositeObjectId。ドロップターゲットがない場合はnull。</param>
        public SpriteDropCommand(CompositeObjectId droppedObjectId, CompositeObjectId targetSlotObjectId = null)
        {
            DroppedObjectId = droppedObjectId;
            TargetSlotObjectId = targetSlotObjectId;
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

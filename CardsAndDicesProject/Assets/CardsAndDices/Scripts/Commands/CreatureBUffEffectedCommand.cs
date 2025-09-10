namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーにバフエフェクトを表示するコマンド。
    /// </summary>
    public class CreatureBUffEffectedCommand : ICommand
    {
        public CompositeObjectId TargetId { get; }
        public VfxDefinition VfxDefinition { get; }
        public CreatureBUffEffectedCommand(CompositeObjectId targetId, VfxDefinition vfxDefinition)
        {
            TargetId = targetId;
            VfxDefinition = vfxDefinition;
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

namespace CardsAndDices
{
    /// <summary>
    /// A command dispatched when a creature is attacked.
    /// </summary>
    public class CreatureAttackedCommand : ICommand
    {
        public CompositeObjectId AttackerId { get; }
        public CompositeObjectId TargetId { get; }

        public CreatureAttackedCommand(CompositeObjectId attackerId, CompositeObjectId targetId)
        {
            AttackerId = attackerId;
            TargetId = targetId;
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

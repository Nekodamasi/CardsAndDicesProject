namespace CardsAndDices
{
    public class CreatureAttackChangedCommand : ICommand
    {
        public CompositeObjectId TargetId { get; }
        public int NewAttack { get; }

        public CreatureAttackChangedCommand(CompositeObjectId targetId, int newAttack)
        {
            TargetId = targetId;
            NewAttack = newAttack;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

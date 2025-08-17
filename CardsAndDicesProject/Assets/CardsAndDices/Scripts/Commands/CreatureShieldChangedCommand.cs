namespace CardsAndDices
{
    public class CreatureShieldChangedCommand : ICommand
    {
        public CompositeObjectId TargetId { get; }
        public int NewShield { get; }
        public int NewMaxShield { get; }

        public CreatureShieldChangedCommand(CompositeObjectId targetId, int newShield, int newMaxShield)
        {
            TargetId = targetId;
            NewShield = newShield;
            NewMaxShield = newMaxShield;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

namespace CardsAndDices
{
    public class CreatureEnergyChangedCommand : ICommand
    {
        public CompositeObjectId TargetId { get; }
        public int NewEnergy { get; }

        public CreatureEnergyChangedCommand(CompositeObjectId targetId, int newEnergy)
        {
            TargetId = targetId;
            NewEnergy = newEnergy;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

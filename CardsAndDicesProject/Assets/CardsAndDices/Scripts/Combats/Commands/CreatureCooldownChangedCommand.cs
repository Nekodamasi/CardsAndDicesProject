namespace CardsAndDices
{
    public class CreatureCooldownChangedCommand : IEvent
    {
        public CompositeObjectId TargetId { get; }
        public int NewCooldown { get; }
        public int NewMaxCooldown { get; }

        public CreatureCooldownChangedCommand(CompositeObjectId targetId, int newCooldown, int newMaxCooldown)
        {
            TargetId = targetId;
            NewCooldown = newCooldown;
            NewMaxCooldown = newMaxCooldown;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

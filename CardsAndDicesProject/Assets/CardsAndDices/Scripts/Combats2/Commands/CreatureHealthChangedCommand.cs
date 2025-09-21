namespace CardsAndDices
{
    public class CreatureHealthChangedCommand : IEvent
    {
        public CompositeObjectId TargetId { get; }
        public int NewHealth { get; }
        public int NewMaxHealth { get; }

        public CreatureHealthChangedCommand(CompositeObjectId targetId, int newHealth, int newMaxHealth)
        {
            TargetId = targetId;
            NewHealth = newHealth;
            NewMaxHealth = newMaxHealth;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

namespace CardsAndDices
{
    public class UpdateEffectExpiredCommand : IEvent
    {
        public TriggerTiming TriggerTiming { get; }

        public UpdateEffectExpiredCommand(TriggerTiming tiggerTiming)
        {
            TriggerTiming = tiggerTiming;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

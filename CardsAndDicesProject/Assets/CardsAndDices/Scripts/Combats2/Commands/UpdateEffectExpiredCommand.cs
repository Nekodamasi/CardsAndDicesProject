namespace CardsAndDices
{
    public class UpdateEffectExpiredCommand : IEvent
    {
        public ActivationTiming TriggerTiming { get; }

        public UpdateEffectExpiredCommand(ActivationTiming tiggerTiming)
        {
            TriggerTiming = tiggerTiming;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

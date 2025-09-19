namespace CardsAndDices
{
    public class InletExecuteAbilityEffectCommand : IEvent
    {
        public TriggerTiming TriggerTiming { get; }
        public CompositeObjectId InletObjectId { get; }

        public InletExecuteAbilityEffectCommand(CompositeObjectId inletObjectId, TriggerTiming triggerTiming)
        {
            InletObjectId = inletObjectId;
            TriggerTiming = triggerTiming;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

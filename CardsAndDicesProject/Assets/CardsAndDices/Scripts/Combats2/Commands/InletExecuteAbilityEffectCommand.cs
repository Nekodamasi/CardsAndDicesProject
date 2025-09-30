namespace CardsAndDices
{
    public class InletExecuteAbilityEffectCommand : IEvent
    {
        public ActivationTiming TriggerTiming { get; }
        public CompositeObjectId InletObjectId { get; }

        public InletExecuteAbilityEffectCommand(CompositeObjectId inletObjectId, ActivationTiming triggerTiming)
        {
            InletObjectId = inletObjectId;
            TriggerTiming = triggerTiming;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

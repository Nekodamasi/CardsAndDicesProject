namespace CardsAndDices
{
    public class ExecuteAbilityEffectCommand : IEvent
    {
        public ActivationTiming TriggerTiming { get; }
        public CompositeObjectId SourceObjectId { get; }
        public CompositeObjectId SubSourceObjectId { get; }

        public ExecuteAbilityEffectCommand(ActivationTiming tiggerTiming, CompositeObjectId sourceObjectId, CompositeObjectId subSourceObjectId)
        {
            TriggerTiming = tiggerTiming;
            SourceObjectId = sourceObjectId;
            SubSourceObjectId = subSourceObjectId;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

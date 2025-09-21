namespace CardsAndDices
{
    public class ExecuteAbilityEffectCommand : IEvent
    {
        public TriggerTiming TriggerTiming { get; }
        public CompositeObjectId SourceObjectId { get; }
        public CompositeObjectId SubSourceObjectId { get; }

        public ExecuteAbilityEffectCommand(TriggerTiming tiggerTiming, CompositeObjectId sourceObjectId, CompositeObjectId subSourceObjectId)
        {
            TriggerTiming = tiggerTiming;
            SourceObjectId = sourceObjectId;
            SubSourceObjectId = subSourceObjectId;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

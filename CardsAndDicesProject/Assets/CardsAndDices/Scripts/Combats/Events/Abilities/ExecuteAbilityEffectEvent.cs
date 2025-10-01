namespace CardsAndDices
{
    /// <summary>
    /// アビリティの実行判定を行うイベント通知
    /// </summary>
    public class ExecuteAbilityEffectEvent : IEvent
    {
        public ActivationTiming TriggerTiming { get; }
        public CompositeObjectId SourceObjectId { get; }
        public CompositeObjectId SubSourceObjectId { get; }

        public ExecuteAbilityEffectEvent(ActivationTiming tiggerTiming, CompositeObjectId sourceObjectId, CompositeObjectId subSourceObjectId)
        {
            TriggerTiming = tiggerTiming;
            SourceObjectId = sourceObjectId;
            SubSourceObjectId = subSourceObjectId;
        }
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// アビリティの実行判定を行うイベント通知
    /// </summary>
    public class UpdateEffectExpiredEvent : IEvent
    {
        public ActivationTiming TriggerTiming { get; }
        public CompositeObjectId SourceObjectId { get; }
        public CompositeObjectId SubSourceObjectId { get; }

        public UpdateEffectExpiredEvent(ActivationTiming tiggerTiming, CompositeObjectId sourceObjectId, CompositeObjectId subSourceObjectId)
        {
            TriggerTiming = tiggerTiming;
            SourceObjectId = sourceObjectId;
            SubSourceObjectId = subSourceObjectId;
        }
    }
}

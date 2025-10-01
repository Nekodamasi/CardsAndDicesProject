namespace CardsAndDices
{
    public class ApplyEffectEvent : IEvent
    {
        public  CompositeObjectId TargetObjectId { get; }
        public EffectTargetType EffectTargetType { get; }
        public int Value { get; }
        public ActivationTiming ExpiredTiming { get; }
        public int RemainingTurns { get; }

        public ApplyEffectEvent(CompositeObjectId targetObjectId, EffectTargetType effectTargetType, int value, ActivationTiming expiredTiming, int remainingTurns)
        {
            TargetObjectId = targetObjectId;
            EffectTargetType = effectTargetType;
            Value = value;
            ExpiredTiming = expiredTiming;
            RemainingTurns = remainingTurns;
        }
    }
}

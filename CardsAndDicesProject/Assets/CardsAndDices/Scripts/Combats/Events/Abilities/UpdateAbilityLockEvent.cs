namespace CardsAndDices
{
    /// <summary>
    /// アビリティの実行判定を行うイベント通知
    /// </summary>
    public class UpdateAbilityLockEvent : IEvent
    {
        public CompositeObjectId SubSourceObjectId { get; }
        public bool IsLock { get; }

        public UpdateAbilityLockEvent(CompositeObjectId subSourceObjectId, bool isLock)
        {
            SubSourceObjectId = subSourceObjectId;
            IsLock = isLock;
        }
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーのアタック演出イベント
    /// </summary>
    public class DisplayCreatureAttackEvent : IEvent
    {
        public CompositeObjectId AttackerId { get; }
        public DisplayCreatureAttackEvent(CompositeObjectId attackerId)
        {
            AttackerId = attackerId;
        }
    }
}

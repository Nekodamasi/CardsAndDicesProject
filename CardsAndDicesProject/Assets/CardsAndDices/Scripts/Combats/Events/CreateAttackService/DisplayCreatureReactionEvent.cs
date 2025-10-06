namespace CardsAndDices
{
    /// <summary>
    /// 攻撃を受けたクリーチャーのリアクション演出を行うイベント
    /// </summary>
    public class DisplayCreatureReactionEvent : IEvent
    {
        public CompositeObjectId TargetId { get; }
        public DisplayCreatureReactionEvent(CompositeObjectId targetId)
        {
            TargetId = targetId;
        }
    }
}

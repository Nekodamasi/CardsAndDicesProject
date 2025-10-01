namespace CardsAndDices
{
    /// <summary>
    /// クリーチャー生成イベント
    /// </summary>
    public class UpdateDisplayCreatureStatusEvent : IEvent
    {
        public CompositeObjectId CreatureCardId { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public UpdateDisplayCreatureStatusEvent(CompositeObjectId creatureCardId)
        {
            CreatureCardId = creatureCardId;
        }
    }
}

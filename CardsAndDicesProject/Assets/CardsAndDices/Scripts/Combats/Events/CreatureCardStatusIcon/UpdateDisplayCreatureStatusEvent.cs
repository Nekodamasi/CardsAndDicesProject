namespace CardsAndDices
{
    /// <summary>
    /// クリーチャー生成イベント
    /// </summary>
    public class UpdateDisplayCreatureStatusEvent : IEvent
    {
        public CompositeObjectId ExecutedObjectId { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public UpdateDisplayCreatureStatusEvent(CompositeObjectId executedObjectId)
        {
            ExecutedObjectId = executedObjectId;
        }
    }
}

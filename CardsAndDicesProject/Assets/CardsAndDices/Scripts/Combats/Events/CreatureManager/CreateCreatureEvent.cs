namespace CardsAndDices
{
    /// <summary>
    /// クリーチャー生成イベント
    /// </summary>
    public class CreateCreatureEvent : IEvent
    {
        public CompositeObjectId CreatureCardId { get; }
        public CardInitializationData CardInitializationData { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public CreateCreatureEvent(CompositeObjectId creatureCardId, CardInitializationData cardInitializationData)
        {
            CreatureCardId = creatureCardId;
            CardInitializationData = cardInitializationData;
        }
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードの最終的なセットアップ
    /// </summary>
    public class CreatureCardSetUpEvent : IEvent
    {
        public CompositeObjectId CreatureCardId { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public CreatureCardSetUpEvent(CompositeObjectId creatureCardId)
        {
            CreatureCardId = creatureCardId;
        }
    }
}

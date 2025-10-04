namespace CardsAndDices
{
    /// <summary>
    /// バフアニメーションの実行イベント
    /// </summary>
    public class DisplayCreatureBuffEvent : IEvent
    {
        public CompositeObjectId _creatureCardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DisplayCreatureBuffEvent(CompositeObjectId creatureCardId)
        {
            _creatureCardId = creatureCardId;
        }

        /// <summary>
        /// クリーチャーカードIdを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;
    }
}

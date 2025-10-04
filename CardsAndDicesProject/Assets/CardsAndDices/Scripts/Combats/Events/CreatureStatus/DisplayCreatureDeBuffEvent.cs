namespace CardsAndDices
{
    /// <summary>
    /// デバフバフアニメーションの実行イベント
    /// </summary>
    public class DisplayCreatureDeBuffEvent : IEvent
    {
        public CompositeObjectId _creatureCardId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DisplayCreatureDeBuffEvent(CompositeObjectId creatureCardId)
        {
            _creatureCardId = creatureCardId;
        }

        /// <summary>
        /// クリーチャーカードIdを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// デバフアニメーションの実行イベント
    /// </summary>
    public class DisplayCreatureDeBuffEvent : IEvent
    {
        public CompositeObjectId _creatureCardId;
        public VfxDefinition _vfxDefinition;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DisplayCreatureDeBuffEvent(CompositeObjectId creatureCardId, VfxDefinition vfxDefinition)
        {
            _creatureCardId = creatureCardId;
            _vfxDefinition = vfxDefinition;
        }

        /// <summary>
        /// クリーチャーカードIdを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;

        /// <summary>
        /// アニメーションで再生させるVFX
        /// </summary>
        public VfxDefinition VfxDefinition => _vfxDefinition;
    }
}

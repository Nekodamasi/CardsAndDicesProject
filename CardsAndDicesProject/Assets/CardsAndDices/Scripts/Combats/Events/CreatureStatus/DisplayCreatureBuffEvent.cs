namespace CardsAndDices
{
    /// <summary>
    /// バフアニメーションの実行イベント
    /// </summary>
    public class DisplayCreatureBuffEvent : IEvent
    {
        public CompositeObjectId _creatureCardId;
        public VfxDefinition _vfxDefinition;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DisplayCreatureBuffEvent(CompositeObjectId creatureCardId, VfxDefinition vfxDefinition)
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

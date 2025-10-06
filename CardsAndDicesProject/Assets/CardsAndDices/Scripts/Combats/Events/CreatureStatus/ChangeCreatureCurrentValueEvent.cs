namespace CardsAndDices
{
    /// <summary>
    /// Statusの現在値を変更するイベント
    /// </summary>
    public class ChangeCreatureCurrentValueEvent : IEvent
    {
        public CompositeObjectId _creatureCardId;
        public EffectTargetType _effectTargetType;
        public int _addValue;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChangeCreatureCurrentValueEvent(CompositeObjectId creatureCardId, EffectTargetType effectTargetType, int addValue)
        {
            _creatureCardId = creatureCardId;
            _effectTargetType = effectTargetType;
            _addValue = addValue;
        }

        /// <summary>
        /// クリーチャーカードIdを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;

        /// <summary>
        /// 変更対象のステータス
        /// </summary>
        public EffectTargetType EffectTargetType => _effectTargetType;

        /// <summary>
        /// 追加のValue
        /// </summary>
        public int AddValue => _addValue;
    }
}

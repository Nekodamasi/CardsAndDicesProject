namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードの最終的なセットアップ
    /// </summary>
    public class CreateAbilityEvent : IEvent
    {
        public CompositeObjectId _creatureCardId;
        private AbilityDataEntity _baseAbilityDataSO;
        private CompositeObjectId _subOwnerId;
        private CreatureStatusInstance _creatureStatusInstance;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreateAbilityEvent(CompositeObjectId creatureCardId, AbilityDataEntity baseAbilityDataSO, CompositeObjectId subOwnerId, CreatureStatusInstance creatureStatusInstance)
        {
            _creatureCardId = creatureCardId;
            _baseAbilityDataSO = baseAbilityDataSO;
            _subOwnerId = subOwnerId;
            _creatureStatusInstance = creatureStatusInstance;
        }

        /// <summary>
        /// クリーチャーカードIdを取得します
        /// </summary>
        public CompositeObjectId CreatureCardId => _creatureCardId;

        /// <summary>
        /// abilityデータを取得します
        /// </summary>
        public AbilityDataEntity BaseAbilityDataSO => _baseAbilityDataSO;

        /// <summary>
        /// サブオーナーIDを取得します
        /// </summary>
        public CompositeObjectId SubOwnerId => _subOwnerId;

        /// <summary>
        /// オーナーのクリーチャーステータスインスタンス
        /// </summary>
        public CreatureStatusInstance CreatureStatusInstance => _creatureStatusInstance;
    }
}

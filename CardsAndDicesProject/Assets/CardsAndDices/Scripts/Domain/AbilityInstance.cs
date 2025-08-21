namespace CardsAndDices
{
    /// <summary>
    /// Represents a runtime instance of an ability attached to a creature.
    /// </summary>
    public class AbilityInstance
    {
        /// <summary>
        /// abilityの所有者のオブジェクトID
        /// </summary>
        public CompositeObjectId OwnerId { get; }

        /// <summary>
        /// アビリティデータ
        /// </summary>
        public BaseAbilityDataSO Data { get; }

        /// <summary>
        /// サブオーナーID
        /// </summary>
        public CompositeObjectId SubOwnerId { get; }

        /// <summary>
        /// 現在クールダウン値
        /// </summary>
        public int CurrentCooldown { get; set; }

        /// <summary>
        /// 残り使用回数
        /// </summary>
        public int RemainingUsages { get; set; }

        /// <summary>
        /// スポーンフラグ
        /// </summary>
        public bool IsSuppressed { get; set; }

        public AbilityInstance(CompositeObjectId ownerId, BaseAbilityDataSO data, CompositeObjectId subOwnerId)
        {
            OwnerId = ownerId;
            Data = data;
            SubOwnerId = subOwnerId;
            IsSuppressed = false;
            data.Duration?.OnReset(this);
        }
    }
}

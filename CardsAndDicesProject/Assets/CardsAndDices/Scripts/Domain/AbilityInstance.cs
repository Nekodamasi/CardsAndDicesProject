namespace CardsAndDices
{
    /// <summary>
    /// Represents a runtime instance of an ability attached to a creature.
    /// </summary>
    public class AbilityInstance
    {
        /// <summary>
        /// Unique identifier for this specific instance of the ability.
        /// </summary>
        public CompositeObjectId Id { get; }

        /// <summary>
        /// The ScriptableObject that defines this ability's behavior.
        /// </summary>
        public BaseAbilityDataSO Data { get; }

        /// <summary>
        /// The creature that owns this ability instance.
        /// </summary>
        public ICreature OwnerCreature { get; }

        /// <summary>
        /// The current cooldown turns remaining.
        /// </summary>
        public int CurrentCooldown { get; set; }

        /// <summary>
        /// The number of times this ability can still be used.
        /// </summary>
        public int RemainingUsages { get; set; }

        /// <summary>
        /// If true, the ability is temporarily suppressed and cannot be triggered.
        /// </summary>
        public bool IsSuppressed { get; set; }

        public AbilityInstance(CompositeObjectId id, BaseAbilityDataSO data, ICreature owner)
        {
            Id = id;
            Data = data;
            OwnerCreature = owner;
            IsSuppressed = false;
            data.Duration?.Reset(this);
        }
    }
}

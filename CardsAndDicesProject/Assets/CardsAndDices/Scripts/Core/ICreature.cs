namespace CardsAndDices
{
    /// <summary>
    /// Represents the runtime instance of a creature in the game.
    /// </summary>
    public interface ICreature
    {
        /// <summary>
        /// Unique identifier for the creature instance.
        /// </summary>
        CompositeObjectId Id { get; }

        /// <summary>
        /// The current health of the creature.
        /// </summary>
        int CurrentHealth { get; }

        /// <summary>
        /// The base maximum health of the creature, including effects.
        /// </summary>
        int BaseHealth { get; }

        /// <summary>
        /// The current attack power of the creature.
        /// </summary>
        int Attack { get; }

        /// <summary>
        /// The current shield value of the creature.
        /// </summary>
        int CurrentShield { get; }

        /// <summary>
        /// The base maximum shield of the creature, including effects.
        /// </summary>
        int BaseShield { get; }

        /// <summary>
        /// The current cooldown count of the creature.
        /// </summary>
        int CurrentCooldown { get; }

        /// <summary>
        /// The base maximum cooldown of the creature, including effects.
        /// </summary>
        int BaseCooldown { get; }

        /// <summary>
        /// The current energy of the creature.
        /// </summary>
        int Energy { get; }

        /// <summary>
        /// Applies a specified amount of damage to the creature.
        /// </summary>
        /// <param name="amount">The amount of damage to take.</param>
        void TakeDamage(int amount);

        /// <summary>
        /// Applies an effect to the creature.
        /// </summary>
        /// <param name="effect">The effect instance to apply.</param>
        void ApplyEffect(EffectInstance effect);

        /// <summary>
        /// Removes an effect from the creature.
        /// </summary>
        /// <param name="effect">The effect instance to remove.</param>
        void RemoveEffect(EffectInstance effect);
    }
}

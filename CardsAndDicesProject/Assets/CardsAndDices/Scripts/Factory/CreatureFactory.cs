namespace CardsAndDices
{
    /// <summary>
    /// Factory for creating ICreature instances.
    /// </summary>
    public class CreatureFactory
    {
        private readonly EffectManager _effectManager;
        private readonly SpriteCommandBus _commandBus;

        public CreatureFactory(EffectManager effectManager, SpriteCommandBus commandBus)
        {
            _effectManager = effectManager;
            _commandBus = commandBus;
        }

        /// <summary>
        /// Creates a new creature instance.
        /// </summary>
        /// <param name="id">The composite object ID for the new creature.</param>
        /// <param name="baseData">The base data for the creature.</param>
        /// <returns>A new ICreature instance.</returns>
        public ICreature Create(CompositeObjectId id, CreatureData baseData)
        {
            return new Creature(id, baseData, _effectManager, _commandBus);
        }
    }
}

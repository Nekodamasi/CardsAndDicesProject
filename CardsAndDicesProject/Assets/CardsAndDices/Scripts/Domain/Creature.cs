namespace CardsAndDices
{
    /// <summary>
    /// Concrete implementation of the ICreature interface.
    /// </summary>
    public class Creature : ICreature
    {
        public CompositeObjectId Id { get; }
        public int CurrentHealth { get; private set; }
        public int BaseHealth => _data.Health + _effectManager.GetTotalEffectValue(Id, EffectTargetType.Health);
        public int Attack => _data.Attack + _effectManager.GetTotalEffectValue(Id, EffectTargetType.Attack);
        public int CurrentShield { get; private set; }
        public int BaseShield => _data.Shield + _effectManager.GetTotalEffectValue(Id, EffectTargetType.Shield);
        public int CurrentCooldown { get; private set; }
        public int BaseCooldown => _data.Cooldown + _effectManager.GetTotalEffectValue(Id, EffectTargetType.Cooldown);
        public int Energy => _data.Energy + _effectManager.GetTotalEffectValue(Id, EffectTargetType.Energy);

        private readonly CreatureData _data;
        private readonly EffectManager _effectManager;
        private readonly SpriteCommandBus _commandBus;

        public Creature(CompositeObjectId id, CreatureData data, EffectManager effectManager, SpriteCommandBus commandBus)
        {
            Id = id;
            _data = data;
            _effectManager = effectManager;
            _commandBus = commandBus;

            CurrentHealth = data.Health;
            CurrentShield = data.Shield;
            CurrentCooldown = data.Cooldown;
        }

        public void TakeDamage(int amount)
        {
            int remainingDamage = amount;

            // Shield absorbs damage first
            if (CurrentShield > 0)
            {
                int shieldDamage = System.Math.Min(remainingDamage, CurrentShield);
                CurrentShield -= shieldDamage;
                remainingDamage -= shieldDamage;
                _commandBus.Emit(new CreatureShieldChangedCommand(Id, CurrentShield, BaseShield));
            }

            // Remaining damage affects health
            if (remainingDamage > 0)
            {
                CurrentHealth -= remainingDamage;
                _commandBus.Emit(new CreatureHealthChangedCommand(Id, CurrentHealth, BaseHealth));
            }
        }

        public void ApplyEffect(EffectInstance effect)
        {
            _effectManager.RegisterEffect(effect);
            RecalculateStats();
        }

        public void RemoveEffect(EffectInstance effect)
        {
            _effectManager.RemoveEffect(effect);
            RecalculateStats();
        }

        private void RecalculateStats()
        {
            // Clamp current values to new base values if necessary
            CurrentHealth = System.Math.Min(CurrentHealth, BaseHealth);
            CurrentShield = System.Math.Min(CurrentShield, BaseShield);
            CurrentCooldown = System.Math.Min(CurrentCooldown, BaseCooldown);

            // Fire events for all stats that might have changed
            _commandBus.Emit(new CreatureHealthChangedCommand(Id, CurrentHealth, BaseHealth));
            _commandBus.Emit(new CreatureShieldChangedCommand(Id, CurrentShield, BaseShield));
            _commandBus.Emit(new CreatureCooldownChangedCommand(Id, CurrentCooldown, BaseCooldown));
            _commandBus.Emit(new CreatureAttackChangedCommand(Id, Attack));
            _commandBus.Emit(new CreatureEnergyChangedCommand(Id, Energy));
        }
    }
}

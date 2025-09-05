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
        public SlotLocation Location { get { return _cardSlotManager.GetSlotDataByReflowPlacedCardId(Id).Location; } }
        public int CurrentHitsPerMainAttack { get; private set; }
        public int MainAttack
        {
            get
            {
                int value = 0;
                if (EffectTargetType.Attack == _data.MainAttackScoresType)
                {
                    value = Attack;
                }
                else if (EffectTargetType.Cooldown == _data.MainAttackScoresType)
                {
                    value = CurrentCooldown;
                }
                else if (EffectTargetType.Health == _data.MainAttackScoresType)
                {
                    value = CurrentHealth;
                }
                else if (EffectTargetType.Energy == _data.MainAttackScoresType)
                {
                    value = Energy;
                }
                else if (EffectTargetType.Shield == _data.MainAttackScoresType)
                {
                    value = CurrentShield;
                }
                return value;
            }
        }
        public AreaOfEffect MainAttackAoE => _data.MainAttackAoE;
        private readonly CreatureData _data;
        private readonly EffectManager _effectManager;
        private readonly SpriteCommandBus _commandBus;
        private readonly CardSlotManager _cardSlotManager;
        public bool IsCooldownFinished { get; private set; }
        public bool IsDamage{ get; private set; }
        public bool IsDeath{ get; private set; }

        public Creature(CompositeObjectId id, CreatureData data, EffectManager effectManager, SpriteCommandBus commandBus, CardSlotManager cardSlotManager)
        {
            Id = id;
            _data = data;
            _effectManager = effectManager;
            _commandBus = commandBus;
            _cardSlotManager = cardSlotManager;

            CurrentHealth = data.Health;
            CurrentShield = data.Shield;
            CurrentCooldown = data.Cooldown;
            CurrentHitsPerMainAttack = data.HitsPerMainAttack;
            IsCooldownFinished = false;
            IsDamage = false;
            IsDeath = false;

            _commandBus.On<CreatureHealthChangedCommand>(OnHealthChanged);
            _commandBus.On<CreatureShieldChangedCommand>(OnShieldChanged);
            _commandBus.On<CreatureCooldownChangedCommand>(OnCooldownChanged);
            _commandBus.On<CreatureEnergyChangedCommand>(OnEnergyChanged);
        }
        public void Dispose()
        {
            // Unsubscribe from events to prevent memory leaks
            _commandBus.Off<CreatureHealthChangedCommand>(OnHealthChanged);
            _commandBus.Off<CreatureShieldChangedCommand>(OnShieldChanged);
            _commandBus.Off<CreatureCooldownChangedCommand>(OnCooldownChanged);
            _commandBus.Off<CreatureEnergyChangedCommand>(OnEnergyChanged);
        }

        private void OnHealthChanged(CreatureHealthChangedCommand cmd)
        {
            if (cmd.TargetId == Id)
            {
//                _view.UpdateHealth(cmd.NewHealth, cmd.NewMaxHealth);
            }
        }

        private void OnShieldChanged(CreatureShieldChangedCommand cmd)
        {
            if (cmd.TargetId == Id)
            {
//                _view.UpdateShield(cmd.NewShield, cmd.NewMaxShield);
            }
        }

        private void OnCooldownChanged(CreatureCooldownChangedCommand cmd)
        {
            if (cmd.TargetId == Id)
            {
                CurrentCooldown = cmd.NewCooldown;
            }
        }

        private void OnEnergyChanged(CreatureEnergyChangedCommand cmd)
        {
            if (cmd.TargetId == Id)
            {
//                _view.UpdateEnergy(cmd.NewEnergy);
            }
        }
        public void PlayMainAttack()
        {
            CurrentHitsPerMainAttack--;
            if (CurrentHitsPerMainAttack == 0)
            {
                IsCooldownFinished = true;
                CurrentHitsPerMainAttack = _data.HitsPerMainAttack;
            }
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
            }

            // Remaining damage affects health
            if (remainingDamage > 0)
            {
                CurrentHealth -= remainingDamage;
            }
            if (CurrentHealth <= 0)
            {
                IsDeath = true;
            }
            else
            {
                IsDamage = true;
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

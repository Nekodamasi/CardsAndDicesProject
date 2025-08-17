using System;

namespace CardsAndDices
{
    /// <summary>
    /// Connects a Creature (Model) to a CreatureCardView (View).
    /// </summary>
    public class CreatureCardPresenter : IDisposable
    {
        private readonly ICreature _creature;
        private readonly CreatureCardView _view;
        private readonly SpriteCommandBus _commandBus;

        public CreatureCardPresenter(ICreature creature, CreatureCardView view, SpriteCommandBus commandBus)
        {
            _creature = creature;
            _view = view;
            _commandBus = commandBus;

            // Initial view setup
/*
            _view.UpdateHealth(_creature.CurrentHealth, _creature.BaseHealth);
            _view.UpdateShield(_creature.CurrentShield, _creature.BaseShield);
            _view.UpdateCooldown(_creature.CurrentCooldown, _creature.BaseCooldown);
            _view.UpdateAttack(_creature.Attack);
            _view.UpdateEnergy(_creature.Energy);
*/

            // Subscribe to events
            _commandBus.On<CreatureHealthChangedCommand>(OnHealthChanged);
            _commandBus.On<CreatureShieldChangedCommand>(OnShieldChanged);
            _commandBus.On<CreatureCooldownChangedCommand>(OnCooldownChanged);
            _commandBus.On<CreatureAttackChangedCommand>(OnAttackChanged);
            _commandBus.On<CreatureEnergyChangedCommand>(OnEnergyChanged);
        }

        private void OnHealthChanged(CreatureHealthChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id)
            {
//                _view.UpdateHealth(cmd.NewHealth, cmd.NewMaxHealth);
            }
        }

        private void OnShieldChanged(CreatureShieldChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id)
            {
//                _view.UpdateShield(cmd.NewShield, cmd.NewMaxShield);
            }
        }

        private void OnCooldownChanged(CreatureCooldownChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id)
            {
//                _view.UpdateCooldown(cmd.NewCooldown, cmd.NewMaxCooldown);
            }
        }

        private void OnAttackChanged(CreatureAttackChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id)
            {
//                _view.UpdateAttack(cmd.NewAttack);
            }
        }

        private void OnEnergyChanged(CreatureEnergyChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id)
            {
//                _view.UpdateEnergy(cmd.NewEnergy);
            }
        }

        public void Dispose()
        {
            // Unsubscribe from events to prevent memory leaks
            _commandBus.Off<CreatureHealthChangedCommand>(OnHealthChanged);
            _commandBus.Off<CreatureShieldChangedCommand>(OnShieldChanged);
            _commandBus.Off<CreatureCooldownChangedCommand>(OnCooldownChanged);
            _commandBus.Off<CreatureAttackChangedCommand>(OnAttackChanged);
            _commandBus.Off<CreatureEnergyChangedCommand>(OnEnergyChanged);
        }
    }
}

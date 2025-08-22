using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Connects a creature's status (Model) to a StatusIconView (View),
    /// controlling the timing of UI updates.
    /// </summary>
    public class StatusIconPresenter : IDisposable
    {
        private readonly ICreature _creature;
        private readonly StatusIconView _view;
        private readonly SpriteCommandBus _commandBus;

        private int _currentValue;

        public StatusIconPresenter(ICreature creature, StatusIconView view, SpriteCommandBus commandBus)
        {
            _creature = creature;
            _view = view;
            _commandBus = commandBus;

            SubscribeToEvents();
            ForceUpdateDisplay(); // Initial display update
        }

        private void OnCreatureCardUpdateDisplay(CreatureCardUpdateDisplayCommand cmd)
        {
            ForceUpdateDisplay();
        }

        private void SubscribeToEvents()
        {
            _commandBus.On<CreatureCardUpdateDisplayCommand>(OnCreatureCardUpdateDisplay);

            switch (_view.StatusIconData.TargetType)
            {
                case EffectTargetType.Health:
                    _commandBus.On<CreatureHealthChangedCommand>(HandleHealthChange);
                    break;
                case EffectTargetType.Attack:
                    _commandBus.On<CreatureAttackChangedCommand>(HandleAttackChange);
                    break;
                case EffectTargetType.Shield:
                    _commandBus.On<CreatureShieldChangedCommand>(HandleShieldChange);
                    break;
                case EffectTargetType.Cooldown:
                    _commandBus.On<CreatureCooldownChangedCommand>(HandleCooldownChange);
                    break;
                case EffectTargetType.Energy:
                    _commandBus.On<CreatureEnergyChangedCommand>(HandleEnergyChange);
                    break;
            }
        }

        private void UnsubscribeFromEvents()
        {
            switch (_view.StatusIconData.TargetType)
            {
                case EffectTargetType.Health:
                    _commandBus.Off<CreatureHealthChangedCommand>(HandleHealthChange);
                    break;
                case EffectTargetType.Attack:
                    _commandBus.Off<CreatureAttackChangedCommand>(HandleAttackChange);
                    break;
                case EffectTargetType.Shield:
                    _commandBus.Off<CreatureShieldChangedCommand>(HandleShieldChange);
                    break;
                case EffectTargetType.Cooldown:
                    _commandBus.Off<CreatureCooldownChangedCommand>(HandleCooldownChange);
                    break;
                case EffectTargetType.Energy:
                    _commandBus.Off<CreatureEnergyChangedCommand>(HandleEnergyChange);
                    break;
            }
        }

        // --- Event Handlers ---
        private void HandleHealthChange(CreatureHealthChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id) _currentValue = cmd.NewHealth;
        }
        private void HandleAttackChange(CreatureAttackChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id) _currentValue = cmd.NewAttack;
        }
        private void HandleShieldChange(CreatureShieldChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id) _currentValue = cmd.NewShield;
        }
        private void HandleCooldownChange(CreatureCooldownChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id) _currentValue = cmd.NewCooldown;
        }
        private void HandleEnergyChange(CreatureEnergyChangedCommand cmd)
        {
            if (cmd.TargetId == _creature.Id) _currentValue = cmd.NewEnergy;
        }


        /// <summary>
        /// Forces the view to update its display with the latest stored value.
        /// This method should be called by an external controller at the desired update time.
        /// </summary>
        public void ForceUpdateDisplay()
        {
            // Re-fetch the value directly from the model to ensure it's the absolute latest
            switch (_view.StatusIconData.TargetType)
            {
                case EffectTargetType.Health:
                    _currentValue = _creature.CurrentHealth;
                    break;
                case EffectTargetType.Attack:
                    _currentValue = _creature.Attack;
                    break;
                case EffectTargetType.Shield:
                    _currentValue = _creature.CurrentShield;
                    break;
                case EffectTargetType.Cooldown:
                    _currentValue = _creature.CurrentCooldown;
                    break;
                case EffectTargetType.Energy:
                    _currentValue = _creature.Energy;
                    break;
            }
            _view.UpdateDisplay(_currentValue);
        }

        public void Dispose()
        {
            UnsubscribeFromEvents();
        }
    }
}

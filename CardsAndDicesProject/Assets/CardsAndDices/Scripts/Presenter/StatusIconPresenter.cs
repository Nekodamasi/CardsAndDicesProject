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

        private void OnAllCreatureCardUpdateDisplay(AllCreatureCardUpdateDisplayCommand cmd)
        {
            ForceUpdateDisplay();
        }
        private void OnCreatureCardUpdateDisplay(CreatureCardUpdateDisplayCommand cmd)
        {
            if (cmd.UpdateId != _creature.Id) return;
            ForceUpdateDisplay();
        }

        private void SubscribeToEvents()
        {
            _commandBus.On<AllCreatureCardUpdateDisplayCommand>(OnAllCreatureCardUpdateDisplay);
            _commandBus.On<CreatureCardUpdateDisplayCommand>(OnCreatureCardUpdateDisplay);
        }

        private void UnsubscribeFromEvents()
        {
            _commandBus.On<AllCreatureCardUpdateDisplayCommand>(OnAllCreatureCardUpdateDisplay);
            _commandBus.On<CreatureCardUpdateDisplayCommand>(OnCreatureCardUpdateDisplay);
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

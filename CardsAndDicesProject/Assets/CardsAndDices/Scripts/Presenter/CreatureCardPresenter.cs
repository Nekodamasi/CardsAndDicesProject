using System;
using System.Collections.Generic;
using UnityEngine;

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
        private readonly List<StatusIconPresenter> _statusIconPresenters = new List<StatusIconPresenter>();

        public CreatureCardPresenter(ICreature creature, CreatureCardView view, SpriteCommandBus commandBus)
        {
            _creature = creature;
            _view = view;
            _commandBus = commandBus;

            List<StatusIconView> statusIconViews = _view.GetStatusIconViews();
            foreach (StatusIconView statusIconView in statusIconViews)
            {
                var presenter = new StatusIconPresenter(_creature, statusIconView, _commandBus);
                _statusIconPresenters.Add(presenter);
            }
            // Initial view setup
            /*
                        _view.UpdateHealth(_creature.CurrentHealth, _creature.BaseHealth);
                        _view.UpdateShield(_creature.CurrentShield, _creature.BaseShield);
                        _view.UpdateCooldown(_creature.CurrentCooldown, _creature.BaseCooldown);
                        _view.UpdateAttack(_creature.Attack);
                        _view.UpdateEnergy(_creature.Energy);
            */

            // Subscribe to events
            _commandBus.On<CreatureBUffEffectedCommand>(OnBUffEffected);
            _commandBus.On<CreatureAttackedCommand>(OnAttacked);
            _commandBus.On<CreatureDamagedCommand>(OnDamaged);
            _commandBus.On<CreatureCooldownChangedCommand>(OnCooldownChanged);
            _commandBus.On<CreatureAttackChangedCommand>(OnAttackChanged);
            _commandBus.On<CreatureEnergyChangedCommand>(OnEnergyChanged);
        }

        private void OnBUffEffected(CreatureBUffEffectedCommand cmd)
        {
            if (cmd.TargetId != _creature.Id) return;
            Debug.Log("<color=Blue>ばふあにめーーーーーーしょーーーん</color>：");
            _view.PlayBuffAnimation(cmd.VfxDefinition);
        }
        private void OnDamaged(CreatureDamagedCommand cmd)
        {
            if (cmd.TargetId != _creature.Id) return;
            if (_creature.IsDeath)
            {
                _view.PlayDeathAnimation();
            }
            else
            {
                _view.PlayDamageAnimation();
            }
        }
        private void OnAttacked(CreatureAttackedCommand cmd)
        {
            if (cmd.AttackerId != _creature.Id) return;
            _view.PlayBodySlamAnimation();
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
            _commandBus.Off<CreatureAttackedCommand>(OnAttacked);
            _commandBus.Off<CreatureDamagedCommand>(OnDamaged);
            _commandBus.Off<CreatureCooldownChangedCommand>(OnCooldownChanged);
            _commandBus.Off<CreatureAttackChangedCommand>(OnAttackChanged);
            _commandBus.Off<CreatureEnergyChangedCommand>(OnEnergyChanged);
        }
    }
}

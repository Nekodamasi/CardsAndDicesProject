using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードを管理するインスタンス。
    /// </summary>
    public class CreatureStatusInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private IEffectValue _iEffectValue;
        private CreatureData _CreatureData;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureStatusInstance(CompositeObjectId compositeObjectId, CreatureData data, IEffectValue iEffectValue)
        {
            _compositeObjectId = compositeObjectId;
            _CreatureData = data;
            _iEffectValue = iEffectValue;

            CurrentHealth = data.Health;
            CurrentShield = data.Shield;
            CurrentCooldown = data.Cooldown;
            IsCooldownFinished = false;
            IsDamage = false;
            IsDeath = false;
        }

        /// <summary>
        /// クリーチャーステータスを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        // ステータス
        public int CurrentHealth { get; private set; }
        public int BaseHealth => _CreatureData.Health + _iEffectValue.GetTotalEffectValue(_compositeObjectId, EffectTargetType.Health);
        public int Attack => _CreatureData.Attack + _iEffectValue.GetTotalEffectValue(_compositeObjectId, EffectTargetType.Attack);
        public int CurrentShield { get; private set; }
        public int BaseShield => _CreatureData.Shield + _iEffectValue.GetTotalEffectValue(_compositeObjectId, EffectTargetType.Shield);
        public int CurrentCooldown { get; private set; }
        public int BaseCooldown => _CreatureData.Cooldown + _iEffectValue.GetTotalEffectValue(_compositeObjectId, EffectTargetType.Cooldown);
        public int Energy => _CreatureData.Energy + _iEffectValue.GetTotalEffectValue(_compositeObjectId, EffectTargetType.Energy);
        public int HitsPerMainAttack => _CreatureData.HitsPerMainAttack;
        public int MainAttack
        {
            get
            {
                int value = 0;
                if (EffectTargetType.Attack == _CreatureData.MainAttackScoresType)
                {
                    value = Attack;
                }
                else if (EffectTargetType.Cooldown == _CreatureData.MainAttackScoresType)
                {
                    value = CurrentCooldown;
                }
                else if (EffectTargetType.Health == _CreatureData.MainAttackScoresType)
                {
                    value = CurrentHealth;
                }
                else if (EffectTargetType.Energy == _CreatureData.MainAttackScoresType)
                {
                    value = Energy;
                }
                else if (EffectTargetType.Shield == _CreatureData.MainAttackScoresType)
                {
                    value = CurrentShield;
                }
                return value;
            }
        }
        public AreaOfEffect MainAttackAoE => _CreatureData.MainAttackAoE;
        public bool IsCooldownFinished { get; private set; }
        public bool IsDamage{ get; private set; }
        public bool IsDeath{ get; private set; }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
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

        private void OnCooldownFinished()
        {
            IsCooldownFinished = true;
        }

        private void RecalculateStats()
        {
            // Clamp current values to new base values if necessary
            CurrentHealth = System.Math.Min(CurrentHealth, BaseHealth);
            CurrentShield = System.Math.Min(CurrentShield, BaseShield);
            CurrentCooldown = System.Math.Min(CurrentCooldown, BaseCooldown);
        }
    }
}

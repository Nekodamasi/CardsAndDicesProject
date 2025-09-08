namespace CardsAndDices
{
    /// <summary>
    /// Represents a runtime instance of an ability attached to a creature.
    /// </summary>
    public class AbilityInstance
    {
        /// <summary>
        /// abilityの所有者のオブジェクトID
        /// </summary>
        public CompositeObjectId OwnerId { get; }

        /// <summary>
        /// アビリティデータ
        /// </summary>
        public BaseAbilityDataSO Data { get; }

        /// <summary>
        /// サブオーナーID
        /// </summary>
        public CompositeObjectId SubOwnerId { get; }

        /// <summary>
        /// 現在クールダウン値
        /// </summary>
        public int CurrentCooldown { get; set; }

        /// <summary>
        /// 残り使用回数
        /// </summary>
        public int RemainingUsages { get; set; }

        /// <summary>
        /// スポーンフラグ
        /// </summary>
        public bool IsSuppressed { get; set; }

        /// <summary>
        /// アビリティの実行
        /// </summary>
        public bool ExecuteAbility(CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager, EffectManager effectManager, SpriteCommandBus spriteCommandBus, TriggerTiming triggerTiming)
        {
            // スポーンフラグOFF
            if (!IsSuppressed) return false;

            // 使用回数が足りない
            if (RemainingUsages <= 0) return false;

            // 発動条件のチェック
            if (!Data.TriggerCondition.Check(OwnerId, triggerTiming, creatureManager, diceManager, abilityManager)) return false;

            // abilityの実行
            var abilityContext = new BaseAbilityEffectDefinitionSO.AbilityContext();
            abilityContext.SourceId = OwnerId;
            abilityContext.TargetIds = Data.TargetSelector.SelectTarget(OwnerId, creatureManager, diceManager);
            Data.EffectDefinition.Execute(abilityContext, spriteCommandBus, creatureManager, diceManager, abilityManager, effectManager);
            Data.Duration.OnUse(this);
            return true;
        }
        public AbilityInstance(CompositeObjectId ownerId, BaseAbilityDataSO data, CompositeObjectId subOwnerId)
        {
            OwnerId = ownerId;
            Data = data;
            SubOwnerId = subOwnerId;
            IsSuppressed = true;
            data.Duration?.OnReset(this);
        }
    }
}

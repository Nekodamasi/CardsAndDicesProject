using System;
using Unity.VisualScripting;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// abilityのインスタンスクラス
    /// </summary>
    public class AbilityInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private BaseAbilityDataSO _baseAbilityDataSO;
        private CompositeObjectId _subOwnerId;
        private CreatureStatusInstance _creatureStatusInstance;
        private int _remainingUsages;
        private bool _isLock;
        private readonly AbilityContext _abilityContext = new();
        private GameEventBus _gameEventBus;
        private ICreatureCardlocation _iCreatureCardlocation;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AbilityInstance(CompositeObjectId ownerId, BaseAbilityDataSO baseAbilityDataSO, CompositeObjectId subOwnerId, CreatureStatusInstance creatureStatusInstance, GameEventBus gameEventBus, ICreatureCardlocation iCreatureCardlocation)
        {
            _compositeObjectId = ownerId;
            _baseAbilityDataSO = baseAbilityDataSO;
            _subOwnerId = subOwnerId;
            _creatureStatusInstance = creatureStatusInstance;
            _iCreatureCardlocation = iCreatureCardlocation;
            _gameEventBus = gameEventBus;

            _abilityContext.CreatureStatusInstance = _creatureStatusInstance;
            _abilityContext.ICreatureCardlocation = _iCreatureCardlocation;
        }
        public void Dispose()
        {
        }

        /// <summary>
        /// abilityの所有者を一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// サブオーナーを一意に識別するID。
        /// </summary>
        public CompositeObjectId SubOwnerId => _subOwnerId;

        /// <summary>
        /// 所有者のステータスインスタンス。
        /// </summary>
        public CreatureStatusInstance CreatureStatusInstance => _creatureStatusInstance;

        /// <summary>
        /// 残り使用回数
        /// </summary>
        public int RemainingUsages => _remainingUsages;

        /// <summary>
        /// 残り使用回数
        /// </summary>
        public void SetRemainingUsages(int value)
        {
            _remainingUsages = value;
        }

        /// <summary>
        /// 残り使用回数
        /// </summary>
        public bool IsLock => _isLock;

        /// <summary>
        /// 残り使用回数
        /// </summary>
        public void SetLock(bool flg)
        {
            _isLock = flg;
        }

        /// <summary>
        /// 現在使用可能か取得します
        /// </summary>
        public bool IsAvailable => _baseAbilityDataSO.Duration.OnCheck(this);

        /// <summary>
        /// 使用回数のリセットタイミング
        /// </summary>
        public ActivationTiming ResetTiming => _baseAbilityDataSO.Duration.ResetTiming;

        /// <summary>
        /// アクティブタイミング
        /// </summary>
        public ActivationTiming ActivationTiming => _baseAbilityDataSO.TriggerCondition.ActivationTiming;

        /// <summary>
        /// Triggerの条件を満たしているか取得します
        /// </summary>
        public bool IsTrigger => _baseAbilityDataSO.TriggerCondition.CheckCondition(_abilityContext);

        /// <summary>
        /// アビリティの効果を実行します。実行に成功したかを返します。
        /// </summary>
        public bool Execute()
        {
            if (!IsTrigger) return false;
            if (!IsAvailable) return false;
            _baseAbilityDataSO.EffectDefinition.Execute(_abilityContext, _gameEventBus);
            return true;
        }
    }
}

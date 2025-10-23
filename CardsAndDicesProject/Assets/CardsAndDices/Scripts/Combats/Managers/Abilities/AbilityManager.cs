using System.Collections.Generic;
using System;
using UnityEngine;
using VContainer;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// ゲーム内のすべてのアクティブな AbilityInstances を管理します
    /// </summary>
    [CreateAssetMenu(fileName = "AbilityManager", menuName = "CardsAndDices/Combats/Managers/Abilities/AbilityManager")]
    public class AbilityManager : ScriptableObject, IDisposable, IIdentifiableManager, IAbilityCheck
    {
        private GameEventBus _eventBus;
        private ICreatureCardlocation _iCreatureCardlocation;
        private ITargetManager _iTargetManager;
        private readonly List<AbilityInstance> _instances = new();
        private readonly List<AbilityController> _controllers = new();

        [Inject]
        public void Initialize(GameEventBus eventBus, ICreatureCardlocation iCreatureCardlocation, ITargetManager iTargetManager)
        {
            _instances.Clear();
            _controllers.Clear();
            _eventBus = eventBus;
            _eventBus.On<CreateAbilityEvent>(OnCreateAbility);
            _eventBus.On<ExecuteAbilityEffectEvent>(OnExecuteAbilityEffect);
            _eventBus.On<UpdateAbilityLockEvent>(OnUpdateAbilityLock);
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);

            _iCreatureCardlocation = iCreatureCardlocation;
            _iTargetManager = iTargetManager;
        }
        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            _eventBus.Off<CreateAbilityEvent>(OnCreateAbility);
            _eventBus.Off<ExecuteAbilityEffectEvent>(OnExecuteAbilityEffect);
            _eventBus.Off<UpdateAbilityLockEvent>(OnUpdateAbilityLock);
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _instances)
            {
                instance.Dispose();
            }
            _instances.Clear();
        }

        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var controller in _controllers)
            {
                controller.Dispose();
            }
            _controllers.Clear();
        }

        /// <summary>
        /// InstanceとControllerを生成します
        /// </summary>
        private void CreateInstance(CompositeObjectId ownerId, AbilityDataEntity baseAbilityDataSO, CompositeObjectId subOwnerId, CreatureStatusInstance creatureStatusInstance, ICreatureCardlocation iCreatureCardlocation, ITargetManager iTargetManager)
        {
            var instance = new AbilityInstance(ownerId, baseAbilityDataSO, subOwnerId, creatureStatusInstance, _eventBus, iCreatureCardlocation, iTargetManager);
            _instances.Add(instance);
            var controller = new AbilityController(instance, _eventBus);
            _controllers.Add(controller);
        }

        /// <summary>
        /// abilityの生成イベント
        /// </summary>
        private void OnCreateAbility(CreateAbilityEvent evt)
        {
            CreateInstance(evt.CreatureCardId, evt.BaseAbilityDataSO, evt.SubOwnerId, evt.CreatureStatusInstance, _iCreatureCardlocation, _iTargetManager);
        }

        /// <summary>
        /// 指定された実行者とタイミングで実行可能なAbilityがあるか返します
        /// </summary>
        public bool HasExecutableAbility(CompositeObjectId ownerId, CompositeObjectId subOwnerId, ActivationTiming activationTiming)
        {
            var instance = GetExecutableAbility(ownerId, subOwnerId, activationTiming);
            if (instance is null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 指定された実行者とタイミングで実行可能なInstanceを返します
        /// </summary>
        private AbilityInstance GetExecutableAbility(CompositeObjectId ownerId, CompositeObjectId subOwnerId, ActivationTiming activationTiming)
        {
            var list = _instances.Where(a => a.CompositeObjectId == ownerId && a.ActivationTiming == activationTiming && a.IsAvailable).ToList();
            Debug.Log("Abilitycheckリスト:" + ownerId + "_ActivationTiming" + activationTiming + "_リスト：" + list.Count);

            foreach (var instance in list)
            {
                Debug.Log("実行check:" + instance.BaseAbilityData.Id + "_Trigger：" + instance.IsTrigger + "_activationTiming:" + activationTiming);
                if (instance.IsTrigger)
                {
                    if (activationTiming == ActivationTiming.Inlet)
                    {
                        if (instance.SubOwnerId == subOwnerId)
                        {
                            return instance;
                        }
                    }
                    else
                    {
                        Debug.Log("ここにこれてない？:" + instance.BaseAbilityData.Id);
                        return instance;
                    }
                }
            }
            Debug.Log("ぬるになってる？");
            return null;
        }

        /// <summary>
        /// タイミングで実行可能なAbilityがあるか返します
        /// </summary>
        public bool HasExecutableAbility(ActivationTiming activationTiming)
        {
            var list = _instances.Where(a => a.ActivationTiming == activationTiming && a.IsAvailable).ToList();
            foreach (var instance in list)
            {
                if (instance.IsTrigger)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// アビリティの実行
        /// </summary>
        private void OnExecuteAbilityEffect(ExecuteAbilityEffectEvent evt)
        {
            // エフェクトの有効期限を更新
            _eventBus.Emit(new UpdateEffectExpiredEvent(evt.TriggerTiming, evt.SourceObjectId, evt.SubSourceObjectId));

            for (var i = 0; i < 99; i++)
            {
                Debug.Log("いんすたんすげっと");
                var instance = GetExecutableAbility(evt.SourceObjectId, evt.SubSourceObjectId, evt.TriggerTiming);
                Debug.Log("いんすたんすもどり");
                if (instance is null)
                {
                    return;
                }
                Debug.Log("ここで実行してるはずだ：" + instance.BaseAbilityData.Id);
                instance.Execute();
            }
            var list = _instances.Where(a => a.CompositeObjectId == evt.SourceObjectId && a.IsExecution).ToList();
            foreach (var instance in list)
            {
                instance.ResetExecution();
            }
        }

        /// <summary>
        /// アビリティロックのアップデート
        /// </summary>
        private void OnUpdateAbilityLock(UpdateAbilityLockEvent evt)
        {
            Debug.Log("ここはロックされるはず？：" + evt.SubSourceObjectId);
            var list = _instances.Where(a => a.SubOwnerId == evt.SubSourceObjectId && a.IsAvailable).ToList();

            foreach (var instance in list)
            {
                instance.SetLock(evt.IsLock);
            }
        }

        public List<AbilityInstance> GetInstanceList()
        {
            return _instances;
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeするイベント
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            DisposeByCompositeObjectId(evt.CompositeObjectId);
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeします
        /// </summary>
        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
            var instances = _instances.Where(i => i.CompositeObjectId == compositeObjectId).ToList();
            foreach (var instance in instances)
            {
                instance.Dispose();
                _instances.Remove(instance);
            }

            var controllers = _controllers.Where(c => c.InstanceId == compositeObjectId).ToList();
            foreach (var controller in controllers)
            {
                controller.Dispose();
                _controllers.Remove(controller);
            }
        }

            /*
                        private SpriteCommandBus _commandBus;
                        private CreatureManager _creatureManager;
                        private DiceManager _diceManager;
                        private AbilityFactory _abilityFactory;
                        private EffectManager _effectManager;

                        private readonly List<AbilityInstance> _abilities = new();

                        [Inject]
                        public void Initialize(SpriteCommandBus commandBus, CreatureManager creatureManager, DiceManager diceManager, AbilityManager abilityManager, EffectManager effectManager)
                        {
                            ClearCollections();
                            _commandBus = commandBus;
                            _creatureManager = creatureManager;
                            _diceManager = diceManager;
                            _effectManager = effectManager;
                            _abilityFactory = new AbilityFactory();
                            // すべてのコマンドをサブスクライブします。より最適化されたアプローチとしては、専用のイベントタイプを使用するとよいでしょう
                            _commandBus.On<ICommand>(OnCommandDispatched);
                            _commandBus.On<ExecuteAbilityEffectCommand>(OnExecuteAbilityEffect);
                            _commandBus.On<InletExecuteAbilityEffectCommand>(OnInletExecuteAbilityEffect);

                        }
                        private void ClearCollections()
                        {
                            _abilities.Clear();
                        }

                        private void OnDisable()
                        {
                            _commandBus.Off<ICommand>(OnCommandDispatched);
                            _commandBus.Off<ExecuteAbilityEffectCommand>(OnExecuteAbilityEffect);
                            _commandBus.Off<InletExecuteAbilityEffectCommand>(OnInletExecuteAbilityEffect);
                        }

                        /// <summary>
                        /// クリーチャーの新しい能力インスタンスを作成して登録します。
                        /// </summary>
                        public void RegisterAbility(BaseAbilityDataSO abilityData, CompositeObjectId ownerId, CompositeObjectId subOwnerId)
                        {
                            var instance = _abilityFactory.Create(abilityData, ownerId, subOwnerId);
                            _abilities.Add(instance);
                        }

                        /// <summary>
                        /// 特定の所有者に関連付けられているすべての機能を登録解除します。
                        /// </summary>
                        public void UnregisterAbilitiesForOwner(CompositeObjectId ownerId)
                        {
                            _abilities.RemoveAll(instance => instance.OwnerId == ownerId);
                        }

                        private async void OnExecuteAbilityEffect(ExecuteAbilityEffectCommand command)
                        {
                            foreach (var instance in _abilities)
                            {
                                await instance.ExecuteAbility(_creatureManager, _diceManager, this, _effectManager, _commandBus, command.TriggerTiming);
                            }
                        }

                        private async void OnInletExecuteAbilityEffect(InletExecuteAbilityEffectCommand command)
                        {
                            Debug.Log("<color=Green>OnInletExecuteAbilityEffect：</color>" + command.InletObjectId);
                            foreach (var instance in _abilities)
                            {
                                Debug.Log("<color=Green>アビリティ：</color>" + instance.SubOwnerId + "/" + command.InletObjectId);
                                if (instance.SubOwnerId == command.InletObjectId)
                                {
                                    await instance.ExecuteAbility(_creatureManager, _diceManager, this, _effectManager, _commandBus, command.TriggerTiming);
                                }
                            }
                        }
                        private void OnCommandDispatched(ICommand command)
                        {
                            /*
                                        // Handle ability triggering
                                        foreach (var instance in _abilities)
                                        {
                                            if (instance.IsSuppressed || instance.Data.TriggerCondition == null) continue;

                                            if (instance.Data.TriggerCondition.Check(command, instance))
                                            {
                                                // TODO: Check for cooldown and usage limits from instance.Data.Duration
                                                var context = new BaseAbilityEffectDefinitionSO.AbilityContext
                                                {
                                                    SourceId = instance.OwnerId
                                                    // TODO: Populate TargetId and other context from the command if available
                                                };
                                                instance.Data.EffectDefinition?.Execute(context, _commandBus);
                                                // TODO: Update duration state (e.g., decrement uses, set cooldown)
                                            }
                                        }

                                        // Handle duration updates
                                        foreach (var instance in _abilities)
                                        {
                                            instance.Data.Duration?.OnEvent(instance, command);
                                        }
            }
                */
        }
}

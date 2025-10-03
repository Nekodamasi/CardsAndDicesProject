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
    public class AbilityManager : ScriptableObject, IDisposable
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
            _iCreatureCardlocation = iCreatureCardlocation;
            _iTargetManager = iTargetManager;
        }
        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            _eventBus.Off<CreateAbilityEvent>(OnCreateAbility);
            _eventBus.Off<ExecuteAbilityEffectEvent>(OnExecuteAbilityEffect);
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
<<<<<<< HEAD
        private void CreateInstance(CompositeObjectId ownerId, AbilityDataEntity baseAbilityDataSO, CompositeObjectId subOwnerId, CreatureStatusInstance creatureStatusInstance, ICreatureCardlocation iCreatureCardlocation, ITargetManager iTargetManager)
=======
        private void CreateInstance(CompositeObjectId ownerId, BaseAbilityDataSO baseAbilityDataSO, CompositeObjectId subOwnerId, CreatureStatusInstance creatureStatusInstance, ICreatureCardlocation iCreatureCardlocation, ITargetManager iTargetManager)
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268
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
        /// アビリティの実行
        /// </summary>
        private void OnExecuteAbilityEffect(ExecuteAbilityEffectEvent evt)
        {
            var list = _instances.Where(a => a.CompositeObjectId == evt.SourceObjectId && a.ActivationTiming == evt.TriggerTiming && a.IsAvailable).ToList();

<<<<<<< HEAD
            foreach (var instance in list)
            {
=======
            Debug.Log("りすと：" + _instances[0].CompositeObjectId + "_" + evt.SourceObjectId);
//            Debug.Log("りすと：" + _instances.Count + "_" + list.Count + "_" + _instances[0].ActivationTiming + "_" + _instances[0].IsAvailable);
            foreach (var instance in list)
            {
            Debug.Log("とりがー：" + instance.IsTrigger);
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268
                if (instance.IsTrigger)
                {
                    instance.Execute();
                }
            }
        }
        public List<AbilityInstance> GetInstanceList()
        {
            return _instances;
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

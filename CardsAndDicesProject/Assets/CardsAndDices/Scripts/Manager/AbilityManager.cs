using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// ゲーム内のすべてのアクティブな AbilityInstances を管理します
    /// </summary>
    [CreateAssetMenu(fileName = "AbilityManager", menuName = "CardsAndDices/Managers/AbilityManager")]
    public class AbilityManager : ScriptableObject
    {
        private SpriteCommandBus _commandBus;
        private CreatureManager _creatureManager;
        private DiceManager _diceManager;
        private AbilityFactory _abilityFactory;

        private readonly List<AbilityInstance> _abilities = new();

        [Inject]
        public void Initialize(SpriteCommandBus commandBus, CreatureManager creatureManager, DiceManager diceManager)
        {
            _commandBus = commandBus;
            _creatureManager = creatureManager;
            _diceManager = diceManager;
            _abilityFactory = new AbilityFactory();
            // すべてのコマンドをサブスクライブします。より最適化されたアプローチとしては、専用のイベントタイプを使用するとよいでしょう
            _commandBus.On<ICommand>(OnCommandDispatched);
            _commandBus.On<ExecuteAbilityEffectCommand>(OnExecuteAbilityEffect);
        }

        private void OnDisable()
        {
            _commandBus.Off<ICommand>(OnCommandDispatched);
            _commandBus.Off<ExecuteAbilityEffectCommand>(OnExecuteAbilityEffect);
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

        private void OnExecuteAbilityEffect(ICommand command)
        {
        }
        private void OnCommandDispatched(ICommand command)
        {
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
    }
}

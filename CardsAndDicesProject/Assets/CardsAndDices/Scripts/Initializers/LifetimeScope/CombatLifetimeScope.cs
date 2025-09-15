using UnityEngine;
using VContainer;
using VContainer.Unity;
using DG.Tweening;
using UnityEditor.Rendering;

namespace CardsAndDices
{
    public class CombatLifetimeScope : LifetimeScope
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _creatureCardPrefab;

        [Header("ScriptableObject Managers")]
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private CreatureCardSpawnInfoManager _creatureCardSpawnInfoManager;
        [SerializeField] private IdentifiableCommandBus _identifiableCommandBus;
        [SerializeField] private SceneInitializer _combatInitializer;
        [SerializeField] private IdentifiableStatusManager _identifiableStatusManager;
        [SerializeField] private IdentifiableStatusViewManager _identifiableStatusViewManager;
        [SerializeField] private IdentifiableUIStateMachine _identifiableUIStateMachine;
        [SerializeField] private DiceSlotManager _diceSlotManager;
        

        [Header("ScriptableObject Registries")]
        [SerializeField] private CompositeObjectRegistry _compositeObjectRegistry;
        [SerializeField] private IdentifiableViewRegistry _identifiableViewRegistry;

        [Header("MonoBehaviour")]
        [SerializeField] private CreatureCardSpawner _creatureCardSpawner;

        [Header("StateOperator")]
        [SerializeField] private DragStateOperator _dragStateOperator;

        protected override void Configure(IContainerBuilder builder)
        {
            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            // ScriptableObject Managers のバインド
            builder.RegisterInstance(_compositeObjectIdManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureCardSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_identifiableCommandBus).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatInitializer).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableStatusManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableStatusViewManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableUIStateMachine).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceSlotManager).AsSelf().AsImplementedInterfaces();

            // ScriptableObject Registries のバインド
            builder.RegisterInstance(_compositeObjectRegistry).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableViewRegistry).AsSelf().AsImplementedInterfaces();

            // ScriptableObject StateOperator のバインド
            builder.RegisterInstance(_dragStateOperator).AsSelf().AsImplementedInterfaces();

            // ScriptableObject Managers の初期化
            _compositeObjectIdManager.Initialize();
            _identifiableCommandBus.Initialize();
            _combatInitializer.Initialize(_identifiableCommandBus);
            _identifiableStatusManager.Initialize(_compositeObjectRegistry, _identifiableCommandBus, _identifiableUIStateMachine);
            _identifiableStatusViewManager.Initialize(_identifiableViewRegistry, _identifiableStatusManager, _identifiableCommandBus);
            _identifiableUIStateMachine.Initialize(_identifiableCommandBus);
            _diceSlotManager.Initialize(_identifiableCommandBus);

            // ScriptableObject Managers の初期化
            _compositeObjectRegistry.Initialize();
            _identifiableViewRegistry.Initialize();

            // ScriptableObject StateOperator の初期化
            _dragStateOperator.Initialize(_identifiableCommandBus);

            // Factoryの登録
            builder.RegisterFactory<CreatureCardSpawnInfo, GameObject>(container => (info) =>
            {
                var card = container.Instantiate(_creatureCardPrefab);
                var IdentifiableGameObject = card.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = card.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = card.GetComponent<BaseIdentifiableView>();
                return card;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_creatureCardSpawner);
        }
    }
}

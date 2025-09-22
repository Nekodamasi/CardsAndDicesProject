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
        [SerializeField] private GameObject _dicePrefab;

        [Header("ScriptableObject Managers")]
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private CreatureCardSpawnInfoManager _creatureCardSpawnInfoManager;
        [SerializeField] private DiceSpawnInfoManager _diceSpawnInfoManager;
        [SerializeField] private GameEventBus _gameEventBus;
        [SerializeField] private SceneInitializer _combatInitializer;
        [SerializeField] private IdentifiableStatusManager _identifiableStatusManager;
        [SerializeField] private IdentifiableStatusViewManager _identifiableStatusViewManager;
        [SerializeField] private IdentifiableUIStateMachine _identifiableUIStateMachine;
        [SerializeField] private DiceSlotManager _diceSlotManager;
        [SerializeField] private DiceManager _diceManager;
        [SerializeField] private SoundManager _soundManager;
        

        [Header("ScriptableObject Registries")]
        [SerializeField] private CompositeObjectRegistry _compositeObjectRegistry;
        [SerializeField] private IdentifiableViewRegistry _identifiableViewRegistry;

        [Header("MonoBehaviour")]
        [SerializeField] private CreatureCardSpawner _creatureCardSpawner;
        [SerializeField] private DiceSpawner _diceSpawner;

        [Header("StateOperator")]
        [SerializeField] private CoreOperator _dragStateOperator;

        protected override void Configure(IContainerBuilder builder)
        {
            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            // ScriptableObject Managers のバインド
            builder.RegisterInstance(_compositeObjectIdManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureCardSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_diceSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_gameEventBus).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatInitializer).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableStatusManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableStatusViewManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableUIStateMachine).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceSlotManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_soundManager).AsSelf().AsImplementedInterfaces();

            // ScriptableObject Registries のバインド
            builder.RegisterInstance(_compositeObjectRegistry).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableViewRegistry).AsSelf().AsImplementedInterfaces();

            // ScriptableObject StateOperator のバインド
            builder.RegisterInstance(_dragStateOperator).AsSelf().AsImplementedInterfaces();

            // ScriptableObject Managers の初期化
            _compositeObjectIdManager.Initialize(_compositeObjectRegistry);
            _gameEventBus.Initialize();
            _combatInitializer.Initialize(_gameEventBus);
            _identifiableStatusManager.Initialize(_compositeObjectRegistry, _gameEventBus, _identifiableUIStateMachine);
            _identifiableStatusViewManager.Initialize(_identifiableViewRegistry, _identifiableStatusManager, _gameEventBus);
            _identifiableUIStateMachine.Initialize(_gameEventBus);
            _diceSlotManager.Initialize(_gameEventBus, _compositeObjectIdManager);
            _diceManager.Initialize(_gameEventBus, _identifiableViewRegistry, _diceSlotManager);
            _soundManager.Initialize();

            // ScriptableObject Managers の初期化
            _compositeObjectRegistry.Initialize();
            _identifiableViewRegistry.Initialize();

            // ScriptableObject StateOperator の初期化
            _dragStateOperator.Initialize(_gameEventBus);

            // Factoryの登録
            builder.RegisterFactory<CreatureCardSpawnInfo, GameObject>(container => (cardinfo) =>
            {
                var card = container.Instantiate(_creatureCardPrefab);
                var IdentifiableGameObject = card.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = card.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = card.GetComponent<BaseIdentifiableView>();
                var SEPlayer = card.GetComponent<SEPlayer>();
                return card;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_creatureCardSpawner);

            // Factoryの登録
            builder.RegisterFactory<DiceSpawnInfo, GameObject>(container => (diceinfo) =>
            {
                var dice = container.Instantiate(_dicePrefab);
                var IdentifiableGameObject = dice.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = dice.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = dice.GetComponent<BaseIdentifiableView>();
                return dice;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_diceSpawner);
        }
    }
}

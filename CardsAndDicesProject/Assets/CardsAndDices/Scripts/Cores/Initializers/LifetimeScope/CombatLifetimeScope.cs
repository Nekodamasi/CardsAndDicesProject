using UnityEngine;
using VContainer;
using VContainer.Unity;
using DG.Tweening;
using UnityEditor.Rendering;
using TMPro;

namespace CardsAndDices
{
    public class CombatLifetimeScope : LifetimeScope
    {
        [Header("Prefab")]
        [SerializeField] private GameObject _creatureCardPrefab;
        [SerializeField] private GameObject _dicePrefab;
        [SerializeField] private GameObject _creatureCardSlotPrefab;
        [SerializeField] private GameObject _enemyCreatureCardPrefab;
        [SerializeField] private GameObject _nextTurnPrefab;
        [SerializeField] private GameObject _combatStartPrefab;

        [Header("ScriptableObject Managers")]
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private GameEventBus _gameEventBus;
        [SerializeField] private SceneInitializer _combatInitializer;
//        [SerializeField] private IdentifiableStatusManager _identifiableStatusManager;
        [SerializeField] private IdentifiableStatusViewManager _identifiableStatusViewManager;
        [SerializeField] private IdentifiableUIStateMachine _identifiableUIStateMachine;
        [SerializeField] private DiceSlotManager _diceSlotManager;
        [SerializeField] private DiceManager _diceManager;
        [SerializeField] private SoundManager _soundManager;
        [SerializeField] private CreatureManager _creatureManager;
        [SerializeField] private PlayerCardDataProvider _playerCardDataProvider;
        [SerializeField] private CreatureCardManager _creatureCardManager;
        [SerializeField] private CreatureCardSlotManager _creatureCardSlotManager;
        [SerializeField] private CreatureStatusManager _creatureStatusManager;
        [SerializeField] private SharedIconElementManager _sharedIconElementManager;
        [SerializeField] private EffectManager _effectManager;
        [SerializeField] private AbilityManager _abilityManager;
        [SerializeField] private TargetManager _targetManager;
        [SerializeField] private DiceInletManager _diceInletManager;
        [SerializeField] private VfxManager _vfxManager;
        [SerializeField] private CombatPhaseStateMachine _combatPhaseStateMachine;
        [SerializeField] private WaveManager _waveManager;
        [SerializeField] private CardAppearanceManager _cardAppearanceManager;

        [Header("PrefabSpawnInfo Managers")]
        [SerializeField] private CreatureCardSpawnInfoManager _creatureCardSpawnInfoManager;
        [SerializeField] private DiceSpawnInfoManager _diceSpawnInfoManager;
        [SerializeField] private CreatureCardSlotSpawnInfoManager _creatureCardSlotSpawnInfoManager;
        [SerializeField] private EnemyCreatureCardSpawnInfoManager _enemyCreatureCardSpawnInfoManager;
        [SerializeField] private NextTurnSpawnInfoManager _nextTurnSpawnInfoManager;
        [SerializeField] private CombatStartSpawnInfoManager _combatStartSpawnInfoManager;

        [Header("ScriptableObject Registries")]
        [SerializeField] private CompositeObjectRegistry _compositeObjectRegistry;
        [SerializeField] private IdentifiableViewRegistry _identifiableViewRegistry;
        [SerializeField] private CombatScenarioRegistry _combatScenarioRegistry;

        [Header("PrefabSpawner")]
        [SerializeField] private CreatureCardSpawner _creatureCardSpawner;
        [SerializeField] private DiceSpawner _diceSpawner;
        [SerializeField] private CreatureCardSlotSpawner _creatureCardSlotSpawner;
        [SerializeField] private EnemyCreatureCardSpawner _enemyCreatureCardSpawner;
        [SerializeField] private NextTurnSpawner _nextTurnSpawner;
        [SerializeField] private CombatStartSpawner _combatStartSpawner;
        

        [Header("StateOperator")]
        [SerializeField] private CoreOperator _dragStateOperator;

        [Header("Debug")]
        [SerializeField] private CreatureCardSlotStatusViewer _creatureCardSlotStatusViewer;
        [SerializeField] private AbilityManagerStatusViewer _abilityManagerStatusViewer;
        [SerializeField] private EffectManagerStatusViewer _effectManagerStatusViewer;

        protected override void Configure(IContainerBuilder builder)
        {
            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            // ScriptableObject Managers のバインド
            builder.RegisterInstance(_compositeObjectIdManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_gameEventBus).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatInitializer).AsSelf().AsImplementedInterfaces();
//            builder.RegisterInstance(_identifiableStatusManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableStatusViewManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableUIStateMachine).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceSlotManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_soundManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_playerCardDataProvider).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureCardManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureCardSlotManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_creatureStatusManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_sharedIconElementManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_effectManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_abilityManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_targetManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceInletManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_vfxManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatPhaseStateMachine).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_waveManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardAppearanceManager).AsSelf().AsImplementedInterfaces();            

            // PrefabSpawnInfo Managers のバインド
            builder.RegisterInstance(_creatureCardSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_diceSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_creatureCardSlotSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_enemyCreatureCardSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_nextTurnSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_combatStartSpawnInfoManager).AsSelf();

            // ScriptableObject Registries のバインド
            builder.RegisterInstance(_compositeObjectRegistry).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_identifiableViewRegistry).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatScenarioRegistry).AsSelf().AsImplementedInterfaces();

            // ScriptableObject StateOperator のバインド
            builder.RegisterInstance(_dragStateOperator).AsSelf().AsImplementedInterfaces();

            // ScriptableObject Managers の初期化
            _compositeObjectIdManager.Initialize(_compositeObjectRegistry);
            _gameEventBus.Initialize();
            _combatInitializer.Initialize(_gameEventBus);
//            _identifiableStatusManager.Initialize(_compositeObjectRegistry, _gameEventBus, _identifiableUIStateMachine);
            _identifiableStatusViewManager.Initialize(_identifiableViewRegistry, _identifiableUIStateMachine, _gameEventBus);
            _identifiableUIStateMachine.Initialize(_gameEventBus);
            _diceSlotManager.Initialize(_gameEventBus, _compositeObjectIdManager);
            _diceManager.Initialize(_gameEventBus, _identifiableViewRegistry, _diceSlotManager);
            _soundManager.Initialize();
            _creatureManager.Initialize(_playerCardDataProvider, _gameEventBus, _identifiableViewRegistry);
            _playerCardDataProvider.Initialize();
            _creatureCardManager.Initialize(_gameEventBus, _creatureCardSlotManager, _identifiableViewRegistry);
            _creatureCardSlotManager.Initialize(_gameEventBus, _compositeObjectIdManager, _identifiableViewRegistry);
            _creatureStatusManager.Initialize(_gameEventBus, _targetManager, _identifiableViewRegistry, _effectManager, _abilityManager, _waveManager, _creatureCardSlotManager);
            _sharedIconElementManager.Initialize(_gameEventBus, _identifiableViewRegistry);
            _targetManager.Initialize(_creatureCardSlotManager, _creatureStatusManager);
            _abilityManager.Initialize(_gameEventBus, _creatureCardSlotManager, _targetManager);
            _effectManager.Initialize(_gameEventBus);
            _diceInletManager.Initialize(_gameEventBus, _identifiableViewRegistry);
            _vfxManager.Initialize(_soundManager, _gameEventBus);
            _combatPhaseStateMachine.Initialize(_gameEventBus);
            _waveManager.Initialize(_gameEventBus, _combatScenarioRegistry, _identifiableViewRegistry, _targetManager);
            _combatScenarioRegistry.Initialize();
            _cardAppearanceManager.Initialize(_gameEventBus, _identifiableViewRegistry);

            // ScriptableObject Managers の初期化
            _compositeObjectRegistry.Initialize();
            _identifiableViewRegistry.Initialize();

            // ScriptableObject StateOperator の初期化
            _dragStateOperator.Initialize(_gameEventBus);

            // CreatureCardの登録
            builder.RegisterFactory<CreatureCardSpawnInfo, GameObject>(container => (cardinfo) =>
            {
                var card = container.Instantiate(_creatureCardPrefab);
                var IdentifiableGameObject = card.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = card.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = card.GetComponent<BaseIdentifiableView>();
                var AnimationContext = card.GetComponent<AnimationContext>();
                var SEPlayer = card.GetComponent<SEPlayer>();
                return card;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_creatureCardSpawner);

            // Diceの登録
            builder.RegisterFactory<DiceSpawnInfo, GameObject>(container => (diceinfo) =>
            {
                var dice = container.Instantiate(_dicePrefab);
                var IdentifiableGameObject = dice.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = dice.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = dice.GetComponent<BaseIdentifiableView>();
                var AnimationContext = dice.GetComponent<AnimationContext>();
                return dice;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_diceSpawner);

            // CreatureCardSlotの登録
            builder.RegisterFactory<CreatureCardSlotSpawnInfo, GameObject>(container => (cardSlotinfo) =>
            {
                var cardSlot = container.Instantiate(_creatureCardSlotPrefab);
                var IdentifiableGameObject = cardSlot.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = cardSlot.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = cardSlot.GetComponent<BaseIdentifiableView>();
                return cardSlot;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_creatureCardSlotSpawner);

            // EnemyCreatureCardの登録
            builder.RegisterFactory<EnemyCreatureCardSpawnInfo, GameObject>(container => (enemycardinfo) =>
            {
                var card = container.Instantiate(_enemyCreatureCardPrefab);
                var IdentifiableGameObject = card.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = card.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = card.GetComponent<BaseIdentifiableView>();
                var AnimationContext = card.GetComponent<AnimationContext>();
                var SEPlayer = card.GetComponent<SEPlayer>();
                return card;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_enemyCreatureCardSpawner);

            // nextTurnの登録
            builder.RegisterFactory<NextTurnSpawnInfo, GameObject>(container => (nextTurninfo) =>
            {
                var nextTurnBtn = container.Instantiate(_nextTurnPrefab);
                var IdentifiableGameObject = nextTurnBtn.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = nextTurnBtn.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = nextTurnBtn.GetComponent<BaseIdentifiableView>();
                var AnimationContext = nextTurnBtn.GetComponent<AnimationContext>();
                var NextTurnManagern = nextTurnBtn.GetComponent<NextTurnManager>();
                var SEPlayer = nextTurnBtn.GetComponent<SEPlayer>();
                return nextTurnBtn;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_nextTurnSpawner);

            // CombatStartの登録
            builder.RegisterFactory<CombatStartSpawnInfo, GameObject>(container => (combatStartinfo) =>
            {
                var combatStartBtn = container.Instantiate(_combatStartPrefab);
                var IdentifiableGameObject = combatStartBtn.GetComponent<IdentifiableGameObject>();
                var IdentifiableInputHandler = combatStartBtn.GetComponent<IdentifiableInputHandler>();
                var BaseIdentifiableView = combatStartBtn.GetComponent<BaseIdentifiableView>();
                var AnimationContext = combatStartBtn.GetComponent<AnimationContext>();
                var SEPlayer = combatStartBtn.GetComponent<SEPlayer>();
                return combatStartBtn;
            },
            Lifetime.Singleton);
            builder.RegisterComponent(_combatStartSpawner);

            // debug
            builder.RegisterComponent(_creatureCardSlotStatusViewer);
            builder.RegisterComponent(_abilityManagerStatusViewer);
            builder.RegisterComponent(_effectManagerStatusViewer);
        }
    }
}

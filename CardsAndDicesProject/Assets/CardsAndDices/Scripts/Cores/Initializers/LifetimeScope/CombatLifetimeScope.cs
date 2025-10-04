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

        [Header("ScriptableObject Managers")]
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private GameEventBus _gameEventBus;
        [SerializeField] private SceneInitializer _combatInitializer;
        [SerializeField] private IdentifiableStatusManager _identifiableStatusManager;
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


        [Header("PrefabSpawnInfo Managers")]
        [SerializeField] private CreatureCardSpawnInfoManager _creatureCardSpawnInfoManager;
        [SerializeField] private DiceSpawnInfoManager _diceSpawnInfoManager;
        [SerializeField] private CreatureCardSlotSpawnInfoManager _creatureCardSlotSpawnInfoManager;


        [Header("ScriptableObject Registries")]
        [SerializeField] private CompositeObjectRegistry _compositeObjectRegistry;
        [SerializeField] private IdentifiableViewRegistry _identifiableViewRegistry;


        [Header("PrefabSpawner")]
        [SerializeField] private CreatureCardSpawner _creatureCardSpawner;
        [SerializeField] private DiceSpawner _diceSpawner;
        [SerializeField] private CreatureCardSlotSpawner _creatureCardSlotSpawner;


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
            builder.RegisterInstance(_identifiableStatusManager).AsSelf().AsImplementedInterfaces();
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

            // PrefabSpawnInfo Managers のバインド
            builder.RegisterInstance(_creatureCardSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_diceSpawnInfoManager).AsSelf();
            builder.RegisterInstance(_creatureCardSlotSpawnInfoManager).AsSelf();

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
            _identifiableStatusViewManager.Initialize(_identifiableViewRegistry, _identifiableUIStateMachine, _gameEventBus);
            _identifiableUIStateMachine.Initialize(_gameEventBus);
            _diceSlotManager.Initialize(_gameEventBus, _compositeObjectIdManager);
            _diceManager.Initialize(_gameEventBus, _identifiableViewRegistry, _diceSlotManager);
            _soundManager.Initialize();
            _creatureManager.Initialize(_playerCardDataProvider, _gameEventBus, _identifiableViewRegistry);
            _playerCardDataProvider.Initialize();
            _creatureCardManager.Initialize(_gameEventBus, _creatureCardSlotManager, _identifiableViewRegistry);
            _creatureCardSlotManager.Initialize(_gameEventBus, _compositeObjectIdManager, _identifiableViewRegistry);
            _creatureStatusManager.Initialize(_gameEventBus, _creatureCardSlotManager, _identifiableViewRegistry, _effectManager);
            _sharedIconElementManager.Initialize(_gameEventBus, _identifiableViewRegistry);
            _targetManager.Initialize(_creatureCardSlotManager);
            _abilityManager.Initialize(_gameEventBus, _creatureCardSlotManager, _targetManager);
            _effectManager.Initialize(_gameEventBus);
            _diceInletManager.Initialize(_gameEventBus, _identifiableViewRegistry);
            _vfxManager.Initialize(_soundManager, _gameEventBus);

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

            // Factoryの登録
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

            // debug
            builder.RegisterComponent(_creatureCardSlotStatusViewer);
            builder.RegisterComponent(_abilityManagerStatusViewer);
            builder.RegisterComponent(_effectManagerStatusViewer);
        }
    }
}

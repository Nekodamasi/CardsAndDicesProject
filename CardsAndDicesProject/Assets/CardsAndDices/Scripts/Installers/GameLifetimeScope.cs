using UnityEngine;
using VContainer;
using VContainer.Unity;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace CardsAndDices
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("ScriptableObject Managers")]
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private SpriteCommandBus _spriteCommandBus;
        [SerializeField] private CardSlotManager _cardSlotManager;
        [SerializeField] private DiceInletManager _diceInletManager;
        [SerializeField] private DiceSlotManager _diceSlotManager;
        [SerializeField] private CardSlotStateRepository _cardSlotStateRepository;
        [SerializeField] private DiceSlotStateRepository _diceSlotStateRepository;
        [SerializeField] private CardPlacementService _cardPlacementService;
        [SerializeField] private DicePlacementService _dicePlacementService;
        [SerializeField] private ReflowService _reflowService;
        [SerializeField] private CardSlotInteractionHandler _cardSlotInteractionHandler;
        [SerializeField] private DiceSlotInteractionHandler _diceSlotInteractionHandler;
        [SerializeField] private CardSlotDebug _cardSlotDebug;
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private CardInteractionOrchestrator _cardInteractionOrchestrator;
        [SerializeField] private DiceInteractionOrchestrator _diceInteractionOrchestrator;
        [SerializeField] private CardInteractionStrategy _cardInteractionStrategy;
        [SerializeField] private DiceInteractionStrategy _diceInteractionStrategy;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;
        [SerializeField] private ViewRegistry _viewRegistry;
        [SerializeField] private CardLifecycleService _cardLifecycleService;
        [SerializeField] private DiceInletAbilityRegistry _diceInletAbilityRegistry;
        [SerializeField] private DiceManager _diceManager;
        [SerializeField] private SystemReflowController _systemReflowController;
        [SerializeField] private CombatManager _combatManager;
        [SerializeField] private PlayerCardDataProvider _playerCardDataProvider;
        [SerializeField] private EnemyCardDataProvider _enemyCardDataProvider;

        [Header("Object Pools")]
        [SerializeField] private List<CreatureCardView> _creatureCardViews = new List<CreatureCardView>();
        [SerializeField] private List<CardSlotView> _cardSlotViews = new List<CardSlotView>();
        [SerializeField] private List<DiceSlotView> _diceSlotViews = new List<DiceSlotView>();
        [SerializeField] private List<DiceView> _diceViews = new List<DiceView>();
        [SerializeField] private List<DiceInletView> _diceInletViews = new List<DiceInletView>();

        protected override void Configure(IContainerBuilder builder)
        {
            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            // ScriptableObject Managers のバインドと初期化
            builder.RegisterInstance(_uiStateMachine).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_spriteCommandBus).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceInletManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceSlotManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotStateRepository).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceSlotStateRepository).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardPlacementService).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_dicePlacementService).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_reflowService).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotInteractionHandler).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceSlotInteractionHandler).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotDebug).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_compositeObjectIdManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardInteractionOrchestrator).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceInteractionOrchestrator).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardInteractionStrategy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceInteractionStrategy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_uiActivationPolicy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_systemReflowController).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_viewRegistry).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_combatManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_playerCardDataProvider).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_enemyCardDataProvider).AsSelf().AsImplementedInterfaces();

            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            _uiStateMachine.Initialize();
            _spriteCommandBus.Initialize();
            _cardSlotManager.Initialize(_cardSlotStateRepository, _cardPlacementService, _cardSlotInteractionHandler, _cardSlotDebug);
            _diceInletManager.Initialize();
            _diceSlotManager.Initialize(_diceSlotStateRepository, _dicePlacementService, _diceSlotInteractionHandler);
            _cardSlotStateRepository.Initialize();
            _diceSlotStateRepository.Initialize();
            _cardPlacementService.Initialize(_cardSlotStateRepository);
            _dicePlacementService.Initialize(_diceSlotStateRepository);
            _cardSlotInteractionHandler.Initialize(_spriteCommandBus, _cardSlotStateRepository, _reflowService, _cardPlacementService);
            _diceSlotInteractionHandler.Initialize(_spriteCommandBus, _diceSlotStateRepository, _reflowService, _dicePlacementService);
            _cardSlotDebug.Initialize(_cardSlotStateRepository, _viewRegistry);
            _compositeObjectIdManager.Initialize();
            _cardInteractionOrchestrator.Initialize(_uiStateMachine, _cardSlotManager, _spriteCommandBus, _reflowService, _uiActivationPolicy, _cardInteractionStrategy, _viewRegistry);
            _diceInteractionOrchestrator.Initialize(_uiStateMachine, _diceSlotManager, _spriteCommandBus, _uiActivationPolicy, _diceInteractionStrategy, _viewRegistry);
            _reflowService.Initialize(_cardSlotStateRepository, _cardSlotDebug);
            _cardInteractionStrategy.Initialize();
            _systemReflowController.Initialize(_spriteCommandBus, _cardInteractionOrchestrator, _diceInteractionOrchestrator);
            _viewRegistry.Initialize();
            _cardLifecycleService.Initialize(_diceInletAbilityRegistry, _viewRegistry);
            _diceInletAbilityRegistry.Clear();
            _diceManager.Initialize(_compositeObjectIdManager, _viewRegistry);
            _combatManager.Initialize(_cardLifecycleService, _cardSlotManager, _playerCardDataProvider, _enemyCardDataProvider, _viewRegistry, _diceManager);
            _playerCardDataProvider.Initialize();
            _enemyCardDataProvider.Initialize();
            _uiActivationPolicy.Initialize(_diceInletAbilityRegistry, _diceManager);

            foreach (var cardView in _creatureCardViews)
            {
                cardView.Construct(_cardInteractionOrchestrator);
            }
            foreach (var cardSlotView in _cardSlotViews)
            {
                cardSlotView.Construct(_cardInteractionOrchestrator);
            }
            foreach (var diceSlotView in _diceSlotViews)
            {
                diceSlotView.Construct(_diceInteractionOrchestrator);
            }
            foreach (var diceView in _diceViews)
            {
                diceView.Construct(_diceInteractionOrchestrator);
            }
            foreach (var diceInletView in _diceInletViews)
            {
                diceInletView.Construct(_diceInteractionOrchestrator);
            }
        }
    }
}

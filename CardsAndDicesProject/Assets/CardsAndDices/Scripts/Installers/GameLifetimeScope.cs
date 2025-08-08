using CardsAndDice;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using DG.Tweening;
using System.Collections.Generic;

namespace CardsAndDice
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("ScriptableObject Managers")]
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private SpriteCommandBus _spriteCommandBus;
        [SerializeField] private CardSlotManager _cardSlotManager;
        [SerializeField] private CardSlotStateRepository _cardSlotStateRepository;
        [SerializeField] private CardPlacementService _cardPlacementService;
        [SerializeField] private ReflowService _reflowService;
        [SerializeField] private CardSlotInteractionHandler _cardSlotInteractionHandler;
        [SerializeField] private CardSlotDebug _cardSlotDebug;
        [SerializeField] private CompositeObjectIdManager _compositeObjectIdManager;
        [SerializeField] private CardInteractionOrchestrator _cardInteractionOrchestrator;
        [SerializeField] private DiceInteractionOrchestrator _diceInteractionOrchestrator;
        [SerializeField] private CardInteractionStrategy _cardInteractionStrategy;
        [SerializeField] private DiceInteractionStrategy _diceInteractionStrategy;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;

        [SerializeField] private SystemReflowController _systemReflowController;
        [SerializeField] private List<CreatureCardView> CreatureCardViews = new List<CreatureCardView>();
        [SerializeField] private List<CardSlotView> CardSlotViews = new List<CardSlotView>();

        protected override void Configure(IContainerBuilder builder)
        {
            // DOTweenの初期化とTween容量の設定
            DOTween.Init(true, true, LogBehaviour.ErrorsOnly).SetCapacity(200, 100);

            // ScriptableObject Managers のバインドと初期化
            builder.RegisterInstance(_uiStateMachine).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_spriteCommandBus).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotStateRepository).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardPlacementService).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_reflowService).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotInteractionHandler).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardSlotDebug).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_compositeObjectIdManager).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardInteractionOrchestrator).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceInteractionOrchestrator).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardInteractionStrategy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_diceInteractionStrategy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_uiActivationPolicy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_systemReflowController).AsSelf().AsImplementedInterfaces();

            _uiStateMachine.Initialize();
            _spriteCommandBus.Initialize();
            _cardSlotManager.Initialize(_cardSlotStateRepository, _cardPlacementService, _cardSlotInteractionHandler, _cardSlotDebug);
            _cardSlotStateRepository.Initialize();
            _cardPlacementService.Initialize(_cardSlotStateRepository);
            _cardSlotInteractionHandler.InInitialize(_spriteCommandBus, _cardSlotStateRepository, _reflowService, _cardPlacementService);
            _cardSlotDebug.InInitialize(_cardSlotStateRepository, _cardInteractionOrchestrator.ViewRegistry);
            _compositeObjectIdManager.Initialize();
            _cardInteractionOrchestrator.Initialize(_uiStateMachine, _cardSlotManager, _spriteCommandBus, _reflowService, _uiActivationPolicy, _cardInteractionStrategy);
            _diceInteractionOrchestrator.Initialize(_uiStateMachine, _spriteCommandBus, _diceInteractionStrategy);
            _reflowService.Initialize(_cardSlotStateRepository, _cardSlotDebug);
            _cardInteractionStrategy.Initialize();
            _uiActivationPolicy.Initialize();
            _systemReflowController.Initialize(_spriteCommandBus, _cardInteractionOrchestrator);

            foreach (var cardView in CreatureCardViews)
            {
                cardView.Construct(_cardInteractionOrchestrator);
            }
            foreach (var cardSlotView in CardSlotViews)
            {
                cardSlotView.Construct(_cardInteractionOrchestrator);
            }
        }
    }
}
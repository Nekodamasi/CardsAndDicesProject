using CardsAndDice;
using UnityEngine; // [SerializeField] と Header を使用するために追加
using VContainer;
using VContainer.Unity;
using DG.Tweening; // 追加

// 名前空間を追加
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
        [SerializeField] private UIInteractionOrchestrator _uiInteractionOrchestrator;
        [SerializeField] private CardInteractionStrategy _cardInteractionStrategy;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;

        [SerializeField] private SystemReflowController _systemReflowController;

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
            builder.RegisterInstance(_uiInteractionOrchestrator).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_cardInteractionStrategy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_uiActivationPolicy).AsSelf().AsImplementedInterfaces();
            builder.RegisterInstance(_systemReflowController).AsSelf().AsImplementedInterfaces();

            _uiStateMachine.Initialize();
            _spriteCommandBus.Initialize();
            _cardSlotManager.Initialize(_cardSlotStateRepository, _cardPlacementService, _cardSlotInteractionHandler, _cardSlotDebug);
            _cardSlotStateRepository.Initialize();
            _cardPlacementService.Initialize(_cardSlotStateRepository);
            _cardSlotInteractionHandler.InInitialize(_spriteCommandBus, _cardSlotStateRepository, _reflowService, _cardPlacementService);
            _cardSlotDebug.InInitialize(_cardSlotStateRepository, _uiInteractionOrchestrator.ViewRegistry);
            _compositeObjectIdManager.Initialize();
            _uiInteractionOrchestrator.Initialize(_uiStateMachine, _cardSlotManager, _spriteCommandBus, _reflowService, _uiActivationPolicy, _cardInteractionStrategy);
            _reflowService.Initialize(_cardSlotStateRepository, _cardSlotDebug);
            _cardInteractionStrategy.Initialize();
            _uiActivationPolicy.Initialize();
            _systemReflowController.Initialize(_spriteCommandBus, _uiInteractionOrchestrator);

            // ViewコンポーネントのDI登録
            builder.RegisterComponentInHierarchy<CreatureCardView>();
            builder.RegisterComponentInHierarchy<CardSlotView>();
        }
    }
}

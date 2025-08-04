using UnityEngine;
using VContainer;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace CardsAndDice
{
    [CreateAssetMenu(fileName = "UIInteractionOrchestrator", menuName = "CardsAndDice/Systems/UIInteractionOrchestrator")]
    public class UIInteractionOrchestrator : ScriptableObject
    {
        private enum ReflowState { Idle, InProgress }
        private ReflowState _currentReflowState = ReflowState.Idle;
        private SpriteHoverCommand _nextHoverCommand = null;

        [Header("Dependencies")]
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private CardSlotManager _cardSlotManager;
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private ReflowService _reflowService;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;

        [Header("Strategies")]
        [SerializeField] private CardInteractionStrategy _cardInteractionStrategy;

        private IInteractionStrategy _activeStrategy;
        private CompositeObjectId _draggedId;
        private ViewRegistry _viewRegistry;
        private bool _isDroppedSuccessfully;

        public UIStateMachine UIStateMachine => _uiStateMachine;
        public CardSlotManager CardSlotManager => _cardSlotManager;
        public ViewRegistry ViewRegistry => _viewRegistry;
        public CompositeObjectId DraggedId => _draggedId;
        public bool IsDroppedSuccessfully { get => _isDroppedSuccessfully; set => _isDroppedSuccessfully = value; }

        [Inject]
        public void Initialize(UIStateMachine uiStateMachine, CardSlotManager cardSlotManager, SpriteCommandBus commandBus, ReflowService reflowService, UIActivationPolicy uiActivationPolicy, CardInteractionStrategy cardInteractionStrategy)
        {
            _uiStateMachine = uiStateMachine;
            _cardSlotManager = cardSlotManager;
            _commandBus = commandBus;
            _reflowService = reflowService;
            _uiActivationPolicy = uiActivationPolicy;
            _cardInteractionStrategy = cardInteractionStrategy;

            _viewRegistry = new ViewRegistry();
            _draggedId = null;
            _activeStrategy = null;
            _currentReflowState = ReflowState.Idle;
            _nextHoverCommand = null;

            _commandBus.On<SpriteBeginDragCommand>(OnBeginDrag);
            _commandBus.On<SpriteHoverCommand>(OnHover);
            _commandBus.On<SpriteUnhoverCommand>(OnUnhover);
            _commandBus.On<SpriteDropCommand>(OnDrop);
            _commandBus.On<SpriteDragCommand>(OnDrag);
            _commandBus.On<SpriteEndDragCommand>(OnEndDrag);
            _commandBus.On<ReflowOperationCompletedCommand>(OnReflowOperationCompleted);
            _commandBus.On<SpriteDragOperationCompletedCommand>(OnSpriteDragOperationCompleted);
            _commandBus.On<ReflowCompletedCommand>(OnReflowCompleted);
            _commandBus.On<DragReflowCompletedCommand>(OnDragReflowCompleted);
            _commandBus.On<ExecuteFrontLoadCommand>(OnExecuteFrontLoad);
        }

        public void Dispose()
        {
            if (_commandBus == null) return;
            _commandBus.Off<SpriteBeginDragCommand>(OnBeginDrag);
            _commandBus.Off<SpriteHoverCommand>(OnHover);
            _commandBus.Off<SpriteUnhoverCommand>(OnUnhover);
            _commandBus.Off<SpriteDropCommand>(OnDrop);
            _commandBus.Off<SpriteDragCommand>(OnDrag);
            _commandBus.Off<SpriteEndDragCommand>(OnEndDrag);
            _commandBus.Off<ReflowOperationCompletedCommand>(OnReflowOperationCompleted);
            _commandBus.Off<SpriteDragOperationCompletedCommand>(OnSpriteDragOperationCompleted);
            _commandBus.Off<ReflowCompletedCommand>(OnReflowCompleted);
            _commandBus.Off<DragReflowCompletedCommand>(OnDragReflowCompleted);
            _commandBus.Off<ExecuteFrontLoadCommand>(OnExecuteFrontLoad);
        }

        public void RegisterView(BaseSpriteView view)
        {
            _viewRegistry.Register(view);
        }

        public void UnregisterView(BaseSpriteView view)
        {
            _viewRegistry.Unregister(view);
        }

        private void OnBeginDrag(SpriteBeginDragCommand command)
        {
            if (UIStateMachine.CurrentState != UIStateMachine.UIState.Idle) return;
            _draggedId = command.TargetObjectId;
            if (command.TargetObjectId.ObjectType == "Card")
            {
                _activeStrategy = _cardInteractionStrategy;
            }
            _activeStrategy?.OnBeginDrag(command, this);
            _uiActivationPolicy.DraggingCardToCardActivations(this);
            _uiActivationPolicy.DraggingCardToCardSlotActivations(this);
        }

        private void OnHover(SpriteHoverCommand command)
        {
            if (_cardInteractionStrategy.ChkCardSlotHover(command, this))
            {
                if (_currentReflowState == ReflowState.InProgress)
                {
                    Debug.Log("<color=Green>ホバースタック</color>");
                    _nextHoverCommand = command;
                    return;
                }
                Debug.Log("<color=Green>OnHover:</color>" + _currentReflowState);
                ExecuteHover(command);
            }
            else if (UIStateMachine.CurrentState == UIStateMachine.UIState.Idle)
            {
                _cardInteractionStrategy?.OnHover(command, this);
            }
        }

        private void ExecuteHover(SpriteHoverCommand command)
        {
            _currentReflowState = ReflowState.InProgress;

            // ホバーリフローを実行
            CardSlotManager.OnCardHoveredOnSlot(DraggedId, command.TargetObjectId);
            
        }

        private void OnUnhover(SpriteUnhoverCommand command)
        {
            if (command.TargetObjectId.ObjectType == "Card")
            {
                _cardInteractionStrategy?.OnUnhover(command, this);
            }
        }

        private void OnDrop(SpriteDropCommand command)
        {
            if (UIStateMachine.CurrentState == UIStateMachine.UIState.Idle) return;
            _commandBus.Emit(new DisableUIInteractionCommand());
            _activeStrategy?.OnDrop(command, this);
        }

        private void OnDrag(SpriteDragCommand command)
        {
            if (UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingCard) return;
            _activeStrategy?.OnDrag(command, this);
        }

        private void OnEndDrag(SpriteEndDragCommand command)
        {
            if (UIStateMachine.CurrentState == UIStateMachine.UIState.Idle) return;
            _commandBus.Emit(new DisableUIInteractionCommand());
            _activeStrategy?.OnEndDrag(command, this);
            _activeStrategy = null;
        }

        private void OnReflowOperationCompleted(ReflowOperationCompletedCommand command)
        {
            Debug.Log("<color=Green>リフローオペレーション完了-></color>" + _currentReflowState);
            _currentReflowState = ReflowState.Idle;

            if (_nextHoverCommand != null)
            {
                Debug.Log("<color=Green>次のホバーコマンドあり-></color>");
                var commandToExecute = _nextHoverCommand;
                _nextHoverCommand = null;
                ExecuteHover(commandToExecute);
            }
            else
            {
                _commandBus.Emit(new EnableUIInteractionCommand());
            }
        }

        private void OnSpriteDragOperationCompleted(SpriteDragOperationCompletedCommand command)
        {
            _uiActivationPolicy.ResetToCardActivations(this);
            _uiActivationPolicy.ResetToCardSlotActivations(this);
            _uiStateMachine.SetState(UIStateMachine.UIState.Idle);
            _isDroppedSuccessfully = false;
            _commandBus.Emit(new EnableUIInteractionCommand());
        }

        private async void OnDragReflowCompleted(DragReflowCompletedCommand command)
        {
            UIStateMachine.SetState(UIStateMachine.UIState.DropedCardMove);
            var animationTasks = new List<UniTask>();
            foreach (var movement in command.CardMovements)
            {
                var cardView = _viewRegistry.GetView<CreatureCardView>(movement.Key);
                if (cardView != null)
                {
                    animationTasks.Add(cardView.MoveToAnimated(movement.Value));
                }
            }
            await UniTask.WhenAll(animationTasks);
            _commandBus.Emit(new ExecuteFrontLoadCommand());
        }

        private async void OnExecuteFrontLoad(ExecuteFrontLoadCommand command)
        {
            var frontLoadMovements = _reflowService.CalculateFrontLoadMovements(_draggedId);
            if (frontLoadMovements.Count > 0)
            {
                var animationTasks = new List<UniTask>();
                foreach (var movement in frontLoadMovements)
                {
                    var cardView = _viewRegistry.GetView<CreatureCardView>(movement.Key);
                    if (cardView != null)
                    {
                        animationTasks.Add(cardView.MoveToAnimated(movement.Value));
                    }
                }
                await UniTask.WhenAll(animationTasks);
            }
            _commandBus.Emit(new SpriteDragOperationCompletedCommand());
        }

        private async void OnReflowCompleted(ReflowCompletedCommand command)
        {
            if (command.CardMovements.Count > 0)
            {
                var animationTasks = new List<UniTask>();
                foreach (var movement in command.CardMovements)
                {
                    var cardView = _viewRegistry.GetView<CreatureCardView>(movement.Key);
                    if (cardView != null)
                    {
                        animationTasks.Add(cardView.MoveToAnimated(movement.Value));
                    }
                }
                await UniTask.WhenAll(animationTasks);
            }
            _commandBus.Emit(new ReflowOperationCompletedCommand());
        }
    }
}

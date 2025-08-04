using UnityEngine;
using VContainer;
using DG.Tweening;
using System;
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
            if (_cardInteractionStrategy.ChkCardBeginDrag(command, this))
            {
                UIStateMachine.SetState(UIStateMachine.UIState.DraggingCard);
                _draggedId = command.TargetObjectId;

                var draggedCardView = ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
                Debug.Log("<color=red>Card_OnBeginDrag-></color>" + draggedCardView._cardName + "->" + UIStateMachine.CurrentState + " Flg:" + IsDroppedSuccessfully);
                draggedCardView.EnterDraggingState();
                _uiActivationPolicy.DraggingCardToCardActivations(this);
                _uiActivationPolicy.DraggingCardToCardSlotActivations(this);
            }
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

            if (_cardInteractionStrategy.ChkCardHover(command, this))
            {
                // ホバーされたカードのViewを取得し、ホバー状態に遷移
                var cardView = ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
                cardView.EnterHoveringState();
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
            if (_cardInteractionStrategy.ChkCardUnhover(command, this))
            {
                // アンホバーされたカードのViewを取得し、通常状態に遷移
                var cardView = ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
                cardView.EnterNormalState();
            }
        }

        private void OnDrop(SpriteDropCommand command)
        {
            if (_cardInteractionStrategy.ChkCardDrop(command, this))
            {
                var draggedCardView = ViewRegistry.GetView<CreatureCardView>(DraggedId);
                Debug.Log("<color=red>Card_OnDrop-></color>" + draggedCardView._cardName + "->" + UIStateMachine.CurrentState + " Flg:" + IsDroppedSuccessfully);

                _commandBus.Emit(new DisableUIInteractionCommand());
                UIStateMachine.SetState(UIStateMachine.UIState.DropedCard);

                // カードスロットマネージャーにドロップ処理を依頼
                CardSlotManager.OnCardDroppedOnSlot(command.DroppedObjectId, command.TargetSlotObjectId);
                // ドロップが成功したことを示すフラグを設定
                IsDroppedSuccessfully = true;
            }
        }

        private void OnDrag(SpriteDragCommand command)
        {
            if (_cardInteractionStrategy.ChkCardDrag(command, this))
            {
                // アンホバーされたカードのViewを取得し、通常状態に遷移
                var draggedCardView = ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
                // ドラッグ中状態に遷移し、カードを新しい位置へ移動
                draggedCardView.EnterDraggingInProgressState();
                draggedCardView.MoveTo(command.NewPosition);
            }
        }

        private void OnEndDrag(SpriteEndDragCommand command)
        {
            if (_cardInteractionStrategy.ChkCardEndDrag(command, this))
            {
                _commandBus.Emit(new DisableUIInteractionCommand());
                CardEndDrag(command);
            }
        }

        /// <summary>
        /// カードのドラッグが終了したときに呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグ終了コマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public async void CardEndDrag(SpriteEndDragCommand command)
        {
            Debug.Log("<color=red>Card_OnEndDrag-></color>" + UIStateMachine.CurrentState + " Flg:" + IsDroppedSuccessfully);

            // UIステートをドロップ済みカードに設定
            UIStateMachine.SetState(UIStateMachine.UIState.DropedCard);

            // 遅延処理でドロップの成否を判定
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // ドロップが成功しなかった場合、リフローを元に戻す
            Debug.Log("Card_OnEndDrag->01秒後に実行するやつ:" + IsDroppedSuccessfully);
            if (!IsDroppedSuccessfully)
            {
                CardSlotManager.OnDropFailed();
            }
            // ドロップ成功フラグをリセット
            IsDroppedSuccessfully = false;
        }

        private void OnReflowOperationCompleted(ReflowOperationCompletedCommand command)
        {
            Debug.Log("<color=Green>リフローオペレーション完了-></color>" + _currentReflowState);
            _currentReflowState = ReflowState.Idle;

            if (_nextHoverCommand != null && UIStateMachine.CurrentState == UIStateMachine.UIState.DraggingCard)
                {
                    Debug.Log("<color=Green>次のホバーコマンドあり-></color>");
                    var commandToExecute = _nextHoverCommand;
                    _nextHoverCommand = null;
                    ExecuteHover(commandToExecute);
                }
                else
                {
                    _nextHoverCommand = null;
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

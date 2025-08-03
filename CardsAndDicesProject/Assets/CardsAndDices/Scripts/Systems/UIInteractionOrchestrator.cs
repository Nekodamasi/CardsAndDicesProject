using UnityEngine;
using VContainer;
using DG.Tweening;

namespace CardsAndDice
{
    [CreateAssetMenu(fileName = "UIInteractionOrchestrator", menuName = "CardsAndDice/Systems/UIInteractionOrchestrator")]
    public class UIInteractionOrchestrator : ScriptableObject
    {
        [Header("Dependencies")]
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private CardSlotManager _cardSlotManager;
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private ReflowService _reflowService;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;

        [Header("Strategies")]
        [SerializeField] private CardInteractionStrategy _cardInteractionStrategy;
        // Add DiceInteractionStrategy here in the future

        [Header("Settings")]
        [SerializeField] protected float _returnToOriginalPositionDelay = 0.1f;

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


        /// <summary>
        /// UIInteractionOrchestratorを破棄します。
        /// </summary>
        public void Dispose()
        {
            // UIイベントの購読解除
            if (_commandBus == null) return;
            _commandBus.Off<SpriteBeginDragCommand>(OnBeginDrag);
            _commandBus.Off<SpriteHoverCommand>(OnHover);
            _commandBus.Off<SpriteUnhoverCommand>(OnUnhover);
            _commandBus.Off<SpriteDropCommand>(OnDrop);
            _commandBus.Off<SpriteDragCommand>(OnDrag);
            _commandBus.Off<SpriteEndDragCommand>(OnEndDrag);
            _commandBus.Off<ReflowOperationCompletedCommand>(OnReflowOperationCompleted);
            _commandBus.Off<SpriteDragOperationCompletedCommand>(OnSpriteDragOperationCompleted);
            _commandBus.Off<ReflowCompletedCommand>(OnReflowCompleted); // ReflowCompletedCommandの購読解除を追加
            _commandBus.Off<DragReflowCompletedCommand>(OnDragReflowCompleted); // ReflowCompletedCommandの購読解除を追加
            _commandBus.Off<ExecuteFrontLoadCommand>(OnExecuteFrontLoad); // Add this
        }

        /// <summary>
        /// Viewをレジストリに登録します。
        /// </summary>
        public void RegisterView(BaseSpriteView view)
        {
            Debug.Log("とうろくーーー！" + view.GetObjectId());
            _viewRegistry.Register(view);
        }

        /// <summary>
        /// Viewをレジストリから登録解除します。
        /// </summary>
        public void UnregisterView(BaseSpriteView view)
        {
            _viewRegistry.Unregister(view);
        }

        /// <summary>
        /// ドラッグ開始コマンドの処理
        /// </summary>
        private void OnBeginDrag(SpriteBeginDragCommand command)
        {
            if(UIStateMachine.CurrentState != UIStateMachine.UIState.Idle) return;
            _draggedId = command.TargetObjectId;
            
            // Determine the strategy based on the dragged object type
            if (command.TargetObjectId.ObjectType == "Card")
            {
                _activeStrategy = _cardInteractionStrategy;
            }
            // else if (command.TargetObjectId.ObjectType == "Dice")
            // {
            //     _activeStrategy = _diceInteractionStrategy;
            // }

            _activeStrategy?.OnBeginDrag(command, this);
            _uiActivationPolicy.DraggingCardToCardActivations(this);
            _uiActivationPolicy.DraggingCardToCardSlotActivations(this);
        }

        /// <summary>
        /// ホバー開始コマンド
        /// </summary>
        private void OnHover(SpriteHoverCommand command)
        {
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.DropedCard) return;
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.Reflow) return;

            if (command.TargetObjectId.ObjectType == "Card")
            {
                _cardInteractionStrategy?.OnHover(command, this);
            }
            if (command.TargetObjectId.ObjectType == "CardSlot")
            {
                _cardInteractionStrategy?.OnHover(command, this);
            }
        }

        /// <summary>
        /// アンホバー開始コマンド
        /// </summary>
        private void OnUnhover(SpriteUnhoverCommand command)
        {
            if (command.TargetObjectId.ObjectType == "Card")
            {
                _cardInteractionStrategy?.OnUnhover(command, this);
            }
        }

        /// <summary>
        /// ドロップ開始コマンド
        /// </summary>
        private void OnDrop(SpriteDropCommand command)
        {
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.Idle) return;
            // 「UI操作制限モード」ON
            _commandBus.Emit(new DisableUIInteractionCommand());

            _activeStrategy?.OnDrop(command, this);
        }

        /// <summary>
        /// ドラッグ中コマンド
        /// </summary>
        private void OnDrag(SpriteDragCommand command)
        {
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.Reflow) return;
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.Idle) return;
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.DropedCard) return;
            _activeStrategy?.OnDrag(command, this);
        }

        /// <summary>
        /// ドラッグ完了コマンド
        /// </summary>
        private void OnEndDrag(SpriteEndDragCommand command)
        {
            if(UIStateMachine.CurrentState == UIStateMachine.UIState.Idle) return;

            // 「UI操作制限モード」ON
            _commandBus.Emit(new DisableUIInteractionCommand());

            _activeStrategy?.OnEndDrag(command, this);
            _activeStrategy = null;
//            _uiActivationPolicy.UpdateActivations(this);
        }

        /// <summary>
        /// リフローオペレーション完了
        /// </summary>
        private void OnReflowOperationCompleted(ReflowOperationCompletedCommand command)
        {
//            var cardView = _viewRegistry.GetView<CreatureCardView>(_draggedId);
            Debug.Log($"<color=red>リフローオペレーション完了</color>:" + UIStateMachine.CurrentState);
            UIStateMachine.SetState(UIStateMachine.UIState.DraggingCard);

            // 「UI操作制限モード」OFF
            _commandBus.Emit(new EnableUIInteractionCommand());
        }

        /// <summary>
        /// ドラッグオペレーション完了
        /// </summary>
        private void OnSpriteDragOperationCompleted(SpriteDragOperationCompletedCommand command)
        {
            var cardView = _viewRegistry.GetView<CreatureCardView>(_draggedId);
            Debug.Log($"<color=red>ドラッグオペレーション完了</color>:" + cardView._cardName + "->" + UIStateMachine.CurrentState);

            //            var targetView = _viewRegistry.GetView<BaseSpriteView>(_draggedId);

            // CreatureCardViewをアクティブ状態にする
            _uiActivationPolicy.ResetToCardActivations(this);
            _uiActivationPolicy.ResetToCardSlotActivations(this);

            _uiStateMachine.SetState(UIStateMachine.UIState.Idle);
            IsDroppedSuccessfully = false;

            // 「UI操作制限モード」OFF
            _commandBus.Emit(new EnableUIInteractionCommand());
        }

        /// <summary>
        /// ドラッグリフロー完了
        /// </summary>
        private void OnDragReflowCompleted(DragReflowCompletedCommand command)
        {
            Debug.Log("ドラッグリフロー完了");
            UIStateMachine.SetState(UIStateMachine.UIState.DropedCardMove);
            foreach (var movement in command.CardMovements)
            {
                var cardId = movement.Key;
                var targetPosition = movement.Value;

                var cardView = _viewRegistry.GetView<CreatureCardView>(cardId);
                if (cardView != null)
                {
                    if (_draggedId is null)
                    {

                        Debug.Log("入れ替え----------------------------------------------");
                        _draggedId = cardId;
                    }

                    // ドラッグしたカードだけExecuteFrontLoadCommandを呼ぶ
                    if (_draggedId == cardId)
                    {
                        cardView.MoveToAnimated(targetPosition, new ExecuteFrontLoadCommand());
                    }
                    else
                    {
                        cardView.MoveToAnimated(targetPosition, null);
                    }
//                        cardView.MoveToAnimated2(targetPosition, new SpriteDragOperationCompletedCommand(_draggedId, _isDroppedSuccessfully));
                }
            }
        }

        /// <summary>
        /// 前詰め処理開始
        /// </summary>
        private void OnExecuteFrontLoad(ExecuteFrontLoadCommand command)
        {
            Debug.Log("前詰め処理開始");
            var frontLoadMovements = _reflowService.CalculateFrontLoadMovements(_draggedId);
            if (frontLoadMovements.Count > 0)
            {
                foreach (var movement in frontLoadMovements)
                {
                    var cardId = movement.Key;
                    var targetPosition = movement.Value;

                    var cardView = _viewRegistry.GetView<CreatureCardView>(cardId);
                    if (cardView != null)
                    {
                        // ドラッグカードだけSpriteDragOperationCompletedCommandを呼ぶ
                        if (_draggedId == cardId)
                        {
                            cardView.MoveToAnimated(targetPosition, new SpriteDragOperationCompletedCommand());
                        }
                        else
                        {
                            cardView.MoveToAnimated(targetPosition, null);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("前詰め処理０件");
                _commandBus.Emit(new SpriteDragOperationCompletedCommand());
            }

        }

        /// <summary>
        /// リフロー完了
        /// </summary>
        private void OnReflowCompleted(ReflowCompletedCommand command)
        {
            Debug.Log("リフロー完了");

            // 「UI操作制限モード」ON
            _commandBus.Emit(new DisableUIInteractionCommand());

            // リフロー対象なし
            if (command.CardMovements.Count == 0)
            {
                Debug.Log("リフロー対象なし");
                _commandBus.Emit(new ReflowOperationCompletedCommand());
            }


            foreach (var movement in command.CardMovements)
            {
                var cardId = movement.Key;
                var targetPosition = movement.Value;

                var cardView = _viewRegistry.GetView<CreatureCardView>(cardId);
                if (cardView != null)
                {
                    cardView.MoveToAnimated(targetPosition, new ReflowOperationCompletedCommand());
                }
            }
        }
    }
}

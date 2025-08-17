using UnityEngine;
using VContainer;
using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "DiceInteractionOrchestrator", menuName = "CardsAndDices/Orchestrators/DiceInteractionOrchestrator")]
    public class DiceInteractionOrchestrator : ScriptableObject, IUIInteractionOrchestrator
    {
        [Header("Dependencies")]
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private DiceSlotManager _diceSlotManager;
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;
        [SerializeField] private DiceInteractionStrategy _diceInteractionStrategy;
        [SerializeField] private ViewRegistry _viewRegistry;

        private CompositeObjectId _draggedId;
        public ViewRegistry ViewRegistry => _viewRegistry;
        public UIStateMachine UIStateMachine => _uiStateMachine;
        public DiceSlotManager DiceSlotManager => _diceSlotManager;
        public CompositeObjectId DraggedId => _draggedId;
        private bool _isDroppedSuccessfully;
        public bool IsDroppedSuccessfully { get => _isDroppedSuccessfully; set => _isDroppedSuccessfully = value; }

        [Inject]
        public void Initialize(UIStateMachine uiStateMachine, DiceSlotManager diceSlotManager, SpriteCommandBus commandBus, UIActivationPolicy uiActivationPolicy, DiceInteractionStrategy diceInteractionStrategy, ViewRegistry viewRegistry)
        {
            _uiStateMachine = uiStateMachine;
            _commandBus = commandBus;
            _diceInteractionStrategy = diceInteractionStrategy;
            _viewRegistry = viewRegistry;
            _diceSlotManager = diceSlotManager;
            _uiActivationPolicy = uiActivationPolicy;

            _commandBus.On<SpriteBeginDragCommand>(OnBeginDrag);
            _commandBus.On<SpriteHoverCommand>(OnHover);
            _commandBus.On<SpriteUnhoverCommand>(OnUnhover);
            _commandBus.On<SpriteDropCommand>(OnDrop);
            _commandBus.On<SpriteDragCommand>(OnDrag);
            _commandBus.On<SpriteEndDragCommand>(OnEndDrag);
            _commandBus.On<DragReflowCompletedCommand>(OnDragReflowCompleted);
            _commandBus.On<ExecuteFrontLoadCommand>(OnExecuteFrontLoad);
            _commandBus.On<SpriteDragOperationCompletedCommand>(OnSpriteDragOperationCompleted);
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
            _commandBus.Off<DragReflowCompletedCommand>(OnDragReflowCompleted);
            _commandBus.Off<ExecuteFrontLoadCommand>(OnExecuteFrontLoad);
            _commandBus.On<SpriteDragOperationCompletedCommand>(OnSpriteDragOperationCompleted);
        }

        public void RegisterView(BaseSpriteView view)
        {
            _viewRegistry.Register(view);
        }

        public void UnregisterView(BaseSpriteView view)
        {
            _viewRegistry.Unregister(view);
        }

        private void OnHover(SpriteHoverCommand command)
        {
            Debug.Log("<color=red>OnHoverここはきてる？</color>");
            if (_diceInteractionStrategy.ChkDiceHover(command, this))
            {
                // ホバーされたカードのViewを取得し、ホバー状態に遷移
                var diceView = ViewRegistry.GetView<DiceView>(command.TargetObjectId);
                diceView.EnterHoveringState();
            }
        }

        private void OnUnhover(SpriteUnhoverCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceUnhover(command, this))
            {
                // アンホバーされたカードのViewを取得し、通常状態に遷移
                var diceView = ViewRegistry.GetView<DiceView>(command.TargetObjectId);
                diceView.EnterNormalState();
            }
        }

        private void OnBeginDrag(SpriteBeginDragCommand command)
        {
            Debug.Log("<color=red>ここはきてる？</color>");
            if (_diceInteractionStrategy.ChkDiceBeginDrag(command, this))
            {
                UIStateMachine.SetState(UIStateMachine.UIState.DraggingDice);
                _draggedId = command.TargetObjectId;

                var draggedView = ViewRegistry.GetView<DiceView>(command.TargetObjectId);
                draggedView.EnterDraggingState();
                _uiActivationPolicy.DraggingDiceToDiceActivations(this);
                _uiActivationPolicy.DraggingDiceToInletActivations(this);
//                _uiActivationPolicy.DraggingCardToCardSlotActivations(this);
            }
        }

        private void OnDrop(SpriteDropCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceDrop(command, this))
            {
                var draggedView = ViewRegistry.GetView<DiceView>(DraggedId);

                _commandBus.Emit(new DisableUIInteractionCommand());
                UIStateMachine.SetState(UIStateMachine.UIState.DropedDice);

                // ダイススロットマネージャーにドロップ処理を依頼
                DiceSlotManager.OnDiceDroppedOnSlot(command.DroppedObjectId, command.TargetSlotObjectId);
                // ドロップが成功したことを示すフラグを設定
                IsDroppedSuccessfully = true;
            }
        }

        private void OnDrag(SpriteDragCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceDrag(command, this))
            {
                // ドラッグ中のダイスのViewを取得し、ドラッグ中状態に移行
                var draggedDiceView = ViewRegistry.GetView<DiceView>(command.TargetObjectId);
                draggedDiceView.EnterDraggingInProgressState();

                //カードを新しい位置へ移動
                draggedDiceView.MoveTo(command.NewPosition);
            }
        }

        private void OnEndDrag(SpriteEndDragCommand command)
        {
            Debug.Log("<color=red>OnEndDragここはきてる？</color>");
            if (_diceInteractionStrategy.ChkDiceEndDrag(command, this))
            {
                _commandBus.Emit(new DisableUIInteractionCommand());
                DiceEndDrag(command);
            }
        }

        /// <summary>
        /// カードのドラッグが終了したときに呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグ終了コマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public async void DiceEndDrag(SpriteEndDragCommand command)
        {
            Debug.Log("<color=red>Card_OnEndDrag-></color>" + UIStateMachine.CurrentState + " Flg:" + IsDroppedSuccessfully);

            // UIステートをドロップ済みカードに設定
            UIStateMachine.SetState(UIStateMachine.UIState.DropedDice);

            // 遅延処理でドロップの成否を判定
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // ドロップが成功しなかった場合、リフローを元に戻す
            Debug.Log("Card_OnEndDrag->01秒後に実行するやつ:" + IsDroppedSuccessfully);
            if (!IsDroppedSuccessfully)
            {
                DiceSlotManager.OnDropFailed();
            }
            // ドロップ成功フラグをリセット
            IsDroppedSuccessfully = false;
        }
        private async void OnDragReflowCompleted(DragReflowCompletedCommand command)
        {
            if (UIStateMachine.CurrentState != UIStateMachine.UIState.DropedDice) return;
            UIStateMachine.SetState(UIStateMachine.UIState.DropedDiceMove);
            var animationTasks = new List<UniTask>();
            foreach (var movement in command.Movements)
            {
                var diceView = _viewRegistry.GetView<DiceView>(movement.Key);
                if (diceView != null)
                {
                    animationTasks.Add(diceView.MoveToAnimated(movement.Value));
                }
            }
            await UniTask.WhenAll(animationTasks);
            _commandBus.Emit(new ExecuteFrontLoadCommand());
        }
        private async void OnExecuteFrontLoad(ExecuteFrontLoadCommand command)
        {
            if (UIStateMachine.CurrentState != UIStateMachine.UIState.DropedDiceMove) return;

            // 0.5秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            _commandBus.Emit(new SpriteDragOperationCompletedCommand());
        }
        private void OnSpriteDragOperationCompleted(SpriteDragOperationCompletedCommand command)
        {
            Debug.Log("<color=red>OnSpriteDragOperationCompletedここはきてる？:</color>" + UIStateMachine.CurrentState);
            if (UIStateMachine.CurrentState != UIStateMachine.UIState.DropedDiceMove) return;
            _uiActivationPolicy.ResetToDiceActivations(this);
            _uiStateMachine.SetState(UIStateMachine.UIState.Idle);
            _isDroppedSuccessfully = false;
            _commandBus.Emit(new EnableUIInteractionCommand());
        }
    }
}
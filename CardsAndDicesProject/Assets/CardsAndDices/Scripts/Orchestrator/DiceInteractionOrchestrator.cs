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
        [SerializeField] private Old_DiceSlotManager _diceSlotManager;
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private UIActivationPolicy _uiActivationPolicy;
        [SerializeField] private DiceInteractionStrategy _diceInteractionStrategy;
        [SerializeField] private ViewRegistry _viewRegistry;
        [SerializeField] private DiceManager _diceManager;

        private CompositeObjectId _draggedId;
        public ViewRegistry ViewRegistry => _viewRegistry;
        public UIStateMachine UIStateMachine => _uiStateMachine;
        public Old_DiceSlotManager DiceSlotManager => _diceSlotManager;
        public CompositeObjectId DraggedId => _draggedId;
        private bool _isDroppedSuccessfully;
        public bool IsDroppedSuccessfully { get => _isDroppedSuccessfully; set => _isDroppedSuccessfully = value; }

        [Inject]
        public void Initialize(UIStateMachine uiStateMachine, Old_DiceSlotManager diceSlotManager, SpriteCommandBus commandBus, UIActivationPolicy uiActivationPolicy, DiceInteractionStrategy diceInteractionStrategy, ViewRegistry viewRegistry, DiceManager diceManager)
        {
            _uiStateMachine = uiStateMachine;
            _commandBus = commandBus;
            _diceInteractionStrategy = diceInteractionStrategy;
            _viewRegistry = viewRegistry;
            _diceSlotManager = diceSlotManager;
            _uiActivationPolicy = uiActivationPolicy;
            _diceManager = diceManager;
            IsDroppedSuccessfully = false;

            _commandBus.On<IdentifiableBeginDragCommand>(OnBeginDrag);
            _commandBus.On<IdentifiableHoverCommand>(OnHover);
            _commandBus.On<IdentifiableUnhoverCommand>(OnUnhover);
            _commandBus.On<IdentifiableDropCommand>(OnDrop);
            _commandBus.On<IdentifiableDragCommand>(OnDrag);
            _commandBus.On<IdentifiableEndDragCommand>(OnEndDrag);
            _commandBus.On<DragReflowCompletedCommand>(OnDragReflowCompleted);
            _commandBus.On<ExecuteFrontLoadCommand>(OnExecuteFrontLoad);
            _commandBus.On<SpriteDragOperationCompletedCommand>(OnSpriteDragOperationCompleted);
            _commandBus.On<DiceInletCountdownCompleteCommand>(OnDiceInletCountdownComplete);
        }

        public void Dispose()
        {
            if (_commandBus == null) return;
            _commandBus.Off<IdentifiableBeginDragCommand>(OnBeginDrag);
            _commandBus.Off<IdentifiableHoverCommand>(OnHover);
            _commandBus.Off<IdentifiableUnhoverCommand>(OnUnhover);
            _commandBus.Off<IdentifiableDropCommand>(OnDrop);
            _commandBus.Off<IdentifiableDragCommand>(OnDrag);
            _commandBus.Off<IdentifiableEndDragCommand>(OnEndDrag);
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

        private void OnHover(IdentifiableHoverCommand command)
        {
//            Debug.Log("<color=red>OnHoverここはきてる？</color>");
            if (_diceInteractionStrategy.ChkDiceHover(command, this))
            {
                // ホバーされたカードのViewを取得し、ホバー状態に遷移
                var diceView = ViewRegistry.GetView<Old_DiceView>(command.ExecutedObjectId);
                diceView.EnterHoveringState();
            }
        }

        private void OnUnhover(IdentifiableUnhoverCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceUnhover(command, this))
            {
                // アンホバーされたカードのViewを取得し、通常状態に遷移
                var diceView = ViewRegistry.GetView<Old_DiceView>(command.ExecutedObjectId);
                diceView.EnterNormalState();
            }
        }

        private void OnBeginDrag(IdentifiableBeginDragCommand command)
        {
            Debug.Log("<color=red>ここはきてる？</color>");
            if (_diceInteractionStrategy.ChkDiceBeginDrag(command, this))
            {
                UIStateMachine.SetState(UIStateMachine.UIState.DraggingDice);
                _draggedId = command.ExecutedObjectId;

                var draggedView = ViewRegistry.GetView<Old_DiceView>(command.ExecutedObjectId);
                draggedView.EnterDraggingState();
                _uiActivationPolicy.DraggingDiceToDiceActivations(this);
                _uiActivationPolicy.DraggingDiceToInletActivations(this);
//                _uiActivationPolicy.DraggingCardToCardSlotActivations(this);
            }
        }

        /// <summary>
        /// ダイスをドロップしたときに呼び出されます。
        /// </summary>
        /// <param name="command">ドロップコマンド。</param>
        private void OnDrop(IdentifiableDropCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceDrop(command, this))
            {
                _commandBus.Emit(new DisableUIInteractionCommand());
                UIStateMachine.SetState(UIStateMachine.UIState.DropedDice);

                // ダイススロットマネージャーにドロップ処理を依頼
//                _commandBus.Emit(new DiceDropInInletCommand(command.ExecutedObjectId, DraggedId, _diceManager.GetDiceData(DraggedId).FaceValue));
                // ドロップが成功したことを示すフラグを設定
                IsDroppedSuccessfully = true;
            }
        }

        private void OnDrag(IdentifiableDragCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceDrag(command, this))
            {
                // ドラッグ中のダイスのViewを取得し、ドラッグ中状態に移行
                var draggedDiceView = ViewRegistry.GetView<Old_DiceView>(command.ExecutedObjectId);
                draggedDiceView.EnterDraggingInProgressState();

                //カードを新しい位置へ移動
                draggedDiceView.MoveTo(command.NewPosition);
            }
        }

        /// <summary>
        /// ダイスのドラッグが終了したときに呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグ終了コマンド。</param>
        private void OnEndDrag(IdentifiableEndDragCommand command)
        {
//            Debug.Log("<color=red>OnEndDragここはきてる？</color>");
            if (_diceInteractionStrategy.ChkDiceEndDrag(command, this))
            {
                _commandBus.Emit(new DisableUIInteractionCommand());
                DiceEndDrag(command);
            }
        }

        /// <summary>
        /// ドロップが成功しなかった場合、リフローを元に戻す処理を行います
        /// </summary>
        /// <param name="command">ドラッグ終了コマンド。</param>
        public async void DiceEndDrag(IdentifiableEndDragCommand command)
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
        private void OnDiceInletCountdownComplete(DiceInletCountdownCompleteCommand command)
        {
            DiceSlotManager.OnDiceDroppedOnInlet();
        }
        
        private async void OnDragReflowCompleted(DragReflowCompletedCommand command)
        {
            if (UIStateMachine.CurrentState != UIStateMachine.UIState.DropedDice) return;
            UIStateMachine.SetState(UIStateMachine.UIState.DropedDiceMove);
            var animationTasks = new List<UniTask>();
            foreach (var movement in command.Movements)
            {
                var diceView = _viewRegistry.GetView<Old_DiceView>(movement.Key);
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
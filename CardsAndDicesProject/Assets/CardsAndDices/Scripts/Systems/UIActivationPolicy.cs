using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "UIActivationPolicy", menuName = "CardsAndDices/Systems/UIActivationPolicy")]
    public class UIActivationPolicy : ScriptableObject
    {
        private DiceInletAbilityRegistry _diceInletAbilityRegistry;
        private DiceManager _diceManager;

        [Inject]
        public void Initialize(DiceInletAbilityRegistry diceInletAbilityRegistry, DiceManager diceManager)
        {
            _diceInletAbilityRegistry = diceInletAbilityRegistry;
            _diceManager = diceManager;
        }

        /// <summary>
        /// カードスロットの状態変化（ドラッグ開始）
        /// </summary>
        public void DraggingCardToCardSlotActivations(CardInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingCard) return;

            var draggedCardSlot = orchestrator.CardSlotManager.GetSlotDataByReflowPlacedCardId(orchestrator.DraggedId);
            bool isFromHand = draggedCardSlot != null && draggedCardSlot.Line == LinePosition.Hand;

            foreach (var slotView in orchestrator.ViewRegistry.GetAllSlotViews())
            {
                // ハンドスロットの場合
                if (slotView.Line == LinePosition.Hand)
                {
                    // ハンドスロット以外のカードをドラッグ中の場合、受け入れ状態、それ以外は非活性
                    if (!isFromHand) slotView.EnterAcceptableState();
                    else slotView.EnterInactiveState();
                }
                else if (slotView.Team == Team.Enemy)
                {
                    slotView.EnterInactiveState();
                }
                else
                {
                    slotView.EnterAcceptableState();
                }
            }
        }

        /// <summary>
        /// ダイスインレットの状態変化（ダイスドラッグ開始）
        /// </summary>
        public void DraggingDiceToInletActivations(DiceInteractionOrchestrator orchestrator)
        {
            Debug.Log("<color=red>DraggingDiceToInletActivations:</color>"+ orchestrator.UIStateMachine.CurrentState);
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingDice) return;

            Debug.Log("<color=Green>DraggingDiceToInletActivations2:</color>"+ orchestrator.DraggedId);
            var draggedDice = _diceManager.GetDiceData(orchestrator.DraggedId);
            if (draggedDice == null) return;

            Debug.Log("<color=Green>DraggingDiceToInletActivations3:</color>"+ orchestrator.DraggedId);
            foreach (var inletView in orchestrator.ViewRegistry.GetAllInletViews())
            {
                var profile = _diceInletAbilityRegistry.GetProfile(inletView.GetObjectId());
                Debug.Log("<color=red>DraggingDiceToInletActivations3:</color>");
                if (profile?.Condition != null && profile.Condition.CanAccept(draggedDice))
                {
                    Debug.Log("<color=red>インレットActiveです</color>");
                    inletView.EnterAcceptableState();
                }
                else
                {
                    Debug.Log("<color=red>インレット非Activeです</color>");
                    inletView.EnterInactiveState();
                }
            }
        }

        /// <summary>
        /// カードスロットの状態変化（リセット）
        /// </summary>
        public void ResetToCardSlotActivations(CardInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DropedCardMove) return;

            foreach (var slotView in orchestrator.ViewRegistry.GetAllSlotViews())
            {
                slotView.EnterInactiveState();
            }
        }

        /// <summary>
        /// カードの状態変化（ドラッグ開始）
        /// </summary>
        public void DraggingCardToCardActivations(CardInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingCard) return;

            var state = orchestrator.UIStateMachine.CurrentState;
            var draggedId = orchestrator.DraggedId;

            foreach (var cardView in orchestrator.ViewRegistry.GetAllCreatureCardViews())
            {
                if (cardView.GetObjectId() != draggedId) cardView.EnterInactiveState();
            }
        }

        /// <summary>
        /// ダイスの状態変化（ドラッグ開始）
        /// </summary>
        public void DraggingDiceToDiceActivations(DiceInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingDice) return;

            var state = orchestrator.UIStateMachine.CurrentState;
            var draggedId = orchestrator.DraggedId;

            foreach (var diceView in orchestrator.ViewRegistry.GetAllDiceViews())
            {
                if (diceView.GetObjectId() != draggedId) diceView.EnterInactiveState();
            }
        }

        /// <summary>
        /// カードの状態変化（リセット）
        /// </summary>
        public void ResetToCardActivations(CardInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DropedCardMove) return;
            Debug.Log("ResetToCardActivations");
            var state = orchestrator.UIStateMachine.CurrentState;
            var draggedId = orchestrator.DraggedId;

            foreach (var cardView in orchestrator.ViewRegistry.GetAllCreatureCardViews())
            {
                cardView.EnterNormalState();
            }
        }
        /// <summary>
        /// ダイスの状態変化（リセット）
        /// </summary>
        public void ResetToDiceActivations(DiceInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DropedDiceMove) return;
            Debug.Log("ResetToDiceActivations");
            var state = orchestrator.UIStateMachine.CurrentState;
            var draggedId = orchestrator.DraggedId;

            foreach (var cardView in orchestrator.ViewRegistry.GetAllCreatureCardViews())
            {
                cardView.EnterNormalState();
            }
            foreach (var diceView in orchestrator.ViewRegistry.GetAllDiceViews())
            {
                Debug.Log("ダイスがあるのか？");
                diceView.EnterNormalState();
            }
            foreach (var slotView in orchestrator.ViewRegistry.GetAllSlotViews())
            {
                slotView.EnterInactiveState();
            }
        }
    }
}

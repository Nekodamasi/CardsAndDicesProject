using UnityEngine;
using VContainer;

namespace CardsAndDice
{
    [CreateAssetMenu(fileName = "UIActivationPolicy", menuName = "CardsAndDice/Systems/UIActivationPolicy")]
    public class UIActivationPolicy : ScriptableObject
    {
        [Inject]
        public void Initialize()
        {
        }

        /// <summary>
        /// カードスロットの状態変化（ドラッグ開始）
        /// </summary>
        public void DraggingCardToCardSlotActivations(UIInteractionOrchestrator orchestrator)
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
//                    Debug.Log("ちゃんと動いてる？");
                    slotView.EnterAcceptableState();
                }
            }
        }

        /// <summary>
        /// カードスロットの状態変化（リセット）
        /// </summary>
        public void ResetToCardSlotActivations(UIInteractionOrchestrator orchestrator)
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
        public void DraggingCardToCardActivations(UIInteractionOrchestrator orchestrator)
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
        /// カードの状態変化（リセット）
        /// </summary>
        public void ResetToCardActivations(UIInteractionOrchestrator orchestrator)
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
    }
}

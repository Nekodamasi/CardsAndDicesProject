using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "DiceInteractionStrategy", menuName = "CardsAndDices/InteractionStrategies/DiceInteractionStrategy")]
    public class DiceInteractionStrategy : ScriptableObject
    {
        public bool ChkDiceHover(IdentifiableHoverCommand command, DiceInteractionOrchestrator orchestrator)
        {
            // UIがアイドル状態の場合
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.Idle)
            {
                // ホバーされたダイスのViewを取得し、ホバー状態に遷移
                var diceView = orchestrator.ViewRegistry.GetView<DiceView>(command.ExecutedObjectId);
                if (diceView != null) return true;
            }
            return false;
        }

        /// <summary>
        /// ダイスがアンホバーされる条件を満たしているかチェックします
        /// </summary>
        /// <param name="command">アンホバーコマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public bool ChkDiceUnhover(IdentifiableUnhoverCommand command, DiceInteractionOrchestrator orchestrator)
        {
//            Debug.Log("ChkDiceUnhover");

            // UIがアイドル状態の場合
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.Idle)
            {
                // ホバーされたダイスのViewを取得し、通常状態に遷移
                var diceView = orchestrator.ViewRegistry.GetView<DiceView>(command.ExecutedObjectId);
                if (diceView != null) return true;
            }
            return false;
        }

        public bool ChkDiceBeginDrag(IdentifiableBeginDragCommand command, DiceInteractionOrchestrator orchestrator)
        {
            // UIがアイドルの場合
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.Idle) return false;
            var draggedDiceView = orchestrator.ViewRegistry.GetView<DiceView>(command.ExecutedObjectId);
            if (draggedDiceView == null) return false;
            return true;
        }

        public bool ChkDiceDrop(IdentifiableDropCommand command, DiceInteractionOrchestrator orchestrator)
        {
            // UIがダイスドラッグ中の場合のみ処理
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingDice) return false;
            return true;
        }

        /// <summary>
        /// ダイスがドラッグ中か判定します
        /// </summary>
        /// <param name="command">ドラッグコマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public bool ChkDiceDrag(IdentifiableDragCommand command, DiceInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingDice) return false;
            var draggedView = orchestrator.ViewRegistry.GetView<DiceView>(orchestrator.DraggedId);
            if (draggedView == null) return false;
            return true;
        }

        /// <summary>
        /// ダイスのドラッグが終了したときに呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグ終了コマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public bool ChkDiceEndDrag(IdentifiableEndDragCommand command, DiceInteractionOrchestrator orchestrator)
        {
            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingDice) return false;
            return true;
        }
    }
}

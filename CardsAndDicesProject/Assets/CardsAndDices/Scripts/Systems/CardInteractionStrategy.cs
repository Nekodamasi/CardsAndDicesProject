using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用するために追加
using System;
using VContainer;

namespace CardsAndDice
{
    /// <summary>
    /// カードのUIインタラクション戦略を実装するクラスです。
    /// ドラッグ、ホバー、ドロップなどのカード固有のUI操作ロジックをカプセル化します。
    /// </summary>
    [CreateAssetMenu(fileName = "CardInteractionStrategy", menuName = "CardsAndDice/InteractionStrategies/CardInteractionStrategy")]
    public class CardInteractionStrategy : ScriptableObject, IInteractionStrategy
    {
        [Inject]
        public void Initialize()
        {
        }
        public bool ChkCardSlotHover(SpriteHoverCommand command, UIInteractionOrchestrator orchestrator)
        {
            // UIがカードドラッグ中の場合
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.DraggingCard)
            {
                // ホバーされたスロットのViewを取得
                var cardSlotView = orchestrator.ViewRegistry.GetView<CardSlotView>(command.TargetObjectId);
                if (cardSlotView != null)
                {
                    // スロットのデータとリフロー配置カードIDを確認
                    var slotData = orchestrator.CardSlotManager.GetSlotData(command.TargetObjectId);
                    Debug.Log("<color=green>ホバーされたカードスロット：</color>" + slotData.Line + "_" + slotData.Location);
                    if (slotData.ReflowPlacedCardId == orchestrator.DraggedId) return false; // 同じカードが既にリフロー配置されている場合は何もしない
                    Debug.Log("<color=green>リフローに進んだカードスロット：</color>" + slotData.Line + "_" + slotData.Location);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// カードのドラッグ開始時に呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグ開始コマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public void OnBeginDrag(SpriteBeginDragCommand command, UIInteractionOrchestrator orchestrator)
        {
            var draggedCardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
            Debug.Log("<color=red>Card_OnBeginDrag-></color>" + draggedCardView._cardName + "->" + orchestrator.UIStateMachine.CurrentState + " Flg:" + orchestrator.IsDroppedSuccessfully);

            orchestrator.UIStateMachine.SetState(UIStateMachine.UIState.DraggingCard);

            // ドラッグ中のカードのViewを取得
//            var draggedCardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);

            // ドラッグ開始状態に遷移
            draggedCardView.EnterDraggingState();
        }

        /// <summary>
        /// カードがホバーされたときに呼び出されます。
        /// ドラッグ中かアイドル状態かで異なる処理を行います。
        /// </summary>
        /// <param name="command">ホバーコマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public void OnHover(SpriteHoverCommand command, UIInteractionOrchestrator orchestrator)
        {
            Debug.Log("Card_OnHover");

            // UIがカードドラッグ中の場合
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.DraggingCard)
            {
                // ホバーされたスロットのViewを取得
                var cardSlotView = orchestrator.ViewRegistry.GetView<CardSlotView>(command.TargetObjectId);
                if (cardSlotView != null)
                {
                    // スロットのデータとリフロー配置カードIDを確認
                    var slotData = orchestrator.CardSlotManager.GetSlotData(command.TargetObjectId);
                    Debug.Log("<color=green>ホバーされたカードスロット：</color>" + slotData.Line + "_" + slotData.Location);
                    if (slotData.ReflowPlacedCardId == orchestrator.DraggedId) return; // 同じカードが既にリフロー配置されている場合は何もしない
                    Debug.Log("<color=green>リフローに進んだカードスロット：</color>" + slotData.Line + "_" + slotData.Location);

                    // ホバーリフローを実行
                    orchestrator.CardSlotManager.OnCardHoveredOnSlot(orchestrator.DraggedId, command.TargetObjectId);
                }
            }
            // UIがアイドル状態の場合
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.Idle)
            {
                // ホバーされたカードのViewを取得し、ホバー状態に遷移
                var cardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
                if (cardView != null) cardView.EnterHoveringState();
            }
        }

        /// <summary>
        /// カードのホバーが解除されたときに呼び出されます。
        /// </summary>
        /// <param name="command">アンホバーコマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public void OnUnhover(SpriteUnhoverCommand command, UIInteractionOrchestrator orchestrator)
        {
            Debug.Log("Card_OnUnhover");

            // UIがアイドル状態の場合
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.Idle)
            {
                // アンホバーされたカードのViewを取得し、通常状態に遷移
                var cardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(command.TargetObjectId);
                if (cardView != null) cardView.EnterNormalState();
            }
        }

        /// <summary>
        /// カードがスロットにドロップされたときに呼び出されます。
        /// </summary>
        /// <param name="command">ドロップコマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public void OnDrop(SpriteDropCommand command, UIInteractionOrchestrator orchestrator)
        {
            var draggedCardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(orchestrator.DraggedId);
            Debug.Log("<color=red>Card_OnDrop-></color>" + draggedCardView._cardName + "->" + orchestrator.UIStateMachine.CurrentState + " Flg:" + orchestrator.IsDroppedSuccessfully);
            // UIがカードドラッグ中の場合のみ処理
            if (orchestrator.UIStateMachine.CurrentState == UIStateMachine.UIState.DraggingCard)
            {
                orchestrator.UIStateMachine.SetState(UIStateMachine.UIState.DropedCard);

                // カードスロットマネージャーにドロップ処理を依頼
                orchestrator.CardSlotManager.OnCardDroppedOnSlot(command.DroppedObjectId, command.TargetSlotObjectId);
                // ドロップが成功したことを示すフラグを設定
                orchestrator.IsDroppedSuccessfully = true;
            }
        }

        /// <summary>
        /// カードがドラッグ中に移動したときに呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグコマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public void OnDrag(SpriteDragCommand command, UIInteractionOrchestrator orchestrator)
        {
            var draggedCardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(orchestrator.DraggedId);
//            Debug.Log("<color=blue>Card_OnDrag：</color>" + draggedCardView._cardName + "->" + orchestrator.UIStateMachine.CurrentState + " Flg:" + orchestrator.IsDroppedSuccessfully);

            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingCard) return;

            // ドラッグ中のカードのViewを取得
//            var draggedCardView = orchestrator.ViewRegistry.GetView<CreatureCardView>(orchestrator.DraggedId);
            if (draggedCardView != null)
            {
//                Debug.Log("カード：" + draggedCardView._cardName + "->" + command.NewPosition);
                // ドラッグ中状態に遷移し、カードを新しい位置へ移動
                draggedCardView.EnterDraggingInProgressState();
                draggedCardView.MoveTo(command.NewPosition);
            }
        }

        /// <summary>
        /// カードのドラッグが終了したときに呼び出されます。
        /// </summary>
        /// <param name="command">ドラッグ終了コマンド。</param>
        /// <param name="orchestrator">UIインタラクションオーケストレーターのインスタンス。</param>
        public async void OnEndDrag(SpriteEndDragCommand command, UIInteractionOrchestrator orchestrator)
        {
            Debug.Log("<color=red>Card_OnEndDrag-></color>" + orchestrator.UIStateMachine.CurrentState + " Flg:" + orchestrator.IsDroppedSuccessfully);

            if (orchestrator.UIStateMachine.CurrentState != UIStateMachine.UIState.DraggingCard) return;
            // UIステートをドロップ済みカードに設定
            orchestrator.UIStateMachine.SetState(UIStateMachine.UIState.DropedCard);

            // 遅延処理でドロップの成否を判定
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

            // ドロップが成功しなかった場合、リフローを元に戻す
            Debug.Log("Card_OnEndDrag->01秒後に実行するやつ:" + orchestrator.IsDroppedSuccessfully);
            if (!orchestrator.IsDroppedSuccessfully)
            {
                orchestrator.CardSlotManager.OnDropFailed();
            }
            // ドロップ成功フラグをリセット
            orchestrator.IsDroppedSuccessfully = false;
        }
    }
}

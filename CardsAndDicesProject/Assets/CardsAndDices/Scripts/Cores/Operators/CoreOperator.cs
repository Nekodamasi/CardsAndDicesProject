using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトの状態変化コマンドを監視し、具体的な処理を実装するためのクラス。
    /// BaseIdentifiableStateOperatorを継承し、各コマンドに対応するメソッドをオーバーライドして使用する。
    /// </summary>
    [CreateAssetMenu(fileName = "CoreOperator", menuName = "CardsAndDices/Core/Operators/CoreOperator")]
    public class CoreOperator : BaseIdentifiableStateOperator
    {
        /// <summary>
        /// DIコンテナから依存性を注入するための初期化メソッド。
        /// </summary>
        [Inject]
        public void Initialize(GameEventBus gameEventBus)
        {
            _eventBus = gameEventBus;
            OnEnable();
        }

        /// <summary>
        /// StateHoverが発生したさいのコマンドを処理します。
        /// </summary>
        protected override async void OnStateHover(IdentifiableStateHoverEvent evt)
        {
            // Statusの変更を通知する
            _eventBus.Emit(new ChangeViewStatusEvent(evt.ExecutedObjectId, IdentifiableStatus.Hover));

            // Statusの反映を通知する
            _eventBus.Emit(new DisplayStatusViewEvent(evt.ExecutedObjectId));

            // 0.2秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(_hoveredTime));

            // ホバー完了を通知
            _eventBus.Emit(new IdentifiableHoveredEvent(evt.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateUnhoverが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateUnhover(IdentifiableStateUnhoverEvent evt)
        {
            // Statusの変更を通知する
            _eventBus.Emit(new ChangeViewStatusEvent(evt.ExecutedObjectId, IdentifiableStatus.Normal));

            // Statusの反映を通知します
            _eventBus.Emit(new DisplayStatusViewEvent(evt.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateBeginDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateBeginDrag(IdentifiableStateBeginDragEvent evt)
        {
            // Statusの変更を通知する
            _eventBus.Emit(new ChangeViewStatusEvent(evt.ExecutedObjectId, IdentifiableStatus.DraggingStarted));

            // Statusの反映を通知します
            _eventBus.Emit(new DisplayStatusViewEvent(evt.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateDrag(IdentifiableStateDragEvent evt)
        {
            //            Debug.Log("おぺれーたー（ドラッグ中）：" + evt.ExecutedObjectId);
            // Statusの変更を通知する
            _eventBus.Emit(new ChangeViewStatusEvent(evt.ExecutedObjectId, IdentifiableStatus.DraggingInProgress));

            // Statusの反映を通知します
            _eventBus.Emit(new MoveToIdentifiableEvent(evt.ExecutedObjectId, evt.TargetPosition));
        }

        /// <summary>
        /// OnStateEndDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override async void OnStateEndDrag(IdentifiableStateEndDragEvent evt)
        {
            Debug.Log("おぺれーたー（ドラッグ終了）：" + evt.ExecutedObjectId);

            // HomePositionへのReturnを実行います
            _eventBus.Emit(new IdentifiableStateDropFailureEvent(evt.ExecutedObjectId));

            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(_hoveredTime));

            // Statusをリセットします
            _eventBus.Emit(new ResetUIStatusEvent());
        }

        /// <summary>
        /// StateClickが発生したさいのコマンドを処理します。
        /// </summary>
        protected override async void OnStateClick(IdentifiableStateClickEvent evt)
        {
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(_clickTime));

            // Statusをリセットします
            _eventBus.Emit(new ResetUIStatusEvent());
        }
    }
}

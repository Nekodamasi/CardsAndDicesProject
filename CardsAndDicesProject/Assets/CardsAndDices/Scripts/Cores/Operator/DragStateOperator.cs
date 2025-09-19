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
    [CreateAssetMenu(fileName = "DragStateOperator", menuName = "CardsAndDices/UI/Identifiable/Operator/DragStateOperator")]
    public class DragStateOperator : BaseIdentifiableStateOperator
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
        protected override async void OnStateHover(IdentifiableStateHoverEvent command)
        {
            Debug.Log("おぺれーたー（ほばー）：" + command.ExecutedObjectId);
            // Statusの変更を通知する
            _eventBus.Emit(new IdentifiableChangeStatusEvent(command.ExecutedObjectId, IdentifiableStatus.Hover));

            // Statusの反映を通知する
            _eventBus.Emit(new DisplayStatusViewEvent(command.ExecutedObjectId));
            

            // 0.2秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(_hoveredTime));

            // ホバー完了を通知
            _eventBus.Emit(new IdentifiableHoveredEvent(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateUnhoverが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateUnhover(IdentifiableStateUnhoverEvent command)
        {
            // Statusの変更を通知する
            _eventBus.Emit(new IdentifiableChangeStatusEvent(command.ExecutedObjectId, IdentifiableStatus.Normal));

            // Statusの反映を通知します
            _eventBus.Emit(new DisplayStatusViewEvent(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateBeginDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateBeginDrag(IdentifiableStateBeginDragEvent command)
        {
            Debug.Log("ほげほげほげ");
            // Statusの変更を通知する
            _eventBus.Emit(new IdentifiableChangeStatusEvent(command.ExecutedObjectId, IdentifiableStatus.DraggingStarted));

            // Statusの反映を通知します
            _eventBus.Emit(new DisplayStatusViewEvent(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateDrag(IdentifiableStateDragEvent command)
        {
            Debug.Log("おぺれーたー（ドラッグ中）：" + command.ExecutedObjectId);
            // Statusの変更を通知する
            _eventBus.Emit(new IdentifiableChangeStatusEvent(command.ExecutedObjectId, IdentifiableStatus.DraggingInProgress));

            // Statusの反映を通知します
            _eventBus.Emit(new MoveToIdentifiableCommand(command.ExecutedObjectId, command.TargetPosition));
        }

        /// <summary>
        /// OnStateEndDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateEndDrag(IdentifiableStateEndDragEvent command)
        {
            Debug.Log("おぺれーたー（ドラッグ終了）：" + command.ExecutedObjectId);

            // HomePositionへのReturnを実行います
            _eventBus.Emit(new IdentifiableReturnHomePositionCommand(command.ExecutedObjectId));
        }
    }
}

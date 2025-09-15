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
        public void Initialize(IdentifiableCommandBus identifiableCommandBus)
        {
            _commandBus = identifiableCommandBus;
            OnEnable();
        }

        /// <summary>
        /// StateHoverが発生したさいのコマンドを処理します。
        /// </summary>
        protected override async void OnStateHover(IdentifiableStateHoverCommand command)
        {
            Debug.Log("おぺれーたー（ほばー）：" + command.ExecutedObjectId);
            // Statusの変更を通知する
            _commandBus.Emit(new IdentifiableChangeStatusCommand(command.ExecutedObjectId, IdentifiableStatus.Hover));

            // 0.2秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(_hoveredTime));

            // ホバーの完了を通知する
            _commandBus.Emit(new IdentifiableHoveredCommand(command.ExecutedObjectId));

            // Statusの反映を通知する
            _commandBus.Emit(new DisplayIdentifiableStatusCommand(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateUnhoverが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateUnhover(IdentifiableStateUnhoverCommand command)
        {
            // Statusの変更を通知する
            _commandBus.Emit(new IdentifiableChangeStatusCommand(command.ExecutedObjectId, IdentifiableStatus.Normal));

            // Statusの反映を通知します
            _commandBus.Emit(new DisplayIdentifiableStatusCommand(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateBeginDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateBeginDrag(IdentifiableStateBeginDragCommand command)
        {
            Debug.Log("ほげほげほげ");
            // Statusの変更を通知する
            _commandBus.Emit(new IdentifiableChangeStatusCommand(command.ExecutedObjectId, IdentifiableStatus.DraggingStarted));

            // Statusの反映を通知します
            _commandBus.Emit(new DisplayIdentifiableStatusCommand(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateDrag(IdentifiableStateDragCommand command)
        {
            Debug.Log("おぺれーたー（ドラッグ中）：" + command.ExecutedObjectId);
            // Statusの変更を通知する
            _commandBus.Emit(new IdentifiableChangeStatusCommand(command.ExecutedObjectId, IdentifiableStatus.DraggingInProgress));

            // Statusの反映を通知します
            _commandBus.Emit(new MoveToIdentifiableCommand(command.ExecutedObjectId, command.TargetPosition));
        }

        /// <summary>
        /// OnStateEndDragが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateEndDrag(IdentifiableStateEndDragCommand command)
        {
            Debug.Log("おぺれーたー（ドラッグ終了）：" + command.ExecutedObjectId);

            // HomePositionへのReturnを実行います
            _commandBus.Emit(new IdentifiableReturnHomePositionCommand(command.ExecutedObjectId));
        }
    }
}

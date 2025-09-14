using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトの状態変化コマンドを監視し、具体的な処理を実装するためのクラス。
    /// BaseIdentifiableStateOperatorを継承し、各コマンドに対応するメソッドをオーバーライドして使用する。
    /// </summary>
    [CreateAssetMenu(fileName = "HoverOnlyStateOperator", menuName = "CardsAndDices/UI/Identifiable/Operator/HoverOnlyStateOperator")]
    public class HoverOnlyStateOperator : BaseIdentifiableStateOperator
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
        protected override void OnStateHover(IdentifiableStateHoverCommand command)
        {
            // Statusの変更を通知する
            _commandBus.Emit(new IdentifiableChangeStatusCommand(command.ExecutedObjectId, IdentifiableStatus.Hover));

            // ホバーの完了を通知する
            _commandBus.Emit(new IdentifiableHoveredCommand(command.ExecutedObjectId));

            // Statusの変更を通知する
            _commandBus.Emit(new DisplayIdentifiableStatusCommand(command.ExecutedObjectId));
        }

        /// <summary>
        /// OnStateUnhoverが発生したさいのコマンドを処理します。
        /// </summary>
        protected override void OnStateUnhover(IdentifiableStateUnhoverCommand command)
        {
            // Statusの変更を通知する
            _commandBus.Emit(new IdentifiableChangeStatusCommand(command.ExecutedObjectId, IdentifiableStatus.Normal));

            // Statusの変更を通知する
            _commandBus.Emit(new DisplayIdentifiableStatusCommand(command.ExecutedObjectId));
        }
    }
}

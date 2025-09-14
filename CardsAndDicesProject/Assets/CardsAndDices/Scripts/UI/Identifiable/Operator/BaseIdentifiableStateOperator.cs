using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 特定の識別可能オブジェクトの状態変化コマンドを監視し、対応する処理を実行するための基底クラス。
    /// ScriptableObjectとして実装し、具体的な振る舞いは継承先で定義する。
    /// </summary>
    public abstract class BaseIdentifiableStateOperator : ScriptableObject
    {
        [Header("監視対象のID")]
        [SerializeField]
        private List<CompositeObjectId> _targetIds = new();

        protected IdentifiableCommandBus _commandBus;

        /// <summary>
        /// CompositeObjectIdの登録
        /// </summary>
        public void RegisterTarget(CompositeObjectId targetId)
        {
            _targetIds.Add(targetId);
        }

        /// <summary>
        /// CompositeObjectIdの解除
        /// </summary>
        public void UnregisterTarget(CompositeObjectId targetId)
        {
            _targetIds.Remove(targetId);
        }

        /// <summary>
        /// オブジェクトが有効化された際にコマンドの購読を開始します。
        /// </summary>
        protected void OnEnable()
        {
            if (_commandBus == null) return;

            _commandBus.On<IdentifiableStateHoverCommand>(HandleStateHover);
            _commandBus.On<IdentifiableStateHoveredCommand>(HandleStateHovered);
            _commandBus.On<IdentifiableStateUnhoverCommand>(HandleStateUnhover);
            _commandBus.On<IdentifiableStateEndDragCommand>(HandleStateEndDrag);
            _commandBus.On<IdentifiableStateEndDragedCommand>(HandleStateEndDraged);
            _commandBus.On<IdentifiableStateDragCommand>(HandleStateDrag);
            _commandBus.On<IdentifiableStateDropCommand>(HandleStateDrop);
            _commandBus.On<IdentifiableStateDropedCommand>(HandleStateDroped);
            _commandBus.On<IdentifiableStateClickCommand>(HandleStateClick);
            _commandBus.On<IdentifiableStateClickedCommand>(HandleStateClicked);
            _commandBus.On<IdentifiableStateBeginDragCommand>(HandleStateBeginDrag);
        }

        /// <summary>
        /// オブジェクトが無効化された際にコマンドの購読を解除します。
        /// </summary>
        private void OnDisable()
        {
            if (_commandBus == null) return;

            _commandBus.Off<IdentifiableStateHoverCommand>(HandleStateHover);
            _commandBus.Off<IdentifiableStateHoveredCommand>(HandleStateHovered);
            _commandBus.Off<IdentifiableStateUnhoverCommand>(HandleStateUnhover);
            _commandBus.Off<IdentifiableStateEndDragCommand>(HandleStateEndDrag);
            _commandBus.Off<IdentifiableStateEndDragedCommand>(HandleStateEndDraged);
            _commandBus.Off<IdentifiableStateDragCommand>(HandleStateDrag);
            _commandBus.Off<IdentifiableStateDropCommand>(HandleStateDrop);
            _commandBus.Off<IdentifiableStateDropedCommand>(HandleStateDroped);
            _commandBus.Off<IdentifiableStateClickCommand>(HandleStateClick);
            _commandBus.Off<IdentifiableStateClickedCommand>(HandleStateClicked);
            _commandBus.Off<IdentifiableStateBeginDragCommand>(HandleStateBeginDrag);
        }

        // --- Command Handlers ---
        private void HandleStateHover(IdentifiableStateHoverCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateHover(command));
        private void HandleStateHovered(IdentifiableStateHoveredCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateHovered(command));
        private void HandleStateUnhover(IdentifiableStateUnhoverCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateUnhover(command));
        private void HandleStateEndDrag(IdentifiableStateEndDragCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateEndDrag(command));
        private void HandleStateEndDraged(IdentifiableStateEndDragedCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateEndDraged(command));
        private void HandleStateDrag(IdentifiableStateDragCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateDrag(command));
        private void HandleStateDrop(IdentifiableStateDropCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateDrop(command));
        private void HandleStateDroped(IdentifiableStateDropedCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateDroped(command));
        private void HandleStateClick(IdentifiableStateClickCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateClick(command));
        private void HandleStateClicked(IdentifiableStateClickedCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateClicked(command));
        private void HandleStateBeginDrag(IdentifiableStateBeginDragCommand command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateBeginDrag(command));

        /// <summary>
        /// コマンドの実行対象IDが監視対象リストに含まれている場合のみ、指定されたアクションを実行します。
        /// </summary>
        private void ExecuteIfTarget(CompositeObjectId executedId, System.Action action)
        {
            Debug.Log("オペレーターの判定：" + executedId);
            if (_targetIds.Any(id => id.Equals(executedId)))
            {
                action?.Invoke();
            }
        }

        // --- Abstract Methods for Subclasses ---
        protected virtual void OnStateHover(IdentifiableStateHoverCommand command) { }
        protected virtual void OnStateHovered(IdentifiableStateHoveredCommand command) { }
        protected virtual void OnStateUnhover(IdentifiableStateUnhoverCommand command) { }
        protected virtual void OnStateEndDrag(IdentifiableStateEndDragCommand command) { }
        protected virtual void OnStateEndDraged(IdentifiableStateEndDragedCommand command) { }
        protected virtual void OnStateDrag(IdentifiableStateDragCommand command) { }
        protected virtual void OnStateDrop(IdentifiableStateDropCommand command) { }
        protected virtual void OnStateDroped(IdentifiableStateDropedCommand command) { }
        protected virtual void OnStateClick(IdentifiableStateClickCommand command) { }
        protected virtual void OnStateClicked(IdentifiableStateClickedCommand command) { }
        protected virtual void OnStateBeginDrag(IdentifiableStateBeginDragCommand command) { }
    }
}

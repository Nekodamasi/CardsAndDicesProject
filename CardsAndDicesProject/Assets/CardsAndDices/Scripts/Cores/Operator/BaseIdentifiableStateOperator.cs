using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardsAndDices
{
    /// <summary>
    /// 特定の識別可能オブジェクトの状態変化コマンドを監視し、対応する処理を実行するための基底クラス。
    /// ScriptableObjectとして実装し、具体的な振る舞いは継承先で定義する。
    /// </summary>
    public abstract class BaseIdentifiableStateOperator : ScriptableObject
    {
        [Header("Setting")]
        [SerializeField] protected float _hoveredTime = 0.2f;

        [Header("監視対象")]
        [SerializeField] private List<string> _ids = new();
        private List<CompositeObjectId> _targetIds = new();

        protected GameEventBus _eventBus;

        /// <summary>
        /// CompositeObjectIdの登録
        /// </summary>
        public void RegisterTarget(CompositeObjectId targetId)
        {
            _targetIds.Add(targetId);
            _ids.Add(targetId.ToString());
        }

        /// <summary>
        /// CompositeObjectIdの解除
        /// </summary>
        public void UnregisterTarget(CompositeObjectId targetId)
        {
            _targetIds.Remove(targetId);
            _ids.Remove(targetId.ToString());
        }

        /// <summary>
        /// オブジェクトが有効化された際にコマンドの購読を開始します。
        /// </summary>
        protected void OnEnable()
        {
            _targetIds.Clear();
            _ids.Clear();
            if (_eventBus == null) return;

            _eventBus.On<IdentifiableStateHoverEvent>(HandleStateHover);
            _eventBus.On<IdentifiableStateUnhoverEvent>(HandleStateUnhover);
            _eventBus.On<IdentifiableStateEndDragEvent>(HandleStateEndDrag);
            _eventBus.On<IdentifiableStateDragEvent>(HandleStateDrag);
            _eventBus.On<IdentifiableStateDropEvent>(HandleStateDrop);
            _eventBus.On<IdentifiableStateClickEvent>(HandleStateClick);
            _eventBus.On<IdentifiableStateBeginDragEvent>(HandleStateBeginDrag);
        }

        /// <summary>
        /// オブジェクトが無効化された際にコマンドの購読を解除します。
        /// </summary>
        private void OnDisable()
        {
            if (_eventBus == null) return;

            _eventBus.Off<IdentifiableStateHoverEvent>(HandleStateHover);
            _eventBus.Off<IdentifiableStateUnhoverEvent>(HandleStateUnhover);
            _eventBus.Off<IdentifiableStateEndDragEvent>(HandleStateEndDrag);
            _eventBus.Off<IdentifiableStateDragEvent>(HandleStateDrag);
            _eventBus.Off<IdentifiableStateDropEvent>(HandleStateDrop);
            _eventBus.Off<IdentifiableStateClickEvent>(HandleStateClick);
            _eventBus.Off<IdentifiableStateBeginDragEvent>(HandleStateBeginDrag);
        }

        // --- Command Handlers ---
        private void HandleStateHover(IdentifiableStateHoverEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateHover(command));
        private void HandleStateUnhover(IdentifiableStateUnhoverEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateUnhover(command));
        private void HandleStateEndDrag(IdentifiableStateEndDragEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateEndDrag(command));
        private void HandleStateDrag(IdentifiableStateDragEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateDrag(command));
        private void HandleStateDrop(IdentifiableStateDropEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateDrop(command));
        private void HandleStateClick(IdentifiableStateClickEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateClick(command));
        private void HandleStateBeginDrag(IdentifiableStateBeginDragEvent command) => ExecuteIfTarget(command.ExecutedObjectId, () => OnStateBeginDrag(command));

        /// <summary>
        /// コマンドの実行対象IDが監視対象リストに含まれている場合のみ、指定されたアクションを実行します。
        /// </summary>
        private void ExecuteIfTarget(CompositeObjectId executedId, System.Action action)
        {
            if (_targetIds.Any(id => id.Equals(executedId)))
            {
                action?.Invoke();
            }
        }

        // --- Abstract Methods for Subclasses ---
        protected virtual void OnStateHover(IdentifiableStateHoverEvent command) { }
        protected virtual void OnStateUnhover(IdentifiableStateUnhoverEvent command) { }
        protected virtual void OnStateEndDrag(IdentifiableStateEndDragEvent command) { }
        protected virtual void OnStateDrag(IdentifiableStateDragEvent command) { }
        protected virtual void OnStateDrop(IdentifiableStateDropEvent command) { }
        protected virtual void OnStateClick(IdentifiableStateClickEvent command) { }
        protected virtual void OnStateBeginDrag(IdentifiableStateBeginDragEvent command) { }
    }
}

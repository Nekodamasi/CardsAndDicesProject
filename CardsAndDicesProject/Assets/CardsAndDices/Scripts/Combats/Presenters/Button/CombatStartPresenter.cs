using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardinstance(Model)とCreatureCardView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CombatStartPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly CombatStartInstance _instance;
        private readonly CombatStartBtnView _view;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public CombatStartPresenter(CombatStartInstance instance, CombatStartBtnView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
            _view.SetBoundState(false);
        }

        /// <summary>
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _eventBus.Emit(new IdentifiableResetUIStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// 何かがドラックされたらインアクティブに変更
        /// </summary>
        private void OnIdentifiableStateBeginDrag(IdentifiableStateBeginDragEvent evt)
        {
            // 違う何かがドラッグされたらインアクティブに
            _eventBus.Emit(new ChangeViewStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// ディスプレイを現在のステータスで表示するイベント
        /// </summary>
        private void OnUpdateDisplayCreatureStatus(UpdateDisplayCreatureStatusEvent evt)
        {
            if (evt.ExecutedObjectId != _instance.CompositeObjectId) return;
            _eventBus.Emit(new IdentifiableCurrentUIStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// 現在のステータスで表示するイベント
        /// </summary>
        private void OnDisplayUIStatus(DisplayUIStatusEvent evt)
        {
            _eventBus.Emit(new IdentifiableCurrentUIStatusEvent(_instance.CompositeObjectId));
        }
    }
}

using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardinstance(Model)とCreatureCardView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CreatureStatusPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly CreatureStatusInstance _instance;
        private readonly CreatureCardView _view;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public CreatureStatusPresenter(CreatureStatusInstance instance, CreatureCardView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
        }

        /// <summary>
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.CardPlacement, _instance.CompositeObjectId, null));
            _eventBus.Emit(new UpdateDisplayCreatureStatusEvent(_instance.CompositeObjectId));            
        }

        /// <summary>
        /// ドラッグされたカード以外はインアクティブに変更
        /// </summary>
        private void OnIdentifiableStateBeginDrag(IdentifiableStateBeginDragEvent evt)
        {
            // 自分がドラッグ対象
            if (evt.ExecutedObjectId == _instance.CompositeObjectId) return;

            // 違う何かがドラッグされたらインアクティブに
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
            _eventBus.Emit(new ChangeViewStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Inactive));
        }
    }
}

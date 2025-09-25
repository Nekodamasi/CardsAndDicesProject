using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotinstance(Model)とCreatureCardSlotView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CreatureCardSlotPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly CreatureCardSlotInstance _instance;
        private readonly CreatureCardSlotView _view;
        private readonly GameEventBus _eventBus;
        private readonly CompositeObjectIdTypeEntity _compositeObjectIdTypeEntity;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public Vector3 HomePosition => _instance.CreatureCardSlotPosition;

        public CreatureCardSlotPresenter(CreatureCardSlotInstance instance, CreatureCardSlotView view, GameEventBus eventBus, CompositeObjectIdTypeEntity compositeObjectIdTypeEntity)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _compositeObjectIdTypeEntity = compositeObjectIdTypeEntity;
            _eventBus.On<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);

        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
        }

        /// <summary>
        /// 受け入れ状態になるイベント
        /// </summary>
        private void OnIdentifiableStateBeginDrag(IdentifiableStateBeginDragEvent evt)
        {
//            Debug.Log("ほげほげほげほげほげ：" + evt.ExecutedObjectId.ObjectType + "/" + _compositeObjectIdTypeEntity);
            // 受け入れ対象がドラッグされた
            if (evt.ExecutedObjectId.ObjectType != _compositeObjectIdTypeEntity) return;

            // Statusを受け入れ状態に変更
            _eventBus.Emit(new ChangeViewStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Acceptable));
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
            _view.DisplayAcceptableStatus();
        }
    }
}

using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotinstance(Model)とCreatureCardSlotView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CreatureCardSlotPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly DiceInstance _instance;
        private readonly DiceView _view;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public CreatureCardSlotPresenter(DiceInstance instance, DiceView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.On<DisplayOffScreenEvent>(OnDisplayOffScreen);
            _eventBus.On<IdentifiableDropEvent>(OnIdentifiableDrop);
            
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.Off<DisplayOffScreenEvent>(OnDisplayOffScreen);
            _eventBus.Off<IdentifiableDropEvent>(OnIdentifiableDrop);
        }

        /// <summary>
        /// オブジェクトをドロップ
        /// </summary>
        private void OnIdentifiableDrop(IdentifiableDropEvent evt)
        {
            if (evt.TargetObjectId != _view.CompositeObjectId) return;
            _eventBus.Emit(new DiceDropInInletEvent(evt.ExecutedObjectId, evt.TargetObjectId, _instance.FaceValue));
            _instance.IsAlive = false;
            _eventBus.Emit(new ChangeViewStatusEvent(_view.CompositeObjectId, IdentifiableStatus.Hide));
            _eventBus.Emit(new DisplayStatusViewEvent(_view.CompositeObjectId));
        }

        /// <summary>
        /// ダイスを画面に投げ入れる
        /// </summary>
        private void OnDisplayOnScreen(DisplayOnScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            if (_instance.IsOnScreen) return;
            _instance.IsOnScreen = true;
            _view.DisplayOnScreen(_instance.DiceHomePosition, _instance.FaceValue);
        }

        /// <summary>
        /// ダイスを画面から退場させる
        /// </summary>
        private void OnDisplayOffScreen(DisplayOffScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            if (!_instance.IsOnScreen) return;
            _instance.IsOnScreen = false;
            _view.DisplayOffScreen();
            _instance.IsAlive = false;
        }
    }
}

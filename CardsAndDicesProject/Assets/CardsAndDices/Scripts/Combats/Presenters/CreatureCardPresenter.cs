using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardinstance(Model)とCreatureCardView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CreatureCardPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly CreatureCardInstance _instance;
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

        public CreatureCardPresenter(CreatureCardInstance instance, CreatureCardView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.On<DisplayOffScreenEvent>(OnDisplayOffScreen);
            
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.Off<DisplayOffScreenEvent>(OnDisplayOffScreen);
        }

        /// <summary>
        /// クリーチャーカードを画面に投げ入れる
        /// </summary>
        private void OnDisplayOnScreen(DisplayOnScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            if (_instance.IsOnScreen) return;
            _instance.IsOnScreen = true;
            _view.DisplayOnScreen(_instance.CreatureCardSlotPosition);
        }

        /// <summary>
        /// クリーチャーカードを画面から退場させる
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

using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardinstance(Model)とCreatureCardView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CardAppearancePresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly CardAppearanceInstance _instance;
        private readonly CardAppearanceView _view;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public CardAppearancePresenter(CardAppearanceInstance instance, CardAppearanceView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<DisplayOnScreenEvent>(OnDisplayOnScreen);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _view.SetBoundState(false);
        }

        /// <summary>
        /// クリーチャーカードを画面に投げ入れる
        /// </summary>
        private void OnDisplayOnScreen(DisplayOnScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _instance.CompositeObjectId) return;
            _view.DisplayCardAppearance(_instance.AppearanceProfile);
        }
    }
}

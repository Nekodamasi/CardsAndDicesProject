using System;
using NUnit;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードのアイコンViewとInstanceを繋ぐ仲介役です
    /// </summary>
    public class SharedIconElementPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly IconStatusInstance _instance;
        private readonly SharedIconElementView _view;
        private readonly GameEventBus _eventBus;

        public SharedIconElementPresenter(IconStatusInstance instance, SharedIconElementView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<DisplaySharedIconElementEvent>(OnDisplaySharedIconElement);
        }

        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<DisplaySharedIconElementEvent>(OnDisplaySharedIconElement);
        }

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        /// <summary>
        /// アイコンの状態を最新に更新します
        /// </summary>
        private void OnDisplaySharedIconElement(DisplaySharedIconElementEvent evt)
        {
            if (evt.ExecutedObjectId != _instance.CompositeObjectId || evt.SharedIconElementTypeEntity != _instance.SharedIconElementTypeEntity) return;
            if (_instance.DisplayiconValue != evt.NumberValue)
            {
                _instance.SetIconValue(evt.NumberValue);
                _view.DisplayChangeNumberAnimation(evt.NumberValue);
            }
            DisplayCurrentStatus();
        }

        /// <summary>
        /// 現在の状態に応じてViewを更新します
        /// </summary>
        private void DisplayCurrentStatus()
        {
            // ノーマル状態
            if (_instance.CurrentStatus == SharedIconElementStatus.Normal)
            {
                _view.DisplayNormalStatus();
            }
            // グレイアウト状態
            else if (_instance.CurrentStatus == SharedIconElementStatus.Grayout)
            {
                _view.DisplayGrayoutStatus();
            }
            // ハイド状態
            else if (_instance.CurrentStatus == SharedIconElementStatus.Hide)
            {
                _view.DisplayHideStatus();
            }
        }
    }
}

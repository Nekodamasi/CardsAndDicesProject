using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// DiceInstance(Model)とDiceView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class DicePresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly DiceInstance _instance;
        private readonly DiceView _view;
        private readonly GameEventBus _eventBus;
        private bool hoge;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public DicePresenter(DiceInstance instance, DiceView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.On<DisplayOffScreenEvent>(OnDisplayOffScreen);
            _eventBus.On<IdentifiableStateDropEvent>(OnIdentifiableStateDrop);
            _eventBus.On<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<DisplayUIStatusEvent>(OnDisplayUIStatus);
            hoge = false;
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.Off<DisplayOffScreenEvent>(OnDisplayOffScreen);
            _eventBus.Off<IdentifiableStateDropEvent>(OnIdentifiableStateDrop);
            _eventBus.Off<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _view.SetBoundState(false);
            hoge = true;
        }

        /// <summary>
        /// ディスプレイを現在のステータスで表示するイベント
        /// </summary>
        private void OnDisplayUIStatus(DisplayUIStatusEvent evt)
        {
            _eventBus.Emit(new IdentifiableCurrentUIStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            if (hoge)
            {
                Debug.LogWarning("ディスポーズ後にいつまでよばれるの？:" + _instance.CompositeObjectId + "_" + hoge);
            }
            _eventBus.Emit(new IdentifiableResetUIStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// ドラッグされたダイス以外はインアクティブに変更
        /// </summary>
        private void OnIdentifiableStateBeginDrag(IdentifiableStateBeginDragEvent evt)
        {
            // 自分がドラッグ対象
            if (evt.ExecutedObjectId == _instance.CompositeObjectId)
            {
                _eventBus.Emit(new DiceBeginDragEvent(_instance.CompositeObjectId, _instance.FaceValue));
                return;
            }

            // 違う何かがドラッグされたらインアクティブに
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// オブジェクトをドロップ
        /// </summary>
        private void OnIdentifiableStateDrop(IdentifiableStateDropEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            var faceValue = _instance.FaceValue;
            _eventBus.Emit(new DisposeByCompositeObjectIdEvent(_view.CompositeObjectId));
            _eventBus.Emit(new DiceDropInInletEvent(evt.TargetObjectId, evt.ExecutedObjectId, faceValue));
            _instance.IsAlive = false;
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

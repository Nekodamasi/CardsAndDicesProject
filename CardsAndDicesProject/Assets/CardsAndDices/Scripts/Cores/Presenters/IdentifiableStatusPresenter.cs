using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace CardsAndDices
{
    /// <summary>
    /// Connects a Creature (Model) to a CreatureCardView (View).
    /// </summary>
    public class IdentifiableStatusPresenter : IDisposable
    {
        private readonly IdentifiableStatusInstance _status;
        private readonly IdentifiableStatusView _view;
        private readonly GameEventBus _eventBus;
        public IdentifiableStatusPresenter(IdentifiableStatusInstance status, IdentifiableStatusView view, GameEventBus eventBus)
        {
            _status = status;
            _view = view;
            _eventBus = eventBus;

            _eventBus.On<DisplayStatusViewEvent>(OnDisplayIdentifiableStatus);
            _eventBus.On<MoveToIdentifiableEvent>(OnMoveToIdentifiable);
            _eventBus.On<ReturnHomePositionAnimationEvent>(OnReturnHomePositionAnimation);
            _eventBus.On<ReturnHomePositionEvent>(OnReturnHomePosition);            
            _eventBus.On<MoveToAnimationIdentifiableEvent>(OnMoveToAnimationIdentifiable);
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _eventBus.On<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
            _eventBus.On<SetCurrentHomeStatusEvent>(OnSetCurrentHomeStatus);
        }
        public void Dispose()
        {
            _eventBus.Off<DisplayStatusViewEvent>(OnDisplayIdentifiableStatus);
            _eventBus.Off<MoveToIdentifiableEvent>(OnMoveToIdentifiable);
            _eventBus.Off<ReturnHomePositionAnimationEvent>(OnReturnHomePositionAnimation);
            _eventBus.Off<ReturnHomePositionEvent>(OnReturnHomePosition);            
            _eventBus.Off<MoveToAnimationIdentifiableEvent>(OnMoveToAnimationIdentifiable);
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _eventBus.Off<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
            _eventBus.Off<SetCurrentHomeStatusEvent>(OnSetCurrentHomeStatus);
        }
        /// <summary>
        /// 現在のUIステートをViewに反映させるコマンド
        /// </summary>
        private void OnDisplayUIStatus(DisplayUIStatusEvent evt)
        {
            DisplayCurrentStatus();
        }

        /// <summary>
        /// UIStatusのreset
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _status.UpdateStatus(_status.CurrentHomeStatus);
            DisplayCurrentStatus();
        }

        /// <summary>
        /// ホームステータスの設定をします
        /// </summary>
        private void OnSetCurrentHomeStatus(SetCurrentHomeStatusEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            _status.CurrentHomeStatus = evt.CurrentHomeStatus;
        }

        /// <summary>
        /// Animation移動の実行
        /// </summary>
        private void OnMoveToAnimationIdentifiable(MoveToAnimationIdentifiableEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            _view.MoveToAnimated(evt.TargetPosition, 0.2f);
        }

        /// <summary>
        /// HomePositionへのreturnアニメーション
        /// </summary>
        private async void OnReturnHomePositionAnimation(ReturnHomePositionAnimationEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            var _currentMoveAnimation = _view.MoveToAnimated(_status.HomePosition, 0.2f);
            await _currentMoveAnimation.AsyncWaitForCompletion();
            _status.UpdateStatus(_status.CurrentHomeStatus);
            DisplayCurrentStatus();
        }

        /// <summary>
        /// HomePositionへのreturn
        /// </summary>
        private void OnReturnHomePosition(ReturnHomePositionEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            _view.MoveTo(_status.HomePosition);
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnMoveToIdentifiable(MoveToIdentifiableEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            _view.MoveTo(evt.TargetPosition);
        }

        /// <summary>
        /// クリックされた
        /// </summary>
        private void OnIdentifiableStateClick(IdentifiableStateClickEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            _status.UpdateStatus(IdentifiableStatus.Click);
            DisplayCurrentStatus();
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnDisplayIdentifiableStatus(DisplayStatusViewEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            DisplayCurrentStatus();
        }

        private void DisplayCurrentStatus()
        {
            // ホバー状態
            if (_status.CurrentStatus == IdentifiableStatus.Hover)
            {
                _view.DisplayHoverStatus();
            }
            // ノーマル状態
            else if (_status.CurrentStatus == IdentifiableStatus.Normal)
            {
                _view.DisplayNormalStatus();
            }
            // グレイアウト状態
            else if (_status.CurrentStatus == IdentifiableStatus.Grayout)
            {
                _view.DisplayGrayoutStatus();
            }
            // ドラッグ開始状態
            else if (_status.CurrentStatus == IdentifiableStatus.DraggingStarted)
            {
                _view.DisplayDragStatus();
            }
            // ハイド状態
            else if (_status.CurrentStatus == IdentifiableStatus.Hide)
            {
                _view.DisplayHideStatus();
            }
            // クリック状態
            else if (_status.CurrentStatus == IdentifiableStatus.Click)
            {
                _view.DisplayClickStatus();
            }
            // クリック状態
            else if (_status.CurrentStatus == IdentifiableStatus.Inactive)
            {
                _view.DisplayInactiveStatus();
            }
            // クリック状態
            else if (_status.CurrentStatus == IdentifiableStatus.Acceptable)
            {
                _view.DisplayAcceptableStatus();
            }
        }
    }
}

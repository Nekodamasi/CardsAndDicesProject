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
        private readonly GameEventBus _identifiableCommandBus;
        public IdentifiableStatusPresenter(IdentifiableStatusInstance status, IdentifiableStatusView view, GameEventBus commandBus)
        {
            _status = status;
            _view = view;
            _identifiableCommandBus = commandBus;

            _identifiableCommandBus.On<DisplayStatusViewEvent>(OnDisplayIdentifiableStatus);
            _identifiableCommandBus.On<MoveToIdentifiableEvent>(OnMoveToIdentifiable);
            _identifiableCommandBus.On<ReturnHomePositionStatusViewEvent>(OnIdentifiableReturnHomePosition);
            _identifiableCommandBus.On<MoveToAnimationIdentifiableEvent>(OnMoveToAnimationIdentifiable);
            _identifiableCommandBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _identifiableCommandBus.On<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _identifiableCommandBus.On<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
            

        }
        public void Dispose()
        {
            _identifiableCommandBus.Off<DisplayStatusViewEvent>(OnDisplayIdentifiableStatus);
            _identifiableCommandBus.Off<MoveToIdentifiableEvent>(OnMoveToIdentifiable);
            _identifiableCommandBus.Off<ReturnHomePositionStatusViewEvent>(OnIdentifiableReturnHomePosition);
            _identifiableCommandBus.Off<MoveToAnimationIdentifiableEvent>(OnMoveToAnimationIdentifiable);
            _identifiableCommandBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _identifiableCommandBus.Off<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _identifiableCommandBus.Off<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
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
        /// HomePositionへのreturn
        /// </summary>
        private async void OnIdentifiableReturnHomePosition(ReturnHomePositionStatusViewEvent evt)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != evt.ExecutedObjectId) return;
            var _currentMoveAnimation = _view.MoveToAnimated(_status.HomePosition, 0.2f);
            await _currentMoveAnimation.AsyncWaitForCompletion();
            _status.UpdateStatus(_status.CurrentHomeStatus);
            DisplayCurrentStatus();
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
        }
    }
}

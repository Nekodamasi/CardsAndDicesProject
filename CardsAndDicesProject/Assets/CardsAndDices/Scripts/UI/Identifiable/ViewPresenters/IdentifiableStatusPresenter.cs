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
        private readonly IdentifiableCommandBus _identifiableCommandBus;
        public IdentifiableStatusPresenter(IdentifiableStatusInstance status, IdentifiableStatusView view, IdentifiableCommandBus commandBus)
        {
            _status = status;
            _view = view;
            _identifiableCommandBus = commandBus;

            _identifiableCommandBus.On<DisplayIdentifiableStatusCommand>(OnDisplayIdentifiableStatus);
            _identifiableCommandBus.On<MoveToIdentifiableCommand>(OnMoveToIdentifiable);
            _identifiableCommandBus.On<IdentifiableReturnHomePositionCommand>(OnIdentifiableReturnHomePosition);
            _identifiableCommandBus.On<MoveToAnimationIdentifiableCommand>(OnMoveToAnimationIdentifiable);

        }
        public void Dispose()
        {
            _identifiableCommandBus.Off<DisplayIdentifiableStatusCommand>(OnDisplayIdentifiableStatus);
            _identifiableCommandBus.Off<MoveToIdentifiableCommand>(OnMoveToIdentifiable);
            _identifiableCommandBus.Off<IdentifiableReturnHomePositionCommand>(OnIdentifiableReturnHomePosition);
        }

        /// <summary>
        /// Animation移動の実行
        /// </summary>
        private void OnMoveToAnimationIdentifiable(MoveToAnimationIdentifiableCommand cmd)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;
            _view.MoveToAnimated(cmd.TargetPosition, 0.2f);
        }

        /// <summary>
        /// HomePositionへのreturn
        /// </summary>
        private async void OnIdentifiableReturnHomePosition(IdentifiableReturnHomePositionCommand cmd)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;
            Debug.Log("ぷれぜんたー(ほーむぽじしょｎ):" + cmd.ExecutedObjectId + " TargetPosition:" + _status.HomePosition);

            var _currentMoveAnimation = _view.MoveToAnimated(_status.HomePosition, 0.2f);
            await _currentMoveAnimation.AsyncWaitForCompletion();
            _status.UpdateStatus(_status.CurrentHomeStatus);
            Debug.Log("ほげほげほげほげほげほげほ：" + _status.CurrentHomeStatus);
            DisplayCurrentStatus();

            _identifiableCommandBus.Emit(new IdentifiableEndDragedCommand(cmd.ExecutedObjectId));
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnMoveToIdentifiable(MoveToIdentifiableCommand cmd)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;
            Debug.Log("ぷれぜんたー(移動中):" + cmd.ExecutedObjectId + " TargetPosition:" + cmd.TargetPosition);
            _view.MoveTo(cmd.TargetPosition);
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnDisplayIdentifiableStatus(DisplayIdentifiableStatusCommand cmd)
        {
            // 自分以外は処理しない
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;
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
        }
    }
}

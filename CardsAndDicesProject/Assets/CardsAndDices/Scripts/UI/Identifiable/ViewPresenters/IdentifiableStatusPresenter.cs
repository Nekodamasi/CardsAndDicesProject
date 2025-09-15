using System;
using System.Collections.Generic;
using UnityEngine;

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
        }
        public void Dispose()
        {
            _identifiableCommandBus.Off<DisplayIdentifiableStatusCommand>(OnDisplayIdentifiableStatus);
            _identifiableCommandBus.Off<MoveToIdentifiableCommand>(OnMoveToIdentifiable);
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnMoveToIdentifiable(MoveToIdentifiableCommand cmd)
        {
            Debug.Log("ぷれぜんたー(移動中):" + cmd.ExecutedObjectId + " TargetPosition:" + cmd.TargetPosition);
            // 自分以外は処理しない
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;
            _view.MoveTo(cmd.TargetPosition);
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnDisplayIdentifiableStatus(DisplayIdentifiableStatusCommand cmd)
        {
            Debug.Log("ぷれぜんたー:" + cmd.ExecutedObjectId);

            // 自分以外は処理しない
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;

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
            // ドラッグ開始状態
            else if (_status.CurrentStatus == IdentifiableStatus.DraggingStarted)
            {
                _view.DisplayDragStatus();
            }
        }
    }
}

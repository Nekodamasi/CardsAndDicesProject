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
        }
        public void Dispose()
        {
            _identifiableCommandBus.Off<DisplayIdentifiableStatusCommand>(OnDisplayIdentifiableStatus);
        }

        /// <summary>
        /// 現在の状態をViewに反映します。
        /// </summary>
        private void OnDisplayIdentifiableStatus(DisplayIdentifiableStatusCommand cmd)
        {
                Debug.Log("ぷれぜんたー:" + cmd.ExecutedObjectId);
            if (_view.CompositeObjectId != cmd.ExecutedObjectId) return;

            // ホバー状態
            if (_status.CurrentStatus == IdentifiableStatus.Hover)
            {
                _view.DisplayHoverStatus();
            }
            else if (_status.CurrentStatus == IdentifiableStatus.Normal)
            {
                _view.DisplayNormalStatus();
            }
        }
    }
}

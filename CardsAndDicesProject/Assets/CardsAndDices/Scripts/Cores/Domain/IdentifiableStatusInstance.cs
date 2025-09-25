using UnityEngine;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 個々の識別可能オブジェクトの現在の状態を保持・管理するインスタンス。
    /// </summary>
    public class IdentifiableStatusInstance : IDisposable, IIdentifiableInstance
    {
       private CompositeObjectId _compositeObjectId;

        private IdentifiableStatus _currentStatus;
        private Vector3 _homePosition;
        private GameEventBus _eventBus;
        private IdentifiableUIStateMachine _identifiableUIStateMachine;

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// オブジェクトの現在の状態。
        /// </summary>
        public Vector3 HomePosition => _homePosition;

        /// <summary>
        /// オブジェクトの現在の状態。
        /// </summary>
        public IdentifiableStatus CurrentStatus => _currentStatus;

        /// <summary>
        /// 現在のユーザーのコントロール状態を返します
        /// </summary>
        public bool CurrentSetColliderEnabled
        {
            get
            {
                if (_currentStatus == IdentifiableStatus.Hide) return false;
                if (_currentStatus == IdentifiableStatus.Inactive) return false;
                if (_currentStatus == IdentifiableStatus.Move) return false;
                if(_identifiableUIStateMachine.CurrentState == IdentifiableUIState.NonResponse) return false;
                return true;
            }
        }

        /// <summary>
        /// 現在のホームステータスを介します
        /// </summary>
        public IdentifiableStatus CurrentHomeStatus;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="compositeObjectId">追跡対象のオブジェクトID。</param>
        public IdentifiableStatusInstance(CompositeObjectId compositeObjectId, GameEventBus identifiableCommandBus, IdentifiableUIStateMachine identifiableUIStateMachine)
        {
            CurrentHomeStatus = IdentifiableStatus.Normal;
            _compositeObjectId = compositeObjectId;
            _currentStatus = IdentifiableStatus.Hide;
            _homePosition = Vector3.zero;
            _eventBus = identifiableCommandBus;
            _identifiableUIStateMachine = identifiableUIStateMachine;
            _eventBus.On<ChangeViewStatusEvent>(OnIdentifiableChangeStatus);
            _eventBus.On<ChangeHomePositionStatusViewEvent>(OnIdentifiableChangeHomePosition);
        }

        public void Dispose()
        {
            _eventBus.Off<ChangeViewStatusEvent>(OnIdentifiableChangeStatus);
        }

        /// <summary>
        /// ポジションの変更コマンド。
        /// </summary>
        private void OnIdentifiableChangeHomePosition(ChangeHomePositionStatusViewEvent evt)
        {
            if (_compositeObjectId != evt.ExecutedObjectId) return;

            _homePosition = evt.TargetPosition;
        }

        /// <summary>
        /// ステータスの変更コマンド。
        /// </summary>
        private void OnIdentifiableChangeStatus(ChangeViewStatusEvent evt)
        {
            if (_compositeObjectId != evt.ExecutedObjectId) return;

            UpdateStatus(evt.NewStatus);
        }

       /// <summary>
        /// オブジェクトの状態を更新します。
        /// </summary>
        /// <param name="newStatus">新しい状態。</param>
        public void UpdateStatus(IdentifiableStatus newStatus)
        {
            if (_currentStatus == newStatus) return;

            _currentStatus = newStatus;
        }
    }
}

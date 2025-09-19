using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 個々の識別可能オブジェクトの現在の状態を保持・管理するインスタンス。
    /// </summary>
    public class IdentifiableStatusInstance
    {
        private CompositeObjectId _objectId;
        private IdentifiableStatus _currentStatus;
        private Vector3 _homePosition;
        private bool _isDragDisabled;
        private GameEventBus _identifiableCommandBus;
        private IdentifiableUIStateMachine _identifiableUIStateMachine;

        /// <summary>
        /// このインスタンスが追跡するオブジェクトのID。
        /// </summary>
        public CompositeObjectId ObjectId => _objectId;

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
        public IdentifiableStatus CurrentHomeStatus {
            get
            {
                if(_isDragDisabled)
                {
                    return IdentifiableStatus.Grayout;
                }
                else
                {
                    return IdentifiableStatus.Normal;
                }
             }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="objectId">追跡対象のオブジェクトID。</param>
        public IdentifiableStatusInstance(CompositeObjectId objectId, GameEventBus identifiableCommandBus, IdentifiableUIStateMachine identifiableUIStateMachine)
        {
            _isDragDisabled = false;
            _objectId = objectId;
            _currentStatus = IdentifiableStatus.Hide;
            _homePosition = Vector3.zero;
            _identifiableCommandBus = identifiableCommandBus;
            _identifiableUIStateMachine = identifiableUIStateMachine;
            _identifiableCommandBus.On<IdentifiableChangeStatusEvent>(OnIdentifiableChangeStatus);
            _identifiableCommandBus.On<IdentifiableChangeHomePositionCommand>(OnIdentifiableChangeHomePosition);
        }

        public void Dispose()
        {
            _identifiableCommandBus.Off<IdentifiableChangeStatusEvent>(OnIdentifiableChangeStatus);
        }

        /// <summary>
        /// ポジションの変更コマンド。
        /// </summary>
        private void OnIdentifiableChangeHomePosition(IdentifiableChangeHomePositionCommand cmd)
        {
            if (_objectId != cmd.ExecutedObjectId) return;

            _homePosition = cmd.TargetPosition;
        }

        /// <summary>
        /// ステータスの変更コマンド。
        /// </summary>
        private void OnIdentifiableChangeStatus(IdentifiableChangeStatusEvent cmd)
        {
            if (_objectId != cmd.ExecutedObjectId) return;

            UpdateStatus(cmd.NewStatus);
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

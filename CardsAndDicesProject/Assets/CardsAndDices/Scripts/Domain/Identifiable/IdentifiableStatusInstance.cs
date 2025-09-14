namespace CardsAndDices
{
    /// <summary>
    /// 個々の識別可能オブジェクトの現在の状態を保持・管理するインスタンス。
    /// </summary>
    public class IdentifiableStatusInstance
    {
        private CompositeObjectId _objectId;
        private IdentifiableStatus _currentStatus;
        private IdentifiableCommandBus _identifiableCommandBus;

        /// <summary>
        /// このインスタンスが追跡するオブジェクトのID。
        /// </summary>
        public CompositeObjectId ObjectId => _objectId;

        /// <summary>
        /// オブジェクトの現在の状態。
        /// </summary>
        public IdentifiableStatus CurrentStatus => _currentStatus;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="objectId">追跡対象のオブジェクトID。</param>
        public IdentifiableStatusInstance(CompositeObjectId objectId, IdentifiableCommandBus identifiableCommandBus)
        {
            _objectId = objectId;
            _currentStatus = IdentifiableStatus.Hide;
            _identifiableCommandBus = identifiableCommandBus;
            _identifiableCommandBus.On<IdentifiableChangeStatusCommand>(OnIdentifiableChangeStatus);
        }

         /// <summary>
        /// ステータスの変更コマンド。
        /// </summary>
        private void OnIdentifiableChangeStatus(IdentifiableChangeStatusCommand cmd)
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

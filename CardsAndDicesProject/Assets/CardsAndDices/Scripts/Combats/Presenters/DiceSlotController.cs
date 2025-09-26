using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// DiceSlotInstanceを管理し、変更をコマンドで通知します
    /// </summary>
    public class DiceSlotController : IDisposable, IIdentifiableController
    {
        private readonly DiceSlotInstance _diceSlotInstance;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _diceSlotInstance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiceSlotController(DiceSlotInstance diceSlotInstance, GameEventBus eventBus)
        {
            _diceSlotInstance = diceSlotInstance;
            _eventBus = eventBus;
            _eventBus.On<PlacedDiceEvent>(OnPlacedDice);
            _eventBus.On<ReflowPlacedDiceEvent>(OnReflowPlacedDice);
            _eventBus.On<RemoveDiceEvent>(OnRemoveDice);
            _eventBus.On<MoveToAnimationReflowDiceEvent>(OnMoveToAnimationReflowDice);
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<IdentifiableStateDropFailureEvent>(OnIdentifiableStateDropFailure);
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<PlacedDiceEvent>(OnPlacedDice);
            _eventBus.Off<ReflowPlacedDiceEvent>(OnReflowPlacedDice);
            _eventBus.Off<RemoveDiceEvent>(OnRemoveDice);
            _eventBus.Off<MoveToAnimationReflowDiceEvent>(OnMoveToAnimationReflowDice);
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<IdentifiableStateDropFailureEvent>(OnIdentifiableStateDropFailure);
        }

        /// <summary>
        /// ドラッグ失敗イベント
        /// </summary>
        private void OnIdentifiableStateDropFailure(IdentifiableStateDropFailureEvent evt)
        {
            // 配置側でリセットする
            _diceSlotInstance.PlacedDice(_diceSlotInstance.PlacedDiceId);

            // ホームポジションを設定
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(_diceSlotInstance.PlacedDiceId, _diceSlotInstance.DiceSlotPosition));
            _eventBus.Emit(new MoveToAnimationIdentifiableEvent(_diceSlotInstance.PlacedDiceId, _diceSlotInstance.DiceSlotPosition));
        }

        /// <summary>
        /// リフロー配置を配置側に適用する
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _diceSlotInstance.PlacedDice(_diceSlotInstance.ReflowPlacedDiceId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(_diceSlotInstance.CompositeObjectId, _diceSlotInstance.DiceSlotPosition));
        }

        /// <summary>
        /// リフロー位置へのダイスAnimation移動
        /// </summary>
        private void OnMoveToAnimationReflowDice(MoveToAnimationReflowDiceEvent evt)
        {
            if (_diceSlotInstance.CompositeObjectId != evt.DiceSlotId) return;
            if (_diceSlotInstance.ReflowPlacedDiceId == null) return;

            _eventBus.Emit(new MoveToIdentifiableEvent(_diceSlotInstance.ReflowPlacedDiceId, _diceSlotInstance.DiceSlotPosition));
        }

        /// <summary>
        /// ダイスの配置処理
        /// </summary>
        private void OnPlacedDice(PlacedDiceEvent evt)
        {
//            Debug.Log("おんぷらいすだいす１：" + _diceSlotInstance.CompositeObjectId + "/" + evt.DiceSlotId + "/リフローダイス：" + _diceSlotInstance.ReflowPlacedDiceId + " Position:" + _diceSlotInstance.DiceSlotPosition);
            if (evt.DiceSlotId != _diceSlotInstance.CompositeObjectId) return;
            if (_diceSlotInstance.IsOccupied)
            {
                _diceSlotInstance.RemoveDice();
            }
            _diceSlotInstance.PlacedDice(evt.DiceId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(evt.DiceId, _diceSlotInstance.DiceSlotPosition));
        }

        /// <summary>
        /// ダイスリフロー配置処理
        /// </summary>
        private void OnReflowPlacedDice(ReflowPlacedDiceEvent evt)
        {
            if(evt.DiceSlotLocation != _diceSlotInstance.DiceSlotLocation) return;
            _diceSlotInstance.ReflowPlacedDice(evt.DiceId);
        }

        /// <summary>
        /// ダイスのリムーブ処理
        /// </summary>
        private void OnRemoveDice(RemoveDiceEvent evt)
        {
            _diceSlotInstance.RemoveDice();
        }

    }
}

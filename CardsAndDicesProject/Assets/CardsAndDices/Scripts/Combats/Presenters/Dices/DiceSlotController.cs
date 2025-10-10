using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// DiceSlotInstanceを管理し、変更をコマンドで通知します
    /// </summary>
    public class DiceSlotController : IDisposable, IIdentifiableController
    {
        private readonly DiceSlotInstance _instance;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiceSlotController(DiceSlotInstance diceSlotInstance, GameEventBus eventBus)
        {
            _instance = diceSlotInstance;
            _eventBus = eventBus;
            _eventBus.On<PlacedDiceEvent>(OnPlacedDice);
            _eventBus.On<ReflowPlacedDiceEvent>(OnReflowPlacedDice);
            _eventBus.On<RemoveDiceEvent>(OnRemoveDice);
            _eventBus.On<MoveToAnimationReflowDiceEvent>(OnMoveToAnimationReflowDice);
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<IdentifiableStateDropFailureEvent>(OnIdentifiableStateDropFailure);
            _eventBus.On<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
            _eventBus.On<UpdateDisplayCreatureStatusEvent>(OnUpdateDisplayCreatureStatus);
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
            _eventBus.On<DisplayUIStatusEvent>(OnDisplayUIStatus);
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
            _eventBus.Off<UpdateDisplayCreatureStatusEvent>(OnUpdateDisplayCreatureStatus);
        }

        /// <summary>
        /// 指定Dispose
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            if (evt.CompositeObjectId == _instance.ReflowPlacedDiceId)
            {
                _instance.ReflowPlacedDice(null);
            }
            if (evt.CompositeObjectId == _instance.PlacedDiceId)
            {
                _instance.PlacedDice(null);
            }
        }

        /// <summary>
        /// ドラッグ失敗イベント
        /// </summary>
        private void OnIdentifiableStateDropFailure(IdentifiableStateDropFailureEvent evt)
        {
            // 配置側でリセットする
            _instance.PlacedDice(_instance.PlacedDiceId);

            // ホームポジションを設定
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(_instance.PlacedDiceId, _instance.DiceSlotPosition));
            _eventBus.Emit(new MoveToAnimationIdentifiableEvent(_instance.PlacedDiceId, _instance.DiceSlotPosition));
        }

        /// <summary>
        /// ディスプレイを現在のステータスで表示するイベント
        /// </summary>
        private void OnUpdateDisplayCreatureStatus(UpdateDisplayCreatureStatusEvent evt)
        {
            if (evt.ExecutedObjectId != _instance.CompositeObjectId) return;
            _eventBus.Emit(new IdentifiableCurrentUIStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// UIリセット
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _instance.PlacedDice(_instance.ReflowPlacedDiceId);
            _eventBus.Emit(new IdentifiableResetUIStatusEvent(_instance.CompositeObjectId));
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(_instance.CompositeObjectId, _instance.DiceSlotPosition));
        }

        /// <summary>
        /// ディスプレイを現在のステータスで表示するイベント
        /// </summary>
        private void OnDisplayUIStatus(DisplayUIStatusEvent evt)
        {
            _eventBus.Emit(new IdentifiableCurrentUIStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// リフロー位置へのダイスAnimation移動
        /// </summary>
        private void OnMoveToAnimationReflowDice(MoveToAnimationReflowDiceEvent evt)
        {
            if (_instance.CompositeObjectId != evt.DiceSlotId) return;
            if (_instance.ReflowPlacedDiceId == null) return;

            _eventBus.Emit(new MoveToIdentifiableEvent(_instance.ReflowPlacedDiceId, _instance.DiceSlotPosition));
        }

        /// <summary>
        /// ダイスの配置処理
        /// </summary>
        private void OnPlacedDice(PlacedDiceEvent evt)
        {
//            Debug.Log("おんぷらいすだいす１：" + _diceSlotInstance.CompositeObjectId + "/" + evt.DiceSlotId + "/リフローダイス：" + _diceSlotInstance.ReflowPlacedDiceId + " Position:" + _diceSlotInstance.DiceSlotPosition);
            if (evt.DiceSlotId != _instance.CompositeObjectId) return;
            if (_instance.IsOccupied)
            {
                _instance.RemoveDice();
            }
            _instance.PlacedDice(evt.DiceId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(evt.DiceId, _instance.DiceSlotPosition));
        }

        /// <summary>
        /// ダイスリフロー配置処理
        /// </summary>
        private void OnReflowPlacedDice(ReflowPlacedDiceEvent evt)
        {
            if(evt.DiceSlotLocation != _instance.DiceSlotLocation) return;
            _instance.ReflowPlacedDice(evt.DiceId);
        }

        /// <summary>
        /// ダイスのリムーブ処理
        /// </summary>
        private void OnRemoveDice(RemoveDiceEvent evt)
        {
            _instance.RemoveDice();
        }

    }
}

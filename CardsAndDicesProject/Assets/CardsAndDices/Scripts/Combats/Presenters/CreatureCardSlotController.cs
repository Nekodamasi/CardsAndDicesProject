using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotInstanceを管理し、変更をコマンドで通知します
    /// </summary>
    public class CreatureCardSlotController : IDisposable, IIdentifiableController
    {
        private readonly CreatureCardSlotInstance _creatureCardSlotInstance;
        private readonly GameEventBus _eventBus;
        private readonly CompositeObjectIdTypeEntity _compositeObjectIdTypeEntity;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _creatureCardSlotInstance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardSlotController(CreatureCardSlotInstance creatureCardSlotInstance, GameEventBus eventBus, CompositeObjectIdTypeEntity compositeObjectIdTypeEntity)
        {
            _creatureCardSlotInstance = creatureCardSlotInstance;
            _eventBus = eventBus;
            _compositeObjectIdTypeEntity = compositeObjectIdTypeEntity;
            _eventBus.On<PlacedCreatureCardSlotEvent>(OnPlacedCreatureCardSlot);
            _eventBus.On<ReflowPlacedCreatureCardSlotEvent>(OnReflowPlacedCreatureCardSlot);
            _eventBus.On<RemoveCreatureCardSlotEvent>(OnRemoveCreatureCardSlot);
            _eventBus.On<MoveToAnimationReflowCreatureCardSlotEvent>(OnMoveToAnimationReflowCreatureCardSlot);
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<IdentifiableStateDropFailureEvent>(OnIdentifiableStateDropFailure);
             
       }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<PlacedCreatureCardSlotEvent>(OnPlacedCreatureCardSlot);
            _eventBus.Off<ReflowPlacedCreatureCardSlotEvent>(OnReflowPlacedCreatureCardSlot);
            _eventBus.Off<RemoveCreatureCardSlotEvent>(OnRemoveCreatureCardSlot);
            _eventBus.Off<MoveToAnimationReflowCreatureCardSlotEvent>(OnMoveToAnimationReflowCreatureCardSlot);
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<IdentifiableStateDropFailureEvent>(OnIdentifiableStateDropFailure);
        }

        /// <summary>
        /// ドラッグ失敗イベント
        /// </summary>
        private void OnIdentifiableStateDropFailure(IdentifiableStateDropFailureEvent evt)
        {
            // 受け入れ対象がドロップ失敗したらリフローを戻すために配置場所をリセットし移動する
            if (_compositeObjectIdTypeEntity != evt.ExecutedObjectId.ObjectType) return;

            // 配置側でリセットする
            _creatureCardSlotInstance.ReflowPlacedCard(_creatureCardSlotInstance.PlacedCardId);

            // ホームポジションを設定
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(_creatureCardSlotInstance.PlacedCardId, _creatureCardSlotInstance.CreatureCardSlotPosition));
            _eventBus.Emit(new MoveToAnimationIdentifiableEvent(_creatureCardSlotInstance.PlacedCardId, _creatureCardSlotInstance.CreatureCardSlotPosition));
        }

        /// <summary>
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _creatureCardSlotInstance.PlacedCard(_creatureCardSlotInstance.ReflowPlacedCardId);
        }

        /// <summary>
        /// リフロー位置へのクリーチャーカードAnimation移動
        /// </summary>
        private void OnMoveToAnimationReflowCreatureCardSlot(MoveToAnimationReflowCreatureCardSlotEvent evt)
        {
            if (_creatureCardSlotInstance.ReflowPlacedCardId == null) return;
            if (evt.CragedCreatureCardId != null && evt.CragedCreatureCardId == _creatureCardSlotInstance.ReflowPlacedCardId) return;
            _eventBus.Emit(new MoveToAnimationIdentifiableEvent(_creatureCardSlotInstance.ReflowPlacedCardId, _creatureCardSlotInstance.CreatureCardSlotPosition));
        }

        /// <summary>
        /// クリーチャーカードの配置処理
        /// </summary>
        private void OnPlacedCreatureCardSlot(PlacedCreatureCardSlotEvent evt)
        {
            if (evt.CreatureCardSlotId != _creatureCardSlotInstance.CompositeObjectId) return;
            if (_creatureCardSlotInstance.IsOccupied)
            {
                _creatureCardSlotInstance.RemoveCard();
            }
            _creatureCardSlotInstance.PlacedCard(evt.CreatureCardId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(evt.CreatureCardSlotId, _creatureCardSlotInstance.CreatureCardSlotPosition));
        }

        /// <summary>
        ///クリーチャーカードリフロー配置処理
        /// </summary>
        private void OnReflowPlacedCreatureCardSlot(ReflowPlacedCreatureCardSlotEvent evt)
        {
            if (evt.CreatureCardSlotId != _creatureCardSlotInstance.CompositeObjectId) return;
            _creatureCardSlotInstance.ReflowPlacedCard(evt.CreatureCard);
        }

        /// <summary>
        /// クリーチャーカードのリムーブ処理
        /// </summary>
        private void OnRemoveCreatureCardSlot(RemoveCreatureCardSlotEvent evt)
        {
            _creatureCardSlotInstance.RemoveCard();
        }

    }
}

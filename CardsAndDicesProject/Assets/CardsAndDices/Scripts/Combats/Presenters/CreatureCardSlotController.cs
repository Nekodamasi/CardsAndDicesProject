using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotInstanceを管理し、変更をコマンドで通知します
    /// </summary>
    public class CreatureCardSlotController : IDisposable, IIdentifiableController
    {
        private readonly CreatureCardSlotInstance _instance;
        private readonly GameEventBus _eventBus;
        private readonly CompositeObjectIdTypeEntity _compositeObjectIdTypeEntity;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardSlotController(CreatureCardSlotInstance creatureCardSlotInstance, GameEventBus eventBus, CompositeObjectIdTypeEntity compositeObjectIdTypeEntity)
        {
            _instance = creatureCardSlotInstance;
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
            _instance.ReflowPlacedCard(_instance.PlacedCardId);

            // ホームポジションを設定
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(_instance.PlacedCardId, _instance.CreatureCardSlotPosition));
            _eventBus.Emit(new MoveToAnimationIdentifiableEvent(_instance.PlacedCardId, _instance.CreatureCardSlotPosition));
        }

        /// <summary>
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _instance.PlacedCard(_instance.ReflowPlacedCardId);
//            _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.CardPlacement, _instance.CompositeObjectId, null));
        }

        /// <summary>
        /// リフロー位置へのクリーチャーカードAnimation移動
        /// </summary>
        private void OnMoveToAnimationReflowCreatureCardSlot(MoveToAnimationReflowCreatureCardSlotEvent evt)
        {
            if (_instance.ReflowPlacedCardId == null) return;
            if (evt.CragedCreatureCardId != null && evt.CragedCreatureCardId == _instance.ReflowPlacedCardId) return;
            _eventBus.Emit(new MoveToAnimationIdentifiableEvent(_instance.ReflowPlacedCardId, _instance.CreatureCardSlotPosition));
        }

        /// <summary>
        /// クリーチャーカードの配置処理
        /// </summary>
        private void OnPlacedCreatureCardSlot(PlacedCreatureCardSlotEvent evt)
        {
            if (evt.CreatureCardSlotId != _instance.CompositeObjectId) return;
            if (_instance.IsOccupied)
            {
                _instance.RemoveCard();
            }
            _instance.PlacedCard(evt.CreatureCardId);
            _eventBus.Emit(new ChangeHomePositionStatusViewEvent(evt.CreatureCardSlotId, _instance.CreatureCardSlotPosition));
        }

        /// <summary>
        ///クリーチャーカードリフロー配置処理
        /// </summary>
        private void OnReflowPlacedCreatureCardSlot(ReflowPlacedCreatureCardSlotEvent evt)
        {
            if (evt.CreatureCardSlotId != _instance.CompositeObjectId) return;
            _instance.ReflowPlacedCard(evt.CreatureCard);
        }

        /// <summary>
        /// クリーチャーカードのリムーブ処理
        /// </summary>
        private void OnRemoveCreatureCardSlot(RemoveCreatureCardSlotEvent evt)
        {
            _instance.RemoveCard();
        }

    }
}

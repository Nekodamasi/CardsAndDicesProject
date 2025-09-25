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

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _creatureCardSlotInstance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardSlotController(CreatureCardSlotInstance creatureCardSlotInstance, GameEventBus eventBus)
        {
            _creatureCardSlotInstance = creatureCardSlotInstance;
            _eventBus = eventBus;
            _eventBus.On<PlacedCreatureCardSlotEvent>(OnPlacedCreatureCardSlot);
            _eventBus.On<ReflowPlacedCreatureCardSlotEvent>(OnReflowPlacedCreatureCardSlot);
            _eventBus.On<RemoveCreatureCardSlotEvent>(OnRemoveCreatureCardSlot);
            _eventBus.On<MoveToAnimationReflowCreatureCardSlotEvent>(OnMoveToAnimationReflowCreatureCardSlot);
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
        }

        /// <summary>
        /// リフロー位置へのクリーチャーカードAnimation移動
        /// </summary>
        private void OnMoveToAnimationReflowCreatureCardSlot(MoveToAnimationReflowCreatureCardSlotEvent evt)
        {
            if (_creatureCardSlotInstance.CompositeObjectId != evt.CreatureCardId) return;
            if (_creatureCardSlotInstance.ReflowPlacedCardId == null) return;
            _eventBus.Emit(new MoveToIdentifiableEvent(_creatureCardSlotInstance.ReflowPlacedCardId, _creatureCardSlotInstance.CreatureCardSlotPosition));
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

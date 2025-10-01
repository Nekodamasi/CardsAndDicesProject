using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotInstanceを管理し、変更をコマンドで通知します
    /// </summary>
    public class AbilityController : IDisposable, IIdentifiableController
    {
        private readonly AbilityInstance _instance;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AbilityController(AbilityInstance instance, GameEventBus eventBus)
        {
            _instance = instance;
            _eventBus = eventBus;
//            _eventBus.On<PlacedCreatureCardSlotEvent>(OnPlacedCreatureCardSlot);
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
//            _eventBus.Off<PlacedCreatureCardSlotEvent>(OnPlacedCreatureCardSlot);
        }
    }
}

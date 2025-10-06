using System;
using UnityEngine;
using System.Collections.Generic;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureStatusInstanceへの変更をステータスアイコンに通知します
    /// </summary>
    public class CreatureCardStatusIconController : IDisposable, IIdentifiableController
    {
        private readonly CreatureStatusInstance _instance;
        private readonly GameEventBus _eventBus;
        private readonly List<CreatureStatusIconData> _creatureStatusIconDataList;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardStatusIconController(CreatureStatusInstance instance, GameEventBus eventBus, List<CreatureStatusIconData> creatureStatusIconDataList)
        {
            _instance = instance;
            _eventBus = eventBus;
            _creatureStatusIconDataList = creatureStatusIconDataList;
            _eventBus.On<UpdateDisplayCreatureStatusEvent>(OnUpdateDisplayCreatureStatus);
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<UpdateDisplayCreatureStatusEvent>(OnUpdateDisplayCreatureStatus);
        }

        /// <summary>
        /// クリーチャーカードアイコンの更新
        /// </summary>
        private void OnUpdateDisplayCreatureStatus(UpdateDisplayCreatureStatusEvent evt)
        {
            UpdateIcon(evt.CreatureCardId);
        }

        /// <summary>
        /// クリーチャーカードの配置処理
        /// </summary>
        private void UpdateIcon(CompositeObjectId creatureCardId)
        {
            if (creatureCardId != _instance.CompositeObjectId) return;
            foreach (var iconData in _creatureStatusIconDataList)
            {
                if (iconData.EffectTargetType == EffectTargetType.Attack)
                {
                    _eventBus.Emit(new DisplaySharedIconElementEvent(creatureCardId, iconData._sharedIconElementTypeEntity, _instance.Attack));
                }
                if (iconData.EffectTargetType == EffectTargetType.Health)
                {
                    _eventBus.Emit(new DisplaySharedIconElementEvent(creatureCardId, iconData._sharedIconElementTypeEntity, _instance.CurrentHealth));
                }
                if (iconData.EffectTargetType == EffectTargetType.Shield)
                {
                    _eventBus.Emit(new DisplaySharedIconElementEvent(creatureCardId, iconData._sharedIconElementTypeEntity, _instance.CurrentShield));
                }
                if (iconData.EffectTargetType == EffectTargetType.Cooldown)
                {
                    _eventBus.Emit(new DisplaySharedIconElementEvent(creatureCardId, iconData._sharedIconElementTypeEntity, _instance.CurrentCooldown));
                }
            }
        }
    }
}

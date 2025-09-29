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
            _eventBus.On<CreatureCardSetUpEvent>(OnCreatureCardSetUp);

        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<CreatureCardSetUpEvent>(OnCreatureCardSetUp);
        }

        /// <summary>
        /// クリーチャーカードの配置処理
        /// </summary>
        private void OnCreatureCardSetUp(CreatureCardSetUpEvent evt)
        {
            if (evt.CreatureCardId != _instance.CompositeObjectId) return;
            foreach (var iconData in _creatureStatusIconDataList)
            {
                Debug.Log("こんとろーらー->iconData.EffectTargetType:" + iconData.EffectTargetType);
                if (iconData.EffectTargetType == EffectTargetType.Attack)
                {
//                    _eventBus.Emit(new DisplaySharedIconElementEvent(evt.CreatureCardId, iconData._sharedIconElementTypeEntity, _instance.Attack));
                    _eventBus.Emit(new DisplaySharedIconElementEvent(evt.CreatureCardId, iconData._sharedIconElementTypeEntity, _instance.Attack));
                }
            }
        }
    }
}

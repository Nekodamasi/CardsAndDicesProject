using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// バフ／デバフ効果を一元管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "EffectManager", menuName = "CardsAndDices/Combats/Managers/Effects/EffectManager")]
    public class EffectManager : ScriptableObject, IEffectValue, IDisposable, IIdentifiableManager
    {
        [Header("Components")]
        private readonly List<EffectInstance> _instances = new();
        private readonly List<EffectController> _controllers = new();
        private GameEventBus _eventBus;

        [Inject]
        public void Initialize(GameEventBus eventBusBus)
        {
            _eventBus = eventBusBus;
            _eventBus.On<ApplyEffectEvent>(OnApplyEffect);
            _eventBus.On<UpdateEffectExpiredEvent>(OnUpdateEffectExpired);
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposeControllers();
            _eventBus.Off<ApplyEffectEvent>(OnApplyEffect);
            _eventBus.Off<UpdateEffectExpiredEvent>(OnUpdateEffectExpired);
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _instances)
            {
                instance.Dispose();
            }
            _instances.Clear();
        }

        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var controller in _controllers)
            {
                controller.Dispose();
            }
            _controllers.Clear();
        }

        /// <summary>
        /// 指定された識別IDへのエフェクトターゲットタイプの合計値を返します。
        /// </summary>
        private void OnApplyEffect(ApplyEffectEvent evt)
        {
            var instance = new EffectInstance(evt.TargetObjectId, evt.EffectTargetType, evt.Value, evt.ExpiredTiming, evt.RemainingTurns);
            _instances.Add(instance);
            var controller = new EffectController(instance, _eventBus);
            _controllers.Add(controller);
        }

        /// <summary>
        /// 指定されたタイミングで有効期限の更新を行う
        /// </summary>
        private void OnUpdateEffectExpired(UpdateEffectExpiredEvent evt)
        {
            var list = _instances.Where(e => e.CompositeObjectId == evt.SourceObjectId && e.ExpiredTiming == evt.TriggerTiming && e.IsExpired == false).ToList();

            foreach (var instance in list)
            {
                instance.UpdateExpired(evt.TriggerTiming);
                if (instance.IsExpired)
                {
                    DisposeEffect(instance);
                }
            }
        }

        /// <summary>
        /// 指定された識別IDへのエフェクトターゲットタイプの合計値を返します。
        /// </summary>
        public int GetTotalEffectValue(CompositeObjectId compositeObjectId, EffectTargetType type)
        {
            return _instances
                .Where(e => e.CompositeObjectId.Equals(compositeObjectId) && e.TargetType == type)
                .Sum(e => e.Value);
        }

        /// <summary>
        /// 指定されたクリーチャーIDに紐づく全てのエフェクトを削除します。
        /// </summary>
        /// <param name="creatureId">所有者であるクリーチャーのID</param>
        public void RemoveEffectsByOwnerId(CompositeObjectId OwnerId)
        {
            var list = _instances.Where(e => e.CompositeObjectId == OwnerId && e.IsExpired == false).ToList();
            foreach (var instance in list)
            {
                DisposeEffect(instance);
            }
        }

        /// <summary>
        /// エフェクトをディスポーズします
        /// </summary>
        private void DisposeEffect(EffectInstance instance)
        {
            var controller = _controllers.Where(e => e.InstanceId == instance.CompositeObjectId).FirstOrDefault();
            instance.Dispose();
            _instances.Remove(instance);
            controller.Dispose();
            _controllers.Remove(controller);
        }
        public List<EffectInstance> GetInstanceList()
        {
            return _instances;
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeするイベント
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            DisposeByCompositeObjectId(evt.CompositeObjectId);
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeします
        /// </summary>
        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
            var instance = _instances.Where(i => i.CompositeObjectId == compositeObjectId).FirstOrDefault();
            if (instance is null)
            {
                return;
            }
            instance.Dispose();
            _instances.Remove(instance);
            var controller = _controllers.Where(c => c.InstanceId == compositeObjectId).FirstOrDefault();
            controller.Dispose();
            _controllers.Remove(controller);
        }
    }
}

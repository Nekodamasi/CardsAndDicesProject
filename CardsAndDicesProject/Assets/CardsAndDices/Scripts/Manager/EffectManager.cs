using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// バフ／デバフ効果を一元管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "EffectManager", menuName = "CardsAndDices/Managers/EffectManager")]
    public class EffectManager : ScriptableObject
    {
        private readonly List<EffectInstance> _activeEffects = new List<EffectInstance>();
        private EffectFactory _effectFactory;
        private SpriteCommandBus _commandBus;
        [Inject]
        public void Initialize(SpriteCommandBus commandBus)
        {
            ClearCollections();
            _commandBus = commandBus;
            _effectFactory = new EffectFactory();
            _commandBus.On<UpdateEffectExpiredCommand>(OnUpdateEffectExpired);
            _commandBus.On<ApplyEffectCommand>(OnApplyEffect);
        }
        private void ClearCollections()
        {
            _activeEffects.Clear();
        }

        private void OnDisable()
        {
            _commandBus.Off<UpdateEffectExpiredCommand>(OnUpdateEffectExpired);
            _commandBus.Off<ApplyEffectCommand>(OnApplyEffect);
        }

        private void OnApplyEffect(ApplyEffectCommand command)
        {
            var instance = _effectFactory.Create(command.TargetObjectId, command.EffectData, command.EffectTargetType, command.Value);
            Debug.Log("<color=Green>えふぇくとあぷらい：</color>" + command.TargetObjectId + "_" + instance.TargetType + "_" + instance.CurrentValue);
            RegisterEffect(instance);
        }

        private void OnUpdateEffectExpired(UpdateEffectExpiredCommand command)
        {
            foreach (var instance in _activeEffects)
            {
                instance.CheckExpired(command.TriggerTiming);
                Debug.Log("<color=Green>OnUpdateEffectExpired</color>" + instance.TargetObjectId + "_" + instance.IsExpired);
            }
            RemoveExpiredEffects();
        }

        /// <summary>
        /// 新しいエフェクトインスタンスを登録します。
        /// </summary>
        /// <param name="effectInstance">登録するエフェクトインスタンス</param>
        public void RegisterEffect(EffectInstance effectInstance)
        {
            _activeEffects.Add(effectInstance);
        }

        /// <summary>
        /// 指定されたエフェクトインスタンスを管理リストから削除します。
        /// </summary>
        /// <param name="effectInstance">削除するエフェクトインスタンス</param>
        public void RemoveEffect(EffectInstance effectInstance)
        {
            _activeEffects.Remove(effectInstance);
        }

        /// <summary>
        /// 期限切れのエフェクトをリストから削除します。
        /// </summary>
        private void RemoveExpiredEffects()
        {
            var expiredEffects = _activeEffects.Where(e => e.IsExpired).ToList();
            foreach (var effect in expiredEffects)
            {
                // TODO: EffectExpirationCommandを発行する
                RemoveEffect(effect);
            }
        }

        /// <summary>
        /// 指定されたカードに適用されている特定タイプのエフェクトの合計値を取得します。
        /// </summary>
        /// <param name="targetObjectId">カードのID</param>
        /// <param name="targetType">エフェクトの対象タイプ</param>
        /// <returns>エフェクトの合計値</returns>
        public int GetTotalEffectValue(CompositeObjectId targetObjectId, EffectTargetType targetType)
        {
            return _activeEffects
                .Where(e => e.TargetObjectId.Equals(targetObjectId) && e.TargetType == targetType)
                .Sum(e => e.CurrentValue);
        }
    }
}

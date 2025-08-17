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
        private IObjectResolver _resolver;

        /// <summary>
        /// DIコンテナから依存性を注入するために使用します。
        /// </summary>
        /// <param name="resolver">DIコンテナ</param>
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            // TODO: SpriteCommandBusをDIコンテナから取得し、イベントを購読する
            // var commandBus = _resolver.Resolve<SpriteCommandBus>();
            // commandBus.On<TurnStartCommand>(HandleTurnStart);
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
        /// ターン開始イベントを処理します。
        /// </summary>
        /// <param name="command">ターン開始コマンド</param>
        private void HandleTurnStart(ICommand command)
        {
            // TurnCountタイプのエフェクトの残りターンを更新
            foreach (var effect in _activeEffects)
            {
                if (effect.Data.DurationType == EffectDurationType.TurnCount)
                {
                    effect.DecrementTurn();
                }
            }

            // 期限切れのエフェクトを削除
            RemoveExpiredEffects();
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
        /// <param name="cardId">カードのID</param>
        /// <param name="targetType">エフェクトの対象タイプ</param>
        /// <returns>エフェクトの合計値</returns>
        public int GetTotalEffectValue(CompositeObjectId cardId, EffectTargetType targetType)
        {
            return _activeEffects
                .Where(e => e.CardId.Equals(cardId) && e.Data.TargetType == targetType)
                .Sum(e => e.Data.Value);
        }
    }
}

using System;
namespace CardsAndDices
{
    /// <summary>
    /// Effectを管理するインスタンス。
    /// </summary>
    public class EffectInstance : IDisposable, IIdentifiableInstance
    {
        /// <summary>
        /// エフェクトの対象者の識別子
        /// </summary>
        public CompositeObjectId CompositeObjectId { get; private set; }

        /// <summary>
        /// 適用対象タイプ
        /// </summary>
        public EffectTargetType TargetType { get; private set; }

        /// <summary>
        /// 残存ターン数
        /// </summary>
        public int RemainingTurns { get; private set; }

        /// <summary>
        /// 有効期限切れフラグ
        /// </summary>
        public bool IsExpired { get; private set; }

        /// <summary>
        /// Effectの値
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 有効期限切れのタイミング
        /// </summary>
        public ActivationTiming ExpiredTiming { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EffectInstance(CompositeObjectId targetObjectId, EffectTargetType effectTargetType, int effectValue, ActivationTiming expiredTiming, int remainingTurns)
        {
            Initialize(targetObjectId, effectTargetType, effectValue, expiredTiming, remainingTurns);
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="TargetObjectId">対象の識別ID</param>
        public void Initialize(CompositeObjectId targetObjectId, EffectTargetType effectTargetType, int effectValue, ActivationTiming expiredTiming, int remainingTurns)
        {
            CompositeObjectId = targetObjectId;
            IsExpired = false;
            TargetType = effectTargetType;
            Value = effectValue;
            ExpiredTiming = expiredTiming;
            RemainingTurns = remainingTurns;
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// 有効期限をチェックし、切れていればIsExpiredフラグを立てます。
        /// </summary>
        public void UpdateExpired(ActivationTiming rxpiredTiming)
        {
            if (ExpiredTiming == rxpiredTiming)
            {
                RemainingTurns--;
            }
            if (RemainingTurns <= 0)
            {
                IsExpired = true;
            }
        }
    }
}

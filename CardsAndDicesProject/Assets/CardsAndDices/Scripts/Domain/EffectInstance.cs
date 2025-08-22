namespace CardsAndDices
{
    /// <summary>
    /// EffectDataを参照し、カードごとに生成される実行時エフェクトインスタンス。
    /// </summary>
    public class EffectInstance
    {
        /// <summary>
        /// エフェクトの対象者の識別子
        /// </summary>
        public CompositeObjectId TargetObjectId { get; private set; }

        /// <summary>
        /// 参照先のEffectData
        /// </summary>
        public EffectData Data { get; private set; }

        /// <summary>
        /// 参照先のEffectData
        /// </summary>
        public int CurrentValue { get; private set; }

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
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="effectData">参照するエフェクトデータ</param>
        /// <param name="cardId">対象カードのID</param>
        public void Initialize(EffectData effectData, CompositeObjectId cardId, EffectTargetType effectTargetType, int currentValue)
        {
            Data = effectData;
            TargetObjectId = cardId;
            IsExpired = false;
            TargetType = effectTargetType;
            CurrentValue = currentValue;

            RemainingTurns = Data.DurationValue;
        }

        /// <summary>
        /// 有効期限をチェックし、切れていればIsExpiredフラグを立てます。
        /// </summary>
        public void CheckExpired(TriggerTiming triggerTiming)
        {
            if (Data.UpdateTiming == triggerTiming)
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

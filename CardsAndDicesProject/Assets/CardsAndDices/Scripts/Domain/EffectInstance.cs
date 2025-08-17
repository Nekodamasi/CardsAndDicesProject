namespace CardsAndDices
{
    /// <summary>
    /// EffectDataを参照し、カードごとに生成される実行時エフェクトインスタンス。
    /// </summary>
    public class EffectInstance
    {
        /// <summary>
        /// 対象カードの識別子
        /// </summary>
        public CompositeObjectId CardId { get; private set; }

        /// <summary>
        /// 参照先のEffectData
        /// </summary>
        public EffectData Data { get; private set; }

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
        public void Initialize(EffectData effectData, CompositeObjectId cardId)
        {
            Data = effectData;
            CardId = cardId;
            IsExpired = false;

            if (Data.DurationType == EffectDurationType.TurnCount)
            {
                RemainingTurns = Data.DurationValue;
            }
        }

        /// <summary>
        /// イベントを受信した際の処理。
        /// </summary>
        /// <param name="eventData">受信したイベントデータ</param>
        public void OnEvent(ICommand eventData)
        {
            // TODO: イベントに応じた処理を実装
            // 例: TurnStartEventならRemainingTurnsをデクリメント
            CheckExpiration();
        }

        /// <summary>
        /// 有効期限をチェックし、切れていればIsExpiredフラグを立てます。
        /// </summary>
        public void CheckExpiration()
        {
            if (Data.DurationType == EffectDurationType.TurnCount && RemainingTurns <= 0)
            {
                IsExpired = true;
            }
            // TODO: 他のDurationTypeに応じた期限切れチェックを実装
        }

        /// <summary>
        /// ターン数を1減少させます。
        /// </summary>
        public void DecrementTurn()
        {
            if (Data.DurationType == EffectDurationType.TurnCount)
            {
                RemainingTurns--;
                CheckExpiration();
            }
        }
    }
}

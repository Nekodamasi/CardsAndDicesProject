namespace CardsAndDices
{
    /// <summary>
    /// エフェクトの持続条件タイプを定義します。
    /// </summary>
    public enum EffectDurationType
    {
        /// <summary>
        /// ターン数で持続
        /// </summary>
        TurnCount,

        /// <summary>
        /// クールダウンのリセット時に終了
        /// </summary>
        CooldownReset,

        /// <summary>
        /// 特定のイベントで終了
        /// </summary>
        EventTrigger
    }
}

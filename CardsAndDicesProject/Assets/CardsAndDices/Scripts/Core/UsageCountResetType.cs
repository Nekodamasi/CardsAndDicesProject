namespace CardsAndDices
{
    /// <summary>
    /// ダイスインレットの使用可能回数がリセットされるタイミングを定義します。
    /// </summary>
    public enum UsageCountResetType
    {
        /// <summary>
        /// ターン終了時にリセット
        /// </summary>
        TurnEnd,

        /// <summary>
        /// クールダウン完了時にリセット
        /// </summary>
        CooldownReset
    }
}

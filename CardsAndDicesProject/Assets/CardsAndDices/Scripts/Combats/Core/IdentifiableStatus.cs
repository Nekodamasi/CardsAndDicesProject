namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトが取りうる状態を定義するenum。
    /// </summary>
    public enum IdentifiableStatus
    {
        /// <summary>
        /// 通常状態。
        /// </summary>
        Normal,

        /// <summary>
        /// グレイアウト状態。
        /// </summary>
        Grayout,

        /// <summary>
        /// ホバー状態。
        /// </summary>
        Hover,

        /// <summary>
        /// ドラッグ開始状態。
        /// </summary>
        DraggingStarted, // ドラッグ開始時

        /// <summary>
        /// ドラッグ中状態。
        /// </summary>
        DraggingInProgress,

        /// <summary>
        /// 移動中状態。
        /// </summary>
        Move,

        /// <summary>
        /// 受け入れ可能状態。
        /// </summary>
        Acceptable,

        /// <summary>
        /// 不活性状態。
        /// </summary>
        Inactive,

        /// <summary>
        /// 非表示状態。
        /// </summary>
        Hide
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// playerに提示される発動条件の見た目の種類。内部処理は同じでも、UI上で「特定の目で発動」や「合計値で発動」など、異なる演出がされる。
    /// </summary>
    public enum InletActivationViewType
    {
        SingleMatchTrigger, // 単一一致トリガー
        TotalSumTrigger   // 合計値トリガー
    }
}

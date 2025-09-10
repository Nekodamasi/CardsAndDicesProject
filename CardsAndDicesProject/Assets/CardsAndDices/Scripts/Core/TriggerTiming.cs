namespace CardsAndDices
{
    /// <summary>
    /// 各abilityの期限や発動のタイミング
    /// </summary>
    public enum TriggerTiming
    {
        CardPlacement, // カード配置
        Inlet, // インレット発動
        TurnEnd, // ターンエンド
        CoolDownEnd, // クールダウンエンド
    }
}

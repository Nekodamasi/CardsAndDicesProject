namespace CardsAndDices
{
    /// <summary>
    /// 各abilityの期限や発動のタイミング
    /// </summary>
    public enum ActivationTiming
    {
        CardPlacement, // カード配置
        Inlet, // インレット発動
        TurnEndBuffDebuff, // ターンエンド
        TurnEndAttack, // ターンエンド
        CoolDownEnd, // クールダウンエンド
        ResetTurnEnd, // リセットターンエンド
    }
}

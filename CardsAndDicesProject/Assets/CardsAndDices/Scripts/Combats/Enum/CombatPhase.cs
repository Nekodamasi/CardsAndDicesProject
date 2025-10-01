
namespace CardsAndDices
{
    /// <summary>
    /// ライン内でのスロットの具体的な役割や位置を定義します。
    /// </summary>
    public enum CombatPhase
    {
        None,
        DiceInletEffectPhase, // インレットへの投下からインレットの効果適用まで
        CooldownPhase,   // DiceInletEffectPhase完了から、クールダウン値を１つずつ変更し、基礎攻撃処理が終わるまで
        TurnEndPhase,   // NextTurn押下から、Abilityの効果発動処理が終わるまで
        NextTurnPhase,   // TurnEndphase完了から、各resetとターン更新処理完了まで
        PlayerInputPhase,     // プレイヤーが入力可能なPhase
    }
}

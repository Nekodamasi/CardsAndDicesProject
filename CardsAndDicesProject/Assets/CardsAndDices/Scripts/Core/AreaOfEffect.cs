namespace CardsAndDices
{
    /// <summary>
    /// 攻撃や固有能力の効果範囲を定義します。
    /// </summary>
    public enum AreaOfEffect
    {
        Self, // 攻撃や固有能力を有するクリーチャーカードが対象
        HostileCreature, // 相手チームの最前衛のクリーチャーカードが対象
        TeammateLine, // 攻撃や固有能力を有するクリーチャーカードの配置場所と同じLineの同じチームクリーチャーカードすべてが対象
        OpponentLine, // 攻撃や固有能力を有するクリーチャーカードの配置場所と同じLineの相手チームのクリーチャーカードすべてが対象
        Teammate, // 攻撃や固有能力を有するクリーチャーカードと同じチームクリーチャーカードすべてが対象
        Opponent // 攻撃や固有能力を有するクリーチャーカードの相手チームのクリーチャーカードすべてが対象
    }
}

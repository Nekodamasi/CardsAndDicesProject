namespace CardsAndDices
{
    /// <summary>
    /// ダイスインレットの実行時インスタンスが実装すべきインターフェース。
    /// </summary>
    public interface IDiceInlet
    {
        /// <summary>
        /// インレットの一意な識別子
        /// </summary>
        CompositeObjectId Id { get; }

        /// <summary>
        /// インレットが所属するクリーチャーカードの一意な識別子
        /// </summary>
        CompositeObjectId CardId { get; }

        /// <summary>
        /// 現在のカウントダウン値
        /// </summary>
        int CurrentCountdownValue { get; }
        
        /// <summary>
        /// 現在の使用可能回数
        /// </summary>
        int CurrentUsageCount { get; }

        DiceInletConditionSO Condition { get; }

        /// <summary>
        /// ダイスがインレットに投入された際の処理。
        /// </summary>
        /// <param name="diceData">投入されたダイスのデータ</param>
        /// <param name="targetCreature">能力の対象となるクリーチャー</param>
        void OnDiceDropped(DiceData diceData, ICreature targetCreature);

        /// <summary>
        /// 指定されたダイスを受け入れ可能かチェックします。
        /// </summary>
        /// <param name="diceData">チェックするダイスのデータ</param>
        /// <returns>受け入れ可能な場合はtrue</returns>
        bool CanAccept(DiceData diceData);
    }
}

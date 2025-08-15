namespace CardsAndDices.Scripts.Data
{
    /// <summary>
    /// ダイスインレットの静的な識別情報を保持する構造体。
    /// </summary>
    public readonly struct DiceInletData
    {
        /// <summary>
        /// このインレットを一意に識別するためのID。
        /// </summary>
        public CompositeObjectId Id { get; }

        public DiceInletData(CompositeObjectId id)
        {
            Id = id;
        }
    }
}
namespace CardsAndDices
{
    /// <summary>
    /// ダイスがインレットに投入された
    /// </summary>
    public class DiceDropInInletEvent : IEvent
    {
        public CompositeObjectId InletId { get; }
        public CompositeObjectId DiceId { get; }
        public int DiceValue { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public DiceDropInInletEvent(CompositeObjectId inletId, CompositeObjectId diceId, int diceValue)
        {
            InletId = inletId;
            DiceValue = diceValue;
            DiceId = diceId;
        }
    }
}

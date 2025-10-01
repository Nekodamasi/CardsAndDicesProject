namespace CardsAndDices
{
    /// <summary>
    /// ダイスがインレットに投入された
    /// </summary>
    public class DiceBeginDragEvent : IEvent
    {
        public CompositeObjectId DiceId { get; }
        public int DiceValue { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public DiceBeginDragEvent(CompositeObjectId diceId, int diceValue)
        {
            DiceValue = diceValue;
            DiceId = diceId;
        }
    }
}

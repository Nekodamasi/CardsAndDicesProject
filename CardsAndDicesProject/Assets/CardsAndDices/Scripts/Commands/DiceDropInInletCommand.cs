namespace CardsAndDices
{
    public class DiceDropInInletCommand : ICommand
    {
        public CompositeObjectId InletId { get; }
        public CompositeObjectId DiceId { get; }
        public int DiceValue { get; }

        public DiceDropInInletCommand(CompositeObjectId inletId, CompositeObjectId diceId, int diceValue)
        {
            InletId = inletId;
            DiceValue = diceValue;
            DiceId = diceId;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

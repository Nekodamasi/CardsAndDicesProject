namespace CardsAndDices
{
    public class DiceInletCountdownCompleteCommand : IEvent
    {
        public CompositeObjectId InletId { get; }

        public DiceInletCountdownCompleteCommand(CompositeObjectId inletId)
        {
            InletId = inletId;
        }
        public void Execute() { }
        public void Undo() { }
    }
}

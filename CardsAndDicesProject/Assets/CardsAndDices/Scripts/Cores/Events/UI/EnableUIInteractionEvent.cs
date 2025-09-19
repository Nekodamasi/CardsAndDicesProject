namespace CardsAndDices
{
    /// <summary>
    /// UI操作制限モードを解除し、UI操作を有効にするコマンド。
    /// </summary>
    public class EnableUIInteractionEvent : IEvent
    {
        public void Execute() { }
        public void Undo() { }
    }
}

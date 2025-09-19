namespace CardsAndDices
{
    /// <summary>
    /// UI操作制限モードを有効にし、UI操作を無効にするコマンド。
    /// </summary>
    public class DisableUIInteractionEvent : IEvent
    {
        public void Execute() { }
        public void Undo() { }
    }
}

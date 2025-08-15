namespace CardsAndDices
{
    /// <summary>
    /// PlayerZoneのスロットが満員かどうかの状態変化を通知するコマンド。
    /// </summary>
    public class PlayerZoneStateChangedCommand : ICommand
    {
        public readonly bool IsFull;

        public PlayerZoneStateChangedCommand(bool isFull)
        {
            IsFull = isFull;
        }

        // ICommandインターフェースの要件を満たすための空のメソッド
        public void Execute() { }
        public void Undo() { }
    }
}

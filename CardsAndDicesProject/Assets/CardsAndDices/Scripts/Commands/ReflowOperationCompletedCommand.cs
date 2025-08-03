
using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// リフロー操作が完全に終了したことを通知するコマンド。
    /// </summary>
    public class ReflowOperationCompletedCommand : ICommand
    {
        public void Execute() { }
        public void Undo() { }
    }
}

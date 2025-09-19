
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// リフロー操作が完全に終了したことを通知するコマンド。
    /// </summary>
    public class ReflowOperationCompletedCommand : IEvent
    {
        public void Execute() { }
        public void Undo() { }
    }
}

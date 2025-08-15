using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 前詰め処理の実行を通知するコマンド。
    /// </summary>
    public class ExecuteFrontLoadCommand : ICommand
    {
//        public Dictionary<CompositeObjectId, Vector3> CardMovements { get; }
//        public ICommand NextCommand { get; }

 //     public ExecuteFrontLoadCommand(Dictionary<CompositeObjectId, Vector3> cardMovements, ICommand nextCommand)
        public ExecuteFrontLoadCommand()
        {
//            CardMovements = cardMovements;
//            NextCommand = nextCommand;
        }

        public void Execute() { }
        public void Undo() { }
    }
}

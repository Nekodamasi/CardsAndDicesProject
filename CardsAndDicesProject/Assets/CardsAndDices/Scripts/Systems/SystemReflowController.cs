using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// システム起因のリフロー処理を実行するコントローラー。
    /// </summary>
    [CreateAssetMenu(fileName = "SystemReflowController", menuName = "CardsAndDice/Systems/SystemReflowController")]
    public class SystemReflowController : ScriptableObject
    {
        private SpriteCommandBus _commandBus;
        private CardInteractionOrchestrator _cardorchestrator;
        private DiceInteractionOrchestrator _diceorchestrator;

        [Inject]
        public void Initialize(SpriteCommandBus commandBus, CardInteractionOrchestrator cardorchestrator, DiceInteractionOrchestrator diceorchestrator)
        {
            _commandBus = commandBus;
            _cardorchestrator = cardorchestrator;
            _diceorchestrator = diceorchestrator;
            _commandBus.On<SystemReflowCommand>(OnSystemReflow);
            _commandBus.On<SystemDiceReflowCommand>(OnSystemDiceReflow);
        }

        private void OnDestroy()
        {
            _commandBus?.Off<SystemReflowCommand>(OnSystemReflow);
            _commandBus?.Off<SystemDiceReflowCommand>(OnSystemDiceReflow);
        }

        private async void OnSystemReflow(SystemReflowCommand command)
        {
            if (command.CardMovements == null || command.CardMovements.Count == 0)
            {
                if (command.NextCommand != null)
                {
                    _commandBus.Emit(command.NextCommand);
                }
                return;
            }

            var animationTasks = new List<UniTask>();

            foreach (var movement in command.CardMovements)
            {
                var cardView = _cardorchestrator.ViewRegistry.GetView<CreatureCardView>(movement.Key);
                if (cardView != null)
                {
                    animationTasks.Add(cardView.MoveToAnimated(movement.Value));
                }
            }

            await UniTask.WhenAll(animationTasks);

            if (command.NextCommand != null)
            {
                _commandBus.Emit(command.NextCommand);
            }
        }
        private async void OnSystemDiceReflow(SystemDiceReflowCommand command)
        {
//            Debug.LogWarning("<color=Yellow>OnSystemDiceReflow:</color>" + command.DiceMovements.Count);

            if (command.DiceMovements == null || command.DiceMovements.Count == 0)
            {
                if (command.NextCommand != null)
                {
                    _commandBus.Emit(command.NextCommand);
                }
                return;
            }

            var animationTasks = new List<UniTask>();

            foreach (var movement in command.DiceMovements)
            {
                var diceView = _diceorchestrator.ViewRegistry.GetView<DiceView>(movement.Key);
//                    Debug.LogWarning("<color=Yellow>むーぶまえ</color>" + movement.Key);
                if (diceView != null)
                {
//                    Debug.LogWarning("<color=Yellow>むーぶにきてない？</color>");
                    animationTasks.Add(diceView.MoveToAnimated(movement.Value));
                }
            }

            await UniTask.WhenAll(animationTasks);

            if (command.NextCommand != null)
            {
                _commandBus.Emit(command.NextCommand);
            }
        }
    }
}

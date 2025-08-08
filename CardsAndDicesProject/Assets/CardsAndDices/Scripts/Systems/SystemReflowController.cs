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
        private CardInteractionOrchestrator _orchestrator;

        [Inject]
        public void Initialize(SpriteCommandBus commandBus, CardInteractionOrchestrator orchestrator)
        {
            _commandBus = commandBus;
            _orchestrator = orchestrator;
            _commandBus.On<SystemReflowCommand>(OnSystemReflow);
        }

        private void OnDestroy()
        {
            _commandBus?.Off<SystemReflowCommand>(OnSystemReflow);
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
                var cardView = _orchestrator.ViewRegistry.GetView<CreatureCardView>(movement.Key);
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
    }
}

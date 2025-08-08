using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "DiceInteractionOrchestrator", menuName = "CardsAndDice/Systems/DiceInteractionOrchestrator")]
    public class DiceInteractionOrchestrator : ScriptableObject, IUIInteractionOrchestrator
    {
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private DiceInteractionStrategy _diceInteractionStrategy;

        private ViewRegistry _viewRegistry;

        [Inject]
        public void Initialize(UIStateMachine uiStateMachine, SpriteCommandBus commandBus, DiceInteractionStrategy diceInteractionStrategy)
        {
            _uiStateMachine = uiStateMachine;
            _commandBus = commandBus;
            _diceInteractionStrategy = diceInteractionStrategy;
            _viewRegistry = new ViewRegistry();

            _commandBus.On<SpriteHoverCommand>(OnHover);
            _commandBus.On<SpriteBeginDragCommand>(OnBeginDrag);
            _commandBus.On<SpriteDropCommand>(OnDrop);
        }

        public void RegisterView(BaseSpriteView view)
        {
            _viewRegistry.Register(view);
        }

        public void UnregisterView(BaseSpriteView view)
        {
            _viewRegistry.Unregister(view);
        }

        private void OnHover(SpriteHoverCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceHover(command, this))
            {
                // Handle dice hover
            }
        }

        private void OnBeginDrag(SpriteBeginDragCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceBeginDrag(command, this))
            {
                // Handle dice begin drag
            }
        }

        private void OnDrop(SpriteDropCommand command)
        {
            if (_diceInteractionStrategy.ChkDiceDrop(command, this))
            {
                // Handle dice drop
            }
        }
    }
}

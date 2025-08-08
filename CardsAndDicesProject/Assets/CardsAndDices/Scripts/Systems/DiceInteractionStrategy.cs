using UnityEngine;

namespace CardsAndDice
{
    [CreateAssetMenu(fileName = "DiceInteractionStrategy", menuName = "CardsAndDice/InteractionStrategies/DiceInteractionStrategy")]
    public class DiceInteractionStrategy : ScriptableObject
    {
        public bool ChkDiceHover(SpriteHoverCommand command, DiceInteractionOrchestrator orchestrator)
        {
            // Implement dice hover logic here
            return false;
        }

        public bool ChkDiceBeginDrag(SpriteBeginDragCommand command, DiceInteractionOrchestrator orchestrator)
        {
            // Implement dice begin drag logic here
            return false;
        }

        public bool ChkDiceDrop(SpriteDropCommand command, DiceInteractionOrchestrator orchestrator)
        {
            // Implement dice drop logic here
            return false;
        }
    }
}

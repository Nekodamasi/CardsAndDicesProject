namespace CardsAndDice
{
    public interface IInteractionStrategy
    {
        void OnBeginDrag(SpriteBeginDragCommand command, UIInteractionOrchestrator orchestrator);
        void OnHover(SpriteHoverCommand command, UIInteractionOrchestrator orchestrator);
        void OnUnhover(SpriteUnhoverCommand command, UIInteractionOrchestrator orchestrator);
        void OnDrop(SpriteDropCommand command, UIInteractionOrchestrator orchestrator);
        void OnDrag(SpriteDragCommand command, UIInteractionOrchestrator orchestrator);
        void OnEndDrag(SpriteEndDragCommand command, UIInteractionOrchestrator orchestrator);
    }
}

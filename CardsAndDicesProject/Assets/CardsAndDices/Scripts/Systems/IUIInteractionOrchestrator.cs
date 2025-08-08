namespace CardsAndDice
{
    public interface IUIInteractionOrchestrator
    {
        void RegisterView(BaseSpriteView view);
        void UnregisterView(BaseSpriteView view);
    }
}

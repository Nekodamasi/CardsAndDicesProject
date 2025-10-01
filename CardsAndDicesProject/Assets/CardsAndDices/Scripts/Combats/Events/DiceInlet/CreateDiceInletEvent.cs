namespace CardsAndDices
{
    /// <summary>
    /// クリーチャー生成イベント
    /// </summary>
    public class CreateDiceInletEvent : IEvent
    {
        public CompositeObjectId CreatureCardId { get; }
        public InletPackageProfile InletPackageProfile { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
        public CreateDiceInletEvent(CompositeObjectId creatureCardId, InletPackageProfile inletPackageProfile)
        {
            CreatureCardId = creatureCardId;
            InletPackageProfile = inletPackageProfile;
        }
    }
}

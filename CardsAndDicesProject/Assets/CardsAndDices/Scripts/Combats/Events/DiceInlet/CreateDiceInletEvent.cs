namespace CardsAndDices
{
    /// <summary>
    /// クリーチャー生成イベント
    /// </summary>
    public class CreateDiceInletEvent : IEvent
    {
        public CompositeObjectId CreatureCardId { get; }
        public InletPackageProfile InletPackageProfile { get; }
        public CreatureStatusInstance CreatureStatusInstance { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreateDiceInletEvent(CompositeObjectId creatureCardId, InletPackageProfile inletPackageProfile, CreatureStatusInstance creatureStatusInstance)
        {
            CreatureCardId = creatureCardId;
            InletPackageProfile = inletPackageProfile;
            CreatureStatusInstance = creatureStatusInstance;
        }
    }
}

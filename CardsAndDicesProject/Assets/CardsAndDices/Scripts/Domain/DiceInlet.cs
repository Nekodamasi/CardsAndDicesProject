namespace CardsAndDices
{
    /// <summary>
    /// ゲーム中に存在するダイスインレットの論理的な表現。
    /// </summary>
    public class DiceInlet : IDiceInlet
    {
        public CompositeObjectId Id { get; private set; }
        public CompositeObjectId CardId { get; private set; }
        public int CurrentCountdownValue { get; private set; }
        public int CurrentUsageCount { get; private set; }

        public DiceInletConditionSO Condition => _profile.Condition;
        private readonly InletAbilityProfile _profile;

        public DiceInlet(CompositeObjectId id, CompositeObjectId cardId, InletAbilityProfile profile)
        {
            Id = id;
            CardId = cardId;
            _profile = profile;
            CurrentCountdownValue = _profile.Condition.InitialCountdownValue;
            CurrentUsageCount = _profile.Condition.InitialUsageCount;
        }

        public int OnDiceDropped(int diceValue)
        {
            CurrentCountdownValue -= diceValue;

            if (CurrentCountdownValue <= 0)
            {
                CurrentCountdownValue = 0;
                CurrentUsageCount--;
            }
            return CurrentCountdownValue;
        }

        public bool CanAccept(DiceData diceData)
        {
            if (CurrentUsageCount <= 0)
            {
                return false;
            }
            return _profile.Condition.CanAccept(diceData);
        }
    }
}

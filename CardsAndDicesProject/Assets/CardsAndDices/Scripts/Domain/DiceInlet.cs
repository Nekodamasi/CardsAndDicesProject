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

        private readonly InletAbilityProfile _profile;

        public DiceInlet(CompositeObjectId id, CompositeObjectId cardId, InletAbilityProfile profile)
        {
            Id = id;
            CardId = cardId;
            _profile = profile;
            CurrentCountdownValue = _profile.Condition.InitialCountdownValue;
            CurrentUsageCount = _profile.Condition.InitialUsageCount;
        }

        public void OnDiceDropped(DiceData diceData, ICreature targetCreature)
        {
            if (!CanAccept(diceData)) return;

            CurrentCountdownValue -= diceData.FaceValue;
            CurrentUsageCount--;

            if (CurrentCountdownValue <= 0)
            {
                _profile.Ability.ExecuteAbility(targetCreature, diceData);
                CurrentCountdownValue = _profile.Condition.InitialCountdownValue;
                // TODO: UsageCountResetTypeに応じた使用回数リセット処理
            }
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

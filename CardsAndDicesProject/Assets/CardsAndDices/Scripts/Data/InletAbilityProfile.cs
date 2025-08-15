namespace CardsAndDices
{
    /// <summary>
    /// インレットの「条件」と「効果」をセットで保持する不変なデータクラス。
    /// </summary>
    public class InletAbilityProfile
    {
        public DiceInletConditionSO Condition { get; }
        public BaseInletAbilitySO Ability { get; }

        public InletAbilityProfile(DiceInletConditionSO condition, BaseInletAbilitySO ability)
        {
            Condition = condition;
            Ability = ability;
        }
    }
}

using System;

namespace CardsAndDices
{
    /// <summary>
    /// インレットの「条件」と「効果」をセットで保持する不変なデータクラス。
    /// </summary>
    [Serializable]
    public class InletAbilityProfile
    {
        /// <summary>
        /// ダイス投入時の発動条件
        /// </summary>
        public DiceInletConditionSO Condition;

        /// <summary>
        /// インレットが発動する能力
        /// </summary>
        public BaseInletAbilitySO Ability;

        public InletAbilityProfile(DiceInletConditionSO condition, BaseInletAbilitySO ability)
        {
            this.Condition = condition;
            this.Ability = ability;
        }
    }
}
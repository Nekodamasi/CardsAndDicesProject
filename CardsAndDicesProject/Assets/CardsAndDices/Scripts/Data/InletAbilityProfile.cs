using System;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// インレットの「条件」と「効果」をセットで保持する不変なデータクラス。
    /// </summary>
    [Serializable]
    public class InletAbilityProfile
    {
        /// <summary>
        /// インレットプロフィールID
        /// </summary>
        public InletProfileIdEntity InletProfileId;

        /// <summary>
        /// ダイス投入時の発動条件
        /// </summary>
        public DiceInletConditionSO Condition;

        /// <summary>
        /// インレットが発動する能力
        /// </summary>
        public List<BaseAbilityDataSO> Abilities;

        public InletAbilityProfile(InletProfileIdEntity inletProfileId, DiceInletConditionSO condition, List<BaseAbilityDataSO> abilities)
        {
            this.InletProfileId = inletProfileId;
            this.Condition = condition;
            this.Abilities = abilities;
        }
    }
}
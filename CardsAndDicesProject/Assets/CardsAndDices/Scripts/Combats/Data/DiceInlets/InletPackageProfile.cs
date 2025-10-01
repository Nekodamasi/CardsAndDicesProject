using System;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// インレットのレアリティ効果と配置場所を付属したパッケージプロファイルクラス
    /// </summary>
    public class InletPackageProfile
    {
        /// <summary>
        /// インレットプロフィールID
        /// </summary>
        public InletProfileIdEntity InletProfileId;

        /// <summary>
        /// インレットの配置場所（カテゴリ）
        /// </summary>
        public CompositeObjectIdTypeEntity InletCategory;

        /// <summary>
        /// レアアビリティ
        /// </summary>
        public List<AbilityDataEntity> RareAbilities;

        /// <summary>
        /// レジェンドアビリティ
        /// </summary>
        public List<AbilityDataEntity> LegendAbilities;
        public InletPackageProfile(InletProfileIdEntity inletProfileId, CompositeObjectIdTypeEntity inletCategory, List<AbilityDataEntity> rareAbilities, List<AbilityDataEntity> legendAbilities)
        {
            InletProfileId = inletProfileId;
            InletCategory = inletCategory;
            RareAbilities = rareAbilities;
            LegendAbilities = legendAbilities;
        }

        public bool ChkFaceAllowed(int faceValue)
        {
            return InletProfileId.AllowedDiceFacesEntity.ChkFaceAllowed(faceValue);
        }
    }
}
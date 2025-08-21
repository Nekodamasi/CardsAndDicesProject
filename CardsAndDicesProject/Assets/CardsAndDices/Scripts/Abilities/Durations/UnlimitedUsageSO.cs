using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// すべてのアビリティの持続時間、クールダウン、および使用制限ロジックの抽象基本クラス
    /// </summary>
    [CreateAssetMenu(fileName = "UnlimitedUsage", menuName = "CardsAndDices/Abilities/Durations/UnlimitedUsage")]
    public class UnlimitedUsageSO : BaseAbilityDurationSO
    {
        public override void OnUse(AbilityInstance instance)
        {
        }
        public override void OnReset(AbilityInstance instance)
        {
            instance.RemainingUsages = 99;
        }
    }
}

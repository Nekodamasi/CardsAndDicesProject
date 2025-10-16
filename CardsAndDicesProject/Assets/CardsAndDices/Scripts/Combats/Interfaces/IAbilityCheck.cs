using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// アビリティのトリガー状況をチェックするインターフェース
    /// </summary>
    public interface IAbilityCheck
    {
        /// <summary>
        /// 指定された実行者とタイミングで実行可能なAbilityがあるか返します
        /// </summary>
        bool HasExecutableAbility(CompositeObjectId ownerId, CompositeObjectId subOwnerId, ActivationTiming activationTiming);

        /// <summary>
        /// タイミングで実行可能なAbilityがあるか返します
        /// </summary>
        bool HasExecutableAbility(ActivationTiming activationTiming);
    }
}

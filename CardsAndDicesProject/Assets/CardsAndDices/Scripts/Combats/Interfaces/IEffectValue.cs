using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// エフェクトの値を取得します。
    /// </summary>
    public interface IEffectValue
    {
        /// <summary>
        /// 指定したエフェクトターゲットの合計エフェクト値を取得します。
        /// </summary>
        int GetTotalEffectValue(CompositeObjectId compositeObjectId, EffectTargetType type);
    }
}

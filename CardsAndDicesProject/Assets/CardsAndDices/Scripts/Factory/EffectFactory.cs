using System;

namespace CardsAndDices
{
    /// <summary>
    /// Effectinstanceの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public class EffectFactory
    {
        /// <summary>
        /// 新しいEffectInstanceを生成します。
        /// </summary>
        /// <param name="id">Viewから取得した、このデータを紐付ける対象のID。</param>
        /// <returns>生成されたDiceData</returns>
        public EffectInstance Create(CompositeObjectId id, EffectData effectdata, EffectTargetType effectTargetType, int value)
        {
            var instance = new EffectInstance();
            instance.Initialize(effectdata, id, effectTargetType, value);
            return instance;
        }
    }
}


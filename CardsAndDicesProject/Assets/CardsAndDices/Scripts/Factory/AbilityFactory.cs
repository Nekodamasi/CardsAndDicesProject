using System;

namespace CardsAndDices
{
    /// <summary>
    /// abilityインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public class AbilityFactory
    {
        /// <summary>
        /// 新しいAbilityInstanceインスタンスを生成します。
        /// </summary>
        /// <param name="id">Viewから取得した、このデータを紐付ける対象のID。</param>
        /// <returns>生成されたAbilityInstance</returns>
        public AbilityInstance Create(BaseAbilityDataSO abilityData, CompositeObjectId ownerId, CompositeObjectId subOwnerId)
        {
            // TODO: ダイスの種類（DiceType）やマスターデータに応じた生成ロジックを実装
            var faceValue = UnityEngine.Random.Range(1, 7);
            return new AbilityInstance(ownerId, abilityData, subOwnerId);
            
        }
    }
}


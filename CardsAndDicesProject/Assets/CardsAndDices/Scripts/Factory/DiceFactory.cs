using System;

namespace CardsAndDices
{
    /// <summary>
    /// DiceDataインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public class DiceFactory
    {
        /// <summary>
        /// 新しいDiceDataインスタンスを生成します。
        /// </summary>
        /// <param name="id">Viewから取得した、このデータを紐付ける対象のID。</param>
        /// <returns>生成されたDiceData</returns>
        public DiceData Create(CompositeObjectId id)
        {
            // TODO: ダイスの種類（DiceType）やマスターデータに応じた生成ロジックを実装
            var faceValue = UnityEngine.Random.Range(1, 7);
            return new DiceData(id, faceValue);
        }
    }
}


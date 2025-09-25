using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// カード初期化データを提供するプロバイダーのインターフェースです。
    /// </summary>
    public interface ICardDataProvider
    {
        /// <summary>
        /// カード初期化データのリストを取得します。
        /// </summary>
        /// <returns>CardInitializationDataのリスト。</returns>
        List<CardInitializationData> GetCardDataList();
    }
}

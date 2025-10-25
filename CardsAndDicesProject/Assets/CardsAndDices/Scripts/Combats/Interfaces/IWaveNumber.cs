using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// ウェーブナンバー用のインターフェース
    /// </summary>
    public interface IWaveNumber
    {
        /// <summary>
        /// ウェーブの最大数を取得する
        /// </summary>
        int MaxWaveNumber { get; }
        /// <summary>
        /// ウェーブの最大数を取得する
        /// </summary>
        int CurrentWaveNumber { get; }

        /// <summary>
        /// 最終ウェーブに到着しているか
        /// </summary>
        bool IslastWave { get; }

        /// <summary>
        /// ウェーブナンバーを次の番号に変更します
        /// </summary>
        void NextWaveNumber();
    }
}

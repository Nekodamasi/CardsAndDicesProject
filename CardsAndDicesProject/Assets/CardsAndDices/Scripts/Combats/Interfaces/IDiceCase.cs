using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// ダイス数用のインターフェース
    /// </summary>
    public interface IDiceCase
    {
        /// <summary>
        /// ダイスの最大数
        /// </summary>
        int MaxDice { get; }
        /// <summary>
        /// 現在のダイス数
        /// </summary>
        int CurrentDiceCount { get; }
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// インレットが発動する「効果」を定義するすべてのScriptableObjectの基底クラス。
    /// </summary>
    public abstract class BaseInletAbilitySO : ScriptableObject
    {
        /// <summary>
        /// 効果を実行します。
        /// </summary>
        /// <param name="placedDice">効果発動のトリガーとなったダイスのデータ。</param>
        public abstract void Execute(DiceData placedDice);
    }
}

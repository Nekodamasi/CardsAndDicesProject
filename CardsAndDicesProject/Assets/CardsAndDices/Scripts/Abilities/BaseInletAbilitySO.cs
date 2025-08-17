using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// インレットが発動する「効果」を定義するすべてのScriptableObjectの基底クラス。
    /// </summary>
    public abstract class BaseInletAbilitySO : ScriptableObject
    {
        /// <summary>
        /// インレットの能力を実行します。
        /// </summary>
        /// <param name="targetCreature">能力の対象となるクリーチャー</param>
        /// <param name="placedDice">投入されたダイスのデータ</param>
        public abstract void ExecuteAbility(ICreature targetCreature, DiceData placedDice);
    }
}
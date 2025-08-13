using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスインレットの発動条件の種類を定義するenum。
    /// </summary>
    public enum InletActivationType
    {
        SingleMatchTrigger, // 特定の目に一致したら発動
        TotalSumTrigger     // 目の合計値が条件を満たしたら発動
    }

    /// <summary>
    /// ダイスインレットの発動条件を定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceInletCondition", menuName = "CardsAndDice/Data/DiceInletConditionSO")]
    public class DiceInletConditionSO : ScriptableObject
    {
        [Tooltip("投入を許可するダイスの目を定義するSOへの参照")]
        public AllowedDiceFacesSO AllowedDiceFaces;

        [Tooltip("発動に必要なカウントダウンの初期値")]
        public int InitialCountdownValue;

        [Tooltip("発動条件のタイプ")]
        public InletActivationType ActivationType;

        [Tooltip("発動時にアビリティを抑制するかどうかのフラグ")]
        public bool SuppressAbilityOnActivation;

        /// <summary>
        /// 指定されたダイスがこの条件に受け入れられるかを判断します。
        /// </summary>
        /// <param name="diceData">判定するダイスのデータ。</param>
        /// <returns>受け入れ可能な場合はtrue、そうでない場合はfalse。</returns>
        public bool CanAccept(DiceData diceData)
        {
            if (AllowedDiceFaces == null || diceData == null)
            {
                return false;
            }

            int faceIndex = diceData.FaceValue - 1;

            if (faceIndex < 0 || faceIndex >= AllowedDiceFaces.IsFaceAllowed.Count)
            {
                return false; // 無効な出目
            }

            return AllowedDiceFaces.IsFaceAllowed[faceIndex];
        }
    }
}
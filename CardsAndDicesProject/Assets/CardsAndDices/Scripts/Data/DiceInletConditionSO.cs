using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスをインレットに配置するための条件を定義するScriptableObjectクラス。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceInletCondition", menuName = "CardsAndDices/Data/DiceInletConditionSO")]
    public class DiceInletConditionSO : ScriptableObject
    {
        [Tooltip("投入可能なダイスの目")]
        [SerializeField] private AllowedDiceFacesSO _allowedDiceFaces;

        [Tooltip("インレットの初期カウントダウン値")]
        [SerializeField] private int _initialCountdownValue = 1;
        public int InitialCountdownValue => _initialCountdownValue;

        [Tooltip("インレットの初期使用可能回数")]
        [SerializeField] private int _initialUsageCount = 1;
        public int InitialUsageCount => _initialUsageCount;

        [Tooltip("使用可能回数がリセットされるタイミング")]
        [SerializeField] private UsageCountResetType _usageCountResetType;
        public UsageCountResetType UsageCountResetType => _usageCountResetType;

        /// <summary>
        /// 指定されたダイスをこのインレットが受け入れ可能か判断します。
        /// </summary>
        /// <param name="diceData">投入しようとしているダイスのデータ</param>
        /// <returns>受け入れ可能な場合はtrue、そうでない場合はfalse</returns>
        public bool CanAccept(DiceData diceData)
        {
            return _allowedDiceFaces.IsFaceAllowed[diceData.FaceValue - 1];
        }
    }
}

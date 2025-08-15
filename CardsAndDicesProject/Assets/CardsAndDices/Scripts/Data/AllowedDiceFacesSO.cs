using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 投入可能なダイスの目の組み合わせを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "AllowedDiceFaces", menuName = "CardsAndDices/Data/AllowedDiceFacesSO")]
    public class AllowedDiceFacesSO : ScriptableObject
    {
        [Tooltip("サイズ6のboolリスト。Index 0が1の目、Index 5が6の目に対応します。")]
        public List<bool> IsFaceAllowed = new(new bool[6]);
    }
}

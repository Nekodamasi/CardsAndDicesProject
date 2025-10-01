using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 投入可能なダイスの目の組み合わせを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "AllowedDF_", menuName = "CardsAndDices/Combats/Data/EntityDefinition/AllowedDiceFacesEntity")]
    public class AllowedDiceFacesEntity : BaseEntityDefinition
    {
        [Tooltip("サイズ6のboolリスト。Index 0が1の目、Index 5が6の目に対応します。")]
        public List<bool> IsFaceAllowed = new(new bool[6]);

        public bool ChkFaceAllowed(int faceValue)
        {
            return IsFaceAllowed[faceValue - 1];
        }
    }
}

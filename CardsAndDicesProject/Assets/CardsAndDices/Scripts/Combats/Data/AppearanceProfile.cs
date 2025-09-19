using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーの「見た目」一式を定義するデータアセット。
    /// </summary>
    [CreateAssetMenu(fileName = "AP_NewAppearance", menuName = "CardsAndDices/Appearance Profile")]
    public class AppearanceProfile : ScriptableObject
    {
        [SerializeField]
        private List<PartSprite> _partSprites;

        /// <summary>
        /// 指定されたパーツIDに対応するスプライトを取得します。
        /// </summary>
        public Sprite GetSpriteForPart(PartId partId)
        {
            var part = _partSprites.FirstOrDefault(p => p.PartId == partId);
            return part?.Sprite;
        }
    }

    /// <summary>
    /// PartIdとSpriteを紐づけるためのシリアライズ可能なクラス。
    /// </summary>
    [System.Serializable]
    public class PartSprite
    {
        public PartId PartId;
        public Sprite Sprite;
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

namespace CardsAndDices
{
    public class CardAppearanceView : BaseIdentifiableView
    {
        /// <summary>
        /// PartIdとSpriteRendererを紐づけるためのシリアライズ可能なクラス。
        /// </summary>
        [System.Serializable]
        public class AppearancePart
        {
            public PartId PartId;
            public SpriteRenderer Renderer;
        }

        [Header("Components")]
        [SerializeField]
        private List<AppearancePart> _parts;

        /// <summary>
        /// 指定された外観プロファイルに基づいて、カードの見た目を更新します。
        /// </summary>
        /// <param name="profile">適用する外観プロファイル。</param>
        public void UpdateAppearance(AppearanceProfile profile)
        {
            // 全てのパーツをループ
            foreach (var part in _parts)
            {
                if (part.Renderer == null) continue;

                // Profileから対応するSpriteを探す
                Sprite spriteToShow = profile.GetSpriteForPart(part.PartId);

                if (spriteToShow != null)
                {
                    // Spriteがあれば表示して設定
                    part.Renderer.gameObject.SetActive(true);
                    part.Renderer.sprite = spriteToShow;
                }
                else
                {
                    // Spriteがなければ非表示にする
                    part.Renderer.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// カードの外観を表示
        /// </summary>
        public void DisplayCardAppearance(AppearanceProfile profile)
        {
            if (profile is null) return;
            UpdateAppearance(profile);
        }
    }
}

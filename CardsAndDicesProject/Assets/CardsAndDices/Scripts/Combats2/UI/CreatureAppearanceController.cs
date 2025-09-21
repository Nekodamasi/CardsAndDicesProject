using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// AppearanceProfileに基づき、実際にゲームオブジェクトの表示を制御するコンポーネント。
    /// </summary>
    public class CreatureAppearanceController : MonoBehaviour
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
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 点滅アニメーションのパラメータを定義するプロファイル。
    /// </summary>
    [CreateAssetMenu(fileName = "BlinkAnimationProfile", menuName = "CardsAndDices/Core/Data/AnimationProfile/BlinkAnimationProfile")]
    public class BlinkAnimationProfile : BaseAnimationProfile
    {
        [Header("Blink Animation Settings")]
        [Tooltip("点滅時の色")]
        [SerializeField] private Color _blinkColor = Color.white;

        [Header("Blink Animation Settings")]
        [Tooltip("点滅時の色")]
        [SerializeField] private Color _originalColor = Color.white;

        /// <summary>
        /// 元のcolorを取得します。
        /// </summary>
        public Color BlinkColor => _blinkColor;

         [SerializeField]
        [Tooltip("目標とする色")]
        public Color OriginalColor => _originalColor;
   }
}

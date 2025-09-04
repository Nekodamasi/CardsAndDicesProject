using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ホバーアニメーションのパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "NormalAnimationProfile", menuName = "CardsAndDices/AnimationProfile/NormalProfile")]
    public class NormalAnimationProfile : BaseAnimationProfile
    {
        [SerializeField]
        [Tooltip("目標とする拡縮率")]
        private Vector3 _targetScale = new Vector3(1.0f, 1.0f, 1.0f);

        [SerializeField]
        [Tooltip("目標とする色")]
        private Color _targetColor = Color.white;

        [SerializeField]
        [Tooltip("目標とする色追加")]
        private float _hoverBrightnessIncrease = 0.0f;

        /// <summary>
        /// 目標とする拡縮率。
        /// </summary>
        public Vector3 TargetScale => _targetScale;

        /// <summary>
        /// 目標とする色（乗算色）。
        /// </summary>
        public Color TargetColor => _targetColor;
        public float HoverBrightnessIncrease => _hoverBrightnessIncrease;
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ホバーアニメーションのパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "DragAnimationProfile", menuName = "CardsAndDices/Core/Data/AnimationProfile/DragAnimationProfile")]
    public class DragAnimationProfile : BaseAnimationProfile
    {
        [SerializeField]
        [Tooltip("目標とする拡縮率")]
        private Vector3 _targetScale = new Vector3(1.0f, 1.0f, 1.0f);

        [SerializeField]
        [Tooltip("目標と透明度")]
        private float _fadeAlpha = 0.7f;

        /// <summary>
        /// 目標とする拡縮率。
        /// </summary>
        public Vector3 TargetScale => _targetScale;

        /// <summary>
        /// 目標と透明度
        /// </summary>
        public float FadeAlpha => _fadeAlpha;
    }
}

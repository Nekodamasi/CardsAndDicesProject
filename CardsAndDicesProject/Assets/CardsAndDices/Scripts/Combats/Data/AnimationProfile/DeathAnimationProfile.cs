using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 死亡時のアニメーションパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "DeathAnimationProfile", menuName = "CardsAndDices/Combats/Data/AnimationProfile/DeathAnimationProfile")]
    public class DeathAnimationProfile : BaseAnimationProfile
    {
        [SerializeField]
        [Tooltip("フェードアウトにかかる時間")]
        private float _fadeDuration = 0.1f;

        /// <summary>
        /// フェードアウトにかかる時間。
        /// </summary>
        public float FadeDuration => _fadeDuration;
    }
}

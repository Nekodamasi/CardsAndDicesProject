using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダメージを受けた際のアニメーションパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "DamageAnimationProfile", menuName = "CardsAndDices/Combats/Data/AnimationProfile/DamageAnimationProfile")]
    public class DamageAnimationProfile : BaseAnimationProfile
    {
        [SerializeField]
        [Tooltip("元の位置に戻るまでの遅延時間")]
        private float _returnDelay = 0.1f;

        [Tooltip("左に下がる距離")]
        private float _backStepDistance = -1.0f;

       [SerializeField]
        [Tooltip("元の位置に戻る動作にかかる時間")]
        private float _returnDuration = 0.3f;

        /// <summary>
        /// 元の位置に戻るまでの遅延時間。
        /// </summary>
        public float ReturnDelay => _returnDelay;

        /// <summary>
        /// 移動距離
        /// </summary>
        public float BackStepDistance => _backStepDistance;
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスのジャンプインアニメーションのパラメータを定義するプロファイル。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceJumpInAnimationProfile", menuName = "CardsAndDices/Combats/Data/AnimationProfile/DiceJumpInAnimationProfile")]
    public class DiceJumpInAnimationProfile : BaseAnimationProfile
    {
        /// <summary>
        /// アニメーションの開始位置のX軸オフセット。
        /// </summary>
        public float StartOffset => _startOffset;
        [SerializeField]
        private float _startOffset = -1.0f;

        /// <summary>
        /// HomePositionを通り過ぎる距離。
        /// </summary>
        public float OvershootDistance => _overshootDistance;
        [SerializeField]
        private float _overshootDistance = 0.2f;
    }
}

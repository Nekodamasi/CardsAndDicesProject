using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 右方向への移動アニメーションのパラメータを定義するプロファイル。
    /// </summary>
    [CreateAssetMenu(fileName = "MoveRightAnimationProfile", menuName = "CardsAndDice/Animation/Profile/MoveRightAnimationProfile")]
    public class MoveRightAnimationProfile : BaseAnimationProfile
    {
        /// <summary>
        /// X軸方向の移動距離。
        /// </summary>
        public float MoveDistance => _moveDistance;
        [SerializeField]
        private float _moveDistance = 1.5f;

        /// <summary>
        /// アニメーション全体の時間。
        /// </summary>
        public float Duration => _duration;
        [SerializeField]
        private float _duration = 0.2f;
    }
}

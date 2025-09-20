using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 右方向への移動アニメーションのパラメータを定義するプロファイル。
    /// </summary>
    [CreateAssetMenu(fileName = "MoveRightAnimationProfile", menuName = "CardsAndDices/Core/Data/AnimationProfile/MoveRightAnimationProfile")]
    public class MoveRightAnimationProfile : BaseAnimationProfile
    {
        /// <summary>
        /// X軸方向の移動距離。
        /// </summary>
        public float MoveDistance => _moveDistance;
        [SerializeField]
        private float _moveDistance = 1.5f;
    }
}

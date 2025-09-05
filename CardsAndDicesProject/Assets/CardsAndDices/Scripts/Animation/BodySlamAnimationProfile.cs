using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 体当たりアニメーションのパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "BodySlamAnimationProfile", menuName = "CardsAndDices/AnimationProfile/BodySlamProfile")]
    public class BodySlamAnimationProfile : BaseAnimationProfile
    {
        [Header("Movement Settings")]
        [SerializeField]
        [Tooltip("左に下がる距離")]
        private float _backStepDistance = 0.5f;

        [SerializeField]
        [Tooltip("右に体当たりする距離")]
        private float _lungeDistance = 1.0f;

        [Header("Duration Settings")]
        [SerializeField]
        [Tooltip("左に下がる動作にかかる時間")]
        private float _backStepDuration = 0.2f;

        [SerializeField]
        [Tooltip("右に体当たりする動作にかかる時間")]
        private float _lungeDuration = 0.1f;
        
        [SerializeField]
        [Tooltip("体当たり後の停止時間")]
        private float _pauseDuration = 0.1f;

        [SerializeField]
        [Tooltip("元の位置に戻る動作にかかる時間")]
        private float _returnDuration = 0.3f;

        /// <summary>
        /// 左に下がる距離。
        /// </summary>
        public float BackStepDistance => _backStepDistance;

        /// <summary>
        /// 右に体当たりする距離。
        /// </summary>
        public float LungeDistance => _lungeDistance;

        /// <summary>
        /// 左に下がる動作にかかる時間。
        /// </summary>
        public float BackStepDuration => _backStepDuration;

        /// <summary>
        /// 右に体当たりする動作にかかる時間。
        /// </summary>
        public float LungeDuration => _lungeDuration;
        
        /// <summary>
        /// 体当たり後の停止時間。
        /// </summary>
        public float PauseDuration => _pauseDuration;

        /// <summary>
        /// 元の位置に戻る動作にかかる時間。
        /// </summary>
        public float ReturnDuration => _returnDuration;
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダメージを受けた際のアニメーションパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "DamageAnimationProfile", menuName = "CardsAndDices/AnimationProfile/Damage")]
    public class DamageAnimationProfile : BaseAnimationProfile
    {
        [SerializeField]
        [Tooltip("移動オフセット")]
        private Vector2 _moveOffset = new Vector2(0.5f, 0f);

        [SerializeField]
        [Tooltip("元の位置に戻るまでの遅延時間")]
        private float _returnDelay = 0.1f;

        /// <summary>
        /// 移動オフセット。
        /// </summary>
        public Vector2 MoveOffset => _moveOffset;

        /// <summary>
        /// 元の位置に戻るまでの遅延時間。
        /// </summary>
        public float ReturnDelay => _returnDelay;
    }
}

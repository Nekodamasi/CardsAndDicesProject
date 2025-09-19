using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーションパラメータを定義するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "BuffAnimationProfile", menuName = "CardsAndDices/Animation Profiles/Buff")]
    public class BuffAnimationProfile : BaseAnimationProfile
    {
        [Header("Squash and Stretch")]
        [SerializeField]
        [Tooltip("つぶれて伸びるアニメーションの合計時間")]
        private float _squashAndStretchDuration = 0.1f;

        [SerializeField]
        [Tooltip("つぶれる際のスケール")]
        private Vector3 _squashScale = new Vector3(1.05f, 0.9f, 1f);

        [SerializeField]
        [Tooltip("伸びる際のスケール")]
        private Vector3 _stretchScale = new Vector3(0.95f, 1.1f, 1f);

        [SerializeField]
        [Tooltip("つぶれる際に下に移動するオフセット")]
        private Vector3 _downOffset = new Vector3(0, -0.1f, 0);

        [SerializeField]
        [Tooltip("伸びる際に上に移動するオフセット")]
        private Vector3 _upOffset = new Vector3(0, 0.1f, 0);

        [Header("Wait and Return")]
        [SerializeField]
        [Tooltip("待機時間")]
        private float _waitDuration = 0.3f;

        [SerializeField]
        [Tooltip("元の状態に戻るアニメーションの時間")]
        private float _returnDuration = 0.1f;

        public float SquashAndStretchDuration => _squashAndStretchDuration;
        public Vector3 SquashScale => _squashScale;
        public Vector3 StretchScale => _stretchScale;
        public Vector3 DownOffset => _downOffset;
        public Vector3 UpOffset => _upOffset;
        public float WaitDuration => _waitDuration;
        public float ReturnDuration => _returnDuration;
    }
}

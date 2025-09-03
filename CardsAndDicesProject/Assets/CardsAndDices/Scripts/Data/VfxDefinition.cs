using UnityEngine;

namespace CardsAndDices
{
    using CardsAndDices;

    /// <summary>
    /// VFXのIDと設定データを兼ねるScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "VfxDefinition", menuName = "CardsAndDices/VFX/VfxDefinition")]
    public class VfxDefinition : BaseEntityDefinition
    {
        [Header("VFX Settings")]
        [Tooltip("再生するパーティクルのPrefab")]
        [SerializeField]
        private GameObject _particlePrefab;

        [Tooltip("SEデータ")]
        [SerializeField]
        private SEData _seData;

        [Tooltip("エフェクトをループ再生するかどうか")]
        [SerializeField]
        private bool _isLooping;

        [Tooltip("ループしない場合、エフェクトを停止するまでの時間。0以下の場合はParticleSystemの再生時間に依存します。")]
        [SerializeField]
        private float _duration;

        /// <summary>
        /// 再生するパーティクルのPrefabを取得します。
        /// </summary>
        public GameObject ParticlePrefab => _particlePrefab;

        /// <summary>
        /// 再生するSEDataを取得します。
        /// </summary>
        public SEData SEData => _seData;

        /// <summary>
        /// AudioClip => _audioClip;

        /// <summary>
        /// エフェクトをループ再生するかどうかを取得します。
        /// </summary>
        public bool IsLooping => _isLooping;

        /// <summary>
        /// ループしない場合のエフェクトの再生時間を取得します。
        /// </summary>
        public float Duration => _duration;
    }
}

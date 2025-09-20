using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 再生するSE（サウンドエフェクト）のデータコンテナとなるScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "SE_", menuName = "CardsAndDices/Data/SEData")]
    public class SEData : ScriptableObject
    {
        [SerializeField]
        [Tooltip("再生するオーディオクリップ")]
        private AudioClip _audioClip;

        [SerializeField]
        [Tooltip("ループ再生するかどうか")]
        private bool _isLoop;

        /// <summary>
        /// 再生するオーディオクリップ。
        /// </summary>
        public AudioClip AudioClip => _audioClip;

        /// <summary>
        /// ループ再生するかどうか。
        /// </summary>
        public bool IsLoop => _isLoop;
    }
}

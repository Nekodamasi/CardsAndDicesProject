using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// BGMのDataEntityクラス
    /// </summary>
    [CreateAssetMenu(fileName = "BGMDataEntity", menuName = "CardsAndDices/Cores/Data/EntityDefinition/BGMDataEntity")]
    public class BGMDataEntity : BaseEntityDefinition
    {
        [Tooltip("この能力の効果範囲")]
        [SerializeField] private AudioClip _audioClip;


        /// <summary>
        /// AudioSource
        /// </summary>
        public AudioClip AudioClip => _audioClip;
    }
}

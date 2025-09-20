using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// SE（サウンドエフェクト）の再生を担当する汎用コンポーネント。
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SEPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private SoundManager _soundManager;

        /// <summary>
        /// VContainerによる依存性注入のためのコンストラクタ。
        /// </summary>
        [Inject]
        public void Construct(SoundManager soundManager)
        {
            _soundManager = soundManager;

            _audioSource = GetComponent<AudioSource>();
            if (_soundManager != null)
            {
                // SoundManagerから取得したAudioMixerGroupをAudioSourceに設定
                _audioSource.outputAudioMixerGroup = _soundManager.SEGroup;
            }
        }

        /// <summary>
        /// 指定されたSEデータを再生します。
        /// </summary>
        /// <param name="seData">再生するSEのデータ</param>
        public void Play(SEData seData)
        {
            if (seData == null || seData.AudioClip == null)
            {
                Debug.LogWarning("再生するSEDataまたはAudioClipが指定されていません。", this);
                return;
            }

            _audioSource.clip = seData.AudioClip;
            _audioSource.loop = seData.IsLoop;
            _audioSource.Play();
        }

        /// <summary>
        /// 指定されたSEデータをPlayOneShotで再生します。
        /// ループ設定は無視されます。
        /// </summary>
        /// <param name="seData">再生するSEのデータ</param>
        public void PlayOneShot(SEData seData)
        {
            if (seData == null || seData.AudioClip == null)
            {
                Debug.LogWarning("再生するSEDataまたはAudioClipが指定されていません。", this);
                return;
            }
            _audioSource.PlayOneShot(seData.AudioClip);
        }

        /// <summary>
        /// 現在再生中のサウンドを停止します。
        /// </summary>
        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}

using UnityEngine;
using UnityEngine.Audio;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// サウンド設定を一元管理するScriptableObject。
    /// AudioMixerの音量制御や、ユーザー設定の永続化を担当します。
    /// </summary>
    [CreateAssetMenu(fileName = "SoundManager", menuName = "CardsAndDices/Managers/SoundManager")]
    public class SoundManager : ScriptableObject
    {
        [SerializeField]
        [Tooltip("制御対象のAudioMixer")]
        private AudioMixer _audioMixer;

        [SerializeField]
        [Tooltip("SE用のAudioMixerGroup")]
        private AudioMixerGroup _seGroup;

        [SerializeField]
        [Tooltip("AudioMixerで公開されているSE音量パラメータ名")]
        private string _seVolumeParameterName = "SEVolume";

        /// <summary>
        /// SE再生に使用するAudioMixerGroup。
        /// </summary>
        public AudioMixerGroup SEGroup => _seGroup;

        private const string SE_VOLUME_KEY = "SE_VOLUME";
        private const float MIN_DB = -80f; // AudioMixerの最小dB値

        /// <summary>
        /// PlayerPrefsから音量設定を読み込み、AudioMixerに適用します。
        /// </summary>
        [Inject]
        public void Initialize()
        {
            // 保存されている音量設定を読み込む。なければデフォルト値(1.0)を使用。
            var volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, 1.0f);
            SetVolume(volume);
        }

        /// <summary>
        /// SEの音量を設定します。
        /// </summary>
        /// <param name="linearVolume">0.0(無音)から1.0(最大)の線形値</param>
        public void SetVolume(float linearVolume)
        {
            // 値を0-1の範囲にクランプ
            linearVolume = Mathf.Clamp01(linearVolume);

            // AudioMixerにデシベル値を設定
            _audioMixer.SetFloat(_seVolumeParameterName, ConvertToDecibel(linearVolume));

            // 設定をPlayerPrefsに保存
            PlayerPrefs.SetFloat(SE_VOLUME_KEY, linearVolume);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 線形値(0-1)を対数(dB)に変換します。
        /// </summary>
        /// <param name="linear">線形値</param>
        /// <returns>デシベル値</returns>
        private float ConvertToDecibel(float linear)
        {
            // 0 の場合は最小dBを返す (log10(0) は負の無限大になるため)
            return linear > 0 ? 20.0f * Mathf.Log10(linear) : MIN_DB;
        }
    }
}

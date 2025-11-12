using UnityEngine;
using UnityEngine.Audio;

namespace CardsAndDices
{
    /// <summary>
    /// BGMを再生するイベント
    /// </summary>
    public class PlayBGMEvent : IEvent
    {
        private readonly AudioClip _audioClip;
        private readonly float _fadeDuration;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayBGMEvent(AudioClip audioClip, float fadeDuration)
        {
            _audioClip = audioClip;
            _fadeDuration = fadeDuration;
        }
        /// <summary>
        /// 再生するBGM
        /// </summary>
        public AudioClip AudioClip => _audioClip;

        /// <summary>
        /// フェードタイム
        /// </summary>
        public float FadeDuration => _fadeDuration;
    }
} 
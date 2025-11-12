using UnityEngine;
using UnityEngine.Audio;

namespace CardsAndDices
{
    /// <summary>
    /// BGMを再生するイベント
    /// </summary>
    public class PlayBGMEvent : IEvent
    {
        private readonly BGMDataEntity _bGMDataEntity;
        private readonly float _fadeDuration;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PlayBGMEvent(BGMDataEntity bGMDataEntity, float fadeDuration)
        {
            _bGMDataEntity = bGMDataEntity;
            _fadeDuration = fadeDuration;
        }
        /// <summary>
        /// 再生するBGM
        /// </summary>
        public BGMDataEntity BGMDataEntity => _bGMDataEntity;

        /// <summary>
        /// フェードタイム
        /// </summary>
        public float FadeDuration => _fadeDuration;
    }
} 
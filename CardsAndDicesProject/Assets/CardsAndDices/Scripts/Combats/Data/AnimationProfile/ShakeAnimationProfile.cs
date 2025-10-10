
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// Shakeアニメーションのパラメータを定義するプロファイル。
    /// </summary>
    [CreateAssetMenu(fileName = "ShakeAnimationProfile", menuName = "CardsAndDices/Combats/Data/AnimationProfile/ShakeAnimationProfile")]
    public class ShakeAnimationProfile : BaseAnimationProfile
    {
        [Header("Shake Parameters")]

        [Tooltip("揺れの強さ")]
        [SerializeField] private float _strength = 3.0f;

        [Tooltip("イベントを発行する時間")]
        [SerializeField] private float _eventTime = 0.1f;

        [Tooltip("振動数")]
        [SerializeField] private int _vibrato = 30;

        [Tooltip("手振れ値")]
        [SerializeField] private float _randomness = 90f;
        
        /// <summary>
        /// 揺れの強さ。
        /// </summary>
        public float Strength => _strength;

        /// <summary>
        /// 手振れ値
        /// </summary>
        public float Randomness => _randomness;

        /// <summary>
        /// イベントを発行する時間。
        /// </summary>
        public float EventTime => _eventTime;

        /// <summary>
        /// 振動数
        /// </summary>
        public int Vibrato => _vibrato;
    }
}

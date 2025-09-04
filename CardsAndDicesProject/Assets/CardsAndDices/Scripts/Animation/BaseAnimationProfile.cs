using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのアニメーションプロファイルの基底クラスとなるScriptableObject。
    /// </summary>
    public abstract class BaseAnimationProfile : ScriptableObject
    {
        [SerializeField]
        [Tooltip("アニメーションの再生時間（秒）")]
        protected float _duration = 0.2f;

        /// <summary>
        /// アニメーションの再生時間（秒）。
        /// </summary>
        public float Duration => _duration;
    }
}

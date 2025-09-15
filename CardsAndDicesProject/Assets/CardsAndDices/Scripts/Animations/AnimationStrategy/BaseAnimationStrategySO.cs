using UnityEngine;
using DG.Tweening;

namespace CardsAndDices
{
    /// <summary>
    /// すべてのアニメーションストラテジーの基底クラス
    /// </summary>
    public abstract class BaseAnimationStrategySO : ScriptableObject
    {
        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト。</param>
        /// <returns>生成されたDOTweenのSequence。</returns>
        public abstract Sequence ExecuteAsync(AnimationContext context);
    }
}

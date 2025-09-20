using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "DragAnimationStrategySO", menuName = "CardsAndDices/Animations/AnimationStrategy/DragAnimationStrategySO")]
    public class DragAnimationStrategySO :  BaseAnimationStrategySO
    {
        private DragAnimationProfile _profile;

        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト。</param>
        /// <returns>生成されたDOTweenのSequence。</returns>
        public override Sequence ExecuteAsync(AnimationContext context)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(context.MultiRendererVisualController.FadeToAlpha(_profile.FadeAlpha, _profile.Duration)); // FadeToAlphaを使用

            sequence.Join(context.ScaleTargetTransform.DOScale(_profile.TargetScale, _profile.Duration));

            return sequence;
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "HoverAnimationStrategySO", menuName = "CardsAndDices/Animations/AnimationStrategy/HoverAnimationStrategySO")]
    public class HoverAnimationStrategySO :  BaseAnimationStrategySO
    {
        [SerializeField] private HoverAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public HoverAnimationStrategySO(HoverAnimationProfile profile)
        {
            _profile = profile;
        }

        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト。</param>
        /// <returns>生成されたDOTweenのSequence。</returns>
        public override Sequence ExecuteAsync(AnimationContext context)
        {
            Sequence sequence = DOTween.Sequence();

            Color brightenedColor = context.GetBrightenedColor(_profile.TargetColor, _profile.HoverBrightnessIncrease);
            sequence.Join(context.MultiRendererVisualController.ColorTo(brightenedColor, _profile.Duration));
            sequence.Join(context.ScaleTargetTransform.DOScale(_profile.TargetScale, _profile.Duration));

            return sequence;
        }
    }
}

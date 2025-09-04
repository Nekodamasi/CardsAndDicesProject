using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ホバーアニメーションを実装する戦略クラス。
    /// </summary>
    public class HoverAnimationStrategy : IAnimationStrategy
    {
        private readonly HoverAnimationProfile _profile;

        /// <summary>
        /// HoverAnimationStrategyを初期化します。
        /// </summary>
        /// <param name="profile">使用するアニメーションプロファイル</param>
        public HoverAnimationStrategy(HoverAnimationProfile profile)
        {
            _profile = profile;
        }

        /// <summary>
        /// ホバーアニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト</param>
        public Sequence ExecuteAsync(AnimationContext context)
        {
            Sequence sequence = DOTween.Sequence();

            Color brightenedColor = context.GetBrightenedColor(_profile.TargetColor, _profile.HoverBrightnessIncrease);
            sequence.Join(context.MultiRendererVisualController.ColorTo(brightenedColor, _profile.Duration));
            sequence.Join(context.transform.DOScale(_profile.TargetScale, _profile.Duration));

            return sequence;
        }
    }
}

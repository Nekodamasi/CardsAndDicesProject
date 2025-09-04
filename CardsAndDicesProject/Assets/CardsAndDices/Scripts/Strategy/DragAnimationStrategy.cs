using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ホバーアニメーションを実装する戦略クラス。
    /// </summary>
    public class DragAnimationStrategy : IAnimationStrategy
    {
        private readonly DragAnimationProfile _profile;

        /// <summary>
        /// HoverAnimationStrategyを初期化します。
        /// </summary>
        /// <param name="profile">使用するアニメーションプロファイル</param>
        public DragAnimationStrategy(DragAnimationProfile profile)
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

            sequence.Join(context.MultiRendererVisualController.FadeToAlpha(_profile.FadeAlpha, _profile.Duration)); // FadeToAlphaを使用

            sequence.Join(context.transform.DOScale(_profile.TargetScale, _profile.Duration));

            return sequence;
        }
    }
}

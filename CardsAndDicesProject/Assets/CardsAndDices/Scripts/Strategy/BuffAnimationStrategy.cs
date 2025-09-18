using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    public class BuffAnimationStrategy : IAnimationStrategy
    {
        [SerializeField] private readonly BuffAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public BuffAnimationStrategy(BuffAnimationProfile profile)
        {
            _profile = profile;
        }

        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト。</param>
        /// <returns>生成されたDOTweenのSequence。</returns>
        public Sequence ExecuteAsync(AnimationContext context)
        {
            if (context?.TargetTransform == null)
            {
                return null;
            }

            var transform = context.TargetTransform;
            var originalPosition = transform.localPosition;
            var originalScale = transform.localScale;

            var sequence = DOTween.Sequence();

            // 1. つぶれて下に移動 -> 伸びて上に移動
            float halfDuration = _profile.SquashAndStretchDuration / 2f;
            sequence.Append(transform.DOScale(_profile.SquashScale, halfDuration))
                    .Join(transform.DOLocalMove(originalPosition + _profile.DownOffset, halfDuration))
                    .Append(transform.DOScale(_profile.StretchScale, halfDuration))
                    .Join(transform.DOLocalMove(originalPosition + _profile.UpOffset, halfDuration));

            // 2. 待機
            sequence.AppendInterval(_profile.WaitDuration);

            // 3. 元の位置とサイズに戻る
            sequence.Append(transform.DOScale(originalScale, _profile.ReturnDuration))
                    .Join(transform.DOLocalMove(originalPosition, _profile.ReturnDuration));

            // 4. コマンド発行
            sequence.OnComplete(() =>
            {
                context.IdentifiableCommandBus.Emit(new PlayVfxCommand(context.VfxDefinition, transform.position, transform.rotation));
            });
            return sequence;
        }
    }
}

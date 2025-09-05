using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダメージを受けた際のアニメーション戦略。
    /// </summary>
    public class DamageAnimationStrategy : IAnimationStrategy
    {
        private readonly DamageAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public DamageAnimationStrategy(DamageAnimationProfile profile)
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
            var sequence = DOTween.Sequence();
            var transform = context.TargetTransform;
            var originalPosition = transform.localPosition;

            // 指定されたオフセットへ素早く移動
            sequence.Append(transform.DOLocalMove(originalPosition + (Vector3)_profile.MoveOffset, _profile.Duration / 2).SetEase(Ease.OutQuad));
            // 少し停止
            sequence.AppendInterval(_profile.ReturnDelay);
            // 元の位置に戻る
            sequence.Append(transform.DOLocalMove(originalPosition, _profile.Duration / 2).SetEase(Ease.InQuad));

            return sequence;
        }
    }
}

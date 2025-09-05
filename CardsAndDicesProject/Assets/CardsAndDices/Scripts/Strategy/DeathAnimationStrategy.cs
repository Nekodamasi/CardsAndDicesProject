using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 死亡時のアニメーション戦略。
    /// </summary>
    public class DeathAnimationStrategy : IAnimationStrategy
    {
        private readonly DeathAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public DeathAnimationStrategy(DeathAnimationProfile profile)
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
            if (context == null)
            {
                return null;
            }

            var sequence = DOTween.Sequence();
            var transform = context.TargetTransform;
            var visualController = context.MultiRendererVisualController;

            // Z軸で1回転
            sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 360), _profile.Duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear));
            sequence.Join(context.MultiRendererVisualController.FadeToAlpha(0.0f, _profile.Duration));

            // アニメーション完了後、IsSpawnedフラグをfalseに設定
            sequence.OnComplete(() =>
            {
                if (context.SpriteView != null)
                {
                    context.SpriteView.SetSpawnedState(false);
                }
            });

            return sequence;
        }
    }
}

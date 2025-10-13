using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "DeathAnimationStrategySO", menuName = "CardsAndDices/Combats/Data/AnimationStrategy/DeathAnimationStrategySO")]
    public class DeathAnimationStrategySO :  BaseAnimationStrategySO
    {
        [SerializeField] private DeathAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public DeathAnimationStrategySO(DeathAnimationProfile profile)
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
            if (context == null)
            {
                return null;
            }

            var sequence = DOTween.Sequence();
            var transform = context.ScaleTargetTransform;
            var visualController = context.MultiRendererVisualController;

            // Z軸で1回転
            sequence.Append(transform.DOLocalRotate(new Vector3(0, 0, 360), _profile.Duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear));
            sequence.Join(context.MultiRendererVisualController.FadeToAlpha(0.0f, _profile.Duration));

            // アニメーション完了後、IsSpawnedフラグをfalseに設定
            sequence.OnComplete(() =>
            {
            });

            return sequence;
        }
    }
}

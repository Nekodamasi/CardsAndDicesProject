using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "NormalAnimationStrategySO", menuName = "CardsAndDices/Animations/AnimationStrategy/NormalAnimationStrategySO")]
    public class NormalAnimationStrategySO :  BaseAnimationStrategySO
    {
        [SerializeField] private NormalAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public NormalAnimationStrategySO(NormalAnimationProfile profile)
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

            sequence.Join(context.MultiRendererVisualController.ColorTo(_profile.TargetColor, _profile.Duration));
            sequence.Join(context.transform.DOScale(_profile.TargetScale, _profile.Duration));

            return sequence;
        }
    }
}

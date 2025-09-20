using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 点滅アニメーションを生成するストラテジー。
    /// </summary>
    [CreateAssetMenu(fileName = "BlinkAnimationStrategySO", menuName = "CardsAndDices/Core/Data/AnimationStrategy/BlinkAnimationStrategySO")]
    public class BlinkAnimationStrategySO : BaseAnimationStrategySO
    {
        [SerializeField] private BlinkAnimationProfile _profile;

        /// <summary>
        /// 指定されたコンテキストに基づいて、点滅アニメーションのDOTweenシーケンスを生成します。
        /// </summary>
        /// <param name="context">アニメーションの実行に必要な情報を含むコンテキスト。</param>
        /// <returns>生成されたDOTweenのシーケンス。</returns>
        public override Sequence ExecuteAsync(AnimationContext context)
        {
            var controller = context.MultiRendererVisualController;
            var originalColor = _profile.OriginalColor;
            var sequence = DOTween.Sequence();

            // アニメーションシーケンスを構築
            // 半分の時間で点滅色に変化
            sequence.Append(controller.ColorTo(_profile.BlinkColor, _profile.Duration / 2));
            // 残り半分の時間で元の色に戻る
            sequence.Append(controller.ColorTo(originalColor, _profile.Duration / 2));

            return sequence;
        }
    }
}

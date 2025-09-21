using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 体当たりアニメーションを実装する戦略クラス。
    /// </summary>
    public class BodySlamAnimationStrategy
    {
        private readonly BodySlamAnimationProfile _profile;

        /// <summary>
        /// BodySlamAnimationStrategyを初期化します。
        /// </summary>
        /// <param name="profile">使用するアニメーションプロファイル</param>
        public BodySlamAnimationStrategy(BodySlamAnimationProfile profile)
        {
            _profile = profile;
        }

        /// <summary>
        /// 体当たりアニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト</param>
        public Sequence ExecuteAsync(AnimationContext context)
        {
            Transform targetTransform = context.MoveTargetTransform;
            Vector3 originalPosition = targetTransform.localPosition;

            Sequence sequence = DOTween.Sequence();

            // 1. 少し左に下がる
            sequence.Append(targetTransform.DOLocalMoveX(originalPosition.x - _profile.BackStepDistance, _profile.BackStepDuration)
                .SetEase(Ease.OutQuad));

            // 2. 右側に移動（体当たり）
            sequence.Append(targetTransform.DOLocalMoveX(originalPosition.x + _profile.LungeDistance, _profile.LungeDuration)
                .SetEase(Ease.InQuad));

            // 3. 止まる
            sequence.AppendInterval(_profile.PauseDuration);

            // 4. すっと元の位置に戻る
            sequence.Append(targetTransform.DOLocalMove(originalPosition, _profile.ReturnDuration)
                .SetEase(Ease.OutCubic));

            return sequence;
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "BodySlamAnimationStrategySO", menuName = "CardsAndDices/Combats/Data/AnimationStrategy/BodySlamAnimationStrategySO")]
    public class BodySlamAnimationStrategySO :  BaseAnimationStrategySO
    {
        [SerializeField] private BodySlamAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public BodySlamAnimationStrategySO(BodySlamAnimationProfile profile)
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
            Transform targetTransform = context.MoveTargetTransform;
            Vector3 originalPosition = context.MoveTargetTransform.localPosition;

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

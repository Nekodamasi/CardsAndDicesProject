using DG.Tweening;
using R3;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 右方向へ移動するアニメーションを実行する戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "MoveRightAnimationStrategy", menuName = "CardsAndDice/Animation/Strategy/MoveRight")]
    public class MoveRightAnimationStrategySO : BaseAnimationStrategySO
    {
        [SerializeField] private MoveRightAnimationProfile _profile;

        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションの実行に必要なコンテキスト情報。</param>
        public override Sequence ExecuteAsync(AnimationContext context)
        {


            var targetTransform = context.TargetTransform;
            var homePosition = context.HomePosition;
            var destinationPosition = homePosition + new Vector3(_profile.MoveDistance, 0, 0);

            var sequence = DOTween.Sequence();
            sequence.SetTarget(targetTransform);

            // 1. 指定された距離だけ右に移動
            sequence.Append(targetTransform.DOMove(destinationPosition, _profile.Duration).SetEase(Ease.OutQuad));

            // 2. 移動完了後にステータス表示コマンドを発行
            sequence.AppendCallback(() =>
            {
                if (context.IdentifiableCommandBus != null)
                {
                    context.IdentifiableCommandBus.Emit(new DisplayIdentifiableStatusCommand(context.IdentifiableGameObject.CompositeObjectId));
                }
            });

            return sequence;
        }
    }
}

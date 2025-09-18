using DG.Tweening;
using R3;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスが画面外から飛び込んでくるアニメーションを実行する戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceJumpInAnimationStrategySO", menuName = "CardsAndDices/Animations/AnimationStrategy/DiceJumpInAnimationStrategySO")]
    public class DiceJumpInAnimationStrategySO : BaseAnimationStrategySO
    {
        [SerializeField] private DiceJumpInAnimationProfile _profile;

        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト。</param>
        /// <returns>生成されたDOTweenのSequence。</returns>
        public override Sequence ExecuteAsync(AnimationContext context)
        {
            var targetTransform = context.TargetTransform;
            var homePosition = context.HomePosition;
            var startPosition = homePosition + new Vector3(_profile.StartOffset, 0, 0);
            var overshootPosition = homePosition + new Vector3(_profile.OvershootDistance, 0, 0);

            // アニメーションの各フェーズの時間
            var durationPhase1 = _profile.Duration * 0.75f;
            var durationPhase2 = _profile.Duration * 0.25f;

            // 初期位置設定
            targetTransform.position = startPosition;

            var sequence = DOTween.Sequence();
            sequence.SetTarget(targetTransform);

            // アニメーション開始時にステータス表示コマンドを発行
            sequence.AppendCallback(() =>
            {
                if (context.IdentifiableCommandBus != null)
                {
                    context.IdentifiableCommandBus.Emit(new DisplayIdentifiableStatusCommand(context.IdentifiableGameObject.CompositeObjectId));
                }
            });

            // 1. 勢いよく飛び出し、HomePositionを少し通り過ぎる
            sequence.Append(targetTransform.DOMove(overshootPosition, durationPhase1).SetEase(Ease.OutQuad));

            // 2. HomePositionに戻る
            sequence.Append(targetTransform.DOMove(homePosition, durationPhase2).SetEase(Ease.InQuad));

            return sequence;
        }
    }
}

using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "DamageAnimationStrategySO", menuName = "CardsAndDices/Combats/Data/AnimationStrategy/DamageAnimationStrategySO")]
    public class DamageAnimationStrategySO :  BaseAnimationStrategySO
    {
        [SerializeField] private DamageAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public DamageAnimationStrategySO(DamageAnimationProfile profile)
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
            var sequence = DOTween.Sequence();
            var transform = context.MoveTargetTransform;
            var originalPosition = context.MoveTargetTransform.localPosition;

            // 指定されたオフセットへ素早く移動
            sequence.Append(transform.DOLocalMoveX(originalPosition.x - _profile.BackStepDistance, _profile.Duration).SetEase(Ease.OutQuad));
            // 少し停止
            sequence.AppendInterval(_profile.ReturnDelay);

            sequence.Append(transform.DOLocalMove(originalPosition, _profile.ReturnDelay)
                .SetEase(Ease.OutCubic));

            // 4. コマンド発行
            sequence.OnComplete(() =>
            {
//                context.GameEventBus.Emit(new PlayVfxEvent(context.VfxDefinition, context.MoveTargetTransform.position, context.MoveTargetTransform.rotation));
                context.GameEventBus.Emit(new UpdateDisplayCreatureStatusEvent(context.IdentifiableGameObject.CompositeObjectId));
            });

            return sequence;
        }
    }
}

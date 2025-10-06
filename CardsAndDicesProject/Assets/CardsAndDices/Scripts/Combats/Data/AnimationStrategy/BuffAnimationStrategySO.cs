using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// バフ効果を受けた際のアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "BuffAnimationStrategySO", menuName = "CardsAndDices/Combats/Data/AnimationStrategy/BuffAnimationStrategySO")]
    public class BuffAnimationStrategySO :  BaseAnimationStrategySO
    {
        [SerializeField] private BuffAnimationProfile _profile;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="profile">アニメーションのパラメータを定義するプロファイル。</param>
        public BuffAnimationStrategySO(BuffAnimationProfile profile)
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
            var originalPosition = context.MoveTargetTransform.localPosition;
            var originalScale = context.MoveTargetTransform.localScale;

            var sequence = DOTween.Sequence();

            // 1. つぶれて下に移動 -> 伸びて上に移動
            float halfDuration = _profile.SquashAndStretchDuration / 2f;
            sequence.Append(context.ScaleTargetTransform.DOScale(_profile.SquashScale, halfDuration))
                    .Join(context.MoveTargetTransform.DOLocalMove(originalPosition + _profile.DownOffset, halfDuration))
                    .Append(context.ScaleTargetTransform.DOScale(_profile.StretchScale, halfDuration))
                    .Join(context.MoveTargetTransform.DOLocalMove(originalPosition + _profile.UpOffset, halfDuration));

            // 2. 待機
            sequence.AppendInterval(_profile.WaitDuration);

            // 3. 元の位置とサイズに戻る
            sequence.Append(context.ScaleTargetTransform.DOScale(originalScale, _profile.ReturnDuration))
                    .Join(context.MoveTargetTransform.DOLocalMove(originalPosition, _profile.ReturnDuration));

            // 4. コマンド発行
            sequence.OnComplete(() =>
            {
                context.GameEventBus.Emit(new PlayVfxEvent(context.VfxDefinition, context.MoveTargetTransform.position, context.MoveTargetTransform.rotation));
                context.GameEventBus.Emit(new UpdateDisplayCreatureStatusEvent(context.IdentifiableGameObject.CompositeObjectId));
            });
            return sequence;
        }
    }
}

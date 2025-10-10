
using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// DOTweenのDOShakeScaleを使用して、オブジェクトを振動させるアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "ShakeAnimationStrategySO", menuName = "CardsAndDices/Combats/Data/AnimationStrategy/ShakeAnimationStrategySO")]
    public class ShakeAnimationStrategySO : BaseAnimationStrategySO
    {
        [SerializeField] private ShakeAnimationProfile _profile;

        /// <summary>
        /// Shakeアニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションの実行に必要なコンテキスト情報。</param>
        /// <returns>実行されるDOTweenのSequence。</returns>
        public override Sequence ExecuteAsync(AnimationContext context)
        {
            var transform = context.ScaleTargetTransform;
            Vector3 originalScale = context.ScaleTargetTransform.localScale;
            if (transform == null)
            {
                Debug.LogWarning("ScaleTargetTransformがnullです。");
                return DOTween.Sequence();
            }

            var sequence = DOTween.Sequence();

            // アニメーションシーケンスにShakeを追加
            sequence.Append(transform.DOShakeScale(_profile.Duration, _profile.Strength, _profile.Vibrato, _profile.Randomness, true))
//            1f, 3f, 30, 90f, true)
//            sequence.Append(context.ScaleTargetTransform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.2f))
                .Append(context.ScaleTargetTransform.DOScale(originalScale, 0.0f));
//            sequence.Append(transform.DOShakeScale(_profile.Duration, _profile.Strength));

            // 指定した時間にイベントを発行するコールバックを挿入
            sequence.InsertCallback(_profile.EventTime, () =>
            {
                // TODO: 「hogehoge()」の代わりとなる具体的なイベントに置き換えてください。
//                // context.GameEventBus.Emit(new YourSpecificAnimationEvent());
//                Debug.Log("Shake animation mid-point event triggered.");
            });

            return sequence;
        }
    }
}

using UnityEngine;
using DG.Tweening;

namespace CardsAndDice
{
    /// <summary>
    /// オブジェクトを所定のターゲット位置に戻すアニメーション戦略。
    /// </summary>
    [CreateAssetMenu(fileName = "ReturnToPositionAnimation", menuName = "CardsAndDice/Animations/Return To Position Animation")]
    public class ReturnToPositionAnimationSO : BaseAnimationSO
    {
        public override Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(targetObject.transform.DOMove(targetPosition, duration)
                                        .SetEase(Ease.OutQuad));

            // 必要に応じて、元のスケールや色に戻すアニメーションも追加
            sequence.Join(targetObject.transform.DOScale(originalScale, duration));
            sequence.Join(targetVisualController.ColorTo(originalColor, duration));

            return sequence;
        }
    }
}
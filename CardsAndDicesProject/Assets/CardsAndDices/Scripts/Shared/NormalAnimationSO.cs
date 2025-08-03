using UnityEngine;
using DG.Tweening;

namespace CardsAndDice
{
    [CreateAssetMenu(fileName = "NormalAnimation", menuName = "CardsAndDice/Animations/Normal Animation")]
    public class NormalAnimationSO : BaseAnimationSO
    {
        public override Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition) // targetPosition を追加
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(targetVisualController.ColorTo(originalColor, duration)); // ColorToを使用

            sequence.Join(targetObject.transform.DOScale(originalScale, duration));

            return sequence;
        }
    }
}
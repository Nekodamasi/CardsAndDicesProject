using UnityEngine;
using DG.Tweening;

namespace CardsAndDice
{
    [CreateAssetMenu(fileName = "DragAnimation", menuName = "CardsAndDice/Animations/Drag Animation")]
    public class DragAnimationSO : BaseAnimationSO
    {
        [SerializeField] private float _dragScale = 1.0f;
        [SerializeField] private float _fadeAlpha = 0.7f;

        public override Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition) // targetPosition を追加
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(targetVisualController.FadeToAlpha(_fadeAlpha, duration)); // FadeToAlphaを使用

            sequence.Join(targetObject.transform.DOScale(originalScale * _dragScale, duration));

            return sequence;
        }
    }
}
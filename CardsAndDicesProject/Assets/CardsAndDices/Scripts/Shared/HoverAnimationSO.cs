using UnityEngine;
using DG.Tweening;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "HoverAnimation", menuName = "CardsAndDices/Animations/Hover Animation")]
    public class HoverAnimationSO : BaseAnimationSO
    {
        [SerializeField] private float _hoverScale = 1.2f;
        [SerializeField] private float _hoverBrightnessIncrease = 0.2f;

        public override Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition) // targetPosition を追加
        {
            Sequence sequence = DOTween.Sequence();

            Color brightenedColor = GetBrightenedColor(originalColor, _hoverBrightnessIncrease);
            sequence.Join(targetVisualController.ColorTo(brightenedColor, duration)); // ColorToを使用

            sequence.Join(targetObject.transform.DOScale(originalScale * _hoverScale, duration));

            return sequence;
        }
    }
}
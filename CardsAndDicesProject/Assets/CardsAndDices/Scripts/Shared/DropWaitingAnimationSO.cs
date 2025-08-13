using UnityEngine;
using DG.Tweening;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "DropWaitingAnimation", menuName = "CardsAndDices/Animations/Drop Waiting Animation")]
    public class DropWaitingAnimationSO : BaseAnimationSO
    {
        [SerializeField] private float _pulseScale = 1.1f;
        [SerializeField] private float _pulseDuration = 0.5f;
        [SerializeField] private int _pulseVibrato = 10;
        [SerializeField] private float _pulseElasticity = 1f;

        public override Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition) // targetPosition を追加
        {
            Sequence sequence = DOTween.Sequence();

            // スケールを脈動させるアニメーション
            sequence.Append(targetObject.transform.DOScale(originalScale * _pulseScale, _pulseDuration)
                                        .SetEase(Ease.OutElastic, _pulseVibrato, _pulseElasticity));
            sequence.Append(targetObject.transform.DOScale(originalScale, _pulseDuration)
                                        .SetEase(Ease.OutElastic, _pulseVibrato, _pulseElasticity));
            sequence.SetLoops(-1, LoopType.Yoyo); // 無限ループでYoyo（往復）

            // 色を少し明るくするアニメーション（オプション）
            // Color brightenedColor = GetBrightenedColor(originalColor, 0.1f);
            // sequence.Join(targetVisualController.ColorTo(brightenedColor, duration));

            return sequence;
        }
    }
}
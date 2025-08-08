using DG.Tweening;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// アニメーション戦略の基底ScriptableObject。
    /// </summary>
    public abstract class BaseAnimationSO : ScriptableObject, IAnimationStrategy
    {
        [SerializeField] protected float _animationDuration = 0.2f; // デフォルトの期間

        /// <summary>
        /// 指定されたGameObjectとMultiRendererVisualControllerに対してアニメーションを実行します。
        /// </summary>
        /// <param name="targetObject">アニメーションの対象となるGameObject。</param>
        /// <param name="targetVisualController">アニメーションの対象となるMultiRendererVisualController。</param>
        /// <param name="originalScale">アニメーション開始時の元のスケール。</param>
        /// <param name="originalColor">アニメーション開始時の元の色。</param>
        /// <param name="duration">アニメーションの期間。</param>
        /// <param name="targetPosition">アニメーションの目標位置。</param> // 追加
        /// <returns>DOTweenのSequenceまたはTween。</returns>
        public virtual Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition) // targetPosition を追加
        {
            return null;
        }

        /// <summary>
        /// 指定された色の明るさを増加させた新しい色を取得します。
        /// </summary>
        /// <param name="baseColor">基準となる色</param>
        /// <param name="increase">増加させる明るさの量</param>
        /// <returns>明るさを増加させた色</returns>
        protected Color GetBrightenedColor(Color baseColor, float increase)
        {
            return new Color(
                Mathf.Clamp01(baseColor.r + increase),
                Mathf.Clamp01(baseColor.g + increase),
                Mathf.Clamp01(baseColor.b + increase),
                baseColor.a
            );
        }
    }
}
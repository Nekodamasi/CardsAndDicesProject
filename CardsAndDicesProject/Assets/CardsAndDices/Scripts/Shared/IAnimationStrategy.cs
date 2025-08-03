using DG.Tweening;
using UnityEngine;

namespace CardsAndDice
{
    /// <summary>
    /// UIアニメーションの戦略を定義するインターフェース。
    /// </summary>
    public interface IAnimationStrategy
    {
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
        Sequence PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition); // targetPosition を追加
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class IdentifiableStatusView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private Transform _moveTransform;
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _hoverAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _normalAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _dragAnimationStrategyEntity;

        private AnimationExecutor _animationExecutor = new AnimationExecutor();

        /// <summary>
        /// Animationの実行を開始します。
        /// </summary>
        public Sequence AnimationExecute(AnimationStrategyEntity animationStrategyEntity)
        {
            var strategy = _animationStrategyRegistry.GetStrategy(animationStrategyEntity);
            var sequence = _animationExecutor.Execute(strategy, _animationContext);
            return sequence;
        }

        /// <summary>
        /// ホバー状態にします
        /// </summary>
        public Sequence DisplayHoverStatus()
        {

            return AnimationExecute(_hoverAnimationStrategyEntity);
        }

        /// <summary>
        /// ノーマル状態にします
        /// </summary>
        public Sequence DisplayNormalStatus()
        {
            
            return AnimationExecute(_normalAnimationStrategyEntity);
        }
        /// <summary>
        /// ドラッグ状態にします
        /// </summary>
        public Sequence DisplayDragStatus()
        {
            
            return AnimationExecute(_dragAnimationStrategyEntity);
        }

        /// <summary>
        /// ドラッグ中の移動を行います
        /// </summary>
        public void MoveTo(Vector3 targetPosition)
        {
            Debug.Log("とらんすふぉーむぽじしょん：" + targetPosition);
            _moveTransform.position = targetPosition;
        }
    }
}

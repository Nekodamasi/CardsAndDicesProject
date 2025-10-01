using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class DiceInletView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _acceptableAnimationStatusEntity;
        [SerializeField] private SpriteSelector _spriteSelector;

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
        /// 受け入れ状態にします
        /// </summary>
        public Sequence DisplayAcceptableStatus()
        {
            return null;
        }
    }
}

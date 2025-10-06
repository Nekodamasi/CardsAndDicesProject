using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class CreatureStatusView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _buffAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _deBuffAnimationStrategyEntity;
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
        /// バフアニメーション
        /// </summary>
        public Sequence DisplayBuff(VfxDefinition vfxDefinition)
        {
            _animationContext.VfxDefinition = vfxDefinition;
            return AnimationExecute(_buffAnimationStrategyEntity);
        }

        /// <summary>
        /// デバフアニメーション
        /// </summary>
        public Sequence DisplayDeBuff(VfxDefinition vfxDefinition)
        {
            _animationContext.VfxDefinition = vfxDefinition;
            return AnimationExecute(_deBuffAnimationStrategyEntity);
        }
    }
}

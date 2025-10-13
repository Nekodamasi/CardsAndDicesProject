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
        [SerializeField] private AnimationStrategyEntity _bodySlamAnimationStrategy;
        [SerializeField] private AnimationStrategyEntity _damageAnimationStrategy;
        [SerializeField] private AnimationStrategyEntity _deathAnimationStrategy;
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
        /// 死亡アニメーション
        /// </summary>
        public Sequence DisplayDeath(VfxDefinition vfxDefinition)
        {
            _animationContext.VfxDefinition = vfxDefinition;
            return AnimationExecute(_deathAnimationStrategy);
        }

        /// <summary>
        /// ダメージアニメーション
        /// </summary>
        public Sequence DisplayDamage(VfxDefinition vfxDefinition)
        {
            _animationContext.VfxDefinition = vfxDefinition;
            return AnimationExecute(_damageAnimationStrategy);
        }

        /// <summary>
        /// アタックアニメーション
        /// </summary>
        public Sequence DisplayBodySlam(VfxDefinition vfxDefinition)
        {
            _animationContext.VfxDefinition = vfxDefinition;
            return AnimationExecute(_bodySlamAnimationStrategy);
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

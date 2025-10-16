using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class NextTurnBtnView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _onScreenAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _offScreenAnimationStrategyEntity;

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
        /// 画面に投げ入れる
        /// </summary>
        public Sequence DisplayOnScreen(Vector3 homePosition)
        {
            _animationContext.HomePosition = homePosition;
            return AnimationExecute(_onScreenAnimationStrategyEntity);
        }

        /// <summary>
        /// 画面から退場
        /// </summary>
        public Sequence DisplayOffScreen()
        {
            return AnimationExecute(_offScreenAnimationStrategyEntity);
        }
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class DiceView : BaseIdentifiableView
    {
        [Header("Components")]
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private AnimationStrategyRegistry _animationStrategyRegistry;
        [SerializeField] private AnimationStrategyEntity _diceOnScreenAnimationStrategyEntity;
        [SerializeField] private AnimationStrategyEntity _diceOffScreenAnimationStrategyEntity;
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
        /// ダイスを画面に投げ入れる
        /// </summary>
        public Sequence DisplayOnScreen(Vector3 homePosition, int diceFace)
        {
            _spriteSelector.SelectSprite("Dice" + diceFace);
            _animationContext.HomePosition = homePosition;
            return AnimationExecute(_diceOnScreenAnimationStrategyEntity);
        }

        /// <summary>
        /// ダイスを画面から退場
        /// </summary>
        public Sequence DisplayOffScreen()
        {
            return AnimationExecute(_diceOffScreenAnimationStrategyEntity);
        }
    }
}

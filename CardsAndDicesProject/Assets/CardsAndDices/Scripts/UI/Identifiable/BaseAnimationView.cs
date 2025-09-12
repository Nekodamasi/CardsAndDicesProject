using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public abstract class BaseAnimationView : MonoBehaviour, IGameInitializable, IIdentifiableView
    {
        [Header("Components")]
        [SerializeField] protected IdentifiableGameObject _identifiableGameObject;
        [SerializeField] protected AnimationContext _animationContext;
        [SerializeField] protected AnimationStrategyRegistry _animationStrategyRegistry;

        private AnimationExecutor _animationExecutor = new AnimationExecutor();

        /// <summary>
        /// このViewインスタンスを識別するための一意なIDを取得します。
        /// </summary>
        public CompositeObjectId CompositeObjectId { get { return _identifiableGameObject.ObjectId; } }

        public virtual void OnAwake()
        {
        }
        public virtual void OnStart()
        {
        }

        /// <summary>
        /// このゲームオブジェクトが現在プールから取り出され、ゲーム内で使用中であるかを示します。
        /// </summary>
        public bool IsSpawned { get; private set; }

        /// <summary>
        /// このゲームオブジェクトのスポーン状態を設定します。
        /// </summary>
        /// <param name="state">trueの場合、ゲーム内で使用中。falseの場合、プールに戻された状態。</param>
        public void SetSpawnedState(bool state)
        {
            IsSpawned = state;
        }
        public Sequence AnimationExecute(AnimationStrategyEntity animationStrategyEntity)
        {
            var strategy = _animationStrategyRegistry.GetStrategy(animationStrategyEntity);
            var sequence = _animationExecutor.Execute(strategy, _animationContext);
            return sequence;
        }
    }
}

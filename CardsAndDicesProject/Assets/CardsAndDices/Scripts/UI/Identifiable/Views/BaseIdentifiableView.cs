using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public abstract class BaseIdentifiableView : MonoBehaviour, IGameInitializable
    {
        [Header("Base Components")]
        [SerializeField] protected IdentifiableGameObject _identifiableGameObject;
        private IdentifiableViewRegistry _identifiableViewRegistry;

		[Inject]
		public void Construct(IdentifiableViewRegistry identifiableViewRegistry)
		{
			_identifiableViewRegistry = identifiableViewRegistry;
        }

        /// <summary>
        /// このViewインスタンスを識別するための一意なIDを取得します。
        /// </summary>
        public CompositeObjectId CompositeObjectId { get { return _identifiableGameObject.ObjectId; } }

        public virtual void OnAwake()
        {
        }

        public virtual void OnStart()
        {
            _identifiableViewRegistry.Register(this);
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
    }
}

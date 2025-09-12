using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public abstract class BaseInteractiveView : MonoBehaviour, IGameInitializable
    {
        [Header("Components")]
        [SerializeField] protected IdentifiableGameObject _identifiableGameObject;
        [SerializeField] protected SpriteCommandBus _commandBus;
        [SerializeField] protected BoxCollider2D _boxCollider2D;

        [Header("Display Root")]
        [SerializeField] private GameObject _displayRootGameObject;
        protected IUIInteractionOrchestrator _orchestrator;

        /// <summary>
        /// このViewインスタンスを識別するための一意なIDを取得します。
        /// </summary>
        CompositeObjectId CompositeObjectId { get { return _identifiableGameObject.ObjectId; } }

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
            if (state == false)
            {
                SetInteractionActive(false);
            }
        }

        /// <summary>
        /// 入力の受付を切り替えます
        /// </summary>
        /// <param name="active">trueで入力OK、falseで受け付けない。</param>
        public void SetInteractionActive(bool active)
        {
        }

        public virtual void OnAwake()
        {
        }
        public virtual void OnStart()
        {
            // 初期状態では非表示にする
            SetInteractionActive(false);
        }

        protected virtual void OnDestroy()
        {
        }

        /// <summary>
        /// Viewを指定された位置へアニメーションで移動させます。
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        public abstract UniTask MoveToAnimated(Vector3 targetPosition);

        // --- Helper Methods ---
        public CompositeObjectId GetObjectId() => _identifiableGameObject.ObjectId;

        public void SetColliderEnabled(bool enable)
        {
            if (_boxCollider2D != null) _boxCollider2D.enabled = enable;
        }
    }
}

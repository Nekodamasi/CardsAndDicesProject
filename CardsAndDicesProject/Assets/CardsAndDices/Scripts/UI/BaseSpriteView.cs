using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDice
{
    public abstract class BaseSpriteView : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected MultiRendererVisualController _multiRendererVisualController;
        [SerializeField] protected SpriteLayerController _spriteLayerController;
        [SerializeField] protected BoxCollider2D _boxCollider2D;
        [SerializeField] protected IdentifiableGameObject _identifiableGameObject;
        [SerializeField] protected SpriteCommandBus _commandBus;
        [Header("Animation Settings")]
        [SerializeField] protected float _animationDuration = 0.2f;

        [Header("Animation Strategies")]
        [SerializeField] protected BaseAnimationSO _hoverAnimation;
        [SerializeField] protected BaseAnimationSO _normalAnimation;
        [SerializeField] protected BaseAnimationSO _dragAnimation;

        [SerializeField] protected SpriteStatus _currentStatus = SpriteStatus.Normal;
        protected IUIInteractionOrchestrator _orchestrator;

        protected Vector3 _originalScale;
        protected Color _originalColor;
        protected bool isDisableUIInteraction = false;
        protected bool _originalColliderEnabled; // コライダーの元の状態を記憶
        protected Sequence _currentAnimation;
        protected Sequence _currentMoveAnimation;
        public SpriteStatus CurrentStatus { get { return _currentStatus; } }

        public CompositeObjectId SlotId { get; private set; }

        protected virtual void Awake()
        {
            _commandBus.On<EnableUIInteractionCommand>(OnEnableUIInteraction);
            _commandBus.On<DisableUIInteractionCommand>(OnDisableUIInteraction);
            if (_multiRendererVisualController == null) _multiRendererVisualController = GetComponent<MultiRendererVisualController>();
            if (_spriteLayerController == null) _spriteLayerController = GetComponent<SpriteLayerController>();
            if (_boxCollider2D == null) _boxCollider2D = GetComponent<BoxCollider2D>();

            _originalScale = transform.localScale;
            _originalColor = _multiRendererVisualController.GetColor();
            _originalColliderEnabled = _boxCollider2D.enabled; // 初期状態を記憶

            // OrchestratorのViewRegistryに自身を登録
            _orchestrator?.RegisterView(this);
        }

        protected virtual void OnDestroy()
        {
            _orchestrator?.UnregisterView(this);
            KillCurrentAnimation();
            KillCurrentMoveAnimation();
            DOTween.Kill(this);

            // コマンドの購読解除
            if (_commandBus != null)
            {
                _commandBus.Off<EnableUIInteractionCommand>(OnEnableUIInteraction);
                _commandBus.Off<DisableUIInteractionCommand>(OnDisableUIInteraction);
            }
        }

        /// <summary>
        /// UI操作を有効にするコマンドを受信した際の処理。
        /// </summary>
        /// <param name="command">EnableUIInteractionCommandのインスタンス。</param>
        protected virtual void OnEnableUIInteraction(EnableUIInteractionCommand command)
        {
            isDisableUIInteraction = false;

            // 本来適用されているコライダーの状態に復元する
            if (_boxCollider2D != null) _boxCollider2D.enabled = _originalColliderEnabled;
        }

        /// <summary>
        /// UI操作を無効にするコマンドを受信した際の処理。
        /// </summary>
        /// <param name="command">DisableUIInteractionCommandのインスタンス。</param>
        protected virtual void OnDisableUIInteraction(DisableUIInteractionCommand command)
        {
            isDisableUIInteraction = true;

            // SetColliderEnabledを呼ばず直接コライダーを操作する
            if (_boxCollider2D != null) _boxCollider2D.enabled = false;
        }

        // --- Public Methods for Orchestrator ---
        public virtual void EnterNormalState() { _currentStatus = SpriteStatus.Normal; }
        public virtual void EnterHoveringState() { _currentStatus = SpriteStatus.Hover; }
        public virtual void EnterDraggingState() { _currentStatus = SpriteStatus.DraggingStarted; }
        public virtual void EnterDraggingInProgressState() { _currentStatus = SpriteStatus.DraggingInProgress; }
        public virtual void EnterAcceptableState() { _currentStatus = SpriteStatus.Acceptable; }
        public virtual void EnterInactiveState() { _currentStatus = SpriteStatus.Inactive; }
        public virtual void MoveTo(Vector3 targetPosition)
        {
            transform.position = targetPosition; // 即座に位置を設定
        }

        /// <summary>
        /// Viewを指定された位置へアニメーションで移動させます。
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        public abstract UniTask MoveToAnimated(Vector3 targetPosition);

        // --- Helper Methods ---
        public CompositeObjectId GetObjectId() => _identifiableGameObject.ObjectId;
        public abstract CompositeObjectId GetCurrentCardId();

        public void SetColliderEnabled(bool enable)
        {
            // コライダーの状態にかかわらず、現在のコライダーの状態は常に更新する
            _originalColliderEnabled = enable;

            // UI操作制限モードがONならばコライダーは変更しない
            if (isDisableUIInteraction) return;

            if (_boxCollider2D != null) _boxCollider2D.enabled = enable;
        }

        public void SetOrderInLayer(int order)
        {
            _spriteLayerController?.SetOrderInLayer(order);
        }

        protected virtual void KillCurrentAnimation()
        {
            _currentAnimation?.Kill();
            _currentAnimation = null;
        }
        protected virtual void KillCurrentMoveAnimation()
        {
            _currentMoveAnimation?.Kill();
            _currentMoveAnimation = null;
        }

        /// <summary>
        /// コマンドを発行します。
        /// </summary>
        /// <typeparam name="TCommand">発行するコマンドの型。</typeparam>
        /// <param name="command">発行するコマンドのインスタンス。</param>
        protected void EmitCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandBus?.Emit(command);
        }
    }
}

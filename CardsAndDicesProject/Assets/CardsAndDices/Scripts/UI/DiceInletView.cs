using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスインレットの視覚表現とUIインタラクションを担当するViewコンポーネント。
    /// </summary>
    public class DiceInletView : BaseSpriteView
    {
        [Inject]
        public void Construct(DiceInteractionOrchestrator orchestrator)
        {
            this._orchestrator = orchestrator;
        }

        [Header("Animations")]
        [SerializeField] private BaseAnimationSO _acceptableAnimation;
        [SerializeField] private BaseAnimationSO _dropWaitingAnimation;

        protected override void Awake()
        {
            base.Awake();
            SetSpawnedState(false);
        }

        /// <summary>
        /// ダイスを受け入れ可能な状態に遷移します。
        /// </summary>
        public override void EnterAcceptableState()
        {
            base.EnterAcceptableState();
            KillCurrentAnimation();
            _currentAnimation = _acceptableAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
            SetColliderEnabled(true);
        }

        /// <summary>
        /// スロットをホバー状態に遷移させます。
        /// </summary>
        public override void EnterHoveringState()
        {
            base.EnterHoveringState();
            KillCurrentAnimation();
            _currentAnimation = _dropWaitingAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
        }

        /// <summary>
        /// ドロップイベントを検知し、コマンドを発行します。
        /// </summary>
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var droppedView = eventData.pointerDrag.GetComponent<BaseSpriteView>();
                if (droppedView != null)
                {
                    _commandBus.Emit(new SpriteDropCommand(droppedView.GetObjectId(), GetObjectId()));
                }
            }
        }

        /// <summary>
        /// Viewを指定された位置へアニメーションで移動させます。
        /// CardSlotViewは通常アニメーションで移動しないため、空の実装。
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        public override UniTask MoveToAnimated(Vector3 targetPosition)
        {
            return UniTask.CompletedTask;
        }
    }
}

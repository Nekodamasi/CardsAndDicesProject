using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;

namespace CardsAndDices
{
    public class CardSlotView : BaseSpriteView
    {
        [Inject]
        public void Construct(CardInteractionOrchestrator orchestrator)
        {
            this._orchestrator = orchestrator;
        }

        [Header("Components")]
        [SerializeField] private CardSlotManager _cardSlotManager;

        [Header("Slot Definition")]
        [SerializeField] private LinePosition _line;
        [SerializeField] private SlotLocation _location;
        [SerializeField] private Team _team;

        [Header("CardSlot Specific Animations")]
        [SerializeField] private BaseAnimationSO _acceptableAnimation;
        [SerializeField] private BaseAnimationSO _dropWaitingAnimation;

        private CardSlotData _slotData;

        protected override void Awake()
        {
            base.Awake();
            _slotData = new CardSlotData(GetObjectId(), transform.position, _line, _location, _team);
            _cardSlotManager.RegisterSlot(_slotData);
        }

        /// <summary>
        /// 現在スロットに配置されているカードのIDを取得します。
        /// </summary>
        public CompositeObjectId GetCurrentCardId() => _slotData.PlacedCardId;

        /// <summary>
        /// スロットを通常状態に遷移させます。
        /// </summary>
        public override void EnterNormalState()
        {
            base.EnterNormalState();
            KillCurrentAnimation();
            _currentAnimation = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
            SetColliderEnabled(false);
        }

        /// <summary>
        /// スロットを受け入れ可能状態に遷移させます。
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
        /// スロットを非アクティブ状態に遷移させます。
        /// </summary>
        public override void EnterInactiveState()
        {
            base.EnterInactiveState();
            KillCurrentAnimation();
            _currentAnimation = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
            SetColliderEnabled(false);
        }

        /// <summary>
        /// Viewを指定された位置へアニメーションで移動させます。（リフロー用）
        /// CardSlotViewは通常アニメーションで移動しないため、空の実装。
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        public override UniTask MoveToAnimated(Vector3 targetPosition)
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// スロットが存在するライン。
        /// </summary>
        public LinePosition Line { get { return _slotData.Line; } }

        /// <summary>
        /// ライン内でのスロットの役割や位置。
        /// </summary>
        public SlotLocation Location { get { return _slotData.Location; } }

        /// <summary>
        /// スロットが所属するチーム。
        /// </summary>
        public Team Team { get { return _slotData.Team; } }
    }
}

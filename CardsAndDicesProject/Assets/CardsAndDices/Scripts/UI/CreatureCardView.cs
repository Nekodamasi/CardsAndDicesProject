using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using VContainer;
using System.Collections.Generic;
using TMPro;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードの視覚的な表示を管理するコンポーネント。
    /// BaseSpriteViewを継承し、カード特有の視覚効果を実装します。
    /// </summary>
    public class CreatureCardView : BaseSpriteView
    {
        private NameService _nameService;

        [Inject]
        public void Construct(CardInteractionOrchestrator orchestrator, NameService nameService)
        {
            this._orchestrator = orchestrator;
            _nameService = nameService;
        }

        [Header("Card Specific Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] public TextMeshProUGUI _cardName;
        [SerializeField] private CreatureCardType _creatureCardType;        
        [SerializeField] private List<DiceInletView> _diceInletViews = new List<DiceInletView>();
        [SerializeField] private List<StatusIconView> _statusIconViews = new List<StatusIconView>();
        [SerializeField] private CreatureAppearanceController _appearanceController;
        [Header("Card Animation Settings")]
        [SerializeField] private AnimationContext _animationContext;
        [SerializeField] private HoverAnimationProfile _hoverAnimationProfile;
        [SerializeField] private NormalAnimationProfile _normalAnimationProfile;
        [SerializeField] private DragAnimationProfile _dragAnimationProfile;
        [SerializeField] private BodySlamAnimationProfile _bodySlamAnimationProfile;
        [SerializeField] private DamageAnimationProfile _damageAnimationProfile;
        [SerializeField] private DeathAnimationProfile _deathAnimationProfile;
        [SerializeField] private BuffAnimationProfile _buffAnimationProfile;
        private HoverAnimationStrategy _hoverAnimationStrategy;
        private NormalAnimationStrategy _normalAnimationStrategy;
        private DragAnimationStrategy _dragAnimationStrategy;
        private BodySlamAnimationStrategy _bodySlamAnimationStrategy;
        private DamageAnimationStrategy _damageAnimationStrategy;
        private DeathAnimationStrategy _deathAnimationStrategy;
        private BuffAnimationStrategy _buffAnimationStrategy;
        public CreatureData CurrentCreatureData { get; private set; } // 追加
        public CreatureCardType CreatureCardType => _creatureCardType;

        private SpriteInputHandler _spriteInputHandler;
        public bool IsGrayscale { get; private set; }

        private bool _animationSkipped = false;
        private bool _playAnimation = false;
        private SpriteStatus _pendingStatus;

        /// <summary>
        /// このカードに紐づく全てのインレットViewのリストを取得します。
        /// </summary>
        /// <returns>DiceInletViewのリスト。</returns>
        public List<StatusIconView> GetStatusIconViews()
        {
            return _statusIconViews;
        }

        /// <summary>
        /// このカードに紐づく全てのインレットViewのリストを取得します。
        /// </summary>
        /// <returns>DiceInletViewのリスト。</returns>
        public List<DiceInletView> GetInletViews()
        {
            return _diceInletViews;
        }

        public override void OnAwake()
        {
            base.OnAwake();
            SetSpawnedState(false);
            _hoverAnimationStrategy = new HoverAnimationStrategy(_hoverAnimationProfile);
            _normalAnimationStrategy = new NormalAnimationStrategy(_normalAnimationProfile);
            _dragAnimationStrategy = new DragAnimationStrategy(_dragAnimationProfile);
            _bodySlamAnimationStrategy = new BodySlamAnimationStrategy(_bodySlamAnimationProfile);
            _damageAnimationStrategy = new DamageAnimationStrategy(_damageAnimationProfile);
            _deathAnimationStrategy = new DeathAnimationStrategy(_deathAnimationProfile);
            _buffAnimationStrategy = new BuffAnimationStrategy(_buffAnimationProfile);

            foreach (var statusIconView in _statusIconViews)
            {
                statusIconView.OnAwake();
            }

            _spriteInputHandler = GetComponent<SpriteInputHandler>();
            _animationContext.SpriteCommandBus = _commandBus;
        }

        /// <summary>
        /// 指定された外観プロファイルに基づいて、カードの見た目を更新します。
        /// </summary>
        /// <param name="profile">適用する外観プロファイル。</param>
        public void SetAppearance(AppearanceProfile profile, CreatureIdEntity creatureIdEntity)
        {
            if (_appearanceController != null && profile != null)
            {
                _appearanceController.UpdateAppearance(profile);
            }
            _cardName.text = _nameService.GetDisplayName(creatureIdEntity);
        }

        public void PlayBuffAnimation(VfxDefinition vfxDefinition)
        {
            _animationContext.VfxDefinition = vfxDefinition;
            _buffAnimationStrategy.ExecuteAsync(_animationContext);
        }
        public void PlayDeathAnimation()
        {
            _deathAnimationStrategy.ExecuteAsync(_animationContext);
        }
        public void PlayBodySlamAnimation()
        {
            _bodySlamAnimationStrategy.ExecuteAsync(_animationContext);
        }
        public void PlayDamageAnimation()
        {
            _damageAnimationStrategy.ExecuteAsync(_animationContext);
        }
        public void SetGrayscale(bool enabled)
        {
            IsGrayscale = enabled;
            _multiRendererVisualController.SetColor(enabled ? Color.gray : _originalColor);
        }

        public void SetInteractionProfile(InteractionProfile profile)
        {
            if (_spriteInputHandler != null)
            {
                _spriteInputHandler.SetProfile(profile);
            }
        }

        public CompositeObjectId GetCurrentCardId() => GetObjectId();

        public override void EnterNormalState()
        {
            base.EnterNormalState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(true);
            SetOrderInLayer(SortingOrders.Cards.Default);
        }

        public override void EnterHoveringState()
        {
            base.EnterHoveringState();
            TryPlayStatusAnimation(CurrentStatus);
            SetOrderInLayer(SortingOrders.Cards.Hovered);
        }

        public override void EnterInactiveState()
        {
            base.EnterInactiveState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(false);
            SetOrderInLayer(SortingOrders.Cards.Default);
        }

        public override void EnterDraggingState()
        {
            base.EnterDraggingState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(false);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = transform.position.z;
            SetOrderInLayer(SortingOrders.Cards.Dragging);
        }

        public override void EnterDraggingInProgressState()
        {
            base.EnterDraggingInProgressState();
        }

        public override void MoveTo(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        public override async UniTask MoveToAnimated(Vector3 targetPosition)
        {
            if (this.transform.position == targetPosition) return;

            _currentMoveAnimation = DOTween.Sequence();
            _currentMoveAnimation.Append(transform.DOMove(targetPosition, _animationDuration)
                                        .SetEase(Ease.OutQuad));
            
            await _currentMoveAnimation.AsyncWaitForCompletion();
        }

        private async void TryPlayStatusAnimation(SpriteStatus targetStatus)
        {
            if(_playAnimation)
            {
                _animationSkipped = true;
                _pendingStatus = targetStatus;
                return;
            }

            Sequence animationSequence = null;
                _playAnimation = true;

            switch (targetStatus)
            {
                case SpriteStatus.Normal:
//                    animationSequence = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    animationSequence = _normalAnimationStrategy.ExecuteAsync(_animationContext);
                    break;
                case SpriteStatus.Hover:
//                    animationSequence = _hoverAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    animationSequence = _hoverAnimationStrategy.ExecuteAsync(_animationContext);
                    break;
                case SpriteStatus.DraggingStarted:
//                    animationSequence = _dragAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    animationSequence = _dragAnimationStrategy.ExecuteAsync(_animationContext);
                    break;
                case SpriteStatus.Inactive:
                    animationSequence = _normalAnimationStrategy.ExecuteAsync(_animationContext);
//                    animationSequence = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    break;
                case SpriteStatus.DraggingInProgress:
                    break;
                case SpriteStatus.Acceptable:
                    break;
                case SpriteStatus.Move:
                    break;
            }

            if (animationSequence != null)
            {
                _currentAnimation = animationSequence;
                await animationSequence.AsyncWaitForCompletion();
                HandleAnimationCompletion();
            }
            else
            {
                HandleAnimationCompletion();
            }
        }

        private void HandleAnimationCompletion()
        {
            if (_animationSkipped)
            {
                _animationSkipped = false;
                TryPlayStatusAnimation(_pendingStatus);
            }
            _playAnimation = false;

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}

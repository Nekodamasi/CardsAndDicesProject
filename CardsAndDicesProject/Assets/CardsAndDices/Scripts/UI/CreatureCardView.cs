using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using VContainer;
using System.Collections.Generic; // 追加

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードの視覚的な表示を管理するコンポーネント。
    /// BaseSpriteViewを継承し、カード特有の視覚効果を実装します。
    /// </summary>
    public class CreatureCardView : BaseSpriteView
    {
        [Inject]
        public void Construct(CardInteractionOrchestrator orchestrator)
        {
            this._orchestrator = orchestrator;
        }

        [Header("Card Specific Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] public string _cardName;
        [SerializeField] private CreatureCardType _creatureCardType;        
        [SerializeField] private List<DiceInletView> _diceInletViews = new List<DiceInletView>();
        [SerializeField] private List<StatusIconView> _statusIconViews = new List<StatusIconView>();
        [SerializeField] private CreatureAppearanceController _appearanceController;

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

            foreach (var diceInletView in _diceInletViews)
            {
                diceInletView.OnAwake();
            }

            foreach (var statusIconView in _statusIconViews)
            {
                statusIconView.OnAwake();
            }

            _spriteInputHandler = GetComponent<SpriteInputHandler>();
        }

        /// <summary>
        /// 指定された外観プロファイルに基づいて、カードの見た目を更新します。
        /// </summary>
        /// <param name="profile">適用する外観プロファイル。</param>
        public void SetAppearance(AppearanceProfile profile)
        {
            if (_appearanceController != null && profile != null)
            {
                _appearanceController.UpdateAppearance(profile);
            }
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
        }

        public override void EnterHoveringState()
        {
            base.EnterHoveringState();
            TryPlayStatusAnimation(CurrentStatus);
        }

        public override void EnterInactiveState()
        {
            base.EnterInactiveState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(false);
        }

        public override void EnterDraggingState()
        {
            base.EnterDraggingState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(false);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = transform.position.z;
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
                    animationSequence = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    break;
                case SpriteStatus.Hover:
                    animationSequence = _hoverAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    break;
                case SpriteStatus.DraggingStarted:
                    animationSequence = _dragAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
                    break;
                case SpriteStatus.Inactive:
                    animationSequence = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
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

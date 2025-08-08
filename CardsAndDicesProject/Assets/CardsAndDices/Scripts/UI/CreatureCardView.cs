using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using VContainer;

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
            Debug.Log($"[CreatureCardView] {gameObject.name} - Construct called. Orchestrator is null: {_orchestrator == null}");
        }

        [Header("Card Specific Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] public string _cardName;

        private SpriteInputHandler _spriteInputHandler;
        public bool IsGrayscale { get; private set; }

        private bool _animationSkipped = false;
        private bool _playAnimation = false;
        private SpriteStatus _pendingStatus;

        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"[CreatureCardView] {gameObject.name} (ID: {GetObjectId().UniqueId}) - Awake called. Orchestrator is null: {_orchestrator == null}");
            // _orchestrator?.RegisterView(this); // BaseSpriteViewのAwakeで既に呼ばれているためコメントアウト
            _spriteInputHandler = GetComponent<SpriteInputHandler>();

            if (_audioSource == null)
            {
                //_audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            //_audioSource.clip = _hoverSound;
            //_audioSource.playOnAwake = false;
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

        public override CompositeObjectId GetCurrentCardId() => GetObjectId();

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
//            TryPlayStatusAnimation(CurrentStatus);
        }

        public override void MoveTo(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        public override async UniTask MoveToAnimated(Vector3 targetPosition)
        {
            if (this.transform.position == targetPosition) return;

            Debug.Log("MoveToAnimated:" + _cardName + ":" + this.transform.position + "->" + targetPosition);
            _currentMoveAnimation = DOTween.Sequence();
            _currentMoveAnimation.Append(transform.DOMove(targetPosition, _animationDuration)
                                        .SetEase(Ease.OutQuad));
            
            await _currentMoveAnimation.AsyncWaitForCompletion();
        }

        private async void TryPlayStatusAnimation(SpriteStatus targetStatus)
        {
            if(_playAnimation)
//            if (_currentAnimation != null && _currentAnimation.IsActive() && _currentAnimation.IsPlaying())
            {
                Debug.Log("<color=blue>カード：</color>" + _cardName + "->をスキップ:" + targetStatus + "->前：" + _currentStatus);
                _animationSkipped = true;
                _pendingStatus = targetStatus;
                return;
            }

            Debug.Log("<color=blue>カード：</color>" + _cardName + "->正規ルートアニメーション:" + targetStatus + "->前：" + _currentStatus);
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
                    // ドラッグ中のアニメーションはOrchestratorが直接transformを操作するため、ここではアニメーションは不要
                    break;
                case SpriteStatus.Acceptable:
                    // Acceptable状態のアニメーションがあればここに追加
                    break;
                case SpriteStatus.Move:
                    // Move状態のアニメーションがあればここに追加
                    break;
            }

            if (animationSequence != null)
            {
                _currentAnimation = animationSequence;
                await animationSequence.AsyncWaitForCompletion();
            Debug.Log("<color=blue>カード：</color>" + _cardName + "->アニメーションえんど:" + targetStatus + "->前：" + _currentStatus);
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
                Debug.Log("<color=blue>カード：</color>" + _cardName + "->スキップアニメーション用State:" + _pendingStatus);
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

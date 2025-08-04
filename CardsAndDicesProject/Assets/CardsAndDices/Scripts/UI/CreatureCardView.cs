using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace CardsAndDice
{
    /// <summary>
    /// クリーチャーカードの視覚的な表示を管理するコンポーネント。
    /// BaseSpriteViewを継承し、カード特有の視覚効果を実装します。
    /// </summary>
    public class CreatureCardView : BaseSpriteView
    {
        [Header("Card Specific Settings")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] public string _cardName;

        private SpriteInputHandler _spriteInputHandler;
        public bool IsGrayscale { get; private set; }

        protected override void Awake()
        {
            base.Awake();
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
            KillCurrentAnimation();
            _currentAnimation = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
            SetColliderEnabled(true);
        }

        public override void EnterHoveringState()
        {
            base.EnterHoveringState();
            KillCurrentAnimation();
            _currentAnimation = _hoverAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
        }

        public override void EnterInactiveState()
        {
            base.EnterInactiveState();
            SetColliderEnabled(false);
        }

        public override void EnterDraggingState()
        {
            base.EnterDraggingState();
            KillCurrentAnimation();
            _currentAnimation = _dragAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
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

            Debug.Log("MoveToAnimated:" + _cardName + ":" + this.transform.position + "->" + targetPosition);
//          KillCurrentMoveAnimation();
            _currentMoveAnimation = DOTween.Sequence();
            _currentMoveAnimation.Append(transform.DOMove(targetPosition, _animationDuration)
                                        .SetEase(Ease.OutQuad));
            
            await _currentMoveAnimation.AsyncWaitForCompletion();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}

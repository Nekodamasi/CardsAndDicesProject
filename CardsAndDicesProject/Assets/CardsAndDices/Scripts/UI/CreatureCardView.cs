using UnityEngine;
using DG.Tweening;

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

        // Card Specific Animation Strategies は BaseSpriteView で管理されるため、ここでは削除
        // [Header("Card Specific Animation Strategies")]
        // [SerializeField] private CardHoverAnimationSO _cardHoverAnimation;
        // [SerializeField] private CardNormalAnimationSO _cardNormalAnimation;
        // [SerializeField] private CardDragAnimationSO _cardDragAnimation;

        private SpriteInputHandler _spriteInputHandler;
        public bool IsGrayscale { get; private set; }

        /// <summary>
        /// コンポーネントの初期化を行います。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _spriteInputHandler = GetComponent<SpriteInputHandler>();

            // AudioSourceがアタッチされていない場合は追加
            if (_audioSource == null)
            {
//                _audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // ホバーサウンドを設定
//            _audioSource.clip = _hoverSound;
//            _audioSource.playOnAwake = false;
        }

        /// <summary>
        /// カードの表示をグレースケールに設定します。
        /// </summary>
        /// <param name="enabled">グレースケールを有効にする場合はtrue。</param>
        public void SetGrayscale(bool enabled)
        {
            Debug.Log("ほげえええええええええええ");
            IsGrayscale = enabled;
            _multiRendererVisualController.SetColor(enabled ? Color.gray : _originalColor);
        }

        /// <summary>
        /// このカードのインタラクションプロファイルを動的に設定します。
        /// </summary>
        /// <param name="profile">設定するインタラクションプロファイル。</param>
        public void SetInteractionProfile(InteractionProfile profile)
        {
            if (_spriteInputHandler != null)
            {
                _spriteInputHandler.SetProfile(profile);
            }
        }

        /// <summary>
        /// 現在のカードのIDを取得します。
        /// </summary>
        public override CompositeObjectId GetCurrentCardId() => GetObjectId(); // CreatureCardViewは自身がカードなので、自身のIDを返す

        /// <summary>
        /// カードを通常状態に遷移させます。
        /// </summary>
        public override void EnterNormalState()
        {
            base.EnterNormalState();
            KillCurrentAnimation();
            _currentAnimation = _normalAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
            SetColliderEnabled(true);
        }

        /// <summary>
        /// カードをホバー状態に遷移させます。
        /// </summary>
        public override void EnterHoveringState()
        {
            base.EnterHoveringState();
            KillCurrentAnimation();
            _currentAnimation = _hoverAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
        }

        /// <summary>
        /// 非アクティブ状態
        /// </summary>
        public override void EnterInactiveState()
        {
            base.EnterInactiveState();
            SetColliderEnabled(false);
        }

        /// <summary>
        /// カードをドラッグ開始状態に遷移させます。
        /// </summary>
        public override void EnterDraggingState()
        {
            base.EnterDraggingState();
            KillCurrentAnimation();
            _currentAnimation = _dragAnimation?.PlayAnimation(gameObject, _multiRendererVisualController, _originalScale, _originalColor, _animationDuration, transform.position);
            SetColliderEnabled(false);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = transform.position.z; // Z座標は変更しない
        }

        /// <summary>
        /// カードをドラッグ中状態に遷移させます。
        /// </summary>
        public override void EnterDraggingInProgressState()
        {
            base.EnterDraggingInProgressState();
            // ドラッグ中のアニメーション（例：マウス追従）はOrchestratorが直接transformを操作するため、ここではアニメーションは不要
            // 必要であれば、ドラッグ中の見た目（例：半透明化の維持など）をここで設定
        }

        /// <summary>
        /// カードを指定された位置へ即座に移動させます。（ドラッグ中のマウス追従用）
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        public override void MoveTo(Vector3 targetPosition)
        {
            transform.position = targetPosition;
        }

        /// <summary>
        /// カードを指定された位置へアニメーションで移動させます。（リフロー用）
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        public override void MoveToAnimated(Vector3 targetPosition, ICommand commandToEmitOnComplete = null)
        {
            Debug.Log("MoveToAnimated（移動前） -> カードName：" + _cardName + "-->" + this.transform.position + ":" + targetPosition);

            // 移動先が今と同じで、コマンド指定なしの場合、移動を行わない
            if (this.transform.position == targetPosition && commandToEmitOnComplete == null)
            {
                Debug.Log("移動しない：" + _cardName);
                return;
            }
            KillCurrentMoveAnimation();
                _currentMoveAnimation = DOTween.Sequence();
            _currentMoveAnimation.Append(transform.DOMove(targetPosition, _animationDuration)
                                        .SetEase(Ease.OutQuad));
//            SetColliderEnabled(false); // 移動中はコライダーを無効化
            _currentMoveAnimation.OnComplete(() =>
            {
                Debug.Log("MoveToAnimated -> カードName：" + _cardName + "-->" + this.transform.position + ":" + targetPosition);
                if (commandToEmitOnComplete != null)
                {
                    EmitCommand(commandToEmitOnComplete);
                }
            });
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
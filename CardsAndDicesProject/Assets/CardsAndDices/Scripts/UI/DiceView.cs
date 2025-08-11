using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスの視覚的な表示と状態遷移を管理するコンポーネント。
    /// </summary>
    public class DiceView : BaseSpriteView
    {
        [Inject]
        public void Construct(DiceInteractionOrchestrator orchestrator)
        {
            this._orchestrator = orchestrator;
        }

        [Header("Dice Specific Settings")]
        [SerializeField] public string _diceName;

        private SpriteInputHandler _spriteInputHandler;
        private bool _animationSkipped = false;
        private bool _playAnimation = false;
        private SpriteStatus _pendingStatus;
        public override CompositeObjectId GetCurrentCardId() => GetObjectId();

        /// <summary>
        /// 指定されたデータに基づいてダイスの表示を更新します。
        /// </summary>
        /// <param name="data">表示に使用するダイスのデータ。</param>
        /// <summary>
        /// Viewを指定された位置へアニメーションで移動させます。（リフロー用）
        /// CardSlotViewは通常アニメーションで移動しないため、空の実装。
        /// </summary>
        /// <param name="targetPosition">移動先のワールド座標。</param>
        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"[DiceView] {gameObject.name} (ID: {GetObjectId().UniqueId}) - Awake called. Orchestrator is null: {_orchestrator == null}");
            // _orchestrator?.RegisterView(this); // BaseSpriteViewのAwakeで既に呼ばれているためコメントアウト
            _spriteInputHandler = GetComponent<SpriteInputHandler>();
        }

        public override async UniTask MoveToAnimated(Vector3 targetPosition)
        {
            if (this.transform.position == targetPosition) return;

            Debug.Log("MoveToAnimated:" + _diceName + ":" + this.transform.position + "->" + targetPosition);
            _currentMoveAnimation = DOTween.Sequence();
            _currentMoveAnimation.Append(transform.DOMove(targetPosition, _animationDuration)
                                        .SetEase(Ease.OutQuad));
            
            await _currentMoveAnimation.AsyncWaitForCompletion();
        }

        public void UpdateView(DiceData data)
        {
            if (data == null) return;
        }

        /// <summary>
        /// 通常状態に遷移します。
        /// </summary>
        public override void EnterNormalState()
        {
            base.EnterNormalState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(true);
        }

        /// <summary>
        /// ホバー状態に遷移します。
        /// </summary>
        public override void EnterHoveringState()
        {
            base.EnterHoveringState();
            TryPlayStatusAnimation(CurrentStatus);
        }

        /// <summary>
        /// ドラッグ開始状態に遷移します。
        /// </summary>
        public override void EnterDraggingState()
        {
            base.EnterDraggingState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(false);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = transform.position.z;
        }

        /// <summary>
        /// 非アクティブ状態に遷移します。
        /// </summary>
        public override void EnterInactiveState()
        {
            base.EnterInactiveState();
            TryPlayStatusAnimation(CurrentStatus);
            SetColliderEnabled(false);
        }

        public override void EnterDraggingInProgressState()
        {
            base.EnterDraggingInProgressState();
            //            TryPlayStatusAnimation(CurrentStatus);
        }

        private async void TryPlayStatusAnimation(SpriteStatus targetStatus)
        {
            if(_playAnimation)
//            if (_currentAnimation != null && _currentAnimation.IsActive() && _currentAnimation.IsPlaying())
            {
                Debug.Log("<color=blue>カード：</color>" + _diceName + "->をスキップ:" + targetStatus + "->前：" + _currentStatus);
                _animationSkipped = true;
                _pendingStatus = targetStatus;
                return;
            }

            Debug.Log("<color=blue>カード：</color>" + _diceName + "->正規ルートアニメーション:" + targetStatus + "->前：" + _currentStatus);
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
            Debug.Log("<color=blue>カード：</color>" + _diceName + "->アニメーションえんど:" + targetStatus + "->前：" + _currentStatus);
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
                Debug.Log("<color=blue>カード：</color>" + _diceName + "->スキップアニメーション用State:" + _pendingStatus);
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
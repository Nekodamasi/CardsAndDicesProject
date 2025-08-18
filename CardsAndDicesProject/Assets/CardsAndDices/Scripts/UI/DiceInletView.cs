using UnityEngine;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using VContainer;
using TMPro;

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

        [Header("Components")]
        [SerializeField] private SpriteSelector _faceSpriteSelector;
        [SerializeField] private TextMeshProUGUI _countdown;
        [Header("Animations")]
        [SerializeField] private BaseAnimationSO _acceptableAnimation;
        [SerializeField] private BaseAnimationSO _dropWaitingAnimation;

        public override void OnAwake()
        {
            Debug.Log("<color=red>いんれっとだけだせるといいんだけどね：</color>" + gameObject.name + "_");
            base.OnAwake();
            SetSpawnedState(false);
        }

        /// <summary>
        /// ダイスインレットの見た目を初期化します
        /// </summary>
        public void InitializeDisplay(DiceInletConditionSO diceInletCondition)
        {
            if (diceInletCondition.InletActivationViewType == InletActivationViewType.SingleMatchTrigger)
            {
                _countdown.enabled = true;
                _countdown.SetText(diceInletCondition.InitialCountdownValue.ToString());
            }
            else
            {
                _countdown.enabled = false;
            }
            _faceSpriteSelector.SelectSprite(diceInletCondition.DiceInletConditionDisplayId);
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

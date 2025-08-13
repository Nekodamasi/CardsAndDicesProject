using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットの視覚的な表示を管理するコンポーネント。
    /// </summary>
    public class DiceSlotView : BaseSpriteView
    {
        [Inject]
        public void Construct(DiceInteractionOrchestrator orchestrator)
        {
            this._orchestrator = orchestrator;
        }

        [Header("Components")]
        [SerializeField] private DiceSlotManager _diceSlotManager;

        [Header("Slot Definition")]
        [SerializeField] private DiceSlotLocation _location;

        private DiceSlotData _diceSlotData;

         protected override void Awake()
        {
            base.Awake();
            _diceSlotData = new DiceSlotData(GetObjectId(), transform.position, _location);
            _diceSlotManager.RegisterSlot(_diceSlotData);
        }

        /// <summary>
        /// 通常状態に遷移します。
        /// </summary>
        public override void EnterNormalState()
        {
            base.EnterNormalState();
        }

        public override void MoveTo(Vector3 targetPosition)
        {
            transform.position = targetPosition;
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
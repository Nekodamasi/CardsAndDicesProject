using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットのリフロー配置処理
    /// </summary>
    public class CombatPhaseReflowCardEvent : IEvent
    {
        private readonly CompositeObjectId _draggedCardId;
        private readonly CompositeObjectId _targetSlotId;

        /// <summary>
        /// ドロップされたクリーチャーカードIDを取得します
        /// </summary>
        public CompositeObjectId DraggedCardId => _draggedCardId;

        /// <summary>
        /// ターゲットのクリーチャーカードスロットIDを取得します
        /// </summary>
        public CompositeObjectId TargetSlotId => _targetSlotId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CombatPhaseReflowCardEvent(CompositeObjectId draggedCardId, CompositeObjectId targetSlotId)
        {
            _draggedCardId = draggedCardId;
            _targetSlotId = targetSlotId;
        }
    }
} 
using CardsAndDice;
using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDice.Tester
{
    /// <summary>
    /// 実行中の各種データをインスペクターに表示するためのデバッグ用クラス。
    /// </summary>
    public class DebugViewer : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private UIStateMachine _uiStateMachine;
        [SerializeField] private CardSlotManager _cardSlotManager;

        [Header("Debug Info - UI State Machine")]
        [SerializeField] private UIStateMachine.UIState _currentState;

        [Header("Debug Info - Card Slots")]
        [SerializeField] private List<SlotDebugInfo> _slotInfos = new();

        private void Update()
        {
            // 'A'キーが押されたら情報を更新する
            if (Input.GetKeyDown(KeyCode.A))
            {
                UpdateDebugInfo();
            }
        }

        /// <summary>
        /// 各マネージャーから最新の情報を取得し、インスペクター表示用のフィールドを更新します。
        /// </summary>
        private void UpdateDebugInfo()
        {
            if (_uiStateMachine != null)
            {
                _currentState = _uiStateMachine.CurrentState;
            }

            if (_cardSlotManager != null)
            {
                _slotInfos = _cardSlotManager.GetDebugSlotData();
            }
            
            Debug.Log("Debug info updated. Press 'A' to refresh.");
        }
    }
}

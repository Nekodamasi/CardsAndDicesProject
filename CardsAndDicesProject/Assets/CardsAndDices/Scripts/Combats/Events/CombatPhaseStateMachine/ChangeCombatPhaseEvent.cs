using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットのリフロー配置処理
    /// </summary>
    public class ChangeCombatPhaseEvent : IEvent
    {
        private CombatPhase _combatPhase;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ChangeCombatPhaseEvent(CombatPhase combatPhase)
        {
            _combatPhase = combatPhase;
        }

        /// <summary>
        /// 変更先のコンバットフェーズ
        /// </summary>
        public CombatPhase CombatPhase => _combatPhase;
    }
} 
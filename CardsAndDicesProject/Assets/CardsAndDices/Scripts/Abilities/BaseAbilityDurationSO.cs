using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// すべてのアビリティの持続時間、クールダウン、および使用制限ロジックの抽象基本クラス
    /// </summary>
    public abstract class BaseAbilityDurationSO : ScriptableObject
    {
        [Tooltip("持続時間の初期値（例：クールダウンターン、使用回数）")]
        public int InitialValue;

        [Tooltip("リセットタイミング")]
        public TriggerTiming ResetTiming;

        /// <summary>
        /// ゲーム イベント (TurnEnd など) で呼び出され、アビリティ インスタンスの持続状態を更新します。
        /// </summary>
        /// <param name="instance">The ability instance to update.</param>
        /// <param name="command">The command that triggered the event.</param>
        public abstract void OnUse(AbilityInstance instance);

        /// <summary>
        /// アビリティインスタンスの持続状態を初期値にリセットします。
        /// </summary>
        /// <param name="instance">The ability instance to reset.</param>
        public virtual void OnReset(AbilityInstance instance)
        {
            instance.RemainingUsages = InitialValue;
            instance.CurrentCooldown = 0;
        }
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// すべてのアビリティの持続時間、クールダウン、および使用制限ロジックの抽象基本クラス
    /// </summary>
    public abstract class BaseAbilityDurationSO : ScriptableObject
    {
        [Tooltip("持続時間の初期値（例：クールダウンターン、使用回数）")]
        [SerializeField] private int _initialValue;

        [Tooltip("リセットタイミング")]
        [SerializeField] private ActivationTiming _resetTiming;

        /// <summary>
        /// リセットタイミング
        /// </summary>
        public ActivationTiming ResetTiming => _resetTiming;

        /// <summary>
        /// 現在使用可能かを返します
        /// </summary>
        /// <param name="instance">The ability instance to update.</param>
        public virtual bool OnCheck(AbilityInstance instance)
        {
            // 使用回数なし
            if (instance.RemainingUsages == 0) return false;

            // ロック状態
            if (instance.IsLock) return false;

            return true;
        }

        /// <summary>
        /// 使用回数を更新します。
        /// </summary>
        /// <param name="instance">The ability instance to update.</param>
        public virtual void OnUse(AbilityInstance instance)
        {
            instance.SetRemainingUsages(instance.RemainingUsages - 1);
        }

        /// <summary>
        /// 使用回数やロックのリセットを行います。
        /// </summary>
        /// <param name="instance">The ability instance to reset.</param>
        public virtual void OnReset(AbilityInstance instance)
        {
            instance.SetRemainingUsages(_initialValue);
        }
    }
}

using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットのリフロー配置処理
    /// </summary>
    public class ReflowDiceSlotsCommand : ICommand
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReflowDiceSlotsCommand()
        {
        }

        /// <summary>
        /// 効果を実行します。
        /// </summary>
        public void Execute()
        {
            // BaseSpriteViewで実装
        }

        /// <summary>
        /// 効果を元に戻します。
        /// </summary>
        public void Undo()
        {
            // BaseSpriteViewで実装
        }
    }
} 
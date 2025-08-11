using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスのデータを保持するクラス。
    /// </summary>
    public class DiceData
    {
        /// <summary>
        /// スロットのワールド座標
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CompositeObjectId UniqueId { get; }

        /// <summary>
        /// ダイスの出目 (1-6)。
        /// </summary>
        public int FaceValue { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="uniqueId">ダイスの一意なID。</param>
        public DiceData(CompositeObjectId uniqueId)
        {
            UniqueId = uniqueId;
            Roll();
        }

        /// <summary>
        /// ダイスを振り、出目を更新します。
        /// </summary>
        public void Roll()
        {
            FaceValue = new System.Random().Next(1, 7);
        }
    }
}

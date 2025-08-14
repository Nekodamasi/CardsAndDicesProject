using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスのデータを保持するクラス。
    /// </summary>
    public class DiceData
    {
        public event Action<int> OnFaceValueChanged;
        
        /// <summary>
        /// スロットのワールド座標
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CompositeObjectId Id { get; }

        private int _faceValue;
        /// <summary>
        /// ダイスの出目 (1-6)。
        /// </summary>
        public int FaceValue
        {
            get => _faceValue;
            private set
            {
                if (_faceValue == value) return;
                _faceValue = value;
                OnFaceValueChanged?.Invoke(_faceValue);
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DiceData(CompositeObjectId id, int faceValue)
        {
            Id = id;
            _faceValue = faceValue;
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

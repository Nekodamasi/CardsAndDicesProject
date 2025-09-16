using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスの状態を保持するデータ
    /// </summary>
    public class DiceInstance : IDisposable
    {
        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public int Id { get; }

        private int _faceValue;

        /// <summary>
        /// ダイスの出目 (1-6)。
        /// </summary>
        public int FaceValue => _faceValue;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DiceInstance(int id, int faceValue)
        {
            Id = id;
            _faceValue = faceValue;
            if (_faceValue < 1 || _faceValue > 6)
            {
                Roll();
            }
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// ダイスを振り、出目を更新します。
        /// </summary>
        public void Roll()
        {
            _faceValue = new System.Random().Next(1, 7);
        }
    }
}

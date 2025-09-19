using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイスの状態を保持するデータ
    /// </summary>
    public class DiceInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        private int _faceValue;

        /// <summary>
        /// ダイスの出目 (1-6)。
        /// </summary>
        public int FaceValue => _faceValue;

        public bool IsOnScreen;
        public bool IsAlive;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DiceInstance(CompositeObjectId compositeObjectId, int faceValue)
        {
            _compositeObjectId = compositeObjectId;
            _faceValue = faceValue;
            IsAlive = true;
            if (_faceValue < 1 || _faceValue > 6)
            {
                Roll();
            }
            IsOnScreen = false;
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

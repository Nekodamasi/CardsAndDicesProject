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
        private IDiceSlotPosition _IDiceSlotPosition;

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        public Vector3 DiceHomePosition => _IDiceSlotPosition.GetDiceHomePosition(CompositeObjectId);

        private int _faceValue;

        /// <summary>
        /// ダイスの出目 (1-6)。
        /// </summary>
        public int FaceValue => _faceValue;

        /// <summary>
        /// 画面に配置しているかどうか。
        /// </summary>
        public bool IsOnScreen;

        /// <summary>
        /// インスタンスが生きているか
        /// </summary>
        public bool IsAlive;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public DiceInstance(CompositeObjectId compositeObjectId, int faceValue, IDiceSlotPosition iDiceSlotPosition)
        {
            _compositeObjectId = compositeObjectId;
            _IDiceSlotPosition = iDiceSlotPosition;
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

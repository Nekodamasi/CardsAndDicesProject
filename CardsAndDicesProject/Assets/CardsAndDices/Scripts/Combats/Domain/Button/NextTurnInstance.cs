using System;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// abilityのインスタンスクラス
    /// </summary>
    public class NextTurnInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private IDiceCase _iDiceCase;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NextTurnInstance(CompositeObjectId ownerId, IDiceCase iDiceCase)
        {
            _compositeObjectId = ownerId;
            _iDiceCase = iDiceCase;
        }
        public void Dispose()
        {
        }

        /// <summary>
        /// abilityの所有者を一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// 現在のダイス数
        /// </summary>
        public int CurrentDiceCount => _iDiceCase.CurrentDiceCount;
    }
}

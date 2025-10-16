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
    public class CombatStartInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CombatStartInstance(CompositeObjectId ownerId)
        {
            _compositeObjectId = ownerId;
        }
        public void Dispose()
        {
        }

        /// <summary>
        /// abilityの所有者を一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;
    }
}

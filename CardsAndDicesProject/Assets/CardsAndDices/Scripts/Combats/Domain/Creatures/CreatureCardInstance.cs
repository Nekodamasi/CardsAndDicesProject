using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードを管理するインスタンス。
    /// </summary>
    public class CreatureCardInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;
        private ICreatureCardSlotPosition _iCreatureCardSlotPosition;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardInstance(CompositeObjectId compositeObjectId, ICreatureCardSlotPosition iCreatureCardSlotPosition)
        {
            _compositeObjectId = compositeObjectId;
            _iCreatureCardSlotPosition = iCreatureCardSlotPosition;
        }

        /// <summary>
        /// 画面に配置しているかどうか。
        /// </summary>
        public bool IsOnScreen;

        /// <summary>
        /// インスタンスが生きているか
        /// </summary>
        public bool IsAlive;

        /// <summary>
        /// クリーチャーカードを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;

        /// <summary>
        /// スロットポジション
        /// </summary>
        public Vector3 CreatureCardSlotPosition => _iCreatureCardSlotPosition.GetCreatureCardHomePosition(CompositeObjectId);


        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }
    }
}

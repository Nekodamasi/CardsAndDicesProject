using System;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードスロットを管理するインスタンス。
    /// </summary>
    public class CreatureCardSlotInstance : IDisposable, IIdentifiableInstance
    {
        private CompositeObjectId _compositeObjectId;

        /// <summary>
        /// クリーチャーカードスロットを一意に識別するID。
        /// </summary>
        public CompositeObjectId CompositeObjectId => _compositeObjectId;
        private DiceSlotPositionEntity _diceSlotPositionEntity;
        private CompositeObjectId _placedDiceId;
        private CompositeObjectId _reflowPlacedDiceId;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardSlotInstance(CompositeObjectId compositeObjectId, DiceSlotPositionEntity diceSlotPositionEntity)
        {
            _compositeObjectId = compositeObjectId;
            _diceSlotPositionEntity = diceSlotPositionEntity;
        }

        /// <summary>
        /// ダイススロットポジション
        /// </summary>
        public Vector3 DiceSlotPosition => _diceSlotPositionEntity.Position;

        /// <summary>
        /// ダイススロットの位置
        /// </summary>
        public DiceSlotLocation DiceSlotLocation => _diceSlotPositionEntity.DiceSlotLocation;

        /// <summary>
        /// ダイスの配置
        /// </summary>
        public void PlacedDice(CompositeObjectId diceId)
        {
            _placedDiceId = diceId;
            _reflowPlacedDiceId = diceId;
        }

        /// <summary>
        /// ダイスのリフロー配置
        /// </summary>
        public void ReflowPlacedDice(CompositeObjectId diceId)
        {
            _reflowPlacedDiceId = diceId;
        }

        /// <summary>
        /// ダイスのリムーブ
        /// </summary>
        public void RemoveDice()
        {
            _placedDiceId = null;
            _reflowPlacedDiceId = null;
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// このスロットに配置されているダイスのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId PlacedDiceId => _placedDiceId;

        /// <summary>
        /// リフロー時に、このスロットに配置されているダイスのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId ReflowPlacedDiceId => _reflowPlacedDiceId;

        /// <summary>
        /// このスロットにダイスが配置されているかどうか。
        /// </summary>
        public bool IsOccupied => PlacedDiceId != null;
    }
}

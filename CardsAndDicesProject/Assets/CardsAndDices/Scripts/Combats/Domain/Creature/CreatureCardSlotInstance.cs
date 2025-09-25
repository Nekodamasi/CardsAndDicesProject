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
        private CreatureCardSlotPositionEntity _creatureCardSlotPositionEntity;
        private CompositeObjectId _placedCardId;
        private CompositeObjectId _reflowPlacedCardId;
        public bool IsOnScreen;
        public bool IsAlive;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureCardSlotInstance(CompositeObjectId compositeObjectId, CreatureCardSlotPositionEntity creatureCardSlotPositionEntity)
        {
            _compositeObjectId = compositeObjectId;
            _creatureCardSlotPositionEntity = creatureCardSlotPositionEntity;
        }

        /// <summary>
        /// スロットポジション
        /// </summary>
        public Vector3 CreatureCardSlotPosition => _creatureCardSlotPositionEntity.Position;

        /// <summary>
        /// チーム
        /// </summary>
        public Team Team => _creatureCardSlotPositionEntity.Team;

        /// <summary>
        /// ラインポジション
        /// </summary>
        public LinePosition LinePosition => _creatureCardSlotPositionEntity.LinePosition;

        /// <summary>
        /// ロケーション
        /// </summary>
        public SlotLocation Location => _creatureCardSlotPositionEntity.Location;

        /// <summary>
        /// カードの配置
        /// </summary>
        public void PlacedCard(CompositeObjectId cardId)
        {
            _placedCardId = cardId;
            _reflowPlacedCardId = cardId;
        }

        /// <summary>
        /// カードのリフロー配置
        /// </summary>
        public void ReflowPlacedCard(CompositeObjectId cardId)
        {
            _reflowPlacedCardId = cardId;
        }

        /// <summary>
        /// カードのリムーブ
        /// </summary>
        public void RemoveCard()
        {
            _placedCardId = null;
            _reflowPlacedCardId = null;
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// このスロットに配置されているカードのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId PlacedCardId => _placedCardId;

        /// <summary>
        /// リフロー時に、このスロットに配置されているカードのID。
        /// 配置されていない場合はnull。
        /// </summary>
        public CompositeObjectId ReflowPlacedCardId => _reflowPlacedCardId;

        /// <summary>
        /// このスロットにカードが配置されているかどうか。
        /// </summary>
        public bool IsOccupied => PlacedCardId != null;
    }
}

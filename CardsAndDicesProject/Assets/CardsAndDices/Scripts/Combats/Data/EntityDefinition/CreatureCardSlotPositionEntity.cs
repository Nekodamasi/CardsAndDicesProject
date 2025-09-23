using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットの位置を一意に識別するためのエンティティ定義です。
    /// </summary>
    [CreateAssetMenu(fileName = "CCSPo_", menuName = "CardsAndDices/Combats/Data/EntityDefinition/CreatureCardSlotPositionEntity")]
    public class CreatureCardSlotPositionEntity : BaseEntityDefinition
    {
        [SerializeField] Team _tram;
        [SerializeField] LinePosition _linePosition;
        [SerializeField] SlotLocation _slotLocation;
        [SerializeField] Vector3 _position;

        public Team Team => _tram;
        public LinePosition LinePosition => _linePosition;
        public SlotLocation Location => _slotLocation;
        public Vector3 Position => _position;
    }
}

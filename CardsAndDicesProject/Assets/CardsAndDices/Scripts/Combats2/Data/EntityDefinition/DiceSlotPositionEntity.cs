using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットの位置を一意に識別するためのエンティティ定義です。
    /// </summary>
    [CreateAssetMenu(fileName = "DiStPo_", menuName = "CardsAndDices/Data/EntityDefinition/DiceSlotPositionEntity")]
    public class DiceSlotPositionEntity : BaseEntityDefinition
    {
        [SerializeField] DiceSlotLocation _diceSlotLocation;
        [SerializeField] Vector3 _position;

        public DiceSlotLocation DiceSlotLocation => _diceSlotLocation;
        public Vector3 Position => _position;
    }
}

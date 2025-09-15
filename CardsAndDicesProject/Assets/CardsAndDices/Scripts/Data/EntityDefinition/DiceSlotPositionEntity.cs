using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットの位置を一意に識別するためのエンティティ定義です。
    /// </summary>
    [CreateAssetMenu(fileName = "DiStPo_", menuName = "CardsAndDices/Data/EntityDefinition/DiceSlotPositionEntity")]
    public class DiceSlotPositionEntity : BaseEntityDefinition
    {
        [SerializeField] DiceSlotLocation DiceSlotLocation;
        [SerializeField] Vector3 Position;
    }
}

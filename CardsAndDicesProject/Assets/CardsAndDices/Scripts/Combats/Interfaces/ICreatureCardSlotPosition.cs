using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードスロットのポジションを返すインターフェース
    /// </summary>
    public interface ICreatureCardSlotPosition
    {
        Vector3 GetCreatureCardHomePosition(CompositeObjectId CreatureCardId);
    }
} 
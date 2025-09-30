using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードスロットのポジションを返すインターフェース
    /// </summary>
    public interface ICreatureCardlocation
    {
        SlotLocation GetSlotLocation(CompositeObjectId cardId);
        LinePosition GetLinePosition(CompositeObjectId cardId);
        Team GetTeam(CompositeObjectId cardId);
    }
} 
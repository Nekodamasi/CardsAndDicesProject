using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードスロットのポジションを返すインターフェース
    /// </summary>
    public interface ICreatureCardPlacedLocation
    {
        Team GetTeam(CompositeObjectId compositeObjectId);
        LinePosition GetLinePosition(CompositeObjectId compositeObjectId);
        SlotLocation GetSlotLocation(CompositeObjectId compositeObjectId);
    }
} 
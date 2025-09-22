using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// ダイススロットのポジションを返すインターフェース
    /// </summary>
    public interface IDiceSlotPosition
    {
        Vector3 GetDiceHomePosition(CompositeObjectId DiceId);
    }
} 
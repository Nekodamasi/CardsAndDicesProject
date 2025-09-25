using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーカードスロットのインスタンスレポジトリインターフェース。
    /// </summary>
    public interface ICreatureCardSlotInstanceRepository
    {
        /// <summary>
        /// インスタンスのリストを返します。
        /// </summary>
        List<CreatureCardSlotInstance> GetInstanceList();

        /// <summary>
        /// 指定した配置場所のインスタンスを返します
        /// </summary>
        CreatureCardSlotInstance GetInstance(Team team, LinePosition linePosition, SlotLocation slotLocation);
    }
}

using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// クリーチャーステータスのインスタンスレポジトリインターフェース。
    /// </summary>
    public interface ICreatureStatusInstanceRepository
    {
        /// <summary>
        /// インスタンスのリストを返します。
        /// </summary>
        List<CreatureStatusInstance> GetInstanceList();

        /// <summary>
        /// 指定されたチームの生きているInstanceを返します
        /// </summary>
        List<CreatureStatusInstance> GetNonHandAliveInstanceList(Team team);

        /// <summary>
        /// 指定した配置場所のインスタンスを返します
        /// </summary>
        CreatureStatusInstance GetInstance(CompositeObjectId CompositeObjectId);
    }
}

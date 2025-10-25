using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// ターゲットの取得を行うインターフェース
    /// </summary>
    public interface ITargetManager
    {
        /// <summary>
        /// 指定された実行者と効果範囲に基づいて、ターゲットとなるカードのIDリストを取得します。
        /// </summary>
        /// <param name="areaOfEffect">効果範囲の定義。</param>
        /// <param name="executorId">効果の実行者のID。</param>
        /// <returns>ターゲットとなるカードのCompositeObjectIdのリスト。</returns>
        List<CompositeObjectId> GetTargetList(AreaOfEffect areaOfEffect, CompositeObjectId executorId);

        /// <summary>
        /// クールダウン０処理待ちIDを取得します
        /// </summary>
        CompositeObjectId GetActionOrderCoolDownZeroId();

        /// <summary>
        /// アクション順にソートされたIdのリストを取得します
        /// </summary>
        List<CompositeObjectId> GetActionOrderList();
        /// <summary>
        /// アクション順にソートされたIdのリストを取得します
        /// </summary>
        List<CompositeObjectId> GetEnemyList(Team team);
    }
}

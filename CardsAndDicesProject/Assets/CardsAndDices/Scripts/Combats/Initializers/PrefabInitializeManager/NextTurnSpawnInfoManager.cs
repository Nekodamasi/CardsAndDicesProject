using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// NextTurnSpawnInfoのリストを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "NextTurnSpawnInfoManager", menuName = "CardsAndDices/Combats/Initializes/PrefabInitializeManager/NextTurnSpawnInfoManager")]
    public class NextTurnSpawnInfoManager : ScriptableObject
    {
        /// <summary>
        /// ダイスのスポーン情報リスト。
        /// </summary>
        [SerializeField]
        private List<NextTurnSpawnInfo> _spawnInfos;

        /// <summary>
        /// 保持している全てのスポーン情報リストを取得します。
        /// </summary>
        /// <returns>CreatureCardSpawnInfoのリスト。</returns>
        public List<NextTurnSpawnInfo> GetSpawnInfos()
        {
            return _spawnInfos;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// CombatStartSpawnInfoのリストを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "CombatStartSpawnInfoManager", menuName = "CardsAndDices/Combats/Initializes/PrefabInitializeManager/CombatStartSpawnInfoManager")]
    public class CombatStartSpawnInfoManager : ScriptableObject
    {
        /// <summary>
        /// ダイスのスポーン情報リスト。
        /// </summary>
        [SerializeField]
        private List<CombatStartSpawnInfo> _spawnInfos;

        /// <summary>
        /// 保持している全てのスポーン情報リストを取得します。
        /// </summary>
        /// <returns>CreatureCardSpawnInfoのリスト。</returns>
        public List<CombatStartSpawnInfo> GetSpawnInfos()
        {
            return _spawnInfos;
        }
    }
}

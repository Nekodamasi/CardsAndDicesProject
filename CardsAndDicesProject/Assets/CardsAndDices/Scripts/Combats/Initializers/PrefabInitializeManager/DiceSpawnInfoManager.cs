using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// DiceSpawnInfoのリストを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceSpawnInfoManager", menuName = "CardsAndDices/Combats/Initializes/PrefabInitializeManager/DiceSpawnInfoManager")]
    public class DiceSpawnInfoManager : ScriptableObject
    {
        /// <summary>
        /// ダイスのスポーン情報リスト。
        /// </summary>
        [SerializeField]
        private List<DiceSpawnInfo> _spawnInfos;

        /// <summary>
        /// 保持している全てのスポーン情報リストを取得します。
        /// </summary>
        /// <returns>CreatureCardSpawnInfoのリスト。</returns>
        public List<DiceSpawnInfo> GetSpawnInfos()
        {
            return _spawnInfos;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSpawnInfoのリストを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "EnemyCreatureCardSpawnInfoManager", menuName = "CardsAndDices/Combats/Initializes/PrefabInitializeManager/EnemyCreatureCardSpawnInfoManager")]
    public class EnemyCreatureCardSpawnInfoManager : ScriptableObject
    {
        /// <summary>
        /// クリーチャーカードのスポーン情報リスト。
        /// </summary>
        [SerializeField]
        private List<EnemyCreatureCardSpawnInfo> _spawnInfos;

        /// <summary>
        /// 保持している全てのスポーン情報リストを取得します。
        /// </summary>
        /// <returns>CreatureCardSpawnInfoのリスト。</returns>
        public List<EnemyCreatureCardSpawnInfo> GetSpawnInfos()
        {
            return _spawnInfos;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSpawnInfoのリストを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureCardSpawnInfoManager", menuName = "CardsAndDices/Combats/Initializes/PrefabInitializeManager/CreatureCardSpawnInfoManager")]
    public class CreatureCardSpawnInfoManager : ScriptableObject
    {
        /// <summary>
        /// クリーチャーカードのスポーン情報リスト。
        /// </summary>
        [SerializeField]
        private List<CreatureCardSpawnInfo> _spawnInfos;

        /// <summary>
        /// 保持している全てのスポーン情報リストを取得します。
        /// </summary>
        /// <returns>CreatureCardSpawnInfoのリスト。</returns>
        public List<CreatureCardSpawnInfo> GetSpawnInfos()
        {
            return _spawnInfos;
        }
    }
}

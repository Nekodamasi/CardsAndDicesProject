using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardSlotSpawnInfoのリストを管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureCardSlotSpawnInfoManager", menuName = "CardsAndDices/Combats/Initializes/PrefabInitializeManager/CreatureCardSlotSpawnInfoManager")]
    public class CreatureCardSlotSpawnInfoManager : ScriptableObject
    {
        /// <summary>
        /// クリーチャーカードスロットのスポーン情報リスト。
        /// </summary>
        [SerializeField]
        private List<CreatureCardSlotSpawnInfo> _spawnInfos;

        /// <summary>
        /// 保持している全てのスポーン情報リストを取得します。
        /// </summary>
        /// <returns>CreatureCardSpawnInfoのリスト。</returns>
        public List<CreatureCardSlotSpawnInfo> GetSpawnInfos()
        {
            return _spawnInfos;
        }
    }
}

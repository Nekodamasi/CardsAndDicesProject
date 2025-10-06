using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices 
{
    [CreateAssetMenu(fileName = "WaveData_", menuName = "CardsAndDices/Combats/Data/Waves/WaveData")]
    /// <summary>
    /// １つのウェーブで配置されるエネミーを管理するデータクラス
    /// </summary>
    public class WaveData : ScriptableObject
    {
        [SerializeField] private int _waveNumber;
        [SerializeField] private List<EnemyPlacement> _enemyPlacements;

        public int WaveNumber => _waveNumber;
        public IReadOnlyList<EnemyPlacement> EnemyPlacements => _enemyPlacements;
    }
}

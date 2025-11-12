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
        [SerializeField] private BGMDataEntity _bGMDataEntity;

        public int WaveNumber => _waveNumber;
        public BGMDataEntity BGMDataEntity => _bGMDataEntity;
        public IReadOnlyList<EnemyPlacement> EnemyPlacements => _enemyPlacements;
    }
}

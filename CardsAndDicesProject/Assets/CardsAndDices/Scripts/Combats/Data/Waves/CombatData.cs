using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices 
{
    [CreateAssetMenu(fileName = "CbtData_", menuName = "CardsAndDices/Combats/Data/Waves/CombatData")]
    /// <summary>
    /// １つ戦闘で発生するウェーブを管理するデータクラス
    /// </summary>
    public class CombatData : ScriptableObject
    {
        [SerializeField] private string _combatId;
        [SerializeField] private List<WaveData> _waves;
        [SerializeField] private float _waveInterval = 5.0f;

        public string CombatId => _combatId;
        public IReadOnlyList<WaveData> Waves => _waves;
        public WaveData GetWaveData(int waveNumber)
        {
            return Waves[waveNumber];
        }
        public IReadOnlyList<EnemyPlacement> GetEnemyPlacementList(int waveNumber)
        {
            return Waves[waveNumber].EnemyPlacements;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices 
{
    [CreateAssetMenu(fileName = "WD_NewWaveData", menuName = "CardsAndDices/WaveSystem/WaveData")]
    public class WaveData : ScriptableObject
    {
        [SerializeField] private int _waveNumber;
        [SerializeField] private List<EnemyPlacement> _enemyPlacements;

        public int WaveNumber => _waveNumber;
        public IReadOnlyList<EnemyPlacement> EnemyPlacements => _enemyPlacements;
    }
}

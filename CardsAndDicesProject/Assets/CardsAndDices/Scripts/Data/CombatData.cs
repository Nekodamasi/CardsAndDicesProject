using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices 
{
    [CreateAssetMenu(fileName = "CD_NewCombatData", menuName = "CardsAndDices/WaveSystem/CombatData")]
    public class CombatData : ScriptableObject
    {
        [SerializeField] private string _combatId;
        [SerializeField] private List<WaveData> _waves;
        [SerializeField] private float _waveInterval = 5.0f;
        [SerializeField] private bool _isInfinite;

        public string CombatId => _combatId;
        public IReadOnlyList<WaveData> Waves => _waves;
        public float WaveInterval => _waveInterval;
        public bool IsInfinite => _isInfinite;
    }
}

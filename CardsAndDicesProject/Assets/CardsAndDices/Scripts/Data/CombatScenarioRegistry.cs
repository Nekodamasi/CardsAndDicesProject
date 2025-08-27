using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "CombatScenarioRegistry", menuName = "CardsAndDices/WaveSystem/CombatScenarioRegistry")]
    public class CombatScenarioRegistry : ScriptableObject
    {
        [SerializeField] private List<CombatScenarioEntry> _combatScenarios;

        public IReadOnlyList<CombatScenarioEntry> CombatScenarios => _combatScenarios;
    }

    [Serializable]
    public class CombatScenarioEntry
    {
        [SerializeField] private string _scenarioName;
        [SerializeField] private AreaId _area;
        [SerializeField] private ChallengeRating _challenge;
        [SerializeField] private CombatData _combatDataAsset;

        public string ScenarioName => _scenarioName;
        public AreaId Area => _area;
        public ChallengeRating Challenge => _challenge;
        public CombatData CombatDataAsset => _combatDataAsset;
    }
}

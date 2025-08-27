using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class CombatDataLoaderService
    {
        private readonly CombatScenarioRegistry _combatScenarioRegistry;

        [Inject]
        public CombatDataLoaderService(CombatScenarioRegistry combatScenarioRegistry)
        {
            _combatScenarioRegistry = combatScenarioRegistry;
        }

        public CombatData GetCombatData(string scenarioName)
        {
            if (_combatScenarioRegistry == null)
            {
                Debug.LogError("CombatScenarioRegistry is not assigned.");
                return null;
            }

            var entry = _combatScenarioRegistry.CombatScenarios.FirstOrDefault(e => e.ScenarioName == scenarioName);
            if (entry == null)
            {
                Debug.LogWarning($"CombatData for scenario '{scenarioName}' not found in registry.");
                return null;
            }
            return entry.CombatDataAsset;
        }

        public CombatData GetCombatData(AreaId area, ChallengeRating challenge)
        {
            if (_combatScenarioRegistry == null)
            {
                Debug.LogError("CombatScenarioRegistry is not assigned.");
                return null;
            }

            var entry = _combatScenarioRegistry.CombatScenarios.FirstOrDefault(e => e.Area == area && e.Challenge == challenge);
            if (entry == null)
            {
                Debug.LogWarning($"CombatData for Area '{area}' and Challenge '{challenge}' not found in registry.");
                return null;
            }
            return entry.CombatDataAsset;
        }

        public CombatData GetRandomCombatData(AreaId area, ChallengeRating challenge)
        {
            if (_combatScenarioRegistry == null)
            {
                Debug.LogError("CombatScenarioRegistry is not assigned.");
                return null;
            }

            var matchingEntries = _combatScenarioRegistry.CombatScenarios
                .Where(e => e.Area == area && e.Challenge == challenge)
                .ToList();

            if (matchingEntries.Count == 0)
            {
                Debug.LogWarning($"No CombatData found for Area '{area}' and Challenge '{challenge}'.");
                return null;
            }

            var randomIndex = UnityEngine.Random.Range(0, matchingEntries.Count);
            return matchingEntries[randomIndex].CombatDataAsset;
        }
    }
}

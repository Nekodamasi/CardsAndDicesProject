using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardsAndDices 
{
    public class WaveGeneratorService
    {
        /// <summary>
        /// WaveGeneratorが出力するFixedCardInitializerのコンテキストが含まれます。
        /// </summary>
        public class WaveGeneratorContext
        {
            public FixedCardInitializer FixedCardInitializer;
            public LinePosition LinePosition;
            public SlotLocation SlotLocation;
        }
        public List<WaveGeneratorContext> GenerateWaveEnemies(CombatData combatData, int waveNumber)
        {
            if (combatData == null)
            {
                Debug.LogError("CombatData is null.");
                return new List<WaveGeneratorContext>();
            }

            var waveData = combatData.Waves.FirstOrDefault(w => w.WaveNumber == waveNumber);
            if (waveData == null)
            {
                Debug.LogError($"WaveData for wave number {waveNumber} not found in CombatData '{combatData.name}'.");
                return new List<WaveGeneratorContext>();
            }

            var result = new List<WaveGeneratorContext>();
            foreach (var placement in waveData.EnemyPlacements)
            {
                if (placement.EnemyGroup == null || placement.EnemyGroup.EnemyProfiles.Count == 0)
                {
                    Debug.LogWarning($"EnemyGroup is null or empty for a placement in Wave {waveNumber}.");
                    continue;
                }

                // Simple random selection for now. Can be expanded with PowerLevel weighting.
                var randomIndex = Random.Range(0, placement.EnemyGroup.EnemyProfiles.Count);
                var selectedProfile = placement.EnemyGroup.EnemyProfiles[randomIndex];

                if (selectedProfile.FixedCardInitializer == null)
                {
                    Debug.LogWarning($"Selected EnemyProfile '{selectedProfile.name}' has no FixedCardInitializer.");
                    continue;
                }
                var context = new WaveGeneratorContext();
                context.FixedCardInitializer = selectedProfile.FixedCardInitializer;
                context.LinePosition = placement.Position;
                context.SlotLocation = placement.Location;
                result.Add(context);
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "CombatScenarioRegistry", menuName = "CardsAndDices/Combats/Data/Waves/CombatScenarioRegistry")]
    /// <summary>
    /// 全てコンバットデータを管理するレジストリクラス
    /// </summary>
    public class CombatScenarioRegistry : ScriptableObject
    {
        [Header("Components")]
        [SerializeField] private List<CombatScenarioEntry> _combatScenarios;

        public IReadOnlyList<CombatScenarioEntry> CombatScenarios => _combatScenarios;

        /// <summary>
        /// ダイスを一意に識別するID。
        /// </summary>
        public CombatData GetCombatData(WaveAreaId areaId, ChallengeRating challenge)
        {
            var list = CombatScenarios.Where(s => s.Area == areaId && s.Challenge == challenge).ToList();
            var num = new System.Random().Next(0, list.Count);
            return list[num].CombatDataAsset;
        }
    }

    [Serializable]
    public class CombatScenarioEntry
    {
        [SerializeField] private string _scenarioName;
        [SerializeField] private WaveAreaId _area;
        [SerializeField] private ChallengeRating _challenge;
        [SerializeField] private CombatData _combatDataAsset;

        public string ScenarioName => _scenarioName;
        public WaveAreaId Area => _area;
        public ChallengeRating Challenge => _challenge;
        public CombatData CombatDataAsset => _combatDataAsset;
    }
}

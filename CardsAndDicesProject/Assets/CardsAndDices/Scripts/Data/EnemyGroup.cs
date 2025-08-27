using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices 
{
    [CreateAssetMenu(fileName = "EG_NewEnemyGroup", menuName = "CardsAndDices/WaveSystem/EnemyGroup")]
    public class EnemyGroup : ScriptableObject
    {
        [SerializeField] private string _groupName;
        [SerializeField] private List<EnemyProfile> _enemyProfiles;

        public string GroupName => _groupName;
        public IReadOnlyList<EnemyProfile> EnemyProfiles => _enemyProfiles;
    }
}

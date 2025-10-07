using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices 
{
    [CreateAssetMenu(fileName = "EneGrp_", menuName = "CardsAndDices/Combats/Data/Waves/EnemyGroup")]
    /// <summary>
    /// １つの配置場所に対するランダム性を持たせるためのグループ
    /// </summary>
    public class EnemyGroup : ScriptableObject
    {
        [SerializeField] private string _groupName;
        [SerializeField] private List<EnemyProfile> _enemyProfiles;

        public string GroupName => _groupName;
        public IReadOnlyList<EnemyProfile> EnemyProfiles => _enemyProfiles;
        public EnemyProfile EnemyProfile => EnemyProfiles[new System.Random().Next(0, EnemyProfiles.Count)];
    }
}

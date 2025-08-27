using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "EP_NewEnemyProfile", menuName = "CardsAndDices/WaveSystem/EnemyProfile")]
    public class EnemyProfile : ScriptableObject
    {
        [SerializeField] private FixedCardInitializer _fixedCardInitializer;
        [SerializeField] private AreaId _areaId;
        [SerializeField] private ChallengeRating _challengeRating;
        [SerializeField] private EnemyRoleId _enemyRoleId;
        [SerializeField] private int _powerLevel = 1;

        public FixedCardInitializer FixedCardInitializer => _fixedCardInitializer;
        public AreaId AreaId => _areaId;
        public ChallengeRating ChallengeRating => _challengeRating;
        public EnemyRoleId EnemyRoleId => _enemyRoleId;
        public int PowerLevel => _powerLevel;
    }
}
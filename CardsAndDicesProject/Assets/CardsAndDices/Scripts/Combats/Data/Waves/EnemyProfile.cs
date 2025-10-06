using UnityEngine;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "EnePro_", menuName = "CardsAndDices/Combats/Data/Waves/EnemyProfile")]

    /// <summary>
    /// エネミー１つ分のデータを格納したprofileクラス
    /// </summary>
    public class EnemyProfile : ScriptableObject
    {
        [SerializeField] private FixedCardInitializer _fixedCardInitializer;
        [SerializeField] private ChallengeRating _challengeRating;
        [SerializeField] private EnemyRoleId _enemyRoleId;
        [SerializeField] private int _powerLevel = 1;

        public FixedCardInitializer FixedCardInitializer => _fixedCardInitializer;
        public ChallengeRating ChallengeRating => _challengeRating;
        public EnemyRoleId EnemyRoleId => _enemyRoleId;
        public int PowerLevel => _powerLevel;
    }
}
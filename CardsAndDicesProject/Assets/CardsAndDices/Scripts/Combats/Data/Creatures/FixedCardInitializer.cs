using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 固定情報からCardInitializationDataを生成するためのScriptableObject。
    /// エネミーや召喚クリーチャーなど、インスペクターで設定されたデータに基づいてクリーチャーカードを初期化するのに使用します。
    /// </summary>
    [CreateAssetMenu(fileName = "FCInit_", menuName = "CardsAndDices/Combats/Creatures/FixedCardInitializer")]
    public class FixedCardInitializer : ScriptableObject
    {
        [SerializeField] private CreatureIdEntity _creatureId;
        [SerializeField] private int _attack;
        [SerializeField] private int _health;
        [SerializeField] private int _shield;
        [SerializeField] private int _cooldown;
        [SerializeField] private int _energy;
        [SerializeField] private List<AbilityDataEntity> _abilities = new List<AbilityDataEntity>();

        [Header("Appearance")]
        [SerializeField] private AppearanceProfile _appearanceProfile;

        // ダイスインレット1のデータ
        [Header("Inlet 1 Data")]
        [SerializeField] private InletProfileIdEntity _inlet1ProfileId;
        [SerializeField] private CompositeObjectIdTypeEntity inlet1Category;
        [SerializeField] private List<AbilityDataEntity> _inlet1RareAbilities = new List<AbilityDataEntity>();
        [SerializeField] private List<AbilityDataEntity> _inlet1LegendAbilities = new List<AbilityDataEntity>();

        // ダイスインレット2のデータ
        [Header("Inlet 2 Data")]
        [SerializeField] private InletProfileIdEntity _inlet2ProfileId;
        [SerializeField] private CompositeObjectIdTypeEntity inlet2Category;
        [SerializeField] private List<AbilityDataEntity> _inlet2RareAbilities = new List<AbilityDataEntity>();
        [SerializeField] private List<AbilityDataEntity> _inlet2LegendAbilities = new List<AbilityDataEntity>();

        [Header("Main Attack Data")]
        [SerializeField] private EffectTargetType _mainAttackScoresType = EffectTargetType.Attack;

        [SerializeField] private int _hitsPerMainAttack = 1;

        [SerializeField] private AreaOfEffect _mainAttackAoE = AreaOfEffect.HostileCreature;

        /// <summary>
        /// インスペクターで設定されたデータに基づいてCardInitializationDataを生成します。
        /// </summary>
        /// <returns>生成されたCardInitializationData。</returns>
        public CardInitializationData CreateCardInitializationData(Team creatureDataTeam)
        {
            // CreatureDataのインスタンスを生成
            CreatureData creatureData = new CreatureData(
                _creatureId,
                _attack,
                _health,
                _shield,
                _cooldown,
                _energy,
                _abilities,
                _mainAttackScoresType,
                _hitsPerMainAttack,
                _mainAttackAoE
            );

            // InletAbilityProfileのリストを生成
            List<InletPackageProfile> inletPackageProfiles = new List<InletPackageProfile>();

            // インレット1のデータが存在すればリストに追加
            if (inlet1Category != null)
//            if (_inlet1Condition != null && _inlet1Ability != null)
            {
                inletPackageProfiles.Add(new InletPackageProfile(_inlet1ProfileId, inlet1Category, _inlet1RareAbilities, _inlet1LegendAbilities));
            }

            // インレット2のデータが存在すればリストに追加
            if (inlet2Category != null)
            {
                inletPackageProfiles.Add(new InletPackageProfile(_inlet2ProfileId, inlet2Category, _inlet2RareAbilities, _inlet2LegendAbilities));
            }
//            Debug.Log("<color=Green>インレットプロフィール：</color>" + inletAbilityProfiles.Count);
            // CardInitializationDataのインスタンスを生成して返す
            return new CardInitializationData(creatureData, inletPackageProfiles, _appearanceProfile, creatureDataTeam);
        }
    }
}

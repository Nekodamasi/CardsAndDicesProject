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
        [SerializeField] private List<BaseAbilityDataSO> _abilities = new List<BaseAbilityDataSO>();

        [Header("Appearance")]
        [SerializeField] private AppearanceProfile _appearanceProfile;

        // ダイスインレット1のデータ
        [Header("Inlet 1 Data")]
        [SerializeField] private InletProfileIdEntity _inlet1ProfileId;
        [SerializeField] private DiceInletConditionSO _inlet1Condition;
        [SerializeField] private List<BaseAbilityDataSO> _inlet1Abilities = new List<BaseAbilityDataSO>();

        // ダイスインレット2のデータ
        [Header("Inlet 2 Data")]
        [SerializeField] private InletProfileIdEntity _inlet2ProfileId;
        [SerializeField] private DiceInletConditionSO _inlet2Condition;
       [SerializeField] private List<BaseAbilityDataSO> _inlet2Abilities = new List<BaseAbilityDataSO>();

        [Header("Main Attack Data")]
        [SerializeField] private EffectTargetType _mainAttackScoresType = EffectTargetType.Attack;

        [SerializeField] private int _hitsPerMainAttack = 1;

        [SerializeField] private AreaOfEffect _mainAttackAoE = AreaOfEffect.HostileCreature;

        /// <summary>
        /// インスペクターで設定されたデータに基づいてCardInitializationDataを生成します。
        /// </summary>
        /// <returns>生成されたCardInitializationData。</returns>
        public CardInitializationData CreateCardInitializationData()
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
            List<InletAbilityProfile> inletAbilityProfiles = new List<InletAbilityProfile>();

            // インレット1のデータが存在すればリストに追加
            if (_inlet1Condition != null)
//            if (_inlet1Condition != null && _inlet1Ability != null)
            {
                inletAbilityProfiles.Add(new InletAbilityProfile(_inlet1ProfileId, _inlet1Condition, _inlet1Abilities));
            }

            // インレット2のデータが存在すればリストに追加
            if (_inlet2Condition != null)
            {
                Debug.Log("ほげほげほげほ：" + _inlet2Abilities.Count);
                inletAbilityProfiles.Add(new InletAbilityProfile(_inlet2ProfileId, _inlet2Condition, _inlet2Abilities));
            }
//            Debug.Log("<color=Green>インレットプロフィール：</color>" + inletAbilityProfiles.Count);
            // CardInitializationDataのインスタンスを生成して返す
            return new CardInitializationData(creatureData, inletAbilityProfiles, _appearanceProfile);
        }
    }
}

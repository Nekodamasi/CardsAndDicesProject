using System.Collections.Generic;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 固定情報からCardInitializationDataを生成するためのScriptableObject。
    /// エネミーや召喚クリーチャーなど、インスペクターで設定されたデータに基づいてクリーチャーカードを初期化するのに使用します。
    /// </summary>
    [CreateAssetMenu(fileName = "FixedCardInitializer", menuName = "CardsAndDices/CardInitializers/FixedCardInitializer")]
    public class FixedCardInitializer : ScriptableObject
    {
        [SerializeField] private string _creatureId;
        [SerializeField] private int _attack;
        [SerializeField] private int _health;
        [SerializeField] private int _shield;
        [SerializeField] private int _cooldown;
        [SerializeField] private int _energy;
        [SerializeField] private List<BaseAbilityDataSO> _abilities = new List<BaseAbilityDataSO>(); // 初期化

        [Header("Appearance")]
        [SerializeField] private AppearanceProfile _appearanceProfile;

        // ダイスインレット1のデータ
        [Header("Inlet 1 Data")]
        [SerializeField] private DiceInletConditionSO _inlet1Condition;
        [SerializeField] private BaseInletAbilitySO _inlet1Ability;

        // ダイスインレット2のデータ
        [Header("Inlet 2 Data")]
        [SerializeField] private DiceInletConditionSO _inlet2Condition;
        [SerializeField] private BaseInletAbilitySO _inlet2Ability;

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
                _abilities
            );

            // InletAbilityProfileのリストを生成
            List<InletAbilityProfile> inletAbilityProfiles = new List<InletAbilityProfile>();

            // インレット1のデータが存在すればリストに追加
            if (_inlet1Condition != null)
//            if (_inlet1Condition != null && _inlet1Ability != null)
            {
                inletAbilityProfiles.Add(new InletAbilityProfile(_inlet1Condition, _inlet1Ability));
            }

            // インレット2のデータが存在すればリストに追加
            if (_inlet2Condition != null)
            {
                inletAbilityProfiles.Add(new InletAbilityProfile(_inlet2Condition, _inlet2Ability));
            }
            Debug.Log("<color=Green>インレットプロフィール：</color>" + inletAbilityProfiles.Count);
            // CardInitializationDataのインスタンスを生成して返す
            return new CardInitializationData(creatureData, inletAbilityProfiles, _appearanceProfile);
        }
    }
}

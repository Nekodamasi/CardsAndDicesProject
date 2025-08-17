using System.Collections.Generic;
using VContainer;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// プレイヤーの現在の状態（装備、キャラクターデータなど）から、
    /// カード生成システムが利用するCardInitializationDataを生成する責務を負います。
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerCardDataProvider", menuName = "CardsAndDices/Services/PlayerCardDataProvider")]
    public class PlayerCardDataProvider : ScriptableObject, ICardDataProvider
    {
        [Inject]
        public void Initialize()
        {
        }

        [SerializeField] private List<FixedCardInitializer> _fixedCardInitializerList = new List<FixedCardInitializer>();

        // TODO: プレイヤーのキャラクター情報や装備データを参照するフィールドを追加
        // [Inject] private PlayerProfile _playerProfile; のような形でDIする

        /// <summary>
        /// プレイヤーの現在の状態から、生成すべきカードの初期化データリストを取得します。
        /// </summary>
        /// <returns>プレイヤーのカード初期化データを含むリスト。</returns>
        public List<CardInitializationData> GetCardDataList()
        {
            var list = new List<CardInitializationData>();

            for (var i = 0; i < _fixedCardInitializerList.Count; i++)
            {
                list.Add(_fixedCardInitializerList[i].CreateCardInitializationData());
            }

            // TODO: _playerProfileからキャラクター情報を読み出し、
                // それぞれに対応するCreatureDataとInletAbilityProfileを生成し、
                // CardInitializationDataを作成してリストに追加するロジックを実装
                // 例:
                // var creatureData = new CreatureData(...);
                // var inletProfiles = new List<InletAbilityProfile>();
                // inletProfiles.Add(new InletAbilityProfile(...));
                // list.Add(new CardInitializationData(creatureData, inletProfiles));

            return list;
        }
    }
}

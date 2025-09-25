using System.Collections.Generic;
using VContainer;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// プレイヤーの現在の状態（装備、キャラクターデータなど）から、
    /// カード生成システムが利用するCardInitializationDataを生成する責務を負います。
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerCardDataProvider", menuName = "CardsAndDices/Combats/Managers/Players/PlayerCardDataProvider")]
    public class PlayerCardDataProvider : ScriptableObject, ICardDataProvider
    {
        [Inject]
        public void Initialize()
        {
        }

        [SerializeField] private List<FixedCardInitializer> _fixedCardInitializerList = new List<FixedCardInitializer>();

        /// <summary>
        /// カードコンテナのリストを返します
        /// </summary>
        public List<CardInitializationData> GetCardDataList()
        {
            var list = new List<CardInitializationData>();

            for (var i = 0; i < _fixedCardInitializerList.Count; i++)
            {
                list.Add(_fixedCardInitializerList[i].CreateCardInitializationData());
            }

            return list;
        }
    }
}

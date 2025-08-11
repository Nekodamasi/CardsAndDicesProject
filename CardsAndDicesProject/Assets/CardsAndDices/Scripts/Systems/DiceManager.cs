using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイスの生成、状態管理、リロールなどを一元的に行うマネージャークラス。
    /// </summary>
    public class DiceManager : MonoBehaviour
    {
        [Inject] private readonly CompositeObjectIdManager _objectIdManager;
        [Inject] private readonly ViewRegistry _viewRegistry;

        [SerializeField] private DiceView _diceViewPrefab;
        [SerializeField] private Transform _diceSpawnPoint;
        [SerializeField] private int _initialDiceCount = 5;

        private readonly List<DiceData> _diceList = new();

        private void Start()
        {
            InitializeDices(_initialDiceCount);
        }

        /// <summary>
        /// 指定された数のダイスを初期化し、生成します。
        /// </summary>
        /// <param name="count">生成するダイスの数。</param>
        public void InitializeDices(int count)
        {
/*
            for (int i = 0; i < count; i++)
            {
                var objectId = _objectIdManager.CreateNewId(ObjectType.Dice);
                var diceData = new DiceData(objectId);
                _diceList.Add(diceData);

                var diceView = Instantiate(_diceViewPrefab, _diceSpawnPoint.position, Quaternion.identity, _diceSpawnPoint);
                diceView.Initialize(objectId);
                diceView.UpdateView(diceData);
                _viewRegistry.Register(diceView);
            }
*/
        }

        /// <summary>
        /// 全てのダイスをリロール（再抽選）します。
        /// </summary>
        public void RerollAllDices()
        {
/*
            foreach (var diceData in _diceList)
            {
                diceData.Roll();
                if (_viewRegistry.GetView(diceData.UniqueId) is DiceView diceView)
                {
                    diceView.UpdateView(diceData);
                }
            }
*/
        }

        /// <summary>
        /// 指定されたIDのダイスデータを取得します。
        /// </summary>
        /// <param name="id">取得するダイスのID。</param>
        /// <returns>見つかったダイスのデータ。見つからない場合はnull。</returns>
        public DiceData GetDiceData(CompositeObjectId id)
        {
            return _diceList.FirstOrDefault(d => d.UniqueId == id);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイスの生成、状態管理、リロールなどを一元的に行うマネージャークラス。
    /// ScriptableObjectとして、ダイスのデータ管理に特化します。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceManager", menuName = "CardsAndDices/Systems/DiceManager")]
    public class DiceManager : ScriptableObject
    {
        private CompositeObjectIdManager _objectIdManager;
        private ViewRegistry _viewRegistry;
        private readonly List<DiceData> _diceList = new();


        /// <summary>
        /// DiceManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(CompositeObjectIdManager objectIdManager, ViewRegistry viewRegistry)
        {
            _objectIdManager = objectIdManager;
            _viewRegistry = viewRegistry;
            _diceList.Clear(); // 初期化時にリストをクリア
        }

        /// <summary>
        /// 新しいダイスデータを生成し、管理リストに追加します。
        /// ダイスのViewの生成は別のクラス（例: DiceSpawner）が担当します。
        /// </summary>
        /// <param name="diceData">追加するダイスのデータ。</param>
        public void AddDice(DiceData diceData)
        {
            _diceList.Add(diceData);
        }

        /// <summary>
        /// 指定されたIDのダイスを管理リストから削除します。
        /// </summary>
        /// <param name="id">削除するダイスのID。</param>
        public void RemoveDice(CompositeObjectId id)
        {
            _diceList.RemoveAll(d => d.Id == id);
        }

        /// <summary>
        /// 全てのダイスをリロール（再抽選）します。
        /// </summary>
        public void RerollAllDices()
        {
            foreach (var diceData in _diceList)
            {
                diceData.Roll();
                // Viewの更新はDicePresenterがイベントを購読して自動的に行うため、ここでの直接操作は不要
            }
        }

        /// <summary>
        /// 指定されたIDのダイスデータを取得します。
        /// </summary>
        /// <param name="id">取得するダイスのID。</param>
        /// <returns>見つかったダイスのデータ。見つからない場合はnull。</returns>
        public DiceData GetDiceData(CompositeObjectId id)
        {
            return _diceList.FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// 管理している全てのダイスデータを取得します。
        /// </summary>
        /// <returns>ダイスデータのリスト。</returns>
        public IReadOnlyList<DiceData> GetAllDiceData()
        {
            return _diceList;
        }
    }
}

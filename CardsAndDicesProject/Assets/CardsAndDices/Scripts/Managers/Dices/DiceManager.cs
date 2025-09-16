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
    [CreateAssetMenu(fileName = "DiceManager", menuName = "CardsAndDices/Managers/DiceManager")]
    public class DiceManager : ScriptableObject
    {
        private IdentifiableViewRegistry _viewRegistry;
        private IdentifiableCommandBus _identifiableCommandBus;
        private readonly List<DiceInstance> _diceInstances = new();
        private readonly List<DicePresenter> _dicePresenters = new();

        /// <summary>
        /// DiceManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(IdentifiableCommandBus identifiableCommandBus, IdentifiableViewRegistry viewRegistry)
        {
            _identifiableCommandBus = identifiableCommandBus;
            _viewRegistry = viewRegistry;
            _identifiableCommandBus.On<SceneLoadedCommand>(OnSceneLoaded);
        }
        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _diceInstances)
            {
                instance.Dispose();
            }
            _diceInstances.Clear();
        }

        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var presenter in _dicePresenters)
            {
                presenter.Dispose();
            }
            _dicePresenters.Clear();
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            _identifiableCommandBus.Off<SceneLoadedCommand>(OnSceneLoaded);
        }

        /// <summary>
        /// ダイススロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedCommand cmd)
        {
            Dispose();
/*
            foreach (var diceSlotPositionEntity in _diceSlotPositionEntities)
            {
                var instance = new DiceSlotInstance(diceSlotPositionEntity, _identifiableCommandBus);
                _diceSlotInstances.Add(instance);
                _iceSlotControllers.Add(new DiceSlotController(instance, _identifiableCommandBus));
            }
*/
        }

        /// <summary>
        /// 新しいダイスデータを生成し、管理リストに追加します。
        /// ダイスのViewの生成は別のクラス（例: DiceSpawner）が担当します。
        /// </summary>
        /// <param name="diceData">追加するダイスのデータ。</param>
        public void AddDice(int diceId, int faceValue)
        {
            var DiceInstance = new DiceInstance(diceId, faceValue);
            _diceInstances.Add(DiceInstance);
        }

        /// <summary>
        /// 指定されたIDのダイスを管理リストから削除します。
        /// </summary>
        /// <param name="id">削除するダイスのID。</param>
        public void RemoveDice(int id)
        {
            _diceInstances.RemoveAll(d => d.Id == id);
        }
    }
}

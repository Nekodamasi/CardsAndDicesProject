using System.Collections.Generic;
using System;
using UnityEngine;
using VContainer;
using UnityEditor.Search;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイスの生成、状態管理、リロールなどを一元的に行うマネージャークラス。
    /// ScriptableObjectとして、ダイスのデータ管理に特化します。
    /// </summary>
    [CreateAssetMenu(fileName = "DiceManager", menuName = "CardsAndDices/Managers/DiceManager")]
    public class DiceManager : ScriptableObject, IDisposable
    {
        private class AddDice
        {
            public int DiceId;
            public int FaceValue;
            public AddDice(int id, int Value)
            {
                DiceId = id;
                FaceValue = Value;
            }
        }
        private IdentifiableViewRegistry _viewRegistry;
        private IdentifiableCommandBus _identifiableCommandBus;
        private readonly List<DiceInstance> _diceInstances = new();
        private readonly List<DicePresenter> _dicePresenters = new();
        private readonly List<AddDice> _addDices = new();
        private int _incrementId;

        /// <summary>
        /// DiceManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(IdentifiableCommandBus identifiableCommandBus, IdentifiableViewRegistry viewRegistry)
        {
            _identifiableCommandBus = identifiableCommandBus;
            _viewRegistry = viewRegistry;
            _incrementId = 1;
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
            SetUpAddDices();
            DiceRoll();
        }

        /// <summary>
        /// ダイスロールを行います
        /// </summary>
        private void DiceRoll()
        {
            foreach (var dice in _addDices)
            {
                var instance = CreateDiceInstance(dice.DiceId, dice.FaceValue);
                CreateDicePresenter(instance);
            }
            _addDices.Clear();
        }

        /// <summary>
        /// ユーザーのダイス数から追加するダイスを作成します
        /// </summary>
        private void SetUpAddDices()
        {
            for (var i = 0; i < 2; i++)
            {
                _addDices.Add(new AddDice(_incrementId++, -1));
            }
        }

        /// <summary>
        /// 新しいDiceInstanceを生成し、管理リストに追加します。
        /// </summary>
        /// <param name="diceData">追加するダイスのデータ。</param>
        /// <param name="faceValue">追加するダイスの目。</param>
        public DiceInstance CreateDiceInstance(int diceId, int faceValue)
        {
            var diceInstance = new DiceInstance(diceId, faceValue);
            _diceInstances.Add(diceInstance);
            return diceInstance;
        }

        /// <summary>
        /// 新しいDicePresenterを生成して管理リストに追加します
        /// </summary>
        /// <param name="diceInstance">追加するDiceInstance。</param>
        private DicePresenter CreateDicePresenter(DiceInstance diceInstance)
        {
            var view = _viewRegistry.GetNonBoundView<DiceView>();
            Debug.Log("CreateDicePresenter:" + "view->" + view.CompositeObjectId + " diceInstance->" + diceInstance.FaceValue);
            view.SetBoundState(true);
            var presenter = new DicePresenter(diceInstance, view, _identifiableCommandBus);
            _dicePresenters.Add(presenter);
            return presenter;
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

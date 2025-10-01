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
    [CreateAssetMenu(fileName = "DiceManager", menuName = "CardsAndDices/Combats/Managers/Dices/DiceManager")]
    public class DiceManager : ScriptableObject, IDisposable
    {
        private class AddDice
        {
            public int FaceValue;
            public AddDice(int Value)
            {
                FaceValue = Value;
            }
        }

        [Header("Components")]
        [SerializeField] private CompositeObjectIdTypeEntity _objectType;
        private IdentifiableViewRegistry _viewRegistry;
        private GameEventBus _eventBus;
        private DiceSlotManager _diceSlotManager;
        private readonly List<DiceInstance> _diceInstances = new();
        private readonly List<DicePresenter> _dicePresenters = new();
        private readonly List<AddDice> _addDices = new();
        //        private int _incrementId;

        /// <summary>
        /// DiceManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(GameEventBus eventBus, IdentifiableViewRegistry viewRegistry, DiceSlotManager diceSlotManager)
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus = eventBus;
            _viewRegistry = viewRegistry;
            _diceSlotManager = diceSlotManager;

            //_incrementId = 1;
            _eventBus.On<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.On<CombatPhaseDiceRollEvent>(OnCombatPhaseDiceRoll);
            
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
            _eventBus.Off<SceneLoadedEvent>(OnSceneLoaded);
            _eventBus.Off<CombatPhaseDiceRollEvent>(OnCombatPhaseDiceRoll);
        }

        /// <summary>
        /// ダイススロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedEvent evt)
        {
            SetUpAddDices();
        }

        /// <summary>
        /// ダイススロットポジションエンティティからインスタンスを生成します。
        /// </summary>
        private void OnCombatPhaseDiceRoll(CombatPhaseDiceRollEvent evt)
        {
            DiceRoll();
        }

        /// <summary>
        /// ダイスロールを行います
        /// </summary>
        private void DiceRoll()
        {
            foreach (var dice in _addDices)
            {
                var view = _viewRegistry.GetNonBoundView<DiceView>();
                view.SetBoundState(true);
                var instance = CreateDiceInstance(view.CompositeObjectId, dice.FaceValue);
                var Presenter = CreateDicePresenter(instance, view);
                _diceSlotManager.PlacedDice(Presenter.CompositeObjectId);
                _eventBus.Emit(new SetCurrentHomeStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Normal));
                _eventBus.Emit(new ChangeViewStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Normal));
                _eventBus.Emit(new DisplayStatusViewEvent(instance.CompositeObjectId));
                _eventBus.Emit(new DisplayOnScreenEvent(view.CompositeObjectId));
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
                _addDices.Add(new AddDice(-1));
            }
        }

        /// <summary>
        /// 新しいDiceInstanceを生成し、管理リストに追加します。
        /// </summary>
        /// <param name="diceData">追加するダイスのデータ。</param>
        /// <param name="faceValue">追加するダイスの目。</param>
        public DiceInstance CreateDiceInstance(CompositeObjectId diceId, int faceValue)
        {
            var diceInstance = new DiceInstance(diceId, faceValue, _diceSlotManager);
            _diceInstances.Add(diceInstance);
            return diceInstance;
        }

        /// <summary>
        /// 新しいDicePresenterを生成して管理リストに追加します
        /// </summary>
        /// <param name="diceInstance">追加するDiceInstance。</param>
        private DicePresenter CreateDicePresenter(DiceInstance diceInstance, DiceView view)
        {
            view.SetBoundState(true);
            var presenter = new DicePresenter(diceInstance, view, _eventBus);
            _dicePresenters.Add(presenter);
            return presenter;
        }

        /// <summary>
        /// 指定されたIDのダイスを管理リストから削除します。
        /// </summary>
        /// <param name="id">削除するダイスのID。</param>
        public void RemoveDice(CompositeObjectId id)
        {
            _diceInstances.RemoveAll(d => d.CompositeObjectId == id);
        }
    }
}

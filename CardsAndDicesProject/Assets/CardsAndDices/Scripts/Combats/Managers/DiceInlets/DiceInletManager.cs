using System.Collections.Generic;
using System;
using UnityEngine;
using VContainer;
using UnityEditor.Search;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイスインレットを一元管理するマネージャークラス
    /// </summary>
    [CreateAssetMenu(fileName = "DiceInletManager", menuName = "CardsAndDices/Combats/Managers/DiceInlets/DiceInletManager")]
    public class DiceInletManager : ScriptableObject, IDisposable
    {
        [Header("Components")]
        [SerializeField] private CompositeObjectIdTypeEntity _acceptableTargetObjectType;
        private IdentifiableViewRegistry _viewRegistry;
        private GameEventBus _eventBus;
        private readonly List<DiceInletInstance> _instances = new();
        private readonly List<DiceInletPresenter> _presenters = new();

        /// <summary>
        /// DiceManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(GameEventBus eventBus, IdentifiableViewRegistry viewRegistry)
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus = eventBus;
            _viewRegistry = viewRegistry;

            _eventBus.On<CreateDiceInletEvent>(OnCreateDiceInlet);
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _instances)
            {
                instance.Dispose();
            }
            _instances.Clear();
        }

        /// <summary>
        /// PresenterをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Dispose();
            }
            _presenters.Clear();
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus.Off<CreateDiceInletEvent>(OnCreateDiceInlet);
        }

        /// <summary>
        /// ダイスインレットのインスタンス化を行う
        /// </summary>
        private void OnCreateDiceInlet(CreateDiceInletEvent evt)
        {
            CreateDiceInletInstance(evt.CreatureCardId, evt.InletPackageProfile);
        }

        private void CreateDiceInletInstance(CompositeObjectId ownerid, InletPackageProfile inletPackageProfile)
        {
            var view = _viewRegistry.GetOwnerAndObjectTypeView<DiceInletView>(ownerid, inletPackageProfile.InletCategory);
            if (view is null)
            {
                Debug.LogWarning("びゅーがとれない");
                return;
            }
            var instance = new DiceInletInstance(view.CompositeObjectId, inletPackageProfile);
            _instances.Add(instance);
            Debug.Log("ここが２回よばえてたりする？" + ownerid);
            var presenter = new DiceInletPresenter(instance, view, _eventBus, _acceptableTargetObjectType);
            _presenters.Add(presenter);
            _eventBus.Emit(new SetCurrentHomeStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new ChangeViewStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new DisplayStatusViewEvent(instance.CompositeObjectId));
            
        }
    }
}

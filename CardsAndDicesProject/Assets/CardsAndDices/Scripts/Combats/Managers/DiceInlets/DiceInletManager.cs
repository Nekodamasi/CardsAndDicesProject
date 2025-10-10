using System.Collections.Generic;
using System;
using UnityEngine;
using VContainer;
using System.Linq;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのダイスインレットを一元管理するマネージャークラス
    /// </summary>
    [CreateAssetMenu(fileName = "DiceInletManager", menuName = "CardsAndDices/Combats/Managers/DiceInlets/DiceInletManager")]
    public class DiceInletManager : ScriptableObject, IDisposable, IIdentifiableManager
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
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
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
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
        }

        /// <summary>
        /// ダイスインレットのインスタンス化を行う
        /// </summary>
        private void OnCreateDiceInlet(CreateDiceInletEvent evt)
        {
            CreateDiceInletInstance(evt.CreatureCardId, evt.InletPackageProfile, evt.CreatureStatusInstance);
        }

        private void CreateDiceInletInstance(CompositeObjectId ownerid, InletPackageProfile inletPackageProfile, CreatureStatusInstance creatureStatusInstance)
        {
            var view = _viewRegistry.GetOwnerAndObjectTypeView<DiceInletView>(ownerid, inletPackageProfile.InletCategory);
            if (view is null)
            {
                Debug.LogWarning("びゅーがとれない");
                return;
            }
            var instance = new DiceInletInstance(view.CompositeObjectId, inletPackageProfile);
            _instances.Add(instance);
            var presenter = new DiceInletPresenter(instance, view, _eventBus, _acceptableTargetObjectType);
            _presenters.Add(presenter);

            // インレットアビリティの追加
            foreach (var ability in inletPackageProfile.InletProfileId.Abilities)
            {
                _eventBus.Emit(new CreateAbilityEvent(instance.CompositeObjectId.Owner, ability, instance.CompositeObjectId, creatureStatusInstance));
            }
            foreach (var ability in inletPackageProfile.RareAbilities)
            {
                _eventBus.Emit(new CreateAbilityEvent(instance.CompositeObjectId.Owner, ability, instance.CompositeObjectId, creatureStatusInstance));
            }
            foreach (var ability in inletPackageProfile.LegendAbilities)
            {
                _eventBus.Emit(new CreateAbilityEvent(instance.CompositeObjectId.Owner, ability, instance.CompositeObjectId, creatureStatusInstance));
            }

            _eventBus.Emit(new SetCurrentHomeStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new ChangeViewStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Inactive));
            _eventBus.Emit(new DisplayStatusViewEvent(instance.CompositeObjectId));
            
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeするイベント
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            DisposeByCompositeObjectId(evt.CompositeObjectId);
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeします
        /// </summary>
        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
            var instance = _instances.Where(i => i.CompositeObjectId == compositeObjectId).FirstOrDefault();
            if (instance is null)
            {
                return;
            }
            instance.Dispose();
            _instances.Remove(instance);
            var presenter = _presenters.Where(p => p.CompositeObjectId == compositeObjectId).FirstOrDefault();
            presenter.Dispose();
            _presenters.Remove(presenter);
        }
    }
}

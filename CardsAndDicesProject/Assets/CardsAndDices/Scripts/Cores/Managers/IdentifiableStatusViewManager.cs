using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全ての識別可能オブジェクトの状態(IdentifiableStatusInstance)を一元管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "IdentifiableStatusViewManager", menuName = "CardsAndDices/UI/Identifiable/ViewManagers/IdentifiableStatusViewManager")]
    public class IdentifiableStatusViewManager : ScriptableObject
    {
        private IdentifiableViewRegistry _registry;
        private IdentifiableStatusManager _instanceManager;
        private GameEventBus _eventBus;
        private readonly List<IdentifiableStatusPresenter> _presenters = new();

        [Inject]
        public void Initialize(IdentifiableViewRegistry registry, IdentifiableStatusManager instanceManager, GameEventBus identifiableCommandBus)
        {
            _registry = registry;
            _instanceManager = instanceManager;
            _eventBus = identifiableCommandBus;
            _eventBus.On<InstanceSetUpedEvent>(OnInstanceSetUped);
        }

        /// <summary>
        /// レジストリに登録された情報からインスタンスを生成します。
        /// </summary>
        private void OnInstanceSetUped(InstanceSetUpedEvent evt)
        {
            SetUpStatusInstances();
        }

        /// <summary>
        /// マネージャーを初期化し、Registryに登録されている全てのオブジェクトのステータスインスタンスを生成します。
        /// </summary>
        private void SetUpStatusInstances()
        {
            Dispose();
            var statusviews = _registry.GetAllStatusViews();
            foreach (var view in statusviews)
            {
                var instance = _instanceManager.GetStatus(view.CompositeObjectId);
                if(instance == null)
                {
                    Debug.LogError("<color=red>IdentifiableStatusManager->instanceがnull</color>");
                }
                _presenters.Add(new IdentifiableStatusPresenter(instance, view, _eventBus));
            }
        }

        private void Dispose()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Dispose();
            }
            _presenters.Clear();
        }            
    }
}

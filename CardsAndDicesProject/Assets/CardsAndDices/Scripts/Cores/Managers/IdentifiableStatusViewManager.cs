using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// 全ての識別可能オブジェクトの状態(IdentifiableStatusInstance)を一元管理するScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "IdentifiableStatusViewManager", menuName = "CardsAndDices/Core/Managers/IdentifiableStatusViewManager")]
    public class IdentifiableStatusViewManager : ScriptableObject
    {
        private IdentifiableViewRegistry _registry;
        private IdentifiableUIStateMachine _identifiableUIStateMachine;
        private GameEventBus _eventBus;
        private readonly List<IdentifiableStatusInstance> _instances = new();
        private readonly List<IdentifiableStatusPresenter> _presenters = new();

        [Inject]
        public void Initialize(IdentifiableViewRegistry registry, IdentifiableUIStateMachine identifiableUIStateMachine, GameEventBus identifiableCommandBus)
        {
            _registry = registry;
            _identifiableUIStateMachine = identifiableUIStateMachine;
            _eventBus = identifiableCommandBus;
            _eventBus.On<SceneLoadedEvent>(OnSceneLoaded);
        }

        /// <summary>
        /// レジストリに登録された情報からインスタンスを生成します。
        /// </summary>
        private void OnSceneLoaded(SceneLoadedEvent evt)
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
                var instance = new IdentifiableStatusInstance(view.CompositeObjectId, _eventBus, _identifiableUIStateMachine);
                view.SetBoundState(true);
                _instances.Add(instance);
                _presenters.Add(new IdentifiableStatusPresenter(instance, view, _eventBus));
            }
        }

        private void Dispose()
        {
            DisposePresenter();
            DisposeInstance();
            _eventBus.Off<SceneLoadedEvent>(OnSceneLoaded);
        }            
        private void DisposePresenter()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Dispose();
            }
            _presenters.Clear();
        }            
        private void DisposeInstance()
        {
            foreach (var instance in _instances)
            {
                instance.Dispose();
            }
            _instances.Clear();
        }            
    }
}

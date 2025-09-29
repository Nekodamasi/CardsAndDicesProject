using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードの状態管理などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "SharedIconElementManager", menuName = "CardsAndDices/Cores/Managers/SharedIconElementManager")]
    public class SharedIconElementManager : ScriptableObject, IDisposable
    {
        [Header("Components")]
        private readonly List<IconStatusInstance> _instances = new();
        private readonly List<SharedIconElementPresenter> _presenters = new();
        private GameEventBus _eventBus;
        private IdentifiableViewRegistry _viewRegistry;

        [Inject]
        public void Initialize(GameEventBus eventBus, IdentifiableViewRegistry viewRegistry)
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus = eventBus;
            _viewRegistry = viewRegistry;
            _eventBus.On<CreateCreatureEvent>(OnCreateCreature);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus.Off<CreateCreatureEvent>(OnCreateCreature);
        }

        /// <summary>
        /// クリーチャーの生成イベント
        /// </summary>
        private void OnCreateCreature(CreateCreatureEvent evt)
        {
            Debug.Log("ここはうごいてる？" + _viewRegistry.GetAllSharedIconElementViews().Count);
            foreach (var iconview in _viewRegistry.GetAllSharedIconElementViews())
            {
                var instance = new IconStatusInstance(iconview.CompositeObjectId, iconview.SharedIconElementTypeEntity, SharedIconElementStatus.Normal);
                _instances.Add(instance);
                var presenter = new SharedIconElementPresenter(instance, iconview, _eventBus);
                _presenters.Add(presenter);
                iconview.SetBoundState(true);
            }
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
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Dispose();
            }
            _presenters.Clear();
        }
   }
}

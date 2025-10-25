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
    [CreateAssetMenu(fileName = "CardAppearanceManager", menuName = "CardsAndDices/Combats/Managers/Cards/CardAppearanceManager")]
    public class CardAppearanceManager : ScriptableObject, IDisposable, IIdentifiableManager
    {
        [Header("Components")]
        private readonly List<CardAppearanceInstance> _instances = new();
        private readonly List<CardAppearancePresenter> _presenters = new();
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
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
            
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus.Off<CreateCreatureEvent>(OnCreateCreature);
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
        }

        /// <summary>
        /// 指定されたIDに紐づくインスタンスをDisposeするイベント
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            DisposeByCompositeObjectId(evt.CompositeObjectId);
        }

        /// <summary>
        /// クリーチャーの生成イベント
        /// </summary>
        private void OnCreateCreature(CreateCreatureEvent evt)
        {
            var instance = new CardAppearanceInstance(evt.CreatureCardId, evt.CardInitializationData.Appearance);
            _instances.Add(instance);
            var view = _viewRegistry.GetView<CardAppearanceView>(evt.CreatureCardId);
            if(view is null)
            {
                Debug.LogWarning("ビューが取得できない");
                return;
            }
            view.SetBoundState(true);
            var presenter = new CardAppearancePresenter(instance, view, _eventBus);
            _presenters.Add(presenter);
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

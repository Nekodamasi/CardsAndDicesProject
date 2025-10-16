using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class CombatStartManager : MonoBehaviour, IDisposable, IIdentifiableManager
    {
        private GameEventBus _eventBus;
        private IdentifiableViewRegistry _viewRegistry;
        private NextTurnBtnView _view;

        [Inject]
        public void Construct(GameEventBus eventBus, IdentifiableViewRegistry viewRegistry)
        {
            _eventBus = eventBus;
            _viewRegistry = viewRegistry;
            var _view = _viewRegistry.GetNonBoundView<NextTurnBtnView>();
            _view.SetBoundState(true);

            _eventBus.On<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
        }

        public void Dispose()
        {
            _eventBus.Off<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
        }

        /// <summary>
        /// Clickされた時のイベント
        /// </summary>
        private void OnIdentifiableStateClick(IdentifiableStateClickEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
        }

        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
        }
    }
}

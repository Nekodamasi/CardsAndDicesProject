using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    public class NextTurnManager : MonoBehaviour, IDisposable, IIdentifiableManager
    {
        private GameEventBus _eventBus;
        private IdentifiableViewRegistry _viewRegistry;
        private NextTurnBtnView _view;
        private NextTurnInstance _instance;
        private DiceManager _diceManager;

        [Inject]
        public void Construct(GameEventBus eventBus, IdentifiableViewRegistry viewRegistry, DiceManager diceManager)
        {
            _eventBus = eventBus;
            _viewRegistry = viewRegistry;
            _diceManager = diceManager;
            _eventBus.On<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
            _eventBus.On<InstanceSetUpedEvent>(OnInstanceSetUped);
            _eventBus.On<CombatPhaseCombatBtnOnScreenEvent>(OnCombatPhaseCombatBtnOnScreen);
            _eventBus.On<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.On<DisplayOffScreenEvent>(OnDisplayOffScreen);
        }

        public void Dispose()
        {
            _eventBus.Off<IdentifiableStateClickEvent>(OnIdentifiableStateClick);
            _eventBus.Off<InstanceSetUpedEvent>(OnInstanceSetUped);
            _eventBus.Off<CombatPhaseCombatBtnOnScreenEvent>(OnCombatPhaseCombatBtnOnScreen);
            _eventBus.Off<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.Off<DisplayOffScreenEvent>(OnDisplayOffScreen);
        }

        /// <summary>
        /// 画面に投げ入れる
        /// </summary>
        private void OnDisplayOnScreen(DisplayOnScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            _view.DisplayOnScreen(new Vector3(0.0f, 0.0f, 0.0f));
        }

        /// <summary>
        /// 画面から退場させる
        /// </summary>
        private void OnDisplayOffScreen(DisplayOffScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            _view.DisplayOffScreen();
        }

        /// <summary>
        /// コンバットボタンの画面配置イベント
        /// </summary>
        private void OnCombatPhaseCombatBtnOnScreen(CombatPhaseCombatBtnOnScreenEvent evt)
        {
            _eventBus.Emit(new SetCurrentHomeStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Normal));
            _eventBus.Emit(new ChangeViewStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Normal));
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
            _eventBus.Emit(new DisplayOnScreenEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// Instanceセットアップ後のイベント
        /// </summary>
        private void OnInstanceSetUped(InstanceSetUpedEvent evt)
        {
            _view = _viewRegistry.GetNonBoundView<NextTurnBtnView>();
            if (_view is null)
            {
                Debug.LogWarning("Viewを取得できない");
            }
            _instance = new NextTurnInstance(_view.CompositeObjectId, _diceManager);
            _view.SetBoundState(true);
        }

        /// <summary>
        /// Clickされた時のイベント
        /// </summary>
        private void OnIdentifiableStateClick(IdentifiableStateClickEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            _eventBus.Emit(new ChangeCombatPhaseEvent(CombatPhase.TurnEndPhase));
            _eventBus.Emit(new CombatPhaseDiceOffScreenEvent());
        }

        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
        }
    }
}

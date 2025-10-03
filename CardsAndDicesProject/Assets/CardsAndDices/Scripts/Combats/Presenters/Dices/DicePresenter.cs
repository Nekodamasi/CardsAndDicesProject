using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// DiceInstance(Model)とDiceView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class DicePresenter : IDisposable, IIdentifiablePresenter
    {
<<<<<<< HEAD:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/Dices/DicePresenter.cs
        private readonly DiceInstance _instance;
        private readonly DiceView _view;
=======
        private readonly CreatureStatusInstance _instance;
        private readonly CreatureCardView _view;
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/CreatureStatusPresenter.cs
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

<<<<<<< HEAD:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/Dices/DicePresenter.cs
        public DicePresenter(DiceInstance instance, DiceView view, GameEventBus eventBus)
=======
        public CreatureStatusPresenter(CreatureStatusInstance instance, CreatureCardView view, GameEventBus eventBus)
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/CreatureStatusPresenter.cs
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
<<<<<<< HEAD:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/Dices/DicePresenter.cs
            _eventBus.On<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.On<DisplayOffScreenEvent>(OnDisplayOffScreen);
            _eventBus.On<IdentifiableDropEvent>(OnIdentifiableDrop);
=======
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/CreatureStatusPresenter.cs
            _eventBus.On<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
<<<<<<< HEAD:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/Dices/DicePresenter.cs
            _eventBus.Off<DisplayOnScreenEvent>(OnDisplayOnScreen);
            _eventBus.Off<DisplayOffScreenEvent>(OnDisplayOffScreen);
            _eventBus.Off<IdentifiableDropEvent>(OnIdentifiableDrop);
=======
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/CreatureStatusPresenter.cs
            _eventBus.Off<IdentifiableStateBeginDragEvent>(OnIdentifiableStateBeginDrag);
        }

        /// <summary>
<<<<<<< HEAD:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/Dices/DicePresenter.cs
        /// ドラッグされたダイス以外はインアクティブに変更
=======
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.CardPlacement, _instance.CompositeObjectId, null));
            _eventBus.Emit(new UpdateDisplayCreatureStatusEvent(_instance.CompositeObjectId));            
        }

        /// <summary>
        /// ドラッグされたカード以外はインアクティブに変更
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/CreatureStatusPresenter.cs
        /// </summary>
        private void OnIdentifiableStateBeginDrag(IdentifiableStateBeginDragEvent evt)
        {
            // 自分がドラッグ対象
            if (evt.ExecutedObjectId == _instance.CompositeObjectId)
            {
                Debug.Log("ここが２回？");
                _eventBus.Emit(new DiceBeginDragEvent(_instance.CompositeObjectId, _instance.FaceValue));                
                return;
            }

            // 違う何かがドラッグされたらインアクティブに
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
<<<<<<< HEAD:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/Dices/DicePresenter.cs
        }

        /// <summary>
        /// オブジェクトをドロップ
        /// </summary>
        private void OnIdentifiableDrop(IdentifiableDropEvent evt)
        {
            if (evt.TargetObjectId != _view.CompositeObjectId) return;
            _eventBus.Emit(new DiceDropInInletEvent(evt.ExecutedObjectId, evt.TargetObjectId, _instance.FaceValue));
            _instance.IsAlive = false;
            _eventBus.Emit(new ChangeViewStatusEvent(_view.CompositeObjectId, IdentifiableStatus.Hide));
            _eventBus.Emit(new DisplayStatusViewEvent(_view.CompositeObjectId));
        }

        /// <summary>
        /// ダイスを画面に投げ入れる
        /// </summary>
        private void OnDisplayOnScreen(DisplayOnScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            if (_instance.IsOnScreen) return;
            _instance.IsOnScreen = true;
            _view.DisplayOnScreen(_instance.DiceHomePosition, _instance.FaceValue);
        }

        /// <summary>
        /// ダイスを画面から退場させる
        /// </summary>
        private void OnDisplayOffScreen(DisplayOffScreenEvent evt)
        {
            if (evt.ExecutedObjectId != _view.CompositeObjectId) return;
            if (!_instance.IsOnScreen) return;
            _instance.IsOnScreen = false;
            _view.DisplayOffScreen();
            _instance.IsAlive = false;
=======
            _eventBus.Emit(new ChangeViewStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Inactive));
>>>>>>> e5ac3da428fc54fc5ea1bc83c493f35a6a69d268:CardsAndDicesProject/Assets/CardsAndDices/Scripts/Combats/Presenters/CreatureStatusPresenter.cs
        }
    }
}

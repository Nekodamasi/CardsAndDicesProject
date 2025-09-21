using System;

namespace CardsAndDices
{
    /// <summary>
    /// DiceData(Model)とDiceView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class DicePresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly DiceInstance _instance;
        private readonly DiceView _view;
        private readonly GameEventBus _commandBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId ViewId => _view.CompositeObjectId;

        public DicePresenter(DiceInstance instance, DiceView view, GameEventBus commandBus)
        {
            _instance = instance;
            _view = view;
            _commandBus = commandBus;
            _commandBus.On<DisplayOnScreenCommand>(OnDisplayOnScreen);
            _commandBus.On<DisplayOffScreenCommand>(OnDisplayOffScreen);
            _commandBus.On<IdentifiableDropEvent>(OnIdentifiableDrop);
            
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _commandBus.Off<DisplayOnScreenCommand>(OnDisplayOnScreen);
            _commandBus.Off<DisplayOffScreenCommand>(OnDisplayOffScreen);
            _commandBus.Off<IdentifiableDropEvent>(OnIdentifiableDrop);
        }

        /// <summary>
        /// オブジェクトをドロップ
        /// </summary>
        private void OnIdentifiableDrop(IdentifiableDropEvent cmd)
        {
            if (cmd.TargetObjectId != _view.CompositeObjectId) return;
            _commandBus.Emit(new DiceDropInInletCommand(cmd.ExecutedObjectId, cmd.TargetObjectId, _instance.FaceValue));
            _instance.IsAlive = false;
            _commandBus.Emit(new ChangeViewStatusEvent(_view.CompositeObjectId, IdentifiableStatus.Hide));
            _commandBus.Emit(new DisplayStatusViewEvent(_view.CompositeObjectId));
        }

        /// <summary>
        /// ダイスを画面に投げ入れる
        /// </summary>
        private void OnDisplayOnScreen(DisplayOnScreenCommand cmd)
        {
            if (cmd.ExecutedObjectId != _view.CompositeObjectId) return;
            if (!_instance.IsOnScreen) return;
            _instance.IsOnScreen = true;
            _view.DisplayOnScreen();
        }

        /// <summary>
        /// ダイスを画面から退場させる
        /// </summary>
        private void OnDisplayOffScreen(DisplayOffScreenCommand cmd)
        {
            if (cmd.ExecutedObjectId != _view.CompositeObjectId) return;
            if (_instance.IsOnScreen) return;
            _instance.IsOnScreen = false;
            _view.DisplayOffScreen();
            _instance.IsAlive = false;
        }
    }
}

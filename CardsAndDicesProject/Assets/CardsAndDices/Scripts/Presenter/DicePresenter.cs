using System;

namespace CardsAndDices
{
    /// <summary>
    /// DiceData(Model)とDiceView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class DicePresenter : IDisposable
    {
        private readonly DiceData _data;
        private readonly DiceView _view;
        private readonly DiceManager _diceManager;
        private readonly ViewRegistry _viewRegistry;
        private readonly SpriteCommandBus _commandBus;


        public DicePresenter(DiceData data, DiceView view, DiceManager diceManager, ViewRegistry viewRegistry, SpriteCommandBus commandBus)
        {
            _data = data;
            _view = view;
            _diceManager = diceManager;
            _viewRegistry = viewRegistry;
            _commandBus = commandBus;

            // Modelの変更をViewに反映
            _data.OnFaceValueChanged += _view.UpdateFace;
            // Viewの破棄イベントを購読
            _view.OnDestroyed += Dispose;

            _view.UpdateFace(_data.FaceValue); // 初期表示
            _commandBus.On<DiceDropInInletCommand>(OnDiceDropInInlet);
        }
        /// <summary>
        /// ダイスがインレットにドロップされたとき
        /// </summary>
        private void OnDiceDropInInlet(DiceDropInInletCommand cmd)
        {
            if (cmd.DiceId != _data.Id) return;

            //エフェクトを実行
            _view.DropVfxPlay();
            Dispose();
        }

        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            if (_data == null) return; // すでにDisposeされている

            _commandBus.Off<DiceDropInInletCommand>(OnDiceDropInInlet);
            _diceManager.RemoveDice(_data.Id);

            // イベント購読を解除
            _data.OnFaceValueChanged -= _view.UpdateFace;
            _view.OnDestroyed -= Dispose;

            // Viewを非アクティブ化してプールに戻す
            _view.SetSpawnedState(false);
        }
    }
}

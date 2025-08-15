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

        public DicePresenter(DiceData data, DiceView view, DiceManager diceManager, ViewRegistry viewRegistry)
        {
            _data = data;
            _view = view;
            _diceManager = diceManager;
            _viewRegistry = viewRegistry;

            // Modelの変更をViewに反映
            _data.OnFaceValueChanged += _view.UpdateFace;
            // Viewの破棄イベントを購読
            _view.OnDestroyed += Dispose;

            _view.UpdateFace(_data.FaceValue); // 初期表示
        }

        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            if (_data == null) return; // すでにDisposeされている

            _diceManager.RemoveDice(_data.Id);
            
            // イベント購読を解除
            _data.OnFaceValueChanged -= _view.UpdateFace;
            _view.OnDestroyed -= Dispose;

            // Viewを非アクティブ化してプールに戻す
            _view.SetSpawnedState(false);
        }
    }
}

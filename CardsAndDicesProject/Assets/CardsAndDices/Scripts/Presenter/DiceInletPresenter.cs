using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// DiceInlet (Model) と DiceInletView (View) の間の仲介役。
    /// </summary>
    public class DiceInletPresenter
    {
        private readonly IDiceInlet _model;
        private readonly DiceInletView _view;
        private readonly SpriteCommandBus _commandBus;

        [Inject]
        public DiceInletPresenter(IDiceInlet model, DiceInletView view, SpriteCommandBus commandBus)
        {
            _model = model;
            _view = view;
            _commandBus = commandBus;

            // TODO: モデルのイベントを購読し、Viewを更新する
            // _model.OnCountdownChanged += UpdateView;
            
            // 初期表示を更新
            UpdateView();
        }

        private void UpdateView()
        {
            // TODO: _viewの表示を_modelの状態に合わせて更新する
            // _view.UpdateCountdownDisplay(_model.CurrentCountdownValue);
        }
    }
}

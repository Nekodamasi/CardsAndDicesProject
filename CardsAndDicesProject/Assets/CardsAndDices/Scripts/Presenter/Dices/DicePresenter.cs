using System;

namespace CardsAndDices
{
    /// <summary>
    /// DiceData(Model)とDiceView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class DicePresenter : IDisposable
    {
        private readonly DiceInstance _instance;
        private readonly Old_DiceView _view;
        private readonly IdentifiableCommandBus _commandBus;


        public DicePresenter(DiceInstance instance, Old_DiceView view, IdentifiableCommandBus commandBus)
        {
            Dispose();
            _instance = instance;
            _view = view;
            _commandBus = commandBus;
            _commandBus.On<DiceDropInInletCommand>(OnDiceDropInInlet);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _commandBus.Off<DiceDropInInletCommand>(OnDiceDropInInlet);
        }

        /// <summary>
        /// ダイスがインレットにドロップされたとき
        /// </summary>
        private void OnDiceDropInInlet(DiceDropInInletCommand cmd)
        {
        }

    }
}

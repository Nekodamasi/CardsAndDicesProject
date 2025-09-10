using VContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

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

            // 初期表示を更新
            _view.InitializeDisplay(_model.Condition);
            _commandBus.On<DiceDropInInletCommand>(OnDiceDropInInlet);
        }

        /// <summary>
        /// ダイスがインレットにドロップされたとき
        /// </summary>
        private async void OnDiceDropInInlet(DiceDropInInletCommand cmd)
        {
            if (cmd.InletId != _model.Id) return;
//            Debug.Log("<color=Blue>インレットにドロップ：");
            int newValue = _model.OnDiceDropped(cmd.DiceValue);
            await UpdateCountdownValueAsync(newValue, 0.2f);

        }

        public async UniTask UpdateCountdownValueAsync(int newValue, float animationTime)
        {
            // Viewのアニメーションメソッドを呼び出し、その完了を待つ
            await _view.AnimateCountdownAsync(newValue, animationTime);

            // アニメーション完了後に実行したい処理をここに記述
            if (newValue > 0)
            {
                // クールダウン処理
                _commandBus.Emit(new CooldownZeroAttacksCommand(new ProcessAllCreaturesCooldownCommand()));
                return;
            }

            // インレット発動によるアビリティ実行
            _commandBus.Emit(new InletExecuteAbilityEffectCommand(_model.Id, TriggerTiming.Inlet));
        }
        public void Dispose()
        {
            _commandBus.Off<DiceDropInInletCommand>(OnDiceDropInInlet);
        }
    }
}

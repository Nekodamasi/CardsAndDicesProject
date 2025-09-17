using System;

namespace CardsAndDices
{
    /// <summary>
    /// DiceSlotInstanceを管理し、の変更をコマンドで通知します
    /// </summary>
    public class DiceSlotController : IDisposable
    {
        private readonly DiceSlotInstance _diceSlotInstance;
        private readonly IdentifiableCommandBus _commandBus;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DiceSlotController(DiceSlotInstance diceSlotInstance, IdentifiableCommandBus commandBus)
        {
            _diceSlotInstance = diceSlotInstance;
            _commandBus = commandBus;
            _commandBus.On<PlacedDiceCommand>(OnPlacedDice);
            _commandBus.On<ReflowPlacedDiceCommand>(OnReflowPlacedDice);
            _commandBus.On<RemoveDiceCommand>(OnRemoveDice);
            _commandBus.On<MoveToAnimationReflowDiceCommand>(OnMoveToAnimationReflowDice);
        }

        /// <summary>
        /// Disposeします
        /// </summary>
        public void Dispose()
        {
            _commandBus.Off<PlacedDiceCommand>(OnPlacedDice);
            _commandBus.Off<ReflowPlacedDiceCommand>(OnReflowPlacedDice);
            _commandBus.Off<RemoveDiceCommand>(OnRemoveDice);
            _commandBus.Off<MoveToAnimationReflowDiceCommand>(OnMoveToAnimationReflowDice);
        }

        /// <summary>
        /// リフロー位置へのダイスAnimation移動
        /// </summary>
        private void OnMoveToAnimationReflowDice(MoveToAnimationReflowDiceCommand cmd)
        {
            if (_diceSlotInstance.DiceSlotLocation != cmd.DiceSlotLocation) return;
            if (_diceSlotInstance.ReflowPlacedDiceId == null) return;

            _commandBus.Emit(new MoveToAnimationIdentifiableCommand(_diceSlotInstance.ReflowPlacedDiceId, _diceSlotInstance.DiceSlotPosition));
        }

        /// <summary>
        /// ダイスの配置処理
        /// </summary>
        private void OnPlacedDice(PlacedDiceCommand cmd)
        {
            if (cmd.DiceSlotLocation != _diceSlotInstance.DiceSlotLocation) return;
            if (_diceSlotInstance.IsOccupied)
            {
                _diceSlotInstance.RemoveDice();
            }
            _diceSlotInstance.PlacedDice(cmd.DiceId);
            _commandBus.Emit(new IdentifiableChangeHomePositionCommand(cmd.DiceId, _diceSlotInstance.DiceSlotPosition));
        }

        /// <summary>
        /// ダイスリフロー配置処理
        /// </summary>
        private void OnReflowPlacedDice(ReflowPlacedDiceCommand cmd)
        {
            if(cmd.DiceSlotLocation != _diceSlotInstance.DiceSlotLocation) return;
            _diceSlotInstance.ReflowPlacedDice(cmd.DiceId);
        }

        /// <summary>
        /// ダイスのリムーブ処理
        /// </summary>
        private void OnRemoveDice(RemoveDiceCommand cmd)
        {
            _diceSlotInstance.RemoveDice();
        }

    }
}

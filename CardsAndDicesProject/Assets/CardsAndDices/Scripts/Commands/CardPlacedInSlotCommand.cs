using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// カードがスロットに配置されたことを通知するコマンド。
    /// リフローを伴わない単純な配置の場合に使用されます。
    /// </summary>
    public class CardPlacedInSlotCommand : ICommand
    {
        /// <summary>
        /// 配置されたカードのCompositeObjectId。
        /// </summary>
        public CompositeObjectId CardId { get; private set; }

        /// <summary>
        /// カードが配置されたスロットのワールド座標。
        /// </summary>
        public Vector3 TargetPosition { get; private set; }

        /// <summary>
        /// CardPlacedInSlotCommandの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="cardId">配置されたカードのCompositeObjectId。</param>
        /// <param name="targetPosition">カードが配置されたスロットのワールド座標。</param>
        public CardPlacedInSlotCommand(CompositeObjectId cardId, Vector3 targetPosition)
        {
            CardId = cardId;
            TargetPosition = targetPosition;
        }

        /// <summary>
        /// コマンドを実行します。（通知用のため、具体的なロジジックは購読側で処理されます）
        /// </summary>
        public void Execute()
        {
            // 実装なし
        }

        /// <summary>
        /// コマンドを元に戻します。（通知用のため、具体的なロジックは購読側で処理されます）
        /// </summary>
        public void Undo()
        {
            // 実装なし
        }
    }
}

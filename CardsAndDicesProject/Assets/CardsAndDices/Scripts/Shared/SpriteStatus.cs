
namespace CardsAndDice
{
    /// <summary>
    /// スロットが存在する大まかなライン（エリア）を定義します。
    /// </summary>
    public enum SpriteStatus
    {
            Normal,
            Hover,
            DraggingStarted, // ドラッグ開始時
            DraggingInProgress, // ドラッグ中
            Move,
            Acceptable,
            Inactive
    }
}

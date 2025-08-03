namespace CardsAndDice
{
    /// <summary>
    /// イベント駆動アーキテクチャにおけるイベントメッセージの基本契約を定義するインターフェース。
    /// このプロジェクトでは、伝統的なコマンドパターンとしてではなく、
    /// システム間で状態やイベント情報を伝達するためのデータコンテナとして使用されます。
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        void Execute();

        /// <summary>
        /// コマンドを元に戻します。
        /// </summary>
        void Undo();
    }
} 
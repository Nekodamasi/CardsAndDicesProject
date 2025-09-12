namespace CardsAndDices
{
    /// <summary>
    /// CompositeObjectIdを通じて一意に識別可能であり、Viewとしての役割を持つオブジェクトのインターフェースです。
    /// </summary>
    public interface IIdentifiableView
    {
        /// <summary>
        /// このViewインスタンスを識別するための一意なIDを取得します。
        /// </summary>
        CompositeObjectId CompositeObjectId { get; }

        /// <summary>
        /// このViewが現在、ゲームワールドに生成され、有効な状態であるかを取得します。
        /// </summary>
        bool IsSpawned { get; }
    }
}

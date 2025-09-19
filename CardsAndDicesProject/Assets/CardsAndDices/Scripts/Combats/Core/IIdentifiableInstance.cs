namespace CardsAndDices
{
    /// <summary>
    /// 識別可能なオブジェクトIDを使用するインターフェース
    /// </summary>
    public interface IIdentifiableInstance
    {
        /// <summary>
        /// 識別可能なオブジェクトID
        /// </summary>
        CompositeObjectId CompositeObjectId { get; }
    }
}

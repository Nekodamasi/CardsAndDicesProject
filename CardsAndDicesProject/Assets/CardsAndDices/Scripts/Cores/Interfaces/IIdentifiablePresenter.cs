namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトIDを使用するPresenterのインターフェース
    /// </summary>
    public interface IIdentifiablePresenter
    {
        /// <summary>
        /// 識別ID
        /// </summary>
        CompositeObjectId CompositeObjectId { get; }
    }
}

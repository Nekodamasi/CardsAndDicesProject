namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトIDを使用するPresenterのインターフェース
    /// </summary>
    public interface IIdentifiablePresenter
    {
        /// <summary>
        /// インスタンス側のID
        /// </summary>
        CompositeObjectId InstanceId { get; }

        /// <summary>
        /// ビュー側のID
        /// </summary>
        CompositeObjectId ViewId { get; }
    }
}

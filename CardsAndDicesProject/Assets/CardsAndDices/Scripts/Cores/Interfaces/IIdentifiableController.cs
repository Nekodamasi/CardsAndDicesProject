namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトIDを使用するControllerのインターフェース
    /// </summary>
    public interface IIdentifiableController
    {
        /// <summary>
        /// インスタンス側のID
        /// </summary>
        CompositeObjectId InstanceId { get; }
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// 識別可能オブジェクトIDを使用するマネージャーのインターフェース
    /// </summary>
    public interface IIdentifiableManager
    {
        /// <summary>
        /// 識別IDを指定したDispose
        /// </summary>
        void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId);
    }
}

namespace CardsAndDices
{
    /// <summary>
    /// 初期化処理のinterface
    /// </summary>
    public interface IGameInitializable
    {
        void OnAwake();
        void OnStart();
    }
}

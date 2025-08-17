using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// DiceInletインスタンスの生成ロジックに特化したFactoryクラス。
    /// </summary>
    public class DiceInletFactory
    {
        public DiceInletFactory()
        {
        }

        /// <summary>
        /// 新しいDiceInletインスタンスを生成し、初期化して返します。
        /// </summary>
        /// <param name="inletId">インレットのID</param>
        /// <param name="cardId">所属するカードのID</param>
        /// <param name="profile">インレットの能力プロファイル</param>
        /// <returns>生成されたDiceInletインスタンス</returns>
        public IDiceInlet Create(CompositeObjectId inletId, CompositeObjectId cardId, InletAbilityProfile profile)
        {
            // DiceInletのコンストラクタに必要な依存性をここで解決・注入することも可能
            return new DiceInlet(inletId, cardId, profile);
        }
    }
}

using DG.Tweening;

namespace CardsAndDices
{
    /// <summary>
    /// アニメーションの振る舞いを定義する戦略インターフェース。
    /// </summary>
    public interface IAnimationStrategy
    {
        /// <summary>
        /// アニメーションを実行します。
        /// </summary>
        /// <param name="context">アニメーションに必要なコンポーネントのコンテキスト。</param>
        /// <returns>アニメーション完了を待つためのUniTask。</returns>
        public Sequence ExecuteAsync(AnimationContext context);
    }
}
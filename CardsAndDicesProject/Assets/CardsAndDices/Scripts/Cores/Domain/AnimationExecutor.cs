using DG.Tweening;

namespace CardsAndDices
{
    /// <summary>
    /// アニメーション戦略(BaseAnimationStrategySO)を実行するサービスクラスです。
    /// </summary>
    public class AnimationExecutor
    {
        /// <summary>
        /// 指定されたアニメーション戦略を実行します。
        /// </summary>
        /// <param name="strategy">実行するアニメーション戦略のScriptableObject。</param>
        /// <param name="context">アニメーションの実行に必要なコンテキスト情報。</param>
        /// <returns>実行されたアニメーションのDOTween Sequence。</returns>
        public Sequence Execute(BaseAnimationStrategySO strategy, AnimationContext context)
        {
            if (strategy == null)
            {
                // 戦略が指定されていない場合は、空のシーケンスを返して即時完了させる
                return DOTween.Sequence();
            }

            // 戦略のExecuteAsyncメソッドを呼び出し、DOTweenのSequenceを返す
            return strategy.ExecuteAsync(context);
        }
    }
}

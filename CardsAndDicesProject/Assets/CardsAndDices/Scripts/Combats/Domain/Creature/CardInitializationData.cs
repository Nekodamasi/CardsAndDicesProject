using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// カード生成に必要な全ての情報を集約したデータコンテナクラスです。
    /// このDTOを介してデータを受け渡しすることで、カード生成ロジックとデータソース間の結合を疎に保ちます。
    /// </summary>
    public class CardInitializationData
    {
        /// <summary>
        /// 生成するカードのクリーチャーとしての基本データ。
        /// </summary>
        public CreatureData CreatureData { get; private set; }

        /// <summary>
        /// カードに付属する各インレットの能力（条件と効果）を定義するプロファイルのリスト。
        /// </summary>
        public List<InletAbilityProfile> InletAbilityProfiles { get; private set; }

        /// <summary>
        /// クリーチャーの外観を定義するプロファイル。
        /// </summary>
        public AppearanceProfile Appearance { get; private set; }

        /// <summary>
        /// CardInitializationDataの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="creatureData">クリーチャーの基本データ。</param>
        /// <param name="profiles">インレット能力プロファイルのリスト。</param>
        /// <param name="appearance">外観プロファイル。</param>
        public CardInitializationData(CreatureData creatureData, List<InletAbilityProfile> profiles, AppearanceProfile appearance)
        {
            CreatureData = creatureData;
            InletAbilityProfiles = profiles;
            Appearance = appearance;
        }
    }
}

using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// カードの生成、初期化、破棄など、ライフサイクル全般を管理するサービス。
    /// </summary>
    [CreateAssetMenu(fileName = "CardLifecycleService", menuName = "CardsAndDices/Systems/CardLifecycleService")]
    public class CardLifecycleService : ScriptableObject
    {
        private DiceInletAbilityRegistry _abilityRegistry;
        private ViewRegistry _viewRegistry;

        [Inject]
        public void Initialize(DiceInletAbilityRegistry abilityRegistry, ViewRegistry viewRegistry)
        {
            _abilityRegistry = abilityRegistry;
            _viewRegistry = viewRegistry;
        }

        /// <summary>
        /// 既存のカードViewを初期化し、能力を登録します。
        /// </summary>
        /// <param name="cardView">初期化するCreatureCardViewのインスタンス。</param>
        /// <param name="initData">カード生成に必要な情報。</param>
        public void InitializeCard(CreatureCardView cardView, CardInitializationData initData)
        {
            // Viewにクリーチャーデータを設定
            cardView.ApplyData(initData.CreatureData);
            cardView.SetSpawnedState(true);

            // インレット能力をRegistryに登録
            var inletViews = cardView.GetInletViews();
            if (inletViews.Count != initData.InletAbilityProfiles.Count)
            {
                Debug.LogError($"CardLifecycleService: インレットの数({inletViews.Count})とプロファイルの数({initData.InletAbilityProfiles.Count})が一致しません。");
                // エラーハンドリング: 不一致の場合の挙動を定義する
            }

            for (int i = 0; i < inletViews.Count; i++)
            {
                var inletId = inletViews[i].GetObjectId();
                inletViews[i].SetSpawnedState(true);
                var profile = initData.InletAbilityProfiles[i];
                _abilityRegistry.Register(inletId, profile);
            }
        }

        /// <summary>
        /// カードの能力登録を解除します。
        /// </summary>
        /// <param name="cardView">解除するカードのView。</param>
        public void TeardownCard(CreatureCardView cardView)
        {
            if (cardView == null || cardView.InletView == null)
            {
                return;
            }

            var inletId = cardView.InletView.GetObjectId();
            _abilityRegistry.Unregister(inletId);
        }
    }
}
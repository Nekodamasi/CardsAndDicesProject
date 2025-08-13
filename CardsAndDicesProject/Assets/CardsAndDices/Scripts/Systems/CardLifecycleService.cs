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
        /// 全てのカードを初期化し、その能力をレジストリに登録します。
        /// </summary>
        public void InitializeCards()
        {
            foreach (var cardView in _viewRegistry.GetAllCreatureCardViews())
            {
                InitializeCard(cardView.GetObjectId());
            }
        }

        /// <summary>
        /// カードを初期化し、その能力をレジストリに登録します。
        /// TODO: このメソッドは、オブジェクトプールからカードが取得され、
        ///       特定のカード定義データで初期化される際に呼び出される必要があります。
        /// </summary>
        /// <param name="cardId">初期化するカードのID。</param>
        private void InitializeCard(CompositeObjectId cardId)
        {
            var cardView = _viewRegistry.GetView<CreatureCardView>(cardId);
            if (cardView == null || cardView.InletView == null || cardView.InletProfile == null)
            {
                return;
            }

            var inletId = cardView.InletView.GetObjectId();
            var profile = cardView.InletProfile;

            _abilityRegistry.Register(inletId, profile);
        }

        /// <summary>
        /// カードが破棄またはプールに戻される際に、能力の登録を解除します。
        /// </summary>
        /// <param name="cardId">破棄されるカードのID。</param>
        public void TeardownCard(CompositeObjectId cardId)
        {
            var cardView = _viewRegistry.GetView<CreatureCardView>(cardId);
            if (cardView == null || cardView.InletView == null)
            {
                return;
            }

            var inletId = cardView.InletView.GetObjectId();
            _abilityRegistry.Unregister(inletId);
        }
    }
}
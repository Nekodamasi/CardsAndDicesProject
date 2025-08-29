using UnityEngine;
using VContainer.Unity;
using VContainer;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "GameInitializer", menuName = "CardsAndDices/initializers/GameInitializer")]
    public class GameInitializer : ScriptableObject, IStartable
    {
        private List<CreatureCardView> _creatureCardViews = new List<CreatureCardView>();
        private List<CardSlotView> _cardSlotViews = new List<CardSlotView>();
        private List<DiceSlotView> _diceSlotViews = new List<DiceSlotView>();
        private List<DiceView> _diceViews = new List<DiceView>();
        private List<DiceInletView> _diceInletViews = new List<DiceInletView>();

        [Header("initialize Objects")]
        [SerializeField] private CombatManager _CombatManager;

        [Inject]
        public void Initialize(List<CreatureCardView> creatureCardViews, List<CardSlotView> cardSlotViews, List<DiceSlotView> diceSlotViews, List<DiceView> diceViews, List<DiceInletView> diceInletViews)
        {
            _creatureCardViews = creatureCardViews;
            _cardSlotViews = cardSlotViews;
            _diceSlotViews = diceSlotViews;
            _diceViews = diceViews;
            _diceInletViews = diceInletViews;
        }
        public void Start()
        {
            foreach (var view in _creatureCardViews)
            {
                view.OnAwake();
                view.OnStart();
            }
            foreach (var view in _cardSlotViews)
            {
                view.OnAwake();
                view.OnStart();
            }
            foreach (var view in _diceSlotViews)
            {
                view.OnAwake();
                view.OnStart();
            }
            foreach (var view in _diceViews)
            {
                view.OnAwake();
                view.OnStart();
            }
            foreach (var view in _diceInletViews)
            {
                view.OnAwake();
                view.OnStart();
            }
            _CombatManager.InitializeCombatField();
        }
    }
}

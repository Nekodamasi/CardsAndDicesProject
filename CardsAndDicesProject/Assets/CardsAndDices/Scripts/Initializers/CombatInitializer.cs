using UnityEngine;
using VContainer.Unity;
using VContainer;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "CombatInitializer", menuName = "CardsAndDices/initializers/CombatInitializer")]
    public class CombatInitializer : ScriptableObject, IPostStartable
    {
        private IdentifiableCommandBus _identifiableCommandBus;
        private List<IGameInitializable> _gameInitializables = new List<IGameInitializable>();

        [Inject]
        public void Initialize(IdentifiableCommandBus identifiableCommandBus)
        {
            _identifiableCommandBus = identifiableCommandBus;
        }

        public void AddInitializables(IGameInitializable initializables)
        {
            _gameInitializables.Add(initializables);
        }

        public void PostStart()
        {
            foreach (var initializable in _gameInitializables)
            {
                initializable.OnAwake();
            }
            foreach (var initializable in _gameInitializables)
            {
                initializable.OnStart();
            }
        }
    }
}

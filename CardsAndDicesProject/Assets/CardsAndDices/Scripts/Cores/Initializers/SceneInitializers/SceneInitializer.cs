using UnityEngine;
using VContainer.Unity;
using VContainer;
using System.Collections.Generic;
using System.Linq;

namespace CardsAndDices
{
    [CreateAssetMenu(fileName = "SceneInitializer", menuName = "CardsAndDices/initializers/SceneInitializers/SceneInitializer")]
    public class SceneInitializer : ScriptableObject, IPostStartable
    {
        private GameEventBus _eventBus;
        private List<IGameInitializable> _gameInitializables = new List<IGameInitializable>();

        [Inject]
        public void Initialize(GameEventBus eventBus)
        {
            _eventBus = eventBus;
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
            _eventBus.Emit(new SceneLoadedEvent());
            _eventBus.Emit(new InstanceSetUpedEvent());
            _eventBus.Emit(new SceneScreenSetUpEvent());
            _eventBus.Emit(new CombatPhaseDiceRollEvent());
            _eventBus.Emit(new CombatPhasePlayerCardinitializedEvent());
            _eventBus.Emit(new CombatPhasePlayerCardOnScreenEvent());
            _eventBus.Emit(new CombatPhaseWaveEnemySetUpEvent());
            _eventBus.Emit(new CombatPhaseEnemyCardOnScreenEvent());
            _eventBus.Emit(new CombatPhaseCombatBtnOnScreenEvent());
        }
    }
}

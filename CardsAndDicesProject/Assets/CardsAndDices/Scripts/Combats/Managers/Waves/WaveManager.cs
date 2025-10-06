using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using System;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードの状態管理などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "WaveManager", menuName = "CardsAndDices/Combats/Managers/Waves/WaveManager")]
    public class WaveManager : ScriptableObject, IDisposable
    {
        [Header("Components")]
        private GameEventBus _eventBus;
        private CombatScenarioRegistry _combatScenarioRegistry;

        [Inject]
        public void Initialize(GameEventBus eventBus, CombatScenarioRegistry combatScenarioRegistry)
        {
            _eventBus = eventBus;
            _combatScenarioRegistry = combatScenarioRegistry;
//            _eventBus.On<CreateCreatureEvent>(OnCreateCreature);
        }

        public void Dispose()
        {
//            _eventBus.Off<CreateCreatureEvent>(OnCreateCreature);
        }
   }
}

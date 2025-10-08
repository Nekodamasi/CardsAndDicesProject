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
    public class WaveManager : ScriptableObject, IDisposable, IWaveNumber
    {
        [Header("Components")]
        [SerializeField] private CompositeObjectIdTypeEntity _enemyCardObjectType;
        private GameEventBus _eventBus;
        private CombatScenarioRegistry _combatScenarioRegistry;
        private IdentifiableViewRegistry _viewRegistry;
        private CombatData _combatData;
        private int _waveNumber;

        [Inject]
        public void Initialize(GameEventBus eventBus, CombatScenarioRegistry combatScenarioRegistry, IdentifiableViewRegistry viewRegistry)
        {
            _eventBus = eventBus;
            _combatScenarioRegistry = combatScenarioRegistry;
            _viewRegistry = viewRegistry;
            _waveNumber = 0;
            _combatData = null;
            _eventBus.On<CombatPhaseWaveEnemySetUpEvent>(OnCombatPhaseWaveEnemySetUp);
        }

        /// <summary>
        /// ウェーブからエネミークリーチャーカードを配置
        /// </summary>
        private void OnCombatPhaseWaveEnemySetUp(CombatPhaseWaveEnemySetUpEvent evt)
        {
            // いずれコンバットデータのセットは別の場所に移動
            SetUpCombatData();

            // １ウェーブ分のクリーチャーを生成
            CreateWaveEnemyCreature();
        }

        /// <summary>
        /// コンバットデータを取得する
        /// </summary>
        private void SetUpCombatData()
        {
            _combatData = _combatScenarioRegistry.GetCombatData(WaveAreaId.Forest, ChallengeRating.Easy);
            if (_combatData is null)
            {
                Debug.LogWarning("コンバットデータが取得できない");
            }
        }

        /// <summary>
        /// ウェーブデータを元に１ウェーブ分のクリーチャーを生成する
        /// </summary>
        private void CreateWaveEnemyCreature()
        {
            var enemyPlacements = _combatData.GetEnemyPlacementList(_waveNumber);
            foreach (var enemyPlacement in enemyPlacements)
            {
                CreateEnemyCreature(enemyPlacement);                
            }
        }

        /// <summary>
        /// エネミーのクリーチャーを生成します
        /// </summary>
        private void CreateEnemyCreature(EnemyPlacement enemyPlacement)
        {
            // プレイヤーカードの生成と配置
            var view = _viewRegistry.GetNonBoundAndObjectTypeView<CreatureCardView>(_enemyCardObjectType);
            if (view is null)
            {
                Debug.LogWarning("viewが取れない");
                return;
            }
            _eventBus.Emit(new CreateCreatureEvent(view.CompositeObjectId, enemyPlacement.CardInitializationData));
            _eventBus.Emit(new UpdateDisplayCreatureStatusEvent(view.CompositeObjectId));
            _eventBus.Emit(new PlacedPpecifiedSlotEvent(view.CompositeObjectId, Team.Enemy, enemyPlacement.Position, enemyPlacement.Location));
        }

        /// <summary>
        /// ウェーブの最大数を取得する
        /// </summary>
        public int MaxWaveNumber => _combatData.Waves.Count;

        /// <summary>
        /// ウェーブの現在値を取得する
        /// </summary>
        public int CurrentWaveNumber => _waveNumber;

        /// <summary>
        /// ウェーブナンバーを次の番号に変更します
        /// </summary>
        public void NextWaveNumber()
        {
            _waveNumber++;
            if (MaxWaveNumber < _waveNumber)
            {
                Debug.LogWarning("WaveNumberが最大値を超えています");
            }
        }

        public void Dispose()
        {
            _eventBus.Off<CombatPhaseWaveEnemySetUpEvent>(OnCombatPhaseWaveEnemySetUp);
        }
   }
}

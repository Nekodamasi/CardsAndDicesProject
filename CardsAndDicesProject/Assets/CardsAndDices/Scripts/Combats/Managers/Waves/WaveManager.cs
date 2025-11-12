using Cysharp.Threading.Tasks;
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
        private ITargetManager _itargetManager;

        [Inject]
        public void Initialize(GameEventBus eventBus, CombatScenarioRegistry combatScenarioRegistry, IdentifiableViewRegistry viewRegistry, ITargetManager itargetManager)
        {
            _eventBus = eventBus;
            _combatScenarioRegistry = combatScenarioRegistry;
            _viewRegistry = viewRegistry;
            _itargetManager = itargetManager;
            _waveNumber = -1;
            _combatData = null;
            _eventBus.On<CombatPhaseWaveEnemySetUpEvent>(OnCombatPhaseWaveEnemySetUp);
            _eventBus.On<CombatPhaseSetUpCombatDataEvent>(OnCombatPhaseSetUpCombatData);
            
        }

        /// <summary>
        /// コンバットデータのセットアップイベント
        /// </summary>
        private void OnCombatPhaseSetUpCombatData(CombatPhaseSetUpCombatDataEvent evt)
        {
            SetUpCombatData();
        }

        /// <summary>
        /// ウェーブからエネミークリーチャーカードを配置
        /// </summary>
        private async void OnCombatPhaseWaveEnemySetUp(CombatPhaseWaveEnemySetUpEvent evt)
        {
            // ウェーブを更新
            var flg = NextWaveNumber();

            if (flg)
            {
                // １ウェーブ分のクリーチャーを生成
                CreateWaveEnemyCreature();

                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            }

            //
            var wave = _combatData.GetWaveData(_waveNumber);
            _eventBus.Emit(new PlayBGMEvent(wave.BGMDataEntity.AudioClip, 5.0f));
            _eventBus.Emit(new CombatPhaseWaveEnemySetUpEndEvent());
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
            _waveNumber = -1;
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
        /// 最終ウェーブに到着しているか
        /// </summary>
        public bool IslastWave => MaxWaveNumber == CurrentWaveNumber;

        /// <summary>
        /// ウェーブナンバーを次の番号に変更します
        /// </summary>
        private bool NextWaveNumber()
        {
            var list = _itargetManager.GetEnemyList(Team.Player);
            if (list.Count > 0) return false;
            _waveNumber++;
            if (MaxWaveNumber < _waveNumber)
            {
                Debug.LogWarning("WaveNumberが最大値を超えています");
                return false;
            }
            return true;
        }

        public void Dispose()
        {
            _eventBus.Off<CombatPhaseWaveEnemySetUpEvent>(OnCombatPhaseWaveEnemySetUp);
            _eventBus.Off<CombatPhaseSetUpCombatDataEvent>(OnCombatPhaseSetUpCombatData);
        }
   }
}

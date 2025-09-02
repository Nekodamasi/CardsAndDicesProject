using System.Collections.Generic;
using VContainer;
using UnityEngine;
using System.Linq;
using System;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// ゲームの戦闘フェーズ全体の流れを制御する責務を負います。
    /// 特に、戦闘の開始、プレイヤーおよびエネミーカードの生成と配置、ウェーブの管理など、高レベルなゲームロジックを統括します。
    /// ScriptableObjectとして、ゲームロジックの統括に特化します。
    /// </summary>
    [CreateAssetMenu(fileName = "CombatManager", menuName = "CardsAndDices/Managers/CombatManager")]
    public class CombatManager : ScriptableObject
    {
        [Inject] private CardLifecycleService _cardLifecycleService;
        [Inject] private CardSlotManager _cardSlotManager;
        [Inject] private PlayerCardDataProvider _playerCardDataProvider;
        [Inject] private EnemyCardDataProvider _enemyCardDataProvider;
        [Inject] private ViewRegistry _viewRegistry;
        [Inject] private DiceManager _diceManager;
        [Inject] private DiceFactory _diceFactory;
        [Inject] private CreatureManager _creatureManager;
        [Inject] private DiceInletManager _diceInletManager;
        [Inject] private DiceSlotManager _diceSlotManager;
        [Inject] private SpriteCommandBus _commandBus;

        private readonly List<DicePresenter> _dicePresenters = new();
        private CombatDataLoaderService _combatDataLoaderService;
        private WaveGeneratorService _waveGeneratorService;

        /// <summary>
        /// CombatManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(CardLifecycleService cardLifecycleService, CardSlotManager cardSlotManager,
                               PlayerCardDataProvider playerCardDataProvider, EnemyCardDataProvider enemyCardDataProvider,
                               ViewRegistry viewRegistry, DiceManager diceManager, CreatureManager creatureManager,
                               DiceInletManager diceInletManager, CombatScenarioRegistry combatScenarioRegistry,
                               DiceSlotManager diceSlotManager, SpriteCommandBus commandBus)
        {
            _cardLifecycleService = cardLifecycleService;
            _cardSlotManager = cardSlotManager;
            _playerCardDataProvider = playerCardDataProvider;
            _enemyCardDataProvider = enemyCardDataProvider;
            _viewRegistry = viewRegistry;
            _diceManager = diceManager;
            _creatureManager = creatureManager;
            _diceInletManager = diceInletManager;
            _diceSlotManager = diceSlotManager;
            _commandBus = commandBus;
            _diceFactory = new DiceFactory();
            _combatDataLoaderService = new CombatDataLoaderService(combatScenarioRegistry);
            _waveGeneratorService = new WaveGeneratorService();

            _commandBus.On<ProcessAllCreaturesCooldownCommand>(HandleCooldownProcessing);
        }

        /// <summary>
        /// 戦闘フィールドを初期化し、戦闘を開始します。
        /// プレイヤーカードと最初のウェーブのエネミーカードを生成・配置します。
        /// </summary>
        public void InitializeCombatField()
        {
            // プレイヤーカードの生成と配置
            List<CardInitializationData> playerInitList = _playerCardDataProvider.GetCardDataList();
            Debug.Log("こんばっとまねーじゃー：" + playerInitList.Count);
            foreach (CardInitializationData initData in playerInitList)
            {
                CreatureCardView cardView = _viewRegistry.GetNextAvailableCreatureCardView(CreatureCardType.PlayerCard); // 利用可能なViewを取得
                if (cardView == null)
                {
                    Debug.LogError("利用可能なCreatureCardViewが見つかりません。シーンに十分な数のカードが配置されているか確認してください。");
                    break; // エラーなのでループを抜ける
                }
                _cardLifecycleService.InitializeCard(cardView, initData); // 既存のViewを初期化

                // 空いているハンドスロットを取得し、カードを配置する
                CardSlotData targetSlot = _cardSlotManager.GetNextEmptyHandSlot();
                if (targetSlot != null)
                {
                    cardView.SetSpawnedState(true);
                    cardView.SetDisplayActive(true);
                    _cardSlotManager.PlaceCardAsSystem(cardView.GetObjectId(), targetSlot.SlotId);
                }
                else
                {
                    Debug.LogWarning($"No empty slot found for player card {cardView.name}.");
                }
            }

            // 最初の敵ウェーブを生成
            SpawnNewWave(0);

            // テスト用にダイスを5つ生成
            RollDices(2);
        }

        /// <summary>
        /// 指定された数のダイスを生成し、ライフサイクルを開始します。
        /// </summary>
        /// <param name="count">生成するダイスの数。</param>
        public void RollDices(int count)
        {
            // 既存のダイスをクリア
            foreach (var presenter in _dicePresenters)
            {
                presenter.Dispose();
            }
            _dicePresenters.Clear();

            for (int i = 0; i < count; i++)
            {
                var diceView = _viewRegistry.GetNextAvailableDiceView();
                if (diceView != null)
                {
                    var diceId = diceView.GetObjectId();
                    var diceData = _diceFactory.Create(diceId);
                    _diceManager.AddDice(diceData);
                    diceView.SetDisplayActive(true);
                    var dicePresenter = new DicePresenter(diceData, diceView, _diceManager, _viewRegistry, _commandBus);
                    _dicePresenters.Add(dicePresenter);
                    _diceSlotManager.PlaceDiceAsSystem(diceView.GetObjectId(), _diceSlotManager.GetNextEmptyHandSlot().SlotId, true);
                }
                else
                {
                    Debug.LogWarning("利用可能なDiceViewが見つかりません。");
                }
            }
        }

        /// <summary>
        /// 指定されたウェーブ番号のエネミーカードを生成し、ボードに配置します。
        /// </summary>
        /// <param name="waveNumber">生成するウェーブの番号。</param>
        public void SpawnNewWave(int waveNumber)
        {
            var combatData = _combatDataLoaderService.GetRandomCombatData(AreaId.None, ChallengeRating.None);
            var waveGeneratorContextList = _waveGeneratorService.GenerateWaveEnemies(combatData, 0);

            foreach (WaveGeneratorService.WaveGeneratorContext waveGeneratorContext in waveGeneratorContextList)
            {
                Debug.Log("FixedCardInitializer:" + waveGeneratorContext.FixedCardInitializer.name);
                CreatureCardView cardView = _viewRegistry.GetNextAvailableCreatureCardView(CreatureCardType.EnemyCard); // 利用可能なViewを取得
                if (cardView == null)
                {
                    Debug.LogError("利用可能なCreatureCardViewが見つかりません。シーンに十分な数のカードが配置されているか確認してください。");
                    break; // エラーなのでループを抜ける
                }
                _cardLifecycleService.InitializeCard(cardView, waveGeneratorContext.FixedCardInitializer.CreateCardInitializationData()); // 既存のViewを初期化

                // 空いているハンドスロットを取得し、カードを配置する
                CardSlotData targetSlot = _cardSlotManager.FindSlotsByLocation(Team.Enemy, waveGeneratorContext.LinePosition, waveGeneratorContext.SlotLocation);

                if (targetSlot != null)
                {
                    cardView.SetSpawnedState(true);
                    cardView.SetDisplayActive(true);
                    _cardSlotManager.PlaceCardAsSystem(cardView.GetObjectId(), targetSlot.SlotId);
                }
                else
                {
                    Debug.LogWarning($"No empty slot found for player card {cardView.name}.");
                }

            }
        }

        private async void HandleCooldownProcessing(ProcessAllCreaturesCooldownCommand command)
        {
            var sortedSlots = _cardSlotManager.GetAllSlots()
                .Where(slot => slot.Line != LinePosition.Hand && slot.IsOccupied)
                .OrderBy(slot => slot.Team == Team.Enemy ? 0 : 1) // Enemy first
                .ThenBy(slot => slot.Location);

            foreach (var slot in sortedSlots)
            {
                var creature = _creatureManager.GetCreature(slot.PlacedCardId);

                Debug.Log("<color=red>クリーチャー：</color>" + creature.Id + "_" + creature.CurrentCooldown);
                if (creature != null && creature.CurrentCooldown > 0)
                {
                    _commandBus.Emit(new CreatureCooldownChangedCommand(creature.Id, creature.CurrentCooldown - 1, creature.BaseCooldown));
                    _commandBus.Emit(new CreatureCardUpdateDisplayCommand());
                    await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                }
            }

            await CooldownZeroAttacks();
            _commandBus.Emit(new DiceInletCountdownCompleteCommand(null));
            Debug.Log("<color=red>クールダウン処理終了</color>");
        }
        public async UniTask CooldownZeroAttacks()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
        }
    }
}

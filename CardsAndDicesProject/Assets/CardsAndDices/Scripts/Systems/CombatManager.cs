using System.Collections.Generic;
using VContainer;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// ゲームの戦闘フェーズ全体の流れを制御する責務を負います。
    /// 特に、戦闘の開始、プレイヤーおよびエネミーカードの生成と配置、ウェーブの管理など、高レベルなゲームロジックを統括します。
    /// ScriptableObjectとして、ゲームロジックの統括に特化します。
    /// </summary>
    [CreateAssetMenu(fileName = "CombatManager", menuName = "CardsAndDices/Systems/CombatManager")]
    public class CombatManager : ScriptableObject
    {
        [Inject] private CardLifecycleService _cardLifecycleService;
        [Inject] private CardSlotManager _cardSlotManager;
        [Inject] private PlayerCardDataProvider _playerCardDataProvider;
        [Inject] private EnemyCardDataProvider _enemyCardDataProvider;
        [Inject] private ViewRegistry _viewRegistry;
        [Inject] private DiceManager _diceManager;
        [Inject] private DiceFactory _diceFactory;

        private readonly List<DicePresenter> _dicePresenters = new();

        /// <summary>
        /// CombatManagerを初期化します。
        /// </summary>
        [Inject]
        public void Initialize(CardLifecycleService cardLifecycleService, CardSlotManager cardSlotManager,
                               PlayerCardDataProvider playerCardDataProvider, EnemyCardDataProvider enemyCardDataProvider,
                               ViewRegistry viewRegistry, DiceManager diceManager)
        {
            _cardLifecycleService = cardLifecycleService;
            _cardSlotManager = cardSlotManager;
            _playerCardDataProvider = playerCardDataProvider;
            _enemyCardDataProvider = enemyCardDataProvider;
            _viewRegistry = viewRegistry;
            _diceManager = diceManager;
            _diceFactory = new DiceFactory();
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
                CreatureCardView cardView = _viewRegistry.GetNextAvailableCreatureCardView(); // 利用可能なViewを取得
                if (cardView == null)
                {
                    Debug.LogError("利用可能なCreatureCardViewが見つかりません。シーンに十分な数のカードが配置されているか確認してください。");
                    break; // エラーなのでループを抜ける
                }
                _cardLifecycleService.InitializeCard(cardView, initData); // 既存のViewを初期化

                // TODO: 適切な空きスロットを探すロジックを実装する
                // 現状は仮でGetNextEmptyHandSlot()を使用するが、これはハンドスロット専用
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
//            SpawnNewWave(1);

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
                    var dicePresenter = new DicePresenter(diceData, diceView, _diceManager, _viewRegistry);
                    _dicePresenters.Add(dicePresenter);
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
            List<CardInitializationData> enemyInitList = _enemyCardDataProvider.GetCardDataListForWave(waveNumber);
            foreach (CardInitializationData initData in enemyInitList)
            {
                CreatureCardView cardView = _viewRegistry.GetNextAvailableCreatureCardView(); // 利用可能なViewを取得
                if (cardView == null)
                {
                    Debug.LogError("利用可能なCreatureCardViewが見つかりません。シーンに十分な数のカードが配置されているか確認してください。");
                    break; // エラーなのでループを抜ける
                }
                _cardLifecycleService.InitializeCard(cardView, initData); // 既存のViewを初期化

                // TODO: 適切な空きスロットを探すロジックを実装する
                // 現状は仮でGetNextEmptyHandSlot()を使用するが、これはハンドスロット専用
                CardSlotData targetSlot = _cardSlotManager.GetNextEmptyHandSlot();
                if (targetSlot != null)
                {
                    _cardSlotManager.PlaceCardAsSystem(cardView.GetObjectId(), targetSlot.SlotId);
                }
                else
                {
                    Debug.LogWarning($"No empty slot found for enemy card {cardView.name}.");
                }
            }
        }

        // TODO: 戦闘終了処理、ターン管理、イベント処理など、他の戦闘ロジックを追加
    }
}

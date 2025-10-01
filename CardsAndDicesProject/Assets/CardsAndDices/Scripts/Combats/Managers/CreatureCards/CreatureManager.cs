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
    [CreateAssetMenu(fileName = "CreatureManager", menuName = "CardsAndDices/Combats/Managers/CreatureCards/CreatureManager")]
    public class CreatureManager : ScriptableObject, IDisposable
    {
        private ICardDataProvider _playerCardDataProvider;
        private GameEventBus _eventBus;
        private IdentifiableViewRegistry _viewRegistry;
        private readonly List<CompositeObjectId> _compositeObjectIds = new();

        /// <summary>
        /// 初期化します。
        /// </summary>
        [Inject]
        public void Initialize(ICardDataProvider playerCardDataProvider, GameEventBus eventBus, IdentifiableViewRegistry viewRegistry)
        {
            _compositeObjectIds.Clear();
            _playerCardDataProvider = playerCardDataProvider;
            _eventBus = eventBus;
            _viewRegistry = viewRegistry;
            _eventBus.On<CombatPhasePlayerCardinitializedEvent>(OnCombatPhasePlayerCardinitialized);
            _eventBus.On<CombatPhasePlayerCardOnScreenEvent>(OnCombatPhasePlayerCardOnScreen);
        }
        public void Dispose()
        {
            _eventBus.Off<CombatPhasePlayerCardinitializedEvent>(OnCombatPhasePlayerCardinitialized);
            _eventBus.Off<CombatPhasePlayerCardOnScreenEvent>(OnCombatPhasePlayerCardOnScreen);
        }

        /// <summary>
        /// 戦闘フィールドを初期化し、戦闘を開始します。
        /// プレイヤーカードと最初のウェーブのエネミーカードを生成・配置します。
        /// </summary>
        private void OnCombatPhasePlayerCardinitialized(CombatPhasePlayerCardinitializedEvent evt)
        {
            // プレイヤーカードの生成と配置
            List<CardInitializationData> playerInitList = _playerCardDataProvider.GetCardDataList();

            foreach (CardInitializationData initData in playerInitList)
            {
                var view = _viewRegistry.GetNonBoundView<CreatureCardView>();
                if (view is null)
                {
                    Debug.LogWarning("viewが取れない");
                    return;
                }
                _compositeObjectIds.Add(view.CompositeObjectId);
                _eventBus.Emit(new CreateCreatureEvent(view.CompositeObjectId, initData));
                _eventBus.Emit(new UpdateDisplayCreatureStatusEvent(view.CompositeObjectId));
            }
        }

        /// <summary>
        /// OnStateEndDragが発生したさいのコマンドを処理します。
        /// </summary>
        private async void OnCombatPhasePlayerCardOnScreen(CombatPhasePlayerCardOnScreenEvent evt)
        {
            foreach (CompositeObjectId id in _compositeObjectIds)
            {
                _eventBus.Emit(new PlacedHandSlotEvent(id));
                _eventBus.Emit(new DisplayOnScreenEvent(id));

                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            }
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }
    }
}

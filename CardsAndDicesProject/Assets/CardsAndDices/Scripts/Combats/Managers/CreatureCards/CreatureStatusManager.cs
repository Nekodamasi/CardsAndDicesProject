using System.Collections.Generic;
using UnityEngine;
using VContainer;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// 全てのクリーチャーカードの状態管理などを担当するマネージャークラス。
    /// </summary>
    [CreateAssetMenu(fileName = "CreatureStatusManager", menuName = "CardsAndDices/Combats/Managers/CreatureCards/CreatureStatusManager")]
    public class CreatureStatusManager : ScriptableObject, IDisposable, ICreatureStatusInstanceRepository, IIdentifiableManager
    {
        [Header("Components")]

        [SerializeField] private List<CreatureStatusIconData> _reatureStatusIconDataList = new();
        private readonly List<CreatureStatusInstance> _creatureStatusInstances = new();
        private readonly List<CreatureStatusPresenter> _creatureStatusPresenters = new();
        private readonly List<CreatureCardStatusIconController> _creatureCardStatusIconControllers = new();
        private GameEventBus _eventBus;
        private ITargetManager _iTargetManager;
        private IdentifiableViewRegistry _viewRegistry;
        private IEffectValue _iEffectValue;
        private IAbilityCheck _iAbilityCheck;
        private CreatureAttackService _creatureAttackService;
        private CreatureTurnEndExecuteAbilityService _creatureTurnEndExecuteAbilityService;

        [Inject]
        public void Initialize(GameEventBus eventBus, ITargetManager iTargetManager, IdentifiableViewRegistry viewRegistry, IEffectValue iEffectValue, IAbilityCheck iAbilityCheck)
        {
            DisposeInstances();
            DisposePresenters();
            _eventBus = eventBus;
            _iTargetManager = iTargetManager;
            _viewRegistry = viewRegistry;
            _iEffectValue = iEffectValue;
            _iAbilityCheck = iAbilityCheck;
            _eventBus.On<CreateCreatureEvent>(OnCreateCreature);
            _eventBus.On<CombatPhasePlayerCardOnScreenEvent>(OnCombatPhasePlayerCardOnScreen);
            _eventBus.On<CombatPhaseEnemyCardOnScreenEvent>(OnCombatPhaseEnemyCardOnScreen);
            _eventBus.On<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
            _eventBus.On<ResetCoolDownEvent>(OnResetCoolDown);

            _creatureAttackService = new CreatureAttackService(_iTargetManager, this, _eventBus);
            _creatureTurnEndExecuteAbilityService = new CreatureTurnEndExecuteAbilityService(_iTargetManager, this, _eventBus, _iAbilityCheck);
        }

        public void Dispose()
        {
            DisposeInstances();
            DisposePresenters();
            DisposeControllers();
            _eventBus.Off<CreateCreatureEvent>(OnCreateCreature);
            _eventBus.Off<CombatPhasePlayerCardOnScreenEvent>(OnCombatPhasePlayerCardOnScreen);
            _eventBus.Off<CombatPhaseEnemyCardOnScreenEvent>(OnCombatPhaseEnemyCardOnScreen);
            _eventBus.Off<DisposeByCompositeObjectIdEvent>(OnDisposeByCompositeObjectId);
            _eventBus.Off<ResetCoolDownEvent>(OnResetCoolDown);
        }

        /// <summary>
        /// プレイヤーカードの画面へ配置
        /// </summary>
        private async void OnResetCoolDown(ResetCoolDownEvent evt)
        {
            var list = _creatureStatusInstances.Where(s => s.IsCooldownFinished == true).ToList();
            Debug.Log("ここにきてるのだろうか？:" + list.Count);
            foreach (var instance in list)
            {
                instance.RecalculateStats();
            }
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.6f));
			_eventBus.Emit(new ResetUIStatusEvent());
        }

        /// <summary>
        /// プレイヤーカードの画面へ配置
        /// </summary>
        private async void OnCombatPhasePlayerCardOnScreen(CombatPhasePlayerCardOnScreenEvent evt)
        {
            var list = _creatureStatusInstances.Where(s => s.CreatureDataTeam == Team.Player).ToList();
            foreach (var instance in list)
            {
                _eventBus.Emit(new PlacedHandSlotEvent(instance.CompositeObjectId));
                _eventBus.Emit(new DisplayOnScreenEvent(instance.CompositeObjectId));

                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            }
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        /// <summary>
        /// エネミーカードの画面へ配置
        /// </summary>
        private async void OnCombatPhaseEnemyCardOnScreen(CombatPhaseEnemyCardOnScreenEvent evt)
        {
            var list = _creatureStatusInstances.Where(s => s.CreatureDataTeam == Team.Enemy).ToList();
            foreach (var instance in list)
            {
                _eventBus.Emit(new DisplayOnScreenEvent(instance.CompositeObjectId));

                // 待機
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            }
            // 待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        }

        /// <summary>
        /// クリーチャーの生成イベント
        /// </summary>
        private void OnCreateCreature(CreateCreatureEvent evt)
        {
            var instance = new CreatureStatusInstance(evt.CreatureCardId, evt.CardInitializationData.CreatureData, _iEffectValue, evt.CardInitializationData.CreatureDataTeam);
            _creatureStatusInstances.Add(instance);
            var controller = new CreatureCardStatusIconController(instance, _eventBus, _reatureStatusIconDataList);
            _creatureCardStatusIconControllers.Add(controller);
            var view = _viewRegistry.GetView<CreatureStatusView>(instance.CompositeObjectId);
            if (view is null)
            {
                Debug.LogWarning("ビューがとれない");
                return;
            }
            var presenter = new CreatureStatusPresenter(instance, view, _eventBus);
            _creatureStatusPresenters.Add(presenter);
            view.SetBoundState(true);

            // クリーチャーの固有アビリティのインスタンス化
            foreach (var ability in evt.CardInitializationData.CreatureData.Abilities)
            {
                _eventBus.Emit(new CreateAbilityEvent(instance.CompositeObjectId, ability, null, instance));
            }

            // インレットのインスタンス化
            foreach (var profile in evt.CardInitializationData.InletPackageProfiles)
            {
                _eventBus.Emit(new CreateDiceInletEvent(instance.CompositeObjectId, profile, instance));
            }
            _eventBus.Emit(new SetCurrentHomeStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Normal));
            _eventBus.Emit(new ChangeViewStatusEvent(instance.CompositeObjectId, IdentifiableStatus.Normal));
            _eventBus.Emit(new DisplayStatusViewEvent(instance.CompositeObjectId));
        }

        /// <summary>
        /// インスタンスをDisposeします
        /// </summary>
        private void DisposeInstances()
        {
            foreach (var instance in _creatureStatusInstances)
            {
                instance.Dispose();
            }
            _creatureStatusInstances.Clear();
        }
        /// <summary>
        /// PresenterをDisposeします
        /// </summary>
        private void DisposePresenters()
        {
            foreach (var presenter in _creatureStatusPresenters)
            {
                presenter.Dispose();
            }
            _creatureStatusPresenters.Clear();
        }

        /// <summary>
        /// コントローラーをDisposeします
        /// </summary>
        private void DisposeControllers()
        {
            foreach (var iconController in _creatureCardStatusIconControllers)
            {
                iconController.Dispose();
            }
            _creatureCardStatusIconControllers.Clear();
        }

        /// <summary>
        /// 指定されたIDのインスタンスを返します。
        /// </summary>
        public CreatureStatusInstance GetInstance(CompositeObjectId CompositeObjectId)
        {
            var instance = _creatureStatusInstances
                .Where(c => c.CompositeObjectId == CompositeObjectId)
                .FirstOrDefault();

            return instance;
        }

        /// <summary>
        /// 全てのインスタンスを返します
        /// </summary>
        public 
        List<CreatureStatusInstance> GetInstanceList()
        {
            return _creatureStatusInstances;
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeするイベント
        /// </summary>
        private void OnDisposeByCompositeObjectId(DisposeByCompositeObjectIdEvent evt)
        {
            DisposeByCompositeObjectId(evt.CompositeObjectId);
        }

        /// <summary>
        /// 指定したIDに紐づいたInstanceをDisposeします
        /// </summary>
        public void DisposeByCompositeObjectId(CompositeObjectId compositeObjectId)
        {
            var instance = _creatureStatusInstances.Where(i => i.CompositeObjectId == compositeObjectId).FirstOrDefault();
            if (instance is null)
            {
                return;
            }
            instance.Dispose();
            _creatureStatusInstances.Remove(instance);
            var presenter = _creatureStatusPresenters.Where(p => p.CompositeObjectId == compositeObjectId).FirstOrDefault();
            presenter.Dispose();
            _creatureStatusPresenters.Remove(presenter);
            var controller = _creatureCardStatusIconControllers.Where(c => c.InstanceId == compositeObjectId).FirstOrDefault();
            controller.Dispose();
            _creatureCardStatusIconControllers.Remove(controller);
        }
    }
}

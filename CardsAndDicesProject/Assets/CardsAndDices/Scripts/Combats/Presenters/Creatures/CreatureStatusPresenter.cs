using System;
using UnityEngine;
namespace CardsAndDices
{
    /// <summary>
    /// CreatureCardinstance(Model)とCreatureCardView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class CreatureStatusPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly CreatureStatusInstance _instance;
        private readonly CreatureStatusView _view;
        private readonly GameEventBus _eventBus;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public CreatureStatusPresenter(CreatureStatusInstance instance, CreatureStatusView view, GameEventBus eventBus)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _eventBus.On<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.On<DisplayCreatureBuffEvent>(OnDisplayCreatureBuff);
            _eventBus.On<DisplayCreatureDeBuffEvent>(OnDisplayCreatureDeBuff);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<DisplayCreatureBuffEvent>(OnDisplayCreatureBuff);
            _eventBus.Off<DisplayCreatureDeBuffEvent>(OnDisplayCreatureDeBuff);
        }

        /// <summary>
        /// UIリセットイベント
        /// </summary>
        private void OnResetUIStatus(ResetUIStatusEvent evt)
        {
            // CardPlacement の設定Abilityを更新
            _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.CardPlacement, _instance.CompositeObjectId, null));
            _eventBus.Emit(new UpdateDisplayCreatureStatusEvent(_instance.CompositeObjectId));
        }

        /// <summary>
        /// バフアニメーション
        /// </summary>
        private void OnDisplayCreatureBuff(DisplayCreatureBuffEvent evt)
        {
            if (evt.CreatureCardId != _view.CompositeObjectId) return;
            _view.DisplayBuff();
        }

        /// <summary>
        /// デバフアニメーション
        /// </summary>
        private void OnDisplayCreatureDeBuff(DisplayCreatureDeBuffEvent evt)
        {
            if (evt.CreatureCardId != _view.CompositeObjectId) return;
            _view.DisplayDeBuff();
        }
    }
}

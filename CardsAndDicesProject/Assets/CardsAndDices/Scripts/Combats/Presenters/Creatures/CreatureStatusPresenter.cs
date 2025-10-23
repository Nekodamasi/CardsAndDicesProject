using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
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
            _eventBus.On<ChangeCreatureCurrentValueEvent>(OnChangeCreatureCurrentValue);
            _eventBus.On<DisplayCreatureAttackEvent>(OnDisplayCreatureAttack);
            _eventBus.On<DisplayCreatureReactionEvent>(OnDisplayCreatureReaction);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<ResetUIStatusEvent>(OnResetUIStatus);
            _eventBus.Off<DisplayCreatureBuffEvent>(OnDisplayCreatureBuff);
            _eventBus.Off<DisplayCreatureDeBuffEvent>(OnDisplayCreatureDeBuff);
            _eventBus.Off<ChangeCreatureCurrentValueEvent>(OnChangeCreatureCurrentValue);
            _eventBus.Off<DisplayCreatureAttackEvent>(OnDisplayCreatureAttack);
            _eventBus.Off<DisplayCreatureReactionEvent>(OnDisplayCreatureReaction);
            _view.SetBoundState(false);
        }

        /// <summary>
        /// リアクション
        /// </summary>
        private async void OnDisplayCreatureReaction(DisplayCreatureReactionEvent evt)
        {
            if (evt.TargetId != _view.CompositeObjectId) return;
            if (_instance.IsDamage)
            {
                _view.DisplayDamage(null);
            }
            else if (_instance.IsDeath)
            {
                Sequence deathSequence = _view.DisplayDeath(null);
                await deathSequence.AsyncWaitForCompletion();
                _eventBus.Emit(new DisposeByCompositeObjectIdEvent(_instance.CompositeObjectId));
            }
        }

        /// <summary>
        /// クリーチャーアタック
        /// </summary>
        private void OnDisplayCreatureAttack(DisplayCreatureAttackEvent evt)
        {
            if (evt.AttackerId != _view.CompositeObjectId) return;
            _view.DisplayBodySlam(null);
        }

        /// <summary>
        /// ステータスValue変更イベント
        /// </summary>
        private void OnChangeCreatureCurrentValue(ChangeCreatureCurrentValueEvent evt)
        {
            if (evt.CreatureCardId != _view.CompositeObjectId) return;
            _instance.ChangeCurrentValue(evt.EffectTargetType, evt.AddValue);
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
            _view.DisplayBuff(evt.VfxDefinition);
        }

        /// <summary>
        /// デバフアニメーション
        /// </summary>
        private void OnDisplayCreatureDeBuff(DisplayCreatureDeBuffEvent evt)
        {
            if (evt.CreatureCardId != _view.CompositeObjectId) return;
            _view.DisplayDeBuff(evt.VfxDefinition);
        }
    }
}

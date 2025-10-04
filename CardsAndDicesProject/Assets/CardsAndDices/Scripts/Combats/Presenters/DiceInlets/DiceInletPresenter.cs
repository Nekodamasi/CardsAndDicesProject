using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
namespace CardsAndDices
{
    /// <summary>
    /// DiceInstance(Model)とDiceView(View)を1対1で紐づけ、両者の状態を同期させる責務を持つ仲介役。
    /// </summary>
    public class DiceInletPresenter : IDisposable, IIdentifiablePresenter
    {
        private readonly DiceInletInstance _instance;
        private readonly DiceInletView _view;
        private readonly GameEventBus _eventBus;
        private readonly CompositeObjectIdTypeEntity _compositeObjectIdTypeEntity;

        /// <summary>
        /// インスタンス側のID
        /// </summary>
        public CompositeObjectId InstanceId => _instance.CompositeObjectId;

        /// <summary>
        /// ビュー側のID
        /// </summary>
        public CompositeObjectId CompositeObjectId => _view.CompositeObjectId;

        public DiceInletPresenter(DiceInletInstance instance, DiceInletView view, GameEventBus eventBus, CompositeObjectIdTypeEntity compositeObjectIdTypeEntity)
        {
            _instance = instance;
            _view = view;
            _eventBus = eventBus;
            _compositeObjectIdTypeEntity = compositeObjectIdTypeEntity;
            _eventBus.On<DiceBeginDragEvent>(OnDiceBeginDrag);
            _eventBus.On<DiceDropInInletEvent>(OnDiceDropInInlet);
        }
        /// <summary>
        /// 関連付けを解除し、Viewをプールに返却します。
        /// </summary>
        public void Dispose()
        {
            _eventBus.Off<DiceBeginDragEvent>(OnDiceBeginDrag);
            _eventBus.Off<DiceDropInInletEvent>(OnDiceDropInInlet);
        }

        /// <summary>
        /// ドラッグされたのがダイス
        /// </summary>
        private void OnDiceBeginDrag(DiceBeginDragEvent evt)
        {
            // ドラッグされたダイスが受け入れられるか
            if (!_instance.ChkFaceAllowed(evt.DiceValue)) return;
            Debug.Log("ここはきている？" + _instance.ChkFaceAllowed(evt.DiceValue));

            _eventBus.Emit(new ChangeViewStatusEvent(_instance.CompositeObjectId, IdentifiableStatus.Acceptable));
            _eventBus.Emit(new DisplayStatusViewEvent(_instance.CompositeObjectId));
            _view.DisplayAcceptableStatus();
        }

        /// <summary>
        /// ダイスをドロップされた
        /// </summary>
        private async void OnDiceDropInInlet(DiceDropInInletEvent evt)
        {
            // 自分以外は無視する
            if (evt.InletId != _instance.CompositeObjectId) return;

            if (_instance.AcceptableDice(evt.DiceValue))
            {
                // インレット発動ならフェーズをDiceInletEffectPhaseに変更
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                _eventBus.Emit(new ChangeCombatPhaseEvent(CombatPhase.DiceInletEffectPhase));

                if (_instance.InletEffectType == InletEffectType.AbilityExecutor)
                {
                    //abilityチェック
                    _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.Inlet, _instance.CompositeObjectId.Owner, _instance.CompositeObjectId));
                }
                else
                {
                    //abilityロック
                    _eventBus.Emit(new UpdateAbilityLockEvent(_instance.CompositeObjectId, true));
                    _instance.SetIsLock(true);
                }
                return;
            }

            // reset
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            _eventBus.Emit(new ResetUIStatusEvent());
        }
    }
}

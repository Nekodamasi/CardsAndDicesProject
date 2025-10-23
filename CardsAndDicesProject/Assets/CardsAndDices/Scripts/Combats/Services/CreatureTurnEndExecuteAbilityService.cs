using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// ターンエンド時のアビリティ実行ロジックを担当するサービスクラスです。
    /// </summary>
    public class CreatureTurnEndExecuteAbilityService : IDisposable
    {
        private ITargetManager _iTargetManager;
        private ICreatureStatusInstanceRepository _iCreatureStatusInstanceRepository;
        private GameEventBus _eventBus;
        private IAbilityCheck _iAbilityCheck;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureTurnEndExecuteAbilityService(ITargetManager iTargetManager, ICreatureStatusInstanceRepository iCreatureStatusInstanceRepository, GameEventBus gameEventBus, IAbilityCheck iAbilityCheck)
        {
            _iTargetManager = iTargetManager;
            _iCreatureStatusInstanceRepository = iCreatureStatusInstanceRepository;
            _eventBus = gameEventBus;
            _iAbilityCheck = iAbilityCheck;
            _eventBus.On<CreatureTurnEndExecuteAbilityEvent>(OnCreatureTurnEndExecuteAbility);

        }

        /// <summary>
        /// TurnEndAbilityの実行
        /// </summary>
        private async void OnCreatureTurnEndExecuteAbility(CreatureTurnEndExecuteAbilityEvent evt)
        {
            var ids = _iTargetManager.GetActionOrderList();
            foreach (var id in ids)
            {
                var instance = _iCreatureStatusInstanceRepository.GetInstance(id);
                if (!instance.IsTurnEndAbilityBuffDebuff)
                {
                    instance.SetIsTurnEndAbilityBuffDebuff(true);
                    bool flg = _iAbilityCheck.HasExecutableAbility(instance.CompositeObjectId, null, ActivationTiming.TurnEndBuffDebuff);
                    if (flg)
                    {
                        _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.TurnEndBuffDebuff, instance.CompositeObjectId, null));
                        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                    }

                    flg = _iAbilityCheck.HasExecutableAbility(instance.CompositeObjectId, null, ActivationTiming.TurnEndAttack);
                    if (flg)
                    {
                        Debug.Log("じゃあここはきどうしてるってこと？！");
                        _eventBus.Emit(new ExecuteAbilityEffectEvent(ActivationTiming.TurnEndAttack, instance.CompositeObjectId, null));
                        return;
                    }
                }
            }
            Debug.Log("ダイスロールよんでるよ");
            _eventBus.Emit(new CombatPhaseSetUpUserDiceEvent());
            
        }

        public void Dispose()
        {
            //            _gameEventBus.Off<CoolDownZeoAttackEvent>(OnCoolDownZeoAttack);
        }

    }
}

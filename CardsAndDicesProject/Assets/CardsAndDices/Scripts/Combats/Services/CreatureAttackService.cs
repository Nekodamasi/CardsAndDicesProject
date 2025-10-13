using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace CardsAndDices
{
    /// <summary>
    /// 攻撃処理の計算ロジックを担当するサービスクラスです。
    /// </summary>
    public class CreatureAttackService : IDisposable
    {
        private ITargetManager _iTargetManager;
        private ICreatureStatusInstanceRepository _iCreatureStatusInstanceRepository;
        private GameEventBus _gameEventBus;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CreatureAttackService(ITargetManager iTargetManager, ICreatureStatusInstanceRepository iCreatureStatusInstanceRepository, GameEventBus gameEventBus)
        {
            _iTargetManager = iTargetManager;
            _iCreatureStatusInstanceRepository = iCreatureStatusInstanceRepository;
            _gameEventBus = gameEventBus;
            _gameEventBus.On<CoolDownZeoAttackEvent>(OnCoolDownZeoAttack);
            _gameEventBus.On<CoolDownStartEvent>(OnCoolDownStart);
            _gameEventBus.On<CreatureAttackEvent>(OnCreatureAttack);
            
        }
        public void Dispose()
        {
            _gameEventBus.Off<CoolDownZeoAttackEvent>(OnCoolDownZeoAttack);
            _gameEventBus.Off<CoolDownStartEvent>(OnCoolDownStart);
            _gameEventBus.Off<CreatureAttackEvent>(OnCreatureAttack);
        }

        /// <summary>
        /// クリーチャーの攻撃
        /// </summary>
        private async void OnCreatureAttack(CreatureAttackEvent evt)
        {
            // １アクション攻撃処理を実行
            await CreateAttack(evt.CreateAttackContext);
            _gameEventBus.Emit(new CreatureAttackEndEvent());
        }

        /// <summary>
        /// クールダウン開始イベント
        /// </summary>
        private async void OnCoolDownStart(CoolDownStartEvent evt)
        {
            var ids = _iTargetManager.GetActionOrderList();
            foreach (var id in ids)
            {
                var instance = _iCreatureStatusInstanceRepository.GetInstance(id);
                instance.ChangeCurrentValue(EffectTargetType.Cooldown, -1);
                _gameEventBus.Emit(new UpdateDisplayCreatureStatusEvent(id));
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
            CoolDownZeoAttack();
        }

        /// <summary>
        /// クールダウン０イベント
        /// </summary>
        private void OnCoolDownZeoAttack(CoolDownZeoAttackEvent evt)
        {
            CoolDownZeoAttack();
        }

        /// <summary>
        /// リセットアタックフラグ
        /// </summary>
        private void ResetAttackFlgs()
        {
            var instances = _iCreatureStatusInstanceRepository.GetInstanceList();
            foreach (var instance in instances)
            {
                instance.ResetAttackFlgs();
            }
        }

        /// <summary>
        /// クールダウン０
        /// </summary>
        private async void CoolDownZeoAttack()
        {
            var cooldownzeroId = _iTargetManager.GetActionOrderCoolDownZeroId();

            // クールダウン０処理待ちなし
            if (cooldownzeroId is null)
            {
                _gameEventBus.Emit(new CoolDownZeoAttackEndEvent());
                return;
            }

            var attacker = _iCreatureStatusInstanceRepository.GetInstance(cooldownzeroId);
            attacker.OnCooldownFinished();

            CreatureAttackContext creatureAttackContext = new CreatureAttackContext(
                                                                                cooldownzeroId,
                                                                                attacker.HitsPerMainAttack,
                                                                                attacker.MainAttackAoE,
                                                                                attacker.MainAttackScoresType,
                                                                                0);

            // １アクション攻撃処理を実行
            await CreateAttack(creatureAttackContext);
            _gameEventBus.Emit(new CoolDownZeoAttackEvent());
        }

        /// <summary>
        /// １アクション分の戦闘処理とアニメーション
        /// </summary>
        private async UniTask CreateAttack(CreatureAttackContext createAttackContext)
        {
            for (var i = 0; i < createAttackContext.HitsPerAttack; i++)
            {
                await PerformAttack(createAttackContext);
            }
        }

        /// <summary>
        /// １回の攻撃分の処理を行う
        /// </summary>
        private async UniTask PerformAttack(CreatureAttackContext createAttackContext)
        {
            var attacker = _iCreatureStatusInstanceRepository.GetInstance(createAttackContext.AttackerId);
            var attackValue = attacker.GetToTargetStatus(createAttackContext.AttackEffectTargetType) + createAttackContext.AddAttackPoint;
            attacker.SetIsAttacker(true);

            var targetIds = _iTargetManager.GetTargetList(createAttackContext.AreaOfEffect, createAttackContext.AttackerId);
            foreach (var id in targetIds)
            {
                var target = _iCreatureStatusInstanceRepository.GetInstance(id);
                target.TakeDamage(attackValue);
            }

            // 攻撃の演出
            await PerformAttackAnimation(createAttackContext.AttackerId, targetIds);
            ResetAttackFlgs();
        }

        /// <summary>
        /// １回の攻撃分のアニメーションを実施します
        /// </summary>
        private async UniTask PerformAttackAnimation(CompositeObjectId attackerId, List<CompositeObjectId> targetIds)
        {
            // アタックアニメーション
            _gameEventBus.Emit(new DisplayCreatureAttackEvent(attackerId));
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            foreach (var id in targetIds)
            {
                _gameEventBus.Emit(new DisplayCreatureReactionEvent(id));
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }
    }
}

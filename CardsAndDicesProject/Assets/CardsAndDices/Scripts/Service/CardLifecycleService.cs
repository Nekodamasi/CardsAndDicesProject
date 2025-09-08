using UnityEngine;
using VContainer;
using System.Collections.Generic;

namespace CardsAndDices
{
    /// <summary>
    /// カードの生成、初期化、破棄など、ライフサイクル全般を管理するサービス。
    /// </summary>
    [CreateAssetMenu(fileName = "CardLifecycleService", menuName = "CardsAndDices/Services/CardLifecycleService")]
    public class CardLifecycleService : ScriptableObject
    {
        private CreatureManager _creatureManager;
        private DiceInletManager _diceInletManager;
        private AbilityManager _abilityManager;
        private EffectManager _effectManager;
        private SpriteCommandBus _commandBus;

        [Inject]
        public void Initialize(CreatureManager creatureManager, DiceInletManager diceInletManager, AbilityManager abilityManager, SpriteCommandBus commandBus, EffectManager effectManager)
        {
            _creatureManager = creatureManager;
            _diceInletManager = diceInletManager;
            _abilityManager = abilityManager;
            _commandBus = commandBus;
            _effectManager = effectManager;

            _commandBus.On<PerformAttackedCommand>(OnPerformAttacked);
        }

        /// <summary>
        /// 既存のカードViewを初期化し、能力を登録します。
        /// </summary>
        /// <param name="cardView">初期化するCreatureCardViewのインスタンス。</param>
        /// <param name="initData">カード生成に必要な情報。</param>
        public void InitializeCard(CreatureCardView cardView, CardInitializationData initData)
        {
            // クリーチャーのスポーン
            _creatureManager.SpawnCreature(initData.CreatureData, cardView);
            cardView.SetSpawnedState(true);

            // 外観の更新
            cardView.SetAppearance(initData.Appearance, initData.CreatureData.CreatureId);

            // アビリティのスポーン
            var Abilities = initData.CreatureData.Abilities;
            for (int i = 0; i < Abilities.Count; i++)
            {
                Debug.Log("<color=Blue>ability:</color>" + cardView.GetCurrentCardId() + "_" + Abilities[i].Id);
                _abilityManager.RegisterAbility(Abilities[i], cardView.GetCurrentCardId(), null);
            }

            // インレット能力をRegistryに登録
            var inletViews = cardView.GetInletViews();
            if (inletViews.Count != initData.InletAbilityProfiles.Count)
            {
                Debug.LogError($"CardLifecycleService: インレットの数({inletViews.Count})とプロファイルの数({initData.InletAbilityProfiles.Count})が一致しません。");
                // エラーハンドリング: 不一致の場合の挙動を定義する
            }

            for (int i = 0; i < inletViews.Count; i++)
            {
                inletViews[i].SetSpawnedState(true);
                inletViews[i].SetDisplayActive(true);
                var profile = initData.InletAbilityProfiles[i];
                _diceInletManager.CreateAndRegisterInlet(inletViews[i], cardView.GetObjectId(), profile);
                //                Debug.Log("<color=red>いんれっと；</color>" + cardView._cardName + "_" + profile.Condition.DiceInletConditionId);
            }
        }

        /// <summary>
        /// カードの能力登録を解除します。
        /// </summary>
        /// <param name="cardView">解除するカードのView。</param>
        public void TeardownCard(CreatureCardView cardView)
        {
            var creatureId = cardView.GetObjectId();
            RemoveCreature(creatureId);
        }
        private void RemoveCreature(CompositeObjectId creatureId)
        {
            // クリーチャーインスタンスのRemove
            var creature = _creatureManager.GetCreature(creatureId);
            if (creature != null)
            {
                _creatureManager.RemoveCreature(creatureId);
            }

            // インレットインスタンスのRemove
            _diceInletManager.RemoveInletsByCreatureId(creatureId);

            // abilityのRemove
            _abilityManager.UnregisterAbilitiesForOwner(creatureId);

            // effectのRemove
            _effectManager.RemoveEffectsByCreatureId(creatureId);
        }
        private void OnPerformAttacked(PerformAttackedCommand command)
        {
            // 攻撃コマンドの完了を待つ（UniTaskなどで遅延を入れるか、コマンドの完了通知を待つ）
            // この例では、簡略化のため即時実行

            var deadCreatureIds = new List<CompositeObjectId>();
            var allCreatures = _creatureManager.GetAllCreatures();

            foreach (var creature in allCreatures)
            {
                if (creature.IsDeath)
                {
                    deadCreatureIds.Add(creature.Id);
                }
            }

            foreach (var id in deadCreatureIds)
            {
                RemoveCreature(id);
            }
        }
    }
}
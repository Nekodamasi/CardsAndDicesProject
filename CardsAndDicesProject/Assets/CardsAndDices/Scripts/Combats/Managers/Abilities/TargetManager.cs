using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
namespace CardsAndDices
{
    /// <summary>
    /// 効果範囲（AreaOfEffect）に基づいて、ターゲットのリストを解決する責務を持つScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "TargetManager", menuName = "CardsAndDices/Combats/Managers/Abilities/TargetManager")]
    public class TargetManager : ScriptableObject, ITargetManager
    {
        [Header("Components")]
        private CreatureCardSlotManager _creatureCardSlotManager;
        private ICreatureStatusInstanceRepository _iCreatureStatusInstanceRepository;

        [Inject]
        public void Initialize(CreatureCardSlotManager creatureCardSlotManager, ICreatureStatusInstanceRepository iCreatureStatusInstanceRepository)
        {
            _creatureCardSlotManager = creatureCardSlotManager;
            _iCreatureStatusInstanceRepository = iCreatureStatusInstanceRepository;
        }

        /// <summary>
        /// 基本的な行動順序に則ってソートされたカードIDの中で、CoolDown処理待ちのIDを取得します。
        /// </summary>
        public CompositeObjectId GetActionOrderCoolDownZeroId()
        {
            var ids = GetActionOrderList();
            foreach (var id in ids)
            {
                var instance = _iCreatureStatusInstanceRepository.GetInstance(id);
                if (instance.IsCooldownFinished == false && instance.CurrentCooldown == 0)
                {
                    return id;
                }
            }
            return null;
        }

        /// <summary>
        /// 基本的な行動順序に則ってソートされたカードIDのリストを取得します
        /// </summary>
        public List<CompositeObjectId> GetActionOrderList()
        {
            var list = _creatureCardSlotManager.GetNonHandInstanceList();
            var sortedSlots = list
                .Where(slot => slot.IsOccupied)
                .OrderBy(slot => slot.Team == Team.Enemy ? 0 : 1) // Enemy first
                .ThenBy(slot => slot.Location);

            List<CompositeObjectId> ids = new();
            foreach (var slot in sortedSlots)
            {
                ids.Add(slot.ReflowPlacedCardId);
            }

            return ids;
        }

        /// <summary>
        /// 指定された実行者と効果範囲に基づいて、ターゲットとなるカードのIDリストを取得します。
        /// </summary>
        /// <param name="areaOfEffect">効果範囲の定義。</param>
        /// <param name="executorId">効果の実行者のID。</param>
        /// <returns>ターゲットとなるカードのCompositeObjectIdのリスト。</returns>
        public List<CompositeObjectId> GetTargetList(AreaOfEffect areaOfEffect, CompositeObjectId executorId)
        {
            if (_creatureCardSlotManager == null)
            {
                Debug.LogError("[TargetManager] CreatureCardSlotManagerがインジェクトされていません。");
                return new List<CompositeObjectId>();
            }

            // 実行者のスロット情報を取得します。多くの効果範囲の基点となります。
            // 実装メモ: CreatureCardSlotManagerには、カードIDからスロット情報（位置や所有者など）を逆引きする機能が必要です。
            var executorSlot = _creatureCardSlotManager.GetInstanceInReflowPlaced(executorId);
            if (executorSlot == null)
            {
                // 実行者が盤面にいない場合、ほとんどのターゲティングは不可能です。
                Debug.LogWarning($"[TargetManager] 実行者（ID: {executorId}）が盤面に見つかりません。");
                return new List<CompositeObjectId>();
            }

            switch (areaOfEffect)
            {
                case AreaOfEffect.Self:
                    return new List<CompositeObjectId> { executorId };
                /*
                                case AreaOfEffect.All:
                                    // 実装メモ: CreatureCardSlotManagerには、盤面上の全てのカードIDを取得する機能が必要です。
                                    return _creatureCardSlotManager.GetAllCardIdsOnBoard();

                                case AreaOfEffect.AllEnemies:
                                    // 実装メモ: CreatureCardSlotManagerには、指定したプレイヤーの敵対プレイヤーが所有する全てのカードIDを取得する機能が必要です。
                                    return _creatureCardSlotManager.GetEnemyCardIds(executorSlot.OwnerPlayerId);

                                case AreaOfEffect.AllAllies:
                                    // 実装メモ: CreatureCardSlotManagerには、指定したプレイヤーが所有する全てのカードIDを取得する機能が必要です。
                                    return _creatureCardSlotManager.GetAlliedCardIds(executorSlot.OwnerPlayerId);

                                // --- 以下、より複雑な範囲指定のサンプル ---
                                /*
                                case AreaOfEffect.Front:
                                    // 実装メモ: CreatureCardSlotManagerには、指定したスロットの正面にあるスロットのカードIDを取得する機能が必要です。
                                    return _creatureCardSlotManager.GetCardIdInOpposingSlot(executorSlot);

                                case AreaOfEffect.SameRow:
                                    // 実装メモ: CreatureCardSlotManagerには、指定したスロットと同じ行にある全てのカードIDを取得する機能が必要です。
                                    return _creatureCardSlotManager.GetCardIdsInSameRow(executorSlot);
                                */

                default:
                    Debug.LogWarning($"[TargetManager] 未対応のAreaOfEffectです: {areaOfEffect}");
                    return new List<CompositeObjectId>();
            }
        }
    }
}

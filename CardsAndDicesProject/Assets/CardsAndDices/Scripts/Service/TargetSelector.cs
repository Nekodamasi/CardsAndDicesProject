
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardsAndDices
{
    /// <summary>
    /// 攻撃やスキルの対象を選択する責務を持つクラス。
    /// </summary>
    public class TargetSelector
    {
        private readonly CardSlotManager _cardSlotManager;

        /// <summary>
        /// TargetSelectorを初期化します。
        /// </summary>
        /// <param name="cardSlotManager">カードスロットを管理するマネージャー。</param>
        public TargetSelector(CardSlotManager cardSlotManager)
        {
            _cardSlotManager = cardSlotManager ?? throw new ArgumentNullException(nameof(cardSlotManager));
        }

        /// <summary>
        /// 効果範囲に基づき、ターゲットとなるクリーチャーカードのIDリストを取得します。
        /// </summary>
        /// <param name="sourceId">攻撃元のクリーチャーカードのID。</param>
        /// <param name="areaOfEffect">効果範囲。</param>
        /// <returns>ターゲットのCompositeObjectIdのリスト。</returns>
        public List<CompositeObjectId> SelectTargets(CompositeObjectId sourceId, AreaOfEffect areaOfEffect)
        {
            var sourceSlotData = _cardSlotManager.GetSlotDataByPlacedCardId(sourceId);
            if (sourceSlotData == null)
            {
                Debug.Log("ほげえええええええええええええ");
                return new List<CompositeObjectId>(); // 攻撃元が見つからない
            }

            var sourceTeam = sourceSlotData.Team;
            var opponentTeam = sourceTeam == Team.Player ? Team.Enemy : Team.Player;

            switch (areaOfEffect)
            {
                case AreaOfEffect.Self:
                    return new List<CompositeObjectId> { sourceId };

                case AreaOfEffect.HostileCreature:
                    return SelectHostileCreature(sourceSlotData, opponentTeam);

                case AreaOfEffect.TeammateLine:
                    return SelectLineTargets(sourceSlotData, sourceTeam);

                case AreaOfEffect.OpponentLine:
                    return SelectLineTargets(sourceSlotData, opponentTeam);

                case AreaOfEffect.Teammate:
                    return SelectAllTeamTargets(sourceTeam);

                case AreaOfEffect.Opponent:
                    return SelectAllTeamTargets(opponentTeam);

                default:
                    throw new ArgumentOutOfRangeException(nameof(areaOfEffect), $"未対応の効果範囲です: {areaOfEffect}");
            }
        }

        /// <summary>
        /// 相手チームの最前衛のクリーチャーカードを1体選択します。
        /// </summary>
        private List<CompositeObjectId> SelectHostileCreature(CardSlotData sourceSlotData, Team opponentTeam)
        {
            var opponentSlots = _cardSlotManager.GetAllSlots()
                .Where(slot => slot.Team == opponentTeam && slot.IsOccupied)
                .ToList();

            if (!opponentSlots.Any())
            {
                return new List<CompositeObjectId>();
            }

            // 1. 自分と同じラインのVanguard -> Center -> Rearを優先
            var sameLineSlots = opponentSlots
                .Where(slot => slot.Line == sourceSlotData.Line)
                .OrderBy(slot => (int)slot.Location);

            var target = sameLineSlots.FirstOrDefault();
            if (target != null)
            {
                return new List<CompositeObjectId> { target.PlacedCardId };
            }

            // 2. 同じラインに敵がいない場合、もう一方のラインのVanguard -> Center -> Rearを検索
            var otherLineSlots = opponentSlots
                .Where(slot => slot.Line != sourceSlotData.Line)
                .OrderBy(slot => (int)slot.Location);

            target = otherLineSlots.FirstOrDefault();
            if (target != null)
            {
                return new List<CompositeObjectId> { target.PlacedCardId };
            }

            return new List<CompositeObjectId>();
        }

        /// <summary>
        /// 指定されたチームの、指定されたラインにいる全てのクリーチャーを選択します。
        /// </summary>
        private List<CompositeObjectId> SelectLineTargets(CardSlotData sourceSlotData, Team targetTeam)
        {
            return _cardSlotManager.GetAllSlots()
                .Where(slot => slot.Team == targetTeam &&
                               slot.Line == sourceSlotData.Line &&
                               slot.IsOccupied)
                .Select(slot => slot.PlacedCardId)
                .ToList();
        }

        /// <summary>
        /// 指定されたチームの全てのクリーチャーを選択します。
        /// </summary>
        private List<CompositeObjectId> SelectAllTeamTargets(Team targetTeam)
        {
            return _cardSlotManager.GetAllSlots()
                .Where(slot => slot.Team == targetTeam && slot.IsOccupied)
                .Select(slot => slot.PlacedCardId)
                .ToList();
        }
    }
}

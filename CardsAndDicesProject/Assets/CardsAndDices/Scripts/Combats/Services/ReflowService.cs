using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace CardsAndDices
{
    /// <summary>
    /// カードのリフロー（再配置）および前詰め処理の計算ロジックを担当するサービスクラスです。
    /// このクラスは状態を持たず、現在のスロット状態とユーザー操作を基に、
    /// 「どのカードがどこへ移動すべきか」という結果だけを返します。
    /// </summary>
    public class ReflowService
    {
        private ICreatureCardSlotInstanceRepository _iCreatureCardSlotInstanceRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ReflowService(ICreatureCardSlotInstanceRepository iCreatureCardSlotInstanceRepository)
        {
            _iCreatureCardSlotInstanceRepository = iCreatureCardSlotInstanceRepository;
        }

        /// <summary>
        /// ドラッグ＆ドロップ操作におけるリフローの移動情報を計算します。
        /// </summary>
        /// <param name="draggedSlotData">ドラッグ中のカードが元々配置されていたスロットのデータ。</param>
        /// <param name="targetSlotData">ドロップ先のターゲットスロットのデータ。</param>
        /// <param name="draggedCardId">ドラッグ中のカードのID。</param>
        /// <returns>リフロー後の各カードの最終ワールド座標の辞書。</returns>
        public Dictionary<CompositeObjectId, Vector3> CalculateReflowMovements(CreatureCardSlotInstance draggedSlot, CreatureCardSlotInstance targetSlot, CompositeObjectId draggedCardId)
        {
            Dictionary<CompositeObjectId, Vector3> cardMovements = new Dictionary<CompositeObjectId, Vector3>();

            // ターゲットスロットに元々配置されていたカードのID
            CompositeObjectId originalCardInTargetSlot = draggedSlot.ReflowPlacedCardId;

            // 0. 空のスロットに配置する場合
            if (originalCardInTargetSlot == null)
            {
                // ドラッグ元スロットのReflowPlacedCardIdをクリア
                draggedSlot.ReflowPlacedCard(null);
                // ターゲットスロットにドラッグ中のカードを設定
                targetSlot.ReflowPlacedCard(draggedCardId);
            }
            // 1. 隣接スワップの場合
            else if (IsAdjacent(draggedSlot, targetSlot))
            {
                // ターゲットスロットにドラッグ中のカードを設定
                targetSlot.ReflowPlacedCard(draggedCardId);
                // 元々ターゲットスロットにあったカードをドラッグ元スロットの位置へ移動
                cardMovements[originalCardInTargetSlot] = draggedSlot.CreatureCardSlotPosition;
                // ドラッグ元スロットに元々ターゲットスロットにあったカードを設定
                draggedSlot.ReflowPlacedCard(originalCardInTargetSlot);
            }
            // 2. 前押し出しの場合 (同じラインでVanguardからRearへの移動)
            else if (draggedSlot.LinePosition == targetSlot.LinePosition &&
                     draggedSlot.Location == SlotLocation.Vanguard &&
                     targetSlot.Location == SlotLocation.Rear)
            {
                // Centerスロットのデータを取得
                CreatureCardSlotInstance centerSlot = _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, draggedSlot.LinePosition, SlotLocation.Center);

                // １つずつずらして移動情報を記録
                // Centerのカードをドラッグ元スロットの位置へ
                cardMovements.Add(centerSlot.ReflowPlacedCardId, draggedSlot.CreatureCardSlotPosition);
                // RearのカードをCenterスロットの位置へ
                cardMovements.Add(targetSlot.ReflowPlacedCardId, centerSlot.CreatureCardSlotPosition);

                // スロットのReflowPlacedCardIdを更新
                draggedSlot.ReflowPlacedCard(centerSlot.ReflowPlacedCardId);
                centerSlot.ReflowPlacedCard(targetSlot.ReflowPlacedCardId);
                targetSlot.ReflowPlacedCard(draggedCardId);
            }
            // 3. 後ろ押し出しの場合 (上記以外)
            else
            {
                int n = 0;
                List<CreatureCardSlotInstance> Slots = new List<CreatureCardSlotInstance>();
                List<CompositeObjectId> ids = new List<CompositeObjectId>();
                CreatureCardSlotInstance slot = targetSlot;

                // 起点スロットの情報をリストに追加
                Slots.Add(targetSlot);
                ids.Add(targetSlot.ReflowPlacedCardId);

                // 循環順序で次のスロットを探索し、リストに追加
                while (n < 5)
                {
                    // ドラッグ元スロットのリフロー配置カードをクリア
                    draggedSlot.ReflowPlacedCard(null);
                    // 次のスロットを取得
                    slot = GetNextSlotInCircularOrder(targetSlot.LinePosition, slot.LinePosition, slot.Location);
                    if (slot is null) break; // 次のスロットがない場合はループを抜ける
                    ids.Add(slot.ReflowPlacedCardId);
                    Slots.Add(slot);

                    // 未配置またはドラッグカードが配置されたスロットに来たら抜ける
                    if (slot.ReflowPlacedCardId is null) break;
                    n++;
                }
                // 収集した情報に基づいてスロットのReflowPlacedCardIdと移動情報を更新
                for (var i = 0; i < ids.Count; i++)
                {
                    if (i == 0)
                    {
                        // 起点スロットにドラッグ中のカードを設定
                        Slots[i].ReflowPlacedCard(draggedCardId);
                    }
                    else
                    {
                        // 後続のスロットに前のスロットのカードを設定
                        Slots[i].ReflowPlacedCard(ids[i - 1]);
                        if (ids[i - 1] != null)
                        {
                            // 移動情報を記録
                            cardMovements[ids[i - 1]] = Slots[i].CreatureCardSlotPosition;
                        }
                    }
                }
            }
            return cardMovements;
        }

        /// <summary>
        /// 指定されたスロットの次に配置されるべきスロットを、定義された循環順序に従って取得します。
        /// このメソッドは、主に「後ろ押し出し」リフローの計算に使用されます。
        /// </summary>
        /// <param name="startLine">循環の開始ライン（TopまたはBottom）。</param>
        /// <param name="currentLine">現在のスロットのライン。</param>
        /// <param name="currentLocation">現在のスロットの場所。</param>
        /// <returns>次のスロットのCardSlotData。ルールに合致しない場合はnull。</returns>
        public CreatureCardSlotInstance GetNextSlotInCircularOrder(LinePosition startLine, LinePosition currentLine, SlotLocation currentLocation)
        {
            // TopLineから開始する場合の循環順序
            if (startLine == LinePosition.TopLine)
            {
                if (currentLine == LinePosition.TopLine)
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Vanguard:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.TopLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.TopLine, SlotLocation.Rear);
                        case SlotLocation.Rear:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.BottomLine, SlotLocation.Rear);
                    }
                }
                else // currentLine == LinePosition.Bottom
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Rear:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.BottomLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.BottomLine, SlotLocation.Vanguard);
                            // case SlotLocation.Vanguard: ルールにないのでnullが返る
                    }
                }
            }
            // BottomLineから開始する場合の循環順序
            else // startLine == LinePosition.Bottom
            {
                if (currentLine == LinePosition.BottomLine)
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Vanguard:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.BottomLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.BottomLine, SlotLocation.Rear);
                        case SlotLocation.Rear:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.TopLine, SlotLocation.Rear);
                    }
                }
                else // currentLine == LinePosition.TopLineLine
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Rear:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.TopLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _iCreatureCardSlotInstanceRepository.GetInstance(Team.Player, LinePosition.TopLine, SlotLocation.Vanguard);
                            // case SlotLocation.Vanguard: ルールにないのでnullが返る
                    }
                }
            }

            // どのルールにも一致しない場合
            return null;
        }

        /// <summary>
        /// 2つのカードスロットが隣接しているか判定します。
        /// </summary>
        /// <param name="slot1">スロット1のデータ。</param>
        /// <param name="slot2">スロット2のデータ。</param>
        /// <returns>隣接している場合はtrue。</returns>
        private bool IsAdjacent(CreatureCardSlotInstance slot1, CreatureCardSlotInstance slot2)
        {
            // Handスロットは隣接判定の対象外
            if (slot1.LinePosition == LinePosition.Hand || slot2.LinePosition == LinePosition.Hand) return false;

            // 同じスロットは隣接しない
            if (slot1.CompositeObjectId.Equals(slot2.CompositeObjectId)) return false;

            // ケース1: 同じラインの場合
            if (slot1.LinePosition == slot2.LinePosition)
            {
                // SlotLocationのenum値の差の絶対値が1なら隣接
                return Mathf.Abs((int)slot1.Location - (int)slot2.Location) == 1;
            }

            // ケース2: 異なるラインで、同じLocationの場合 (例: PlayerVanguardとEnemyVanguard)
            if (slot1.LinePosition != slot2.LinePosition && slot1.Location == slot2.Location)
            {
                return true;
            }

            // 上記以外は隣接しない
            return false;
        }

        /// <summary>
        /// １つのライン分の前詰処理をします
        /// </summary>
        private void CalculateFrontLoadMovement(Team team, LinePosition line)
        {

            // 各位置のスロットデータを取得
            var rear = _iCreatureCardSlotInstanceRepository.GetInstance(team, line, SlotLocation.Rear);
            var center = _iCreatureCardSlotInstanceRepository.GetInstance(team, line, SlotLocation.Center);
            var vanguard = _iCreatureCardSlotInstanceRepository.GetInstance(team, line, SlotLocation.Vanguard);

            if (vanguard.ReflowPlacedCardId == null)
            {
                vanguard.ReflowPlacedCard(center.ReflowPlacedCardId);
                center.ReflowPlacedCard(null);
            }
            if (center.ReflowPlacedCardId == null)
            {
                center.ReflowPlacedCard(rear.ReflowPlacedCardId);
                rear.ReflowPlacedCard(null);
            }
            // 空,空,有対応のために、最後にもう１度、vanguardをチェック
            if (vanguard.ReflowPlacedCardId == null)
            {
                vanguard.ReflowPlacedCard(center.ReflowPlacedCardId);
                center.ReflowPlacedCard(null);
            }
        }

        /// <summary>
        /// 各ラインで前衛方向に無配置のカードスロットがある場合、そちらに向かって隙間なく詰める移動を計算します。
        /// </summary>
        public void CalculateFrontLoadMovements()
        {
            CalculateFrontLoadMovement(Team.Player, LinePosition.TopLine);
            CalculateFrontLoadMovement(Team.Player, LinePosition.BottomLine);
            CalculateFrontLoadMovement(Team.Enemy, LinePosition.TopLine);
            CalculateFrontLoadMovement(Team.Enemy, LinePosition.BottomLine);
        }
    }
}

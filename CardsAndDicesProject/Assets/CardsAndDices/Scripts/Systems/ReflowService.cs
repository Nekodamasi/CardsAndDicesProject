using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;

namespace CardsAndDice
{
    /// <summary>
    /// カードのリフロー（再配置）および前詰め処理の計算ロジックを担当するサービスクラスです。
    /// このクラスは状態を持たず、現在のスロット状態とユーザー操作を基に、
    /// 「どのカードがどこへ移動すべきか」という結果だけを返します。
    /// </summary>
    [CreateAssetMenu(fileName = "ReflowService", menuName = "CardsAndDice/Systems/ReflowService")]
    public class ReflowService : ScriptableObject
    {
        [SerializeField] private CardSlotStateRepository _repository;
        [SerializeField] private CardSlotDebug _cardSlotDebug;

        /// <summary>
        /// VContainerによるコンストラクタインジェクション。
        /// </summary>
        /// <param name="repository">カードスロットの状態リポジトリ。</param>
        [Inject]
        public void Initialize(CardSlotStateRepository repository, CardSlotDebug cardSlotDebug)
        {
            _repository = repository;
            _cardSlotDebug = cardSlotDebug;
        }

        /// <summary>
        /// 各ラインで前衛方向に無配置のカードスロットがある場合、そちらに向かって隙間なく詰める移動を計算します。
        /// </summary>
        /// <returns>前詰めによって移動するカードのIDと移動先のワールド座標の辞書。</returns>
        public Dictionary<CompositeObjectId, Vector3> CalculateFrontLoadMovements(CompositeObjectId draggedCardId)
        {
            var movements = new Dictionary<CompositeObjectId, Vector3>();

            // TopLineとBottomLineの処理
            foreach (var line in new[] { LinePosition.TopLine, LinePosition.BottomLine })
            {
                if (line == LinePosition.TopLine || line == LinePosition.BottomLine)
                {
                    // 各位置のスロットデータを取得
                    var rear = _repository.GetSlotDataByLocation(line, SlotLocation.Rear);
                    var center = _repository.GetSlotDataByLocation(line, SlotLocation.Center);
                    var vanguard = _repository.GetSlotDataByLocation(line, SlotLocation.Vanguard);

                    // CenterからVanguardへの移動
                    // Centerにカードがあり、Vanguardが空いている場合
                    if (center != null && center.IsOccupied)
                    {
                        if (vanguard != null && !vanguard.IsOccupied)
                        {
                            MoveCard(center, vanguard, movements);
                        }
                    }

                    // RearからCenter/Vanguardへの移動
                    // Rearにカードがあり、Centerが空いている場合
                    if (rear != null && rear.IsOccupied)
                    {
                        if (center != null && !center.IsOccupied)
                        {
                            // Vanguardも空いている場合、RearのカードをVanguardへ移動
                            if (vanguard != null && !vanguard.IsOccupied)
                            {
                                MoveCard(rear, vanguard, movements);
                            }
                            // Vanguardが空いていない場合、RearのカードをCenterへ移動
                            else
                            {
                                MoveCard(rear, center, movements);
                            }
                        }
                    }
                }
            }

            // HandLineの処理
            // HandLineのスロットをLocationでソートして取得
            var handSlots = _repository.GetAllSlots().Where(s => s != null && s.Line == LinePosition.Hand).OrderBy(s => s.Location).ToList();
//            Debug.Log($"HandSlots count: {handSlots.Count}");
            for (int i = 0; i < handSlots.Count; i++)
            {
//                Debug.Log($"HandSlots[{i}]: {handSlots[i]?.SlotId?.ToString() ?? "NULL"}");
                // 現在のスロットがnullでないことを確認
                if (handSlots[i] == null) continue;

                // 現在のスロットにカードが配置されている場合
                if (handSlots[i].IsOccupied)
                {
                    // 現在のスロットより若い番号の空きスロットを探す
                    for (int j = 0; j < i; j++)
                    {
                        // ターゲットスロットがnullでないことを確認
                        if (handSlots[j] == null) continue;

                        if (!handSlots[j].IsOccupied)
                        {
                            // 空きスロットが見つかったら、カードを移動
                            MoveCard(handSlots[i], handSlots[j], movements);
                            break; // 1つ移動したら次のカードへ
                        }
                    }
                }
            }
/*
            // draggedCardIdの移動がmovementsに含まれていない場合、その現在配置への移動を追加
            if (draggedCardId != null && !movements.ContainsKey(draggedCardId))
            {
                CardSlotData currentDraggedCardSlot = _repository.GetSlotDataByReflowPlacedCardId(draggedCardId);
                if (currentDraggedCardSlot != null)
                {
                    movements[draggedCardId] = currentDraggedCardSlot.Position;
                }
            }
            Debug.Log("movementsはいくつー？＞" + movements.Count);
*/
            return movements;
        }

        /// <summary>
        /// カードをあるスロットから別のスロットへ移動させ、移動情報を記録します。
        /// このメソッドは、スロットのPlacedCardIdを直接更新します。
        /// </summary>
        /// <param name="from">移動元のカードスロットデータ。</param>
        /// <param name="to">移動先のカードスロットデータ。</param>
        /// <param name="movements">移動情報を記録する辞書。</param>
        private void MoveCard(CardSlotData from, CardSlotData to, Dictionary<CompositeObjectId, Vector3> movements)
        {
            Debug.Log(from.Line + "_" + from.Location + " -> " + to.Line + "_" + to.Location);

            // 移動元スロットのカードIDと移動先スロットのワールド座標を記録
            movements[from.PlacedCardId] = to.Position;
            // 移動先スロットに移動元のカードを設定
            to.PlacedCardId = from.PlacedCardId;
            to.ReflowPlacedCardId = from.ReflowPlacedCardId;
            // 移動元スロットのカードをクリア
            from.PlacedCardId = null;
            from.ReflowPlacedCardId = null;
        }

        /// <summary>
        /// ドラッグ＆ドロップ操作におけるリフローの移動情報を計算します。
        /// </summary>
        /// <param name="draggedSlotData">ドラッグ中のカードが元々配置されていたスロットのデータ。</param>
        /// <param name="targetSlotData">ドロップ先のターゲットスロットのデータ。</param>
        /// <param name="draggedCardId">ドラッグ中のカードのID。</param>
        /// <returns>リフロー後の各カードの最終ワールド座標の辞書。</returns>
        public Dictionary<CompositeObjectId, Vector3> CalculateReflowMovements(CardSlotData draggedSlotData, CardSlotData targetSlotData, CompositeObjectId draggedCardId)
        {
            Dictionary<CompositeObjectId, Vector3> cardMovements = new Dictionary<CompositeObjectId, Vector3>();

            // ドラッグ元またはターゲットスロットデータがnullの場合は空の辞書を返す
            if (draggedSlotData == null || targetSlotData == null)
            {
                if (draggedSlotData == null)
                {
                    Debug.LogWarning($"draggedSlotData:null->draggedCardId:{draggedCardId}");
                }
                if (targetSlotData == null)
                {
                    Debug.LogWarning("targetSlotData:null");
                }
                return cardMovements;
            }

            // ターゲットスロットに元々配置されていたカードのID
            CompositeObjectId originalCardInTargetSlot = targetSlotData.ReflowPlacedCardId;

            // 0. 空のスロットに配置する場合
            if (originalCardInTargetSlot == null)
            {
                // ドラッグ元スロットのReflowPlacedCardIdをクリア
                draggedSlotData.ReflowPlacedCardId = null;
                // ターゲットスロットにドラッグ中のカードを設定
                targetSlotData.ReflowPlacedCardId = draggedCardId;
            }
            // 1. 隣接スワップの場合
            else if (IsAdjacent(draggedSlotData, targetSlotData))
            {
                // ターゲットスロットにドラッグ中のカードを設定
                targetSlotData.ReflowPlacedCardId = draggedCardId;
                // 元々ターゲットスロットにあったカードをドラッグ元スロットの位置へ移動
                cardMovements[originalCardInTargetSlot] = draggedSlotData.Position;
                // ドラッグ元スロットに元々ターゲットスロットにあったカードを設定
                draggedSlotData.ReflowPlacedCardId = originalCardInTargetSlot;
            }
            // 2. 前押し出しの場合 (同じラインでVanguardからRearへの移動)
            else if (draggedSlotData.Line == targetSlotData.Line &&
                     draggedSlotData.Location == SlotLocation.Vanguard &&
                     targetSlotData.Location == SlotLocation.Rear)
            {
                // Centerスロットのデータを取得
                CardSlotData centerSlotData = _repository.GetSlotDataByLocation(draggedSlotData.Line, SlotLocation.Center);

                // １つずつずらして移動情報を記録
                // Centerのカードをドラッグ元スロットの位置へ
                cardMovements.Add(centerSlotData.ReflowPlacedCardId, draggedSlotData.Position);
                // RearのカードをCenterスロットの位置へ
                cardMovements.Add(targetSlotData.ReflowPlacedCardId, centerSlotData.Position);

                // スロットのReflowPlacedCardIdを更新
                draggedSlotData.ReflowPlacedCardId = centerSlotData.ReflowPlacedCardId;
                centerSlotData.ReflowPlacedCardId = targetSlotData.ReflowPlacedCardId;
                targetSlotData.ReflowPlacedCardId = draggedCardId;
            }
            // 3. 後ろ押し出しの場合 (上記以外)
            else
            {
                int n = 0;
                List<CardSlotData> Slots = new List<CardSlotData>();
                List<CompositeObjectId> ids = new List<CompositeObjectId>();
                CardSlotData slot = targetSlotData;

                // 起点スロットの情報をリストに追加
                Slots.Add(targetSlotData);
                ids.Add(targetSlotData.ReflowPlacedCardId);

                // 循環順序で次のスロットを探索し、リストに追加
                while (n < 5)
                {
                    // ドラッグ元スロットのリフロー配置カードをクリア
                    draggedSlotData.ReflowPlacedCardId = null;
                    // 次のスロットを取得
                    slot = GetNextSlotInCircularOrder(targetSlotData.Line, slot.Line, slot.Location);
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
                        Slots[i].ReflowPlacedCardId = draggedCardId;
                    }
                    else
                    {
                        // 後続のスロットに前のスロットのカードを設定
                        Slots[i].ReflowPlacedCardId = ids[i - 1];
                        if (ids[i - 1] != null)
                        {
                            // 移動情報を記録
                            cardMovements[ids[i - 1]] = Slots[i].Position;
                        }
                    }
                }
            }

            
            return cardMovements;
        }

        /// <summary>
        /// 2つのカードスロットが隣接しているか判定します。
        /// </summary>
        /// <param name="slot1">スロット1のデータ。</param>
        /// <param name="slot2">スロット2のデータ。</param>
        /// <returns>隣接している場合はtrue。</returns>
        private bool IsAdjacent(CardSlotData slot1, CardSlotData slot2)
        {
            // Handスロットは隣接判定の対象外
            if (slot1.Line == LinePosition.Hand || slot2.Line == LinePosition.Hand) return false;

            // 同じスロットは隣接しない
            if (slot1.SlotId.Equals(slot2.SlotId)) return false;

            // ケース1: 同じラインの場合
            if (slot1.Line == slot2.Line)
            {
                // SlotLocationのenum値の差の絶対値が1なら隣接
                return Mathf.Abs((int)slot1.Location - (int)slot2.Location) == 1;
            }

            // ケース2: 異なるラインで、同じLocationの場合 (例: PlayerVanguardとEnemyVanguard)
            if (slot1.Line != slot2.Line && slot1.Location == slot2.Location)
            {
                return true;
            }

            // 上記以外は隣接しない
            return false;
        }

        /// <summary>
        /// 指定されたスロットの次に配置されるべきスロットを、定義された循環順序に従って取得します。
        /// このメソッドは、主に「後ろ押し出し」リフローの計算に使用されます。
        /// </summary>
        /// <param name="startLine">循環の開始ライン（TopまたはBottom）。</param>
        /// <param name="currentLine">現在のスロットのライン。</param>
        /// <param name="currentLocation">現在のスロットの場所。</param>
        /// <returns>次のスロットのCardSlotData。ルールに合致しない場合はnull。</returns>
        public CardSlotData GetNextSlotInCircularOrder(LinePosition startLine, LinePosition currentLine, SlotLocation currentLocation)
        {
            // TopLineから開始する場合の循環順序
            if (startLine == LinePosition.TopLine)
            {
                if (currentLine == LinePosition.TopLine)
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Vanguard:
                            return _repository.GetSlotDataByLocation(LinePosition.TopLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _repository.GetSlotDataByLocation(LinePosition.TopLine, SlotLocation.Rear);
                        case SlotLocation.Rear:
                            return _repository.GetSlotDataByLocation(LinePosition.BottomLine, SlotLocation.Rear);
                    }
                }
                else // currentLine == LinePosition.Bottom
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Rear:
                            return _repository.GetSlotDataByLocation(LinePosition.BottomLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _repository.GetSlotDataByLocation(LinePosition.BottomLine, SlotLocation.Vanguard);
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
                            return _repository.GetSlotDataByLocation(LinePosition.BottomLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _repository.GetSlotDataByLocation(LinePosition.BottomLine, SlotLocation.Rear);
                        case SlotLocation.Rear:
                            return _repository.GetSlotDataByLocation(LinePosition.TopLine, SlotLocation.Rear);
                    }
                }
                else // currentLine == LinePosition.TopLineLine
                {
                    switch (currentLocation)
                    {
                        case SlotLocation.Rear:
                            return _repository.GetSlotDataByLocation(LinePosition.TopLine, SlotLocation.Center);
                        case SlotLocation.Center:
                            return _repository.GetSlotDataByLocation(LinePosition.TopLine, SlotLocation.Vanguard);
                        // case SlotLocation.Vanguard: ルールにないのでnullが返る
                    }
                }
            }

            // どのルールにも一致しない場合
            return null;
        }
        
        /// <summary>
        /// 指定されたラインと場所に対応するカードスロットデータをリポジトリから取得します。
        /// </summary>
        /// <param name="line">スロットのライン。</param>
        /// <param name="location">スロットの場所。</param>
        /// <returns>対応するCardSlotData。見つからない場合はnull。</returns>
        private CardSlotData GetSlotDataByLocation(LinePosition line, SlotLocation location)
        {
            return _repository.GetSlotDataByLocation(line, location);
        }
    }
}

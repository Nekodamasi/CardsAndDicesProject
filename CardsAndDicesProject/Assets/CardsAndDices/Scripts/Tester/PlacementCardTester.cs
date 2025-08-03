using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用するために追加
using System;

namespace CardsAndDice
{
    /// <summary>
    /// ゲームの初期状態をセットアップし、テスト用のカード配置を行うデバッグ用クラス。
    /// </summary>
    public class PlacementCardTester : MonoBehaviour
    {
        [Header("System References")]
        [Tooltip("カードスロットを管理するCardSlotManager")]
        [SerializeField] private CardSlotManager _cardSlotManager;
        [SerializeField] private SpriteCommandBus _commandBus;


        [Header("Test Placements")]
        [Tooltip("テスト用に配置するカードとスロットの情報のリスト")]
        [SerializeField] private List<PlacementInfo> _placements;

        /// <summary>
        /// 配置するカードとそのターゲットスロットを定義するシリアライズ可能なクラス。
        /// </summary>
        [System.Serializable]
        public class PlacementInfo
        {
            [Tooltip("配置するクリーチャーカードのIdentifiableGameObject")]
            public IdentifiableGameObject CreatureCard;
            [Tooltip("配置先のライン")]
            public LinePosition TargetLine;
            [Tooltip("配置先のライン内での位置")]
            public SlotLocation TargetLocation;
        }

        /// <summary>
        /// ゲーム開始時にテスト配置を実行します。
        /// </summary>
        private async UniTask Start()
        {
            // 「UI操作制限モード」ON
            _commandBus.Emit(new DisableUIInteractionCommand());

            PlaceCardsForTest();

            // 0.5秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            // 「UI操作制限モード」OFF
            _commandBus.Emit(new EnableUIInteractionCommand());
        }

        /// <summary>
        /// Unityのインスペクターのコンテキストメニューからこのメソッドを呼び出してテストを実行します。
        /// </summary>
        [ContextMenu("Place Cards for Test")]
        public void PlaceCardsForTest()
        {
            if (_cardSlotManager == null)
            {
                Debug.LogError("CardSlotManagerがアタッチされていません。");
                return;
            }

            if (_placements == null || _placements.Count == 0)
            {
                Debug.LogWarning("配置するカードの情報が設定されていません。");
                return;
            }

            foreach (var placement in _placements)
            {
                if (placement.CreatureCard == null)
                {
                    Debug.LogWarning("配置情報にクリーチャーカードが設定されていません。スキップします。");
                    continue;
                }

                // 指定されたLineとLocationでスロットを検索
                List<CardSlotData> slots = _cardSlotManager.FindSlotsByLocation(placement.TargetLine, placement.TargetLocation);

                if (slots != null && slots.Count > 0)
                {
                    // 該当する最初のスロットを取得
                    CardSlotData targetSlot = slots[0];

                    // カードをスロットに配置するロジックを呼び出し
                    _cardSlotManager.PlaceCardAsSystem(placement.CreatureCard.ObjectId, targetSlot.SlotId, true);

                    Debug.Log($"カード {placement.CreatureCard.ObjectId} をスロット {targetSlot.SlotId} に配置しました。");
                }
                else
                {
                    Debug.LogWarning($"スロットが見つかりませんでした: Line={placement.TargetLine}, Location={placement.TargetLocation}");
                }
            }
        }
    }
}

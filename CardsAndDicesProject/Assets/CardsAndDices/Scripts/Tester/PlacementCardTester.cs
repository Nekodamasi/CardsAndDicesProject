using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks; // UniTaskを使用するために追加
using System;

namespace CardsAndDices
{
    /// <summary>
    /// ゲームの初期状態をセットアップし、テスト用のカード配置を行うデバッグ用クラス。
    /// </summary>
    public class PlacementCardTester : MonoBehaviour
    {
        [Header("System References")]
        [Tooltip("カードスロットを管理するCardSlotManager")]
        [SerializeField] private CardSlotManager _cardSlotManager;
        [SerializeField] private DiceSlotManager _diceSlotManager;
        [SerializeField] private SpriteCommandBus _commandBus;
        [SerializeField] private CardLifecycleService _cardLifecycleService;



        [Header("Test Placements")]
        [Tooltip("テスト用に配置するカードとスロットの情報のリスト")]
        [SerializeField] private List<PlacementInfo> _placements;

        [Tooltip("テスト用に配置するダイスとスロットの情報のリスト")]
        [SerializeField] private List<DicePlacementInfo> _diceplacements;
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
        /// 配置するカードとそのターゲットスロットを定義するシリアライズ可能なクラス。
        /// </summary>
        [System.Serializable]
        public class DicePlacementInfo
        {
            [Tooltip("配置するダイスのIdentifiableGameObject")]
            public IdentifiableGameObject CreatureDice;
            [Tooltip("配置先のライン内での位置")]
            public DiceSlotLocation TargetLocation;
        }

        /// <summary>
        /// ゲーム開始時にテスト配置を実行します。
        /// </summary>
        private async UniTask Start()
        {
            // カードの初期化
            _cardLifecycleService.InitializeCards();

            // 「UI操作制限モード」ON
            _commandBus.Emit(new DisableUIInteractionCommand());

            PlaceCardsForTest();
            PlaceDicesForTest();

            // 0.5秒待機
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));

            // 「UI操作制限モード」OFF
            _commandBus.Emit(new EnableUIInteractionCommand());
        }

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

        public void PlaceDicesForTest()
        {
            if (_diceSlotManager == null)
            {
                Debug.LogError("DiceSlotManagerがアタッチされていません。");
                return;
            }

            if (_diceplacements == null || _diceplacements.Count == 0)
            {
                Debug.LogWarning("配置するダイスの情報が設定されていません。");
                return;
            }

            foreach (var diceplacement in _diceplacements)
            {
                if (diceplacement.CreatureDice == null)
                {
                    Debug.LogWarning("配置情報にダイスが設定されていません。スキップします。");
                    continue;
                }

                // 指定されたLineとLocationでスロットを検索
                List<DiceSlotData> slots = _diceSlotManager.FindSlotsByLocation(diceplacement.TargetLocation);

                if (slots != null && slots.Count > 0)
                {
                    // 該当する最初のスロットを取得
                    DiceSlotData targetSlot = slots[0];

                    // カードをスロットに配置するロジックを呼び出し
//                    Debug.LogWarning("<color=Yellow>すろっと？:</color>" + slots.Count);
                    _diceSlotManager.PlaceDiceAsSystem(diceplacement.CreatureDice.ObjectId, targetSlot.SlotId, true);
                }
            }
        }
    }
}

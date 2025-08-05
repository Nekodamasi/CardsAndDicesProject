# sys_card_slot_manager.md - CardSlotManagerシステム設計書

---

## 概要

このドキュメントは、カードスロット関連システムの設計について詳述します。このシステムは、単一の巨大な`CardSlotManager`クラスから、関心の分離の原則に基づき、責務ごとに分割された複数のクラス群へとリファクタリングされました。

中心となる`CardSlotManager`は、外部からのAPI呼び出しを受け付ける**ファサード**として機能し、実際の処理は専門のサービスクラスへと委譲します。

---

## 設計思想とアーキテクチャ

本システムは、保守性、テスト容易性、拡張性を高めることを目的に、以下の5つの主要な`ScriptableObject`ベースのクラスに分割されています。

1.  **`CardSlotManager` (ファサード)**: システム全体の窓口。外部からの要求を受け取り、適切なサービスクラスに処理を委譲する。状態や複雑なロジックは保持しない。
2.  **`CardSlotStateRepository` (状態管理)**: 全てのスロットの状態（`CardSlotData`）を保持する唯一の信頼できる情報源（Single Source of Truth）。スロットデータの登録、取得、検索機能を提供する。
3.  **`CardPlacementService` (カード配置)**: カードをスロットに配置・削除する責務を持つ。状態の変更は`CardSlotStateRepository`に依頼する。
4.  **`ReflowService` (リフロー計算)**: カードのリフロー（再配置）に関する複雑な計算ロジックのみを担当する。状態を持たず、与えられた情報に基づいて移動結果を算出する純粋な計算サービス。
5.  **`CardSlotInteractionHandler` (UIイベント処理)**: UIからのドロップやホバーといったイベントを処理する。`ReflowService`や`CardPlacementService`を呼び出し、一連のインタラクションを調整（オーケストレーション）する。
6.  **`CardSlotDebug` (デバッグ)**: デバッグ用の機能（状態のログ出力、重複検知など）を提供する。

このアーキテクチャにより、各クラスは単一責任の原則に従い、それぞれの役割が明確になっています。

---

## クラス詳細

### 1. `CardSlotManager`

*   **役割**: ファサード
*   **責務**:
    *   外部アセンブリや他のシステムからのAPI呼び出しの窓口となる。
    *   インスペクター上で、依存する各サービスクラスへの参照を保持する。
    *   受け取った要求を、対応するサービスクラスのメソッドにそのまま渡す。

### 2. `CardSlotStateRepository`

*   **役割**: 状態管理 (Model/Repository)
*   **責務**:
    *   `_slotDataMap` (`Dictionary<CompositeObjectId, CardSlotData>`) を保持し、全てのスロットの状態を管理する。
    *   スロットの登録 (`RegisterSlot`)。
    *   各種条件によるスロットデータの検索・取得 (`GetSlotData`, `GetSlotDataByLocation`, `GetNextEmptyHandSlot`など)。
    *   PlayerZoneの満員状態の判定 (`IsPlayerZoneFull`)。

### 3. `CardPlacementService`

*   **役割**: カード配置ロジック
*   **責務**:
    *   指定されたスロットにカードを配置する (`PlaceCard`)。この際、配置前に `UnplaceCard` を呼び出し、カードの重複配置を防ぐ。
    *   指定されたカードをスロットから取り除く (`UnplaceCard`)。
    *   処理の際には`CardSlotStateRepository`の状態を更新する。

### 4. `ReflowService`

*   **役割**: リフロー計算ロジック
*   **責務**:
    *   リフローの移動情報を計算する (`CalculateReflowMovements`)。
    *   前詰め処理の移動情報を計算する (`CalculateFrontLoadMovements`)。
    *   隣接判定 (`IsAdjacent`) や、押し出し・循環ロジック (`GetNextSlotInCircularOrder`) など、計算に必要なヘルパーメソッドを提供する。
    *   **注意**: このクラスは状態を持たず、計算に必要なスロットデータは引数として受け取る。

### 5. `CardSlotInteractionHandler`

*   **役割**: UIイベントのオーケストレーション (Controller)
*   **責務**:
    *   カードのドロップ (`OnCardDroppedOnSlot`)、ホバー (`OnCardHoveredOnSlot`)、ドロップ失敗 (`OnDropFailed`) といったUIイベントを処理する。
    *   イベントに応じて`ReflowService`を呼び出し、計算結果をコマンドとして発行する。
    *   ドロップ成功時に、リフローのプレビュー状態を確定させる (`ReflowConfirm`)。

### 6. `CardSlotDebug`

*   **役割**: デバッグ機能
*   **責務**:
    *   スロットの状態を整形してリストとして取得する (`GetDebugSlotData`)。
    *   スロットの状態をコンソールにログ出力する (`DebugSlotData`)。
    *   カードの重複配置などの不正な状態を検知し、警告を出力する。

---

## データフローの例：カードドロップ時

1.  `SpriteInputHandler` が `OnDrop` イベントを検知し、`SpriteDropCommand` を発行する。
2.  `UIInteractionOrchestrator` がコマンドを受信し、`CardSlotManager.OnCardDroppedOnSlot` を呼び出す。
3.  `CardSlotManager` は `CardSlotInteractionHandler.OnCardDroppedOnSlot` を呼び出す。
4.  `CardSlotInteractionHandler` が処理を開始：
    a. `ReflowConfirm` を実行。これにより、全てのスロットでプレビュー用の `ReflowPlacedCardId` の値が、確定用の `PlacedCardId` に上書きされ、配置が確定する。
    b. `ReflowCardsCurrentValue` を実行。現在の確定配置 (`PlacedCardId`) に基づいて、各カードが最終的に表示されるべき位置を `Dictionary` にまとめ、`DragReflowCompletedCommand` としてコマンドバスに発行する。
5.  `UIInteractionOrchestrator` が `DragReflowCompletedCommand` を受け取る。
6.  `UIInteractionOrchestrator` は、受け取った移動情報に基づき、影響を受ける全ての `CreatureCardView` にアニメーション付きの移動を指示する。
7.  移動アニメーション完了後、`UIInteractionOrchestrator` は `ExecuteFrontLoadCommand` を発行し、前詰め処理のフローを開始する。
8.  `UIInteractionOrchestrator` は `ExecuteFrontLoadCommand` を受け取り、`ReflowService.CalculateFrontLoadMovements` を呼び出して前詰めの移動計算を行う。
9.  計算された移動情報に基づき、再度 `CreatureCardView` の移動アニメーションを実行する。
10. 全てのアニメーション完了後、`UIInteractionOrchestrator` が `SpriteDragOperationCompletedCommand` を発行し、一連のドラッグ操作が完了する。

---

## 関連ファイル

- [guide_rules.md](../../guide/guide_rules.md)
- [guide_files.md](../../guide/guide_files.md)
- [guide_ui_interaction_design.md](../../guide/guide_ui_interaction_design.md)
- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [gdd_reflow_system.md](../gdd/gdd_reflow_system.md)

---

## 更新履歴

- 2025-08-04: データフローの例をソースコードのロジックと完全に一致するように修正し、関連ファイルを整理 (Gemini - Codebase Analyst)
- 2025-08-01: `CardSlotManager`を複数のサービスクラスに分割するリファクタリングを反映。 (Gemini)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)

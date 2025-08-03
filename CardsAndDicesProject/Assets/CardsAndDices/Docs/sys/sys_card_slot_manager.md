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
    *   指定されたスロットにカードを配置する (`PlaceCard`)。
    *   指定されたカードをスロットから取り除く (`UnplaceCard`)。
    *   処理の際には`CardSlotStateRepository`の状態を更新する。

### 4. `ReflowService`

*   **役割**: リフロー計算ロジック
*   **責務**:
    *   リフローの移動情報を計算する (`CalculateReflowMovements`)。
    *   隣接判定 (`IsAdjacent`) や、押し出し・循環ロジック (`GetNextSlotInCircularOrder`) など、計算に必要なヘルパーメソッドを提供する。
    *   **注意**: このクラスは状態を持たず、計算に必要なスロットデータは引数として受け取る。

### 5. `CardSlotInteractionHandler`

*   **役割**: UIイベントのオーケストレーション (Controller)
*   **責務**:
    *   カードのドロップ (`OnCardDroppedOnSlot`)、ホバー (`OnCardHoveredOnSlot`)、ドロップ失敗 (`OnDropFailed`) といったUIイベントを処理する。
    *   イベントに応じて`CardPlacementService`や`ReflowService`を適切な順序で呼び出す。
    *   リフローの確定 (`ReflowConfirm`) や、結果のコマンドバスへの通知 (`ReflowCardsCurrentValue`, `ReflowCardsIfNeeded`) を行う。
    *   PlayerZoneの状態変化を監視し、必要に応じてコマンドを発行する (`CheckAndNotifyPlayerZoneState`)。

### 6. `CardSlotDebug`

*   **役割**: デバッグ機能
*   **責務**:
    *   スロットの状態を整形してリストとして取得する (`GetDebugSlotData`)。
    *   スロットの状態をコンソールにログ出力する (`DebugSlotData`)。
    *   カードの重複配置などの不正な状態を検知し、警告を出力する。

---

## データフローの例：カードドロップ時

1.  **`CardSlotView`** が `OnDrop` イベントを検知。
2.  `UIInteractionOrchestrator` を経由して **`CardSlotManager`** の `OnCardDroppedOnSlot` が呼び出される。
3.  **`CardSlotManager`** は **`CardSlotInteractionHandler`** の `OnCardDroppedOnSlot` を呼び出す。
4.  **`CardSlotInteractionHandler`** が処理を開始：
    a. **`CardPlacementService`** の `PlaceCard` を呼び出し、カードをスロットに配置するよう依頼。
    b. **`CardSlotStateRepository`** のスロットデータが更新される。
    c. `ReflowConfirm` を実行し、リフロー状態を確定させる。
    d. `ReflowCardsCurrentValue` を実行し、確定した配置を `DragReflowCompletedCommand` としてコマンドバスに発行する。
    e. `CheckAndNotifyPlayerZoneState` を実行し、必要であれば `PlayerZoneStateChangedCommand` を発行する。
5.  `UIInteractionOrchestrator` が `DragReflowCompletedCommand` を受け取る。
6.  `ReflowService` の `CalculateFrontLoadMovements` を呼び出し、前詰めの移動計算を行う。
7.  移動がある場合、`ExecuteFrontLoadCommand` を発行する。ない場合は、`SpriteDragOperationCompletedCommand` を発行して終了。
8.  `UIInteractionOrchestrator` が `ExecuteFrontLoadCommand` を受け取り、各Viewにアニメーション付きの移動を指示する。
9.  アニメーション完了後、Viewから `SpriteDragOperationCompletedCommand` が発行され、一連の操作が完了する。

---

## 関連ファイル

- **クラス定義**:
    - `Assets/CardsAndDices/Scripts/Systems/CardSlotManager.cs`
    - `Assets/CardsAndDices/Scripts/Systems/CardSlotStateRepository.cs`
    - `Assets/CardsAndDices/Scripts/Systems/CardPlacementService.cs`
    - `Assets/CardsAndDices/Scripts/Systems/ReflowService.cs`
    - `Assets/CardsAndDices/Scripts/Systems/CardSlotInteractionHandler.cs`
    - `Assets/CardsAndDices/Scripts/Systems/CardSlotDebug.cs`
- **データクラス**:
    - `Assets/CardsAndDices/Scripts/Data/CardSlotData.cs`
- **関連ドキュメント**:
    - [guide_project_files.md](../../guide/guide_project_files.md)
    - [gdd_reflow_system.md](../gdd/gdd_reflow_system.md)

---

## 更新履歴

- 2025-08-01: `CardSlotManager`を複数のサービスクラスに分割するリファクタリングを反映。 (Gemini)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)

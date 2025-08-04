# sys_card-reflow.md - カードリフローシステム設計書

---

## 概要

このドキュメントは、ゲームボード上でのカードの再配置（リフロー）システムについて詳述します。
リフローには、ユーザーのドラッグ操作に起因するものと、システムが自動的に実行するものの2種類が存在し、それぞれ異なるコントローラーとコマンドによって管理されます。

---

## 1. 主要コンポーネント

-   **`CardSlotManager`**: 全てのカードスロットの論理的な状態を管理し、リフローの必要性を判断して、適切なコマンドを発行するファサードクラス。
-   **`UIInteractionOrchestrator`**: ユーザー操作（ドラッグ中のホバーなど）に起因するリフローを管理する。
-   **`SystemReflowController`**: システム（カード配置の初期化など）に起因するリフローを管理する。
-   **`CreatureCardView`**: 自身の移動アニメーションを実行するViewコンポーネント。
-   **`SpriteCommandBus`**: 各コンポーネント間の通知を伝達する中央ハブ。
-   **`ReflowCompletedCommand`**: **ユーザー操作**によるリフロープレビューの実行を通知するコマンド。
-   **`DragReflowCompletedCommand`**: **ユーザー操作**によるリフローが確定し、カードを移動させることを通知するコマンド。
-   **`SystemReflowCommand`**: **システム**によるリフローの実行を通知するコマンド。

---

## 2. リフローのフロー

### フローA：ユーザー操作によるリフロー（プレビュー）

ドラッグ中にリフロー結果をプレビュー表示する際のフローです。

1.  **トリガー:** ドラッグ中のカードがスロットにホバーする。
2.  **`UIInteractionOrchestrator`** が `SpriteHoverCommand` を受け取る。
3.  `CardSlotManager.OnCardHoveredOnSlot()` を呼び出す。
4.  `CardSlotManager` は `ReflowService` を使ってリフローを計算し、結果を `ReflowCompletedCommand` として発行する。
5.  **`UIInteractionOrchestrator`** が `ReflowCompletedCommand` を受け取り、各 `CreatureCardView` に `MoveToAnimated()` を実行させてプレビューを表示する。

### フローB：ユーザー操作によるリフロー（確定）

カードがスロットにドロップされ、配置が確定した際のリフローです。

1.  **トリガー:** カードがスロットにドロップされる。
2.  **`UIInteractionOrchestrator`** が `SpriteDropCommand` を受け取る。
3.  `CardSlotManager.OnCardDroppedOnSlot()` を呼び出す。
4.  `CardSlotManager` は配置を確定し、`DragReflowCompletedCommand` を発行する。
5.  **`UIInteractionOrchestrator`** が `DragReflowCompletedCommand` を受け取り、各 `CreatureCardView` に `MoveToAnimated()` を実行させてカードを最終的な位置へ移動させる。

### フローC：システムによるリフロー

テスト用の初期配置など、システムがリフローを強制的に実行する際のフローです。

1.  **トリガー:** `PlacementCardTester` などが `CardSlotManager.PlaceCardAsSystem()` を呼び出す。
2.  `CardSlotManager` は `SystemReflowCardsCurrentValue()` を呼び出す。
3.  `CardSlotInteractionHandler` が、現在の確定済み配置に基づき `SystemReflowCommand` を発行する。
4.  **`SystemReflowController`** が `SystemReflowCommand` を購読し、受け取る。
5.  `SystemReflowController` は、コマンド内の情報に基づき、各 `CreatureCardView` の `MoveToAnimated()` を呼び出してカードを移動させる。
6.  全てのアニメーション完了後、`SystemReflowCommand` に指定されていれば、後続のコマンド（例: `SpriteDragOperationCompletedCommand`）を発行する。

---

## 3. 「再配置」ロジックの記述場所

「再配置」の具体的なアルゴリズム（例: 隣接スワップ、前押し出しなど）は、`ReflowService.CalculateReflowMovements()` メソッド内に実装されます。このメソッドは状態を持たず、純粋な計算のみを行います。

---

## 関連ファイル

-   [sys_ui_interaction_orchestrator.md](./sys_ui_interaction_orchestrator.md)
-   [sys_card_slot_manager.md](./sys_card_slot_manager.md)
-   `Assets/CardsAndDices/Scripts/Systems/SystemReflowController.cs`
-   `Assets/CardsAndDices/Scripts/Commands/SystemReflowCommand.cs`

---

## 更新履歴

-   2025-08-03: `SystemReflowController` の導入と、ユーザー操作/システム起因のリフローの分離を反映。
-   2025-07-21: 初版作成
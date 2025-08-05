# gdd_reflow_system.md - リフローシステム設計書

---

## 概要

このドキュメントは、ゲームボード上でのカードのリフロー（再配置）システムの詳細なロジックを定義します。リフロー処理は、計算ロジックを担当する `ReflowService` と、全体的なフロー制御を担う `UIInteractionOrchestrator` および `CardSlotManager` を中心としたクラス群の連携によって実現されます。

---

## 用語

- **ドラッグカード**: ドラッグされたクリーチャーカード。
- **ドラッグスロット**: ドラッグカードが元々配置されていたスロット。
- **起点スロット**: ドラッグカードでホバーされ、リフローの起点となったスロット。
- **PlacedCardId**: スロットに**確定配置**されているカードのID。`CardPlacementService` によって管理され、リフローが完了するまで変更されない。
- **ReflowPlacedCardId**: リフローの**プレビュー中**に、スロットに仮配置されているカードのID。`CardSlotStateRepository` 内で管理され、プレビュー計算に使用される。

---

## クラスの責務

-   **`ReflowService`**: リフロー計算の専門家。状態を持たず、現在のスロット情報から「どのカードがどこへ移動すべきか」という計算のみを行います。（`CalculateReflowMovements`, `CalculateFrontLoadMovements`）
-   **`CardSlotManager`**: スロット関連処理のファサード。内部の `CardSlotStateRepository`, `CardPlacementService`, `CardSlotInteractionHandler` に処理を委譲します。
-   **`UIInteractionOrchestrator`**: UIインタラクションの司令塔。ユーザー操作（ホバー、ドロップ等）をトリガーに `CardSlotManager` を呼び出し、リフローのプレビューや確定のフローを開始します。また、`ReflowCompletedCommand` 等のコマンドを受け取り、Viewのアニメーションを実行します。

---

## リフロー処理のフロー

リフローは「プレビュー」と「確定」の2つのフェーズで実行されます。

### フェーズ1: リフロープレビュー

- **トリガー**: `UIInteractionOrchestrator` が、ドラッグ中のカードがスロットにホバーしたことを検知 (`OnHover`)。
- **目的**: プレイヤーにリフロー後の結果を視覚的に提示する。
- **処理**:
    1. `UIInteractionOrchestrator` は `CardSlotManager.OnCardHoveredOnSlot` を呼び出します。
    2. `CardSlotManager` は内部で `CardSlotInteractionHandler` に処理を委譲します。
    3. `CardSlotInteractionHandler` は、現在のスロット状態（`PlacedCardId`）を `ReflowPlacedCardId` にコピーした後、`ReflowService.CalculateReflowMovements` を呼び出して、プレビューのための移動情報を計算します。
    4. 計算結果（移動するカードと移動先のマップ）を `ReflowCompletedCommand` として発行します。
    5. `UIInteractionOrchestrator` がこのコマンドを購読し、影響を受ける各 `CreatureCardView` に `MoveToAnimated` を指示して、プレビューアニメーションを実行させます。
    6. このフェーズでは、`PlacedCardId` は一切変更されません。

### フェーズ2: リフローの確定またはキャンセル

#### A. 確定 (ドロップ成功時)

- **トリガー**: `UIInteractionOrchestrator` が、カードが有効なスロットにドロップされたことを検知 (`OnDrop`)。
- **処理**:
    1. `UIInteractionOrchestrator` は `CardSlotManager.OnCardDroppedOnSlot` を呼び出します。
    2. `CardSlotManager` は `CardPlacementService` を使って、まずドラッグされていたカードをスロットから取り除き（`UnplaceCard`）、次にドロップ先のスロットに配置します（`PlaceCard`）。これにより `PlacedCardId` が更新されます。
    3. `CardSlotInteractionHandler` が `SystemReflowCardsCurrentValue` を呼び出し、現在の `PlacedCardId` の状態を元に `ReflowService` で最終的なリフローを計算させ、`DragReflowCompletedCommand` を発行します。
    4. `UIInteractionOrchestrator` がこのコマンドを受け取り、各カードを最終的な確定位置へ移動させます。

#### B. キャンセル (ドロップ失敗時)

- **トリガー**: `UIInteractionOrchestrator` が、スロット外でのドロップを検知 (`OnEndDrag` 内の遅延判定ロジック)。
- **処理**:
    1. `UIInteractionOrchestrator` は `CardSlotManager.OnDropFailed` を呼び出します。
    2. `CardSlotInteractionHandler` は、全ての `ReflowPlacedCardId` を、元の `PlacedCardId` の状態にリセットします。
    3. `SystemReflowCardsCurrentValue` を呼び出し、全てのカードをリフロー開始前の元の位置に戻すための `ReflowCompletedCommand` を発行します。

---

## リフロー計算ロジック (`ReflowService`)

リフローの具体的な計算ロジックは `ReflowService` にカプセル化されており、プレビューと確定の両方で利用されます。

### 1. 隣接スワップ

- **条件**: ドラッグスロットと起点スロットが隣接している (`IsAdjacent` で判定)。
- **処理**: 起点スロットカードがドラッグスロットに移動するように計算します。

### 2. 前押し出し

- **条件**: 同じラインでドラッグスロットが`Vanguard`、起点スロットが`Rear`の時。
- **処理**: 同ラインの`Center`スロットのカードが`Vanguard`に、起点スロットカードが`Center`に移動するように計算します。

### 3. 後ろ押し出し

- **条件**: 上記2つ以外。
- **処理**: 起点スロットのカードから、`GetNextSlotInCircularOrder` ヘルパーメソッドで定義された循環順序に従って、順次押し出されるように計算します。
  - 途中で未配置のスロットか、ドラッグスロットに来たらストップします。

---

## 前詰め処理 (`ReflowService`)

### 概要

- 各ラインで前衛方向（Vanguard側）に空きスロットがある場合、カードを隙間なく詰める処理です。
- この処理の計算ロジックは `ReflowService.CalculateFrontLoadMovements` に実装されています。

### 実行フロー

1.  **トリガー**: `UIInteractionOrchestrator` が `DragReflowCompletedCommand` を受信し、リフローの確定アニメーションが完了した直後。
2.  **コマンド発行**: `UIInteractionOrchestrator` は `ExecuteFrontLoadCommand` を発行します。
3.  **計算と実行**: `UIInteractionOrchestrator` は `OnExecuteFrontLoad` でこのコマンドを受け取り、`ReflowService.CalculateFrontLoadMovements` を呼び出して前詰めの移動情報を計算します。
4.  **アニメーション**: 計算結果に基づき、各カードがアニメーション付きで移動します。
5.  **完了通知**: アニメーション完了後、`UIInteractionOrchestrator` は `SpriteDragOperationCompletedCommand` を発行し、一連のドラッグ操作を完全に終了させます。

---

## 関連ファイル

- [guide_rules.md](../guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](../guide/guide_files.md): ドキュメントファイル命名・管理ルール
- [gdd_sprite_ui_design.md](./gdd_sprite_ui_design.md): SpriteUI基本設計

---

## 更新履歴

- 2025-08-04: ソースコードの責務分担（ReflowService, UIInteractionOrchestrator等）に合わせて、フロー全体を実装に即した形に更新 (Gemini - Codebase Analyst)
- 2025-07-26: 実装に合わせて、プレビューと確定の2段階フローを反映し、用語を更新 (Gemini - Technical Writer for Game Development)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)

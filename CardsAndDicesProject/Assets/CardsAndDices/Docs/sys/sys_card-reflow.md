# sys_card-reflow.md - カードリフローシステム設計書

---

## 概要

このドキュメントは、ゲームボード上でのカードの再配置（リフロー）システムについて詳述します。
特に、`CardSlotManager` が論理的な配置を決定し、各カードのViewが視覚的な移動を行う際の連携フローに焦点を当てます。

---

## 1. 主要コンポーネント

-   **`CardSlotManager` (Controller):**
    -   全てのカードスロットの論理的な状態（どのカードがどのスロットにあるか）を管理します。
    -   リフローの必要性を判断し、カードの新しい論理的な配置を決定します。
    -   リフロー完了後、関連するViewに通知するためのコマンドを発行します。

-   **`CreatureCardView` (View):**
    -   クリーチャーカードの視覚的な表現を担当します。
    -   `CardSlotManager` から発行されるリフローコマンドを購読し、自身の新しい位置への移動アニメーションを実行します。

-   **`SpriteCommandBus`:**
    -   `CardSlotManager` から `CreatureCardView` へ、リフロー完了の通知を伝達するための中央ハブです。

-   **`ReflowCompletedCommand` (新規コマンド):**
    -   リフローが完了し、カードが新しい位置へ移動する必要があることを通知するためのコマンドです。
    -   移動が必要な各カードの `CompositeObjectId` と、そのカードが移動すべき最終的なワールド座標（または新しいスロットのID）のリストを含みます。

---

## 2. リフローのフロー（本番）

カードの配置が確定した際のリフローは、以下のステップで進行します。

1.  **リフローのトリガー:**
    -   `CardSlotManager` 内の `OnCardDroppedOnSlot` メソッドが、カードがスロットにドロップされたことを受けて実行されます。

2.  **`CardSlotManager` による論理的な配置の確定:**
    -   `OnCardDroppedOnSlot` メソッド内で、`_slotDataMap` の `PlacedCardId` が直接更新されます。これにより、カードの新しい論理的な配置が**即座に確定**します。

3.  **Viewへの通知コマンド発行:**
    -   `OnCardDroppedOnSlot` は、続けて `ReflowCardsCurrentValue()` を呼び出します。
    -   `ReflowCardsCurrentValue()` は、現在の確定済み配置 (`PlacedCardId`) を元に、移動が必要なカードのリスト（IDと最終座標）を生成します。
    -   生成された移動情報を含む `ReflowCompletedCommand` を `SpriteCommandBus` を介して発行します。

4.  **`CreatureCardView` による視覚的な移動:**
    -   各 `CreatureCardView` は `ReflowCompletedCommand` を購読しています。
    -   コマンドを受け取った `CreatureCardView` は、コマンドに自身のIDが含まれていれば、指定された新しい位置へ移動するアニメーションを実行します。

---

## 3. リフローのフロー（プレビュー）

ドラッグ中にリフロー結果をプレビュー表示する際のフローです。

1.  **プレビューのトリガー:**
    -   ドラッグ中のカードがスロットにホバーした際、`CardSlotView` が `CardSlotManager.RequestReflowPreview()` を呼び出します。

2.  **`CardSlotManager` によるプレビュー計算:**
    -   `RequestReflowPreview()` は、現在の配置にドラッグ中のカードを加味した場合のリフロー結果を**計算するだけ**で、`PlacedCardId` は変更しません。
    -   計算結果（仮の移動先リスト）を `ReflowCompletedCommand` として発行します。

3.  **`CreatureCardView` によるプレビュー移動:**
    -   各 `CreatureCardView` はコマンドを受け取り、指定された位置へ移動アニメーションをします。これがリフローのプレビューとなります。

4.  **プレビューのキャンセル:**
    -   カードがスロットからアンホバーされると、`CardSlotManager.CancelReflowPreview()` が呼ばれます。
    -   `CancelReflowPreview()` は、現在の確定済み配置 (`PlacedCardId`) に基づく `ReflowCompletedCommand` を発行し、プレビュー表示されていたカードを元の位置に戻します。

---

## 4. 「再配置」ロジックの記述場所

「再配置」の具体的なアルゴリズム（例: 隣接スワップ、前押し出しなど）は、`CardSlotManager.CalculateReflowMovements()` メソッド内に実装されます。このメソッドは主にリフロープレビューの計算に使用されます。

---

## 関連ファイル

-   [guide_overview.md](../guide/guide_overview.md)
-   [guide_design-principles.md](../guide/guide_design-principles.md)
-   [sys_domain-model.md](./sys_domain-model.md)
-   [../Scripts/Systems/CardSlotManager.cs](../../Scripts/Systems/CardSlotManager.cs)
-   [../Scripts/UI/CreatureCardView.cs](../../Scripts/UI/CreatureCardView.cs)
-   [../Scripts/Commands/ReflowCompletedCommand.cs](../../Scripts/Commands/ReflowCompletedCommand.cs) (新規作成予定)

---

## 更新履歴

-   2025-07-21: 初版作成 (Gemini - Technical Writer for Game Development)

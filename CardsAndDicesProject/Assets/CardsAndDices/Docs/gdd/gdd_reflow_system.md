# gdd_reflow_system.md - リフローシステム設計書

---

## 概要

このドキュメントは、ゲームボード上でのカードのリフロー（再配置）システムの詳細なロジックを定義します。特に、ドラッグ中のカードとスロットのインタラクションに基づいて発生する、**プレビュー**と**確定**の2段階のリフロー挙動に焦点を当てます。

---

## 用語

- **ドラッグカード**: ドラッグされたクリーチャーカード。
- **ドラッグスロット**: ドラッグカードの配置されていたスロット。
- **起点スロット**: ドラッグカードでホバーされ、リフローの起点となったスロット。
- **起点スロットカード**: 起点スロットに元々配置されていたカード。
- **PlacedCardId**: スロットに**確定配置**されているカードのID。リフローが完了するまで変更されない。
- **ReflowPlacedCardId**: リフローの**プレビュー中**に、スロットに仮配置されているカードのID。プレビューや計算に使用される。

---

## 前提

- HandZoonの処理はまだつめきれていないため、ここでは、PlayerZoonの6スロットで発生するリフローに限定する。

---

## リフロー処理のフロー

リフローは「プレビュー」と「確定」の2つのフェーズで実行されます。

### フェーズ1: リフロープレビュー

- **トリガー**: ドラッグ中のカードが、配置可能なスロットにホバーした時 (`CardSlotManager.RequestReflowPreview`)。
- **目的**: プレイヤーにリフロー後の結果を視覚的に提示する。
- **処理**:
    1. `CardSlotManager` は、現在の `PlacedCardId` を元に、`ReflowPlacedCardId` を使ってリフロー後の仮の配置を計算します。
    2. 計算結果（どのカードがどの位置に移動するか）を `ReflowCompletedCommand` として発行します。
    3. 各カードのView (`CreatureCardView`) はこのコマンドを受け取り、指定された位置へ移動する**プレビューアニメーション**を実行します。
    4. このフェーズでは、`PlacedCardId` は変更されません。

### フェーズ2: リフローの確定またはキャンセル

#### A. 確定 (ドロップ成功時)

- **トリガー**: カードが有効なスロットにドロップされた時 (`CardSlotManager.OnCardDroppedOnSlot`)。
- **処理**:
    1. `CardSlotManager` は、まずドロップされたカードをスロットに配置し、`PlacedCardId` と `ReflowPlacedCardId` を更新します。
    2. その後、現在の `PlacedCardId` の状態が正しい位置になるように、`ReflowCardsCurrentValue` を呼び出します。
    3. `ReflowCompletedCommand` が発行され、各カードは最終的な確定位置へ移動します。

#### B. キャンセル (ドロップ失敗時)

- **トリガー**: カードがスロット外でドロップされた、またはドラッグがキャンセルされた時 (`CardSlotManager.OnDropFailed`)。
- **処理**:
    1. `CardSlotManager` は、全ての `ReflowPlacedCardId` を、元の `PlacedCardId` の状態にリセットします。
    2. その後、`ReflowCardsCurrentValue` を呼び出し、全てのカードをリフロー開始前の元の位置に戻すための `ReflowCompletedCommand` を発行します。

---

## リフロー計算ロジック

リフローの具体的な計算ロジック（`CalculateReflowMovements`）は、プレビューと確定の両方で利用されます。

### 1. 隣接スワップ

- **条件**: ドラッグスロットと起点スロットが隣接している。
- **処理**: 起点スロットカードがドラッグスロットに移動。

### 2. 前押し出し

- **条件**: 同じラインでドラッグスロットの`SlotLocation`が`Vanguard`で、起点スロットが`Rear`の時。
- **処理**: 同ラインの`Center`スロットのカードが`Vanguard`に移動、起点スロットカードは`Center`に移動。

### 3. 後ろ押し出し

- **条件**: 上記2つ以外。
- **処理**: 起点スロットのカードから順次押し出していく。
  - 順序は`Vanguard` -> `Center` -> `Rear` -> 別line.`Rear` -> `Center` -> `Vanguard`。
  - 途中で未配置のスロットか、ドラッグスロットに来たらストップ。

---

## 前詰め処理

### 概要

- 各ラインで前衛方向（Vanguard側）に空きスロットがある場合、カードを隙間なく詰める処理です。
- 通常のリフローとは異なり、敵味方問わず、全てのカードが対象となります。

### 実行タイミング

- カードのドラッグ＆ドロップによるリフローが完了し、配置が確定した直後に実行されます。
- 処理フローとしては、`DragReflowCompletedCommand` の後に `ExecuteFrontLoadCommand` が発行され、このコマンドによって前詰め処理がトリガーされます。

### 処理内容

- **計算ロジック (`CalculateFrontLoadMovements`)**:
    - **Top/Bottom Line**: `Rear`から`Center`、`Center`から`Vanguard`の順に空きスロットを確認し、カードが存在すれば空きスロットまで移動させます。
    - **Hand Line**: `Hand9`から`Hand1`の順にスロットを確認し、より番号の若い空きスロットがあればそこまで移動させます。
- **実行**: 計算された移動情報に基づき、各カードがアニメーション付きで移動します。アニメーション完了後、`SpriteDragOperationCompletedCommand`が発行され、一連のドラッグ操作が完了します。

---

## 関連ファイル

- [guide_design-principles.md](../guide/guide_design-principles.md)
- [guide_unity-cs.md](../guide/guide_unity-cs.md)
- [sys_card_slot_manager.md](../sys/sys_card_slot_manager.md)
- [sys_card-reflow.md](../sys/sys_card-reflow.md)
- [ui_creature_card_interaction.md](../ui/ui_creature_card_interaction.md)
- [ui_card_slot_interaction.md](../ui/ui_card_slot_interaction.md)

---

## 更新履歴

- 2025-07-26: 実装に合わせて、プレビューと確定の2段階フローを反映し、用語を更新 (Gemini - Technical Writer for Game Development)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)

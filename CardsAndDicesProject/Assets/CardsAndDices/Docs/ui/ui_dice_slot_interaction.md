# ui_dice_slot_interaction.md - ダイススロットインタラクション設計書

---

## 概要

このドキュメントは、ダイススロット (`DiceSlotView`) のUIインタラクションに関する仕様を定義します。
ダイススロットは、基本的にダイスのドロップを受け付けず、ダイスの置き場として機能します。

---

## 共通の前提・制約

- `DiceSlotView` は `BaseSpriteView` を継承します。
- `IdentifiableGameObject` コンポーネントがアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`DiceSlotView.cs`**: ダイススロットの視覚表現を担当するViewコンポーネント。
- **`UIInteractionOrchestrator.cs`**: UIインタラクション全体の司令塔。
- **`UIActivationPolicy.cs`**: UI要素の有効/無効ルールを定義するポリシークラス。

---

## インタラクションフロー

ダイススロットは、プレイヤーからの直接的なインタラクション（ホバー、クリック、ドロップ）を受け付けません。その状態は、`UIActivationPolicy` によって一元的に管理されます。

### 1. ダイスドラッグ開始時

- **トリガー**: プレイヤーがダイスのドラッグを開始する。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy` を呼び出します。
    2. `UIActivationPolicy` は、全ての `DiceSlotView` に対して `EnterInactiveState()` を呼び出し、非アクティブ状態（操作を受け付けない状態）にします。

### 2. ドラッグ終了時

- **トリガー**: ダイスのドラッグ＆ドロップ操作が完了する。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy` を呼び出します。
    2. `UIActivationPolicy` は、全ての `DiceSlotView` に対して `EnterNormalState()` を呼び出し、通常状態に戻します。

---

## 状態遷移メソッド詳細

- **`EnterNormalState()`**: 通常状態に遷移します。
- **`EnterInactiveState()`**: 非アクティブ状態に遷移します。

---

## 関連ファイル

- [guide_ui_interaction_design.md](../guide/guide_ui_interaction_design.md)
- [ui_dice_interaction.md](./ui_dice_interaction.md)

---

## 更新履歴

- 2025-08-08: 初版 (Gemini - Technical Writer for Game Development)

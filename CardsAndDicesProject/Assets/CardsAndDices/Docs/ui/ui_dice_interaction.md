# ui_dice_interaction.md - ダイスインタラクション設計書

---

## 概要

このドキュメントは、ダイス (`DiceView`) のUIインタラクションに関する仕様を定義します。
ダイスのインタラクションは、`DiceInteractionOrchestrator` を中心としたクラス群によって制御されます。

---

## 共通の前提・制約

- `DiceView` は `BaseSpriteView` を継承します。
- `IdentifiableGameObject` コンポーネントがアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`DiceView.cs`**: ダイスの視覚表現と状態遷移を担当するViewコンポーネント。
- **`SpriteInputHandler.cs`**: マウスイベントを検知し、コマンドを発行します。
- **`DiceInteractionOrchestrator.cs`**: UIインタラクション全体の司令塔。
- **`DiceInteractionStrategy.cs`**: ダイスのインタラクションが可能か判断する戦略クラス。
- **`UIActivationPolicy.cs`**: UI要素の有効/無効ルールを定義するポリシークラス。
- **アニメーションScriptableObject**: `_hoverAnimation`, `_dragAnimation` など。
- **`DicePresenter.cs` (新規)**: `DiceData`と`DiceView`を紐づけ、状態を同期させる仲介役。UIインタラクションの結果、最終的にこのクラスを介してデータが更新されます。

---

## インタラクションフロー

### 1. アイドル時のホバー

- **トリガー**: `UIStateMachine` が `Idle` の状態で、プレイヤーがダイス上にマウスポインターを乗せる。
- **処理の流れ**:
    1. `SpriteInputHandler` が `SpriteHoverCommand` を発行します。
    2. `DiceInteractionOrchestrator` は `DiceInteractionStrategy.ChkDiceHover` でホバー可否を確認します。
    3. `Strategy` が許可した場合、`DiceView` の `EnterHoveringState()` を呼び出します。
    4. `DiceView` はホバーアニメーション (`_hoverAnimation`) を再生します。

### 2. ダイスドラッグ開始時

- **トリガー**: プレイヤーがダイスのドラッグを開始する。
- **処理の流れ**:
    1. `SpriteInputHandler` が `SpriteBeginDragCommand` を発行します。
    2. `DiceInteractionOrchestrator` は `DiceInteractionStrategy.ChkDiceBeginDrag` でドラッグ開始が可能か確認します。
    3. `Strategy` が許可した場合、`Orchestrator` は以下の処理を行います。
        a. `UIStateMachine` の状態を `DraggingDice` に設定します。
        b. ドラッグされた `DiceView` の `EnterDraggingState()` を呼び出します。
        c. `UIActivationPolicy` を通じて、他の全ての `DiceView` を非アクティブ化します。

### 3. ドラッグ終了時

- **トリガー**: ドラッグ＆ドロップ操作が完了する。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` は `UIActivationPolicy` を通じて、全ての `DiceView` を通常状態に戻します。

---

## 状態遷移メソッド詳細

- **`EnterNormalState()`**: 通常状態に遷移します。
- **`EnterHoveringState()`**: ホバー状態に遷移します。
- **`EnterDraggingState()`**: ドラッグ開始状態に遷移します。
- **`EnterInactiveState()`**: 非アクティブ状態に遷移します。

---

## 関連ファイル

- [guide_ui_interaction_design.md](../guide/guide_ui_interaction_design.md)
- [sys_dice_inlet_design.md](../sys/sys_dice_inlet_design.md)

---

## 更新履歴

- 2025-08-08: 初版 (Gemini - Technical Writer for Game Development)

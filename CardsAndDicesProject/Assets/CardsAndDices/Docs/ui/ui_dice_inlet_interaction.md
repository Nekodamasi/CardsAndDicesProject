# ui_dice_inlet_interaction.md - ダイスインレットインタラクション設計書

---

## 概要

このドキュメントは、ダイスインレット (`DiceInletView`) のUIインタラクションに関する仕様を定義します。
ダイスインレットは、特定の条件を満たすダイスのドロップを受け付け、能力発動のトリガーとなります。

---

## 共通の前提・制約

- `DiceInletView` は `BaseSpriteView` を継承します。
- `IdentifiableGameObject` コンポーネントがアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`DiceInletView.cs`**: ダイスインレットの視覚表現と状態遷移を担当するViewコンポーネント。
- **`SpriteInputHandler.cs`**: マウスイベントを検知し、コマンドを発行します。
- **`UIInteractionOrchestrator.cs`**: UIインタラクション全体の司令塔。
- **`DiceInteractionStrategy.cs`**: ダイスインレットへのドロップが可能か判断する戦略クラス。
- **`UIActivationPolicy.cs`**: UI要素の有効/無効ルールを定義するポリシークラス。
- **アニメーションScriptableObject**: `_acceptableAnimation`, `_dropWaitingAnimation` など。

---

## インタラクションフロー

### 1. ダイスドラッグ開始時

- **トリガー**: プレイヤーがダイスのドラッグを開始する。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy` を呼び出します。
    2. `UIActivationPolicy` は、各 `DiceInletView` がドラッグされているダイスを受け入れ可能か `DiceInteractionStrategy.ChkDiceDrop` に問い合わせます。
    3. 受け入れ可能なインレットは `EnterAcceptableState()` を、不可なインレットは `EnterInactiveState()` を呼び出します。

### 2. ダイスドラッグ中のホバー時

- **トリガー**: ドラッグ中のダイスが、`Acceptable` 状態のインレット上でホバーされる。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は、ホバーされた `DiceInletView` の `EnterHoveringState()` を呼び出します。
    2. `DiceInletView` は、ドロップを待っていることを示すアニメーション (`_dropWaitingAnimation`) を再生します。

### 3. ダイスドロップ時

- **トリガー**: プレイヤーがダイスをインレットにドロップする。
- **処理の流れ**:
    1. `SpriteInputHandler` が `SpriteDropCommand` を発行します。
    2. `UIInteractionOrchestrator` は `DiceInteractionStrategy.ChkDiceDrop` でドロップが可能か最終確認します。
    3. `Strategy` が許可した場合、`DiceInletManager` に能力発動処理を依頼します。

### 4. ドラッグ終了時

- **トリガー**: ダイスのドラッグ＆ドロップ操作が完了する。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy` を通じて、全ての `DiceInletView` を非アクティブ状態に戻します。

---

## 状態遷移メソッド詳細

- **`EnterNormalState()`**: 通常状態に遷移します。
- **`EnterAcceptableState()`**: ダイスを受け入れ可能な状態に遷移します。
- **`EnterHoveringState()`**: ダイスがホバーしている状態に遷移します。
- **`EnterInactiveState()`**: 非アクティブ状態に遷移します。

---

## 関連ファイル

- [guide_ui_interaction_design.md](../guide/guide_ui_interaction_design.md)
- [sys_dice_inlet_design.md](../sys/sys_dice_inlet_design.md)
- [ui_dice_interaction.md](./ui_dice_interaction.md)

---

## 更新履歴

- 2025-08-08: 初版 (Gemini - Technical Writer for Game Development)

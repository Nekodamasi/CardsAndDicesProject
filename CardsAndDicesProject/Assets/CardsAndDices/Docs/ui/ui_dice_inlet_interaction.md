# ui_dice_inlet_interaction.md - ダイスインレットインタラクション設計書

---

## 概要

このドキュメントは、ダイスインレット (`DiceInletView`) のUIインタラクションに関する仕様を定義します。
ダイスインレットのインタラクションは、`DiceInteractionOrchestrator` を中心としたクラス群によって制御され、特定の条件を満たすダイスのドロップを受け付け、能力発動のトリガーとなります。

---

## 共通の前提・制約

- `DiceInletView` は `BaseSpriteView` を継承します。
- `IdentifiableGameObject` コンポーネントがアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`DiceInletView.cs`**: ダイスインレットの視覚表現と状態遷移を担当するViewコンポーネント。
- **`SpriteInputHandler.cs`**: インレットに対するマウスイベントを検知し、`SpriteDropCommand` などを発行します。
- **`DiceInteractionOrchestrator.cs`**: ダイス関連のUIインタラクション全体の司令塔。
- **`DiceInteractionStrategy.cs`**: ダイスインレットへのドロップが可能か判断する戦略クラス。
- **`UIActivationPolicy.cs`**: UIの状態に応じて、各インレットのインタラクション可否を決定するポリシークラス。
- **`DiceInletManager.cs`**: インレットの状態管理と能力発動のロジックを担当するコントローラー。
- **アニメーションScriptableObject**: `_acceptableAnimation`, `_dropWaitingAnimation` など、各状態に対応するアニメーションアセット。

---

## インタラクションフロー

### 1. ダイスドラッグ開始時

- **トリガー**: プレイヤーがダイスのドラッグを開始し、`DiceInteractionOrchestrator` が `OnBeginDrag` を処理するタイミング。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` は `UIActivationPolicy.DraggingDiceToDiceInletActivations()` を呼び出します。
    2. `UIActivationPolicy` は、全ての `DiceInletView` に対して、`DiceInteractionStrategy.ChkDiceDrop` を用いてドラッグ中のダイスを受け入れ可能か問い合わせます。
    3. 受け入れ可能なインレットは `EnterAcceptableState()` を、不可なインレットは `EnterInactiveState()` を呼び出し、状態を遷移させます。

### 2. ダイスドラッグ中のホバー時

- **トリガー**: ドラッグ中のダイスが、`Acceptable` 状態のインレット上でホバーされる。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` は、ホバーされた `DiceInletView` の `EnterHoveringState()` を呼び出します。
    2. `DiceInletView` は、ドロップを待っていることを示すアニメーション (`_dropWaitingAnimation`) を再生します。

### 3. ダイスドロップ時

- **トリガー**: プレイヤーがダイスを `Acceptable` 状態のインレットにドロップし、`SpriteInputHandler` が `SpriteDropCommand` を発行する。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` はコマンドを受信し、`DiceInteractionStrategy.ChkDiceDrop` でドロップが可能か最終確認します。
    2. `Strategy` が許可した場合、`DiceInteractionOrchestrator` は `DiceInletManager.OnDiceDroppedOnInlet()` を呼び出し、能力発動のロジックを依頼します。

### 4. ドラッグ終了時

- **トリガー**: ダイスのドラッグ＆ドロップ操作が完了し、`DiceInteractionOrchestrator` が `OnSpriteDragOperationCompleted` を処理するタイミング。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` は `UIActivationPolicy.ResetToDiceInletActivations()` を呼び出します。
    2. `UIActivationPolicy` は、全ての `DiceInletView` に対して `EnterNormalState()` または `EnterInactiveState()` を呼び出し、インレットを操作不可能な初期状態に戻します。

---

## 状態遷移メソッド詳細

`DiceInletView` は、外部の `DiceInteractionOrchestrator` からの指示に応じて自身の状態と見た目を変更するため、以下の `public` メソッドを実装します。

- **`EnterNormalState()`**: 通常状態に遷移します。コライダーは無効化されます。
- **`EnterAcceptableState()`**: ダイスを受け入れ可能な状態に遷移します。コライダーは有効化され、受け入れ可能アニメーションが再生されます。
- **`EnterHoveringState()`**: ダイスがホバーしている状態に遷移します。ドロップ待ちアニメーションが再生されます。
- **`EnterInactiveState()`**: 非アクティブ状態に遷移します。コライダーは無効化されます。

---

## 関連ファイル

- [guide_ui_interaction_design.md](../guide/guide_ui_interaction_design.md)
- [sys_dice_inlet_design.md](../sys/sys_dice_inlet_design.md)

---

## 更新履歴

- 2025-08-13: 設計思想とデータ構造の変更を反映し、ドキュメントを最新化 (Gemini)
- 2025-08-08: 初版 (Gemini - Technical Writer for Game Development)
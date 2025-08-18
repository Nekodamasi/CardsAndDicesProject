# ui_dice_inlet_interaction.md - ダイスインレットインタラクション設計書

---

## 概要

このドキュメントは、ダイスインレット (`DiceInletView`) のUIインタラクションに関する仕様を定義します。
ダイスインレットのインタラクションは、`DiceInteractionOrchestrator` を中心としたクラス群によって制御され、プレイヤーがダイスをドラッグした際に、そのダイスを受け入れ可能なインレットが視覚的に反応する仕組みを提供します。

---

## 共通の前提・制約

- `DiceInletView` は `BaseSpriteView` を継承します。
- `IdentifiableGameObject` コンポーネントがアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`DiceInletView.cs`**: ダイスインレットの視覚表現と状態遷移を担当するViewコンポーネント。
- **`DiceInteractionOrchestrator.cs`**: ダイス関連のUIインタラクション全体の司令塔。ダイスのドラッグ開始・終了といったイベントを検知し、関連クラスに処理を委譲します。
- **`UIActivationPolicy.cs`**: UIの状態に応じて、各インレットのインタラクション可否（Acceptable/Inactive）を決定するポリシークラス。
- **`DiceInteractionStrategy.cs`**: ダイス関連の操作（ドラッグ開始など）が、現在の状況で実行可能かを判断する戦略クラス。
- **`DiceInletManager.cs`**: ゲーム内に存在するすべての `DiceInlet` インスタンスを一元的に管理するクラスです。`UIActivationPolicy` はこの情報を参照して受け入れ可否を判断します。
- **`InletAbilityProfile.cs`**: インレットの「発動条件」と「効果」をセットで保持するデータコンテナ。
- **`DiceInletConditionSO.cs`**: インレットの具体的な発動条件（例: 偶数の目のみ許可）を定義する `ScriptableObject`。
- **アニメーションScriptableObject**: `_acceptableAnimation`, `_dropWaitingAnimation` など、各状態に対応するアニメーションアセット。

---

## インタラクションフロー

### 1. ダイスドラッグ開始時

- **トリガー**: プレイヤーがダイスのドラッグを開始し、`DiceInteractionOrchestrator` が `OnBeginDrag` を処理するタイミング。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` は、`DiceInteractionStrategy.ChkDiceBeginDrag` で操作が有効か確認します。
    2. 有効な場合、`UIStateMachine` の状態を `DraggingDice` に遷移させます。
    3. `DiceInteractionOrchestrator` は `UIActivationPolicy.DraggingDiceToInletActivations()` を呼び出します。
    4. `UIActivationPolicy` は、`ViewRegistry` から全ての `DiceInletView` を取得します。
    5. 各インレットについて、`DiceInletManager` を参照してダイスインレットインスタンス (`DiceInlet`) を取得します。
    6. 発動条件 `DiceInlet` の `CanAccept` を用いて、ドラッグ中のダイスを受け入れ可能か判断します。
    7. 受け入れ可能なインレットは `EnterAcceptableState()` を、不可なインレットは `EnterInactiveState()` を呼び出し、状態を遷移させます。

### 2. ダイスドラッグ中のホバー時

- **トリガー**: ドラッグ中のダイスが、`Acceptable` 状態のインレット上でホバーされる。
- **処理の流れ**:
    1. `SpriteInputHandler` がホバーを検知し、`SpriteHoverCommand` を発行します。
    2. `DiceInteractionOrchestrator` はコマンドを受信し、`DiceInletView` の `EnterHoveringState()` を呼び出します。
    3. `DiceInletView` は、ドロップを待っていることを示すアニメーション (`_dropWaitingAnimation`) を再生します。

### 3. ダイスドロップ後の処理

- **トリガー**: プレイヤーがダイスインレット(`DiceInletView`) にダイスをドロップしたタイミング。
- **処理の流れ**:
    - `sys_dice_inlet_management.md`の「全体フロー (ダイス投入から効果発動まで)」を参照の事
    - 一連の効果発動処理が終了した後、「ドラッグ終了時」の処理を行う。

### 4. ドラッグ終了時

- **トリガー**: ダイスのドラッグ＆ドロップ操作が完了し、`DiceInteractionOrchestrator` が `OnSpriteDragOperationCompleted` を処理するタイミング。
- **処理の流れ**:
    1. `DiceInteractionOrchestrator` は `UIActivationPolicy.ResetToDiceActivations()` を呼び出します。
    2. `UIActivationPolicy` は、全ての `DiceInletView` に対して `EnterNormalState()` または `EnterInactiveState()` を呼び出し、インレットを操作不可能な初期状態に戻します。

---

## 状態遷移メソッド詳細

`DiceInletView` は、外部の `DiceInteractionOrchestrator` や `UIActivationPolicy` からの指示に応じて自身の状態と見た目を変更するため、以下の `public` メソッドを実装します。

- **`EnterNormalState()`**: 通常状態に遷移します。コライダーは無効化され、見た目は初期状態に戻ります。
- **`EnterAcceptableState()`**: ダイスを受け入れ可能な状態に遷移します。コライダーは有効化され、受け入れ可能アニメーションが再生されます。
- **`EnterHoveringState()`**: ダイスがホバーしている状態に遷移します。ドロップ待ちアニメーションが再生されます。
- **`EnterInactiveState()`**: 非アクティブ状態に遷移します。コライダーは無効化され、操作不能であることが視覚的に示されます。

---

## 関連ファイル

- [sys_dice_inlet_management.md](../sys/sys_dice_inlet_management.md)
- [guide_ui_interaction_design.md](../guide/guide_ui_interaction_design.md)

---

## 更新履歴

- 2025-08-13: 最新のソースコードと設計に合わせて、全体を全面的に更新 (Gemini)
- 2025-08-08: 初版 (Gemini - Technical Writer for Game Development)

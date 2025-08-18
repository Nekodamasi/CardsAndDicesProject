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

-   **View**:
    -   `DiceInletView.cs`: ダイスインレットの視覚表現と状態遷移を担当するViewコンポーネント。
-   **Orchestrator / State**:
    -   `DiceInteractionOrchestrator.cs`: ダイス関連のUIインタラクション全体の司令塔。
    -   `UIStateMachine.cs`: UI全体の状態（例: `DraggingDice`）を管理する。
-   **Policy / Strategy**:
    -   `UIActivationPolicy.cs`: UIの状態に応じて、各インレットのインタラクション可否（Acceptable/Inactive）を決定する。
    -   `DiceInteractionStrategy.cs`: ダイス関連の操作が、現在の状況で実行可能かを判断する。
-   **Model / Manager**:
    -   `DiceInletManager.cs`: ゲーム内に存在するすべての `IDiceInlet` （ダイスインレットの論理表現）インスタンスを管理する。
    -   `IDiceInlet.cs`: ダイスインレットの論理インスタンスのインターフェース。`CanAccept` メソッドを持つ。
-   **Commands**:
    -   `SpriteBeginDragCommand`: ドラッグ開始を通知する。
    -   `SpriteHoverCommand`: ホバー開始を通知する。
    -   `SpriteDropCommand`: ドロップ操作を通知する。
    -   `SpriteDragOperationCompletedCommand`: 一連のドラッグ操作の完了を通知する。
-   **Animations (ScriptableObject)**:
    -   `_acceptableAnimation`: 受け入れ可能状態を示すアニメーション。
    -   `_dropWaitingAnimation`: ドロップ待機状態（ホバー時）を示すアニメーション。

---

## インタラクションフロー

### 1. ダイスドラッグ開始時

-   **トリガー**: プレイヤーが `DiceView` のドラッグを開始し、`SpriteInputHandler` が `SpriteBeginDragCommand` を発行。
-   **処理の流れ**:
    1.  `DiceInteractionOrchestrator` は `OnBeginDrag` メソッドでコマンドを受信します。
    2.  `DiceInteractionStrategy.ChkDiceBeginDrag` を呼び出し、操作が有効か確認します。
    3.  有効な場合、`UIStateMachine` の状態を `DraggingDice` に遷移させ、ドラッグ対象のダイスIDを記録します。
    4.  `DiceInteractionOrchestrator` は `UIActivationPolicy.DraggingDiceToInletActivations()` を呼び出します。
    5.  `UIActivationPolicy` は、`ViewRegistry` から全ての `DiceInletView` を取得します。
    6.  各インレットについて、`DiceInletManager.GetInlet()` を通じて論理インスタンス `IDiceInlet` を取得します。
    7.  `IDiceInlet.CanAccept(draggedDiceData)` を呼び出し、ドラッグ中のダイスを受け入れ可能か判断します。
    8.  `UIActivationPolicy` は、結果に基づき各 `DiceInletView` の `EnterAcceptableState()` （受け入れ可）または `EnterInactiveState()` （受け入れ不可）を呼び出し、状態を遷移させます。

### 2. ダイスドラッグ中のホバー

-   **トリガー**: ドラッグ中のダイスが、`Acceptable` 状態の `DiceInletView` 上でホバーされる。`SpriteInputHandler` が `SpriteHoverCommand` を発行。
-   **処理の流れ**:
    1.  `DiceInteractionOrchestrator` は `OnHover` メソッドでコマンドを受信します。
    2.  `DiceInteractionStrategy.ChkDiceHover` でホバー操作が有効か（UIの状態が `DraggingDice` かつ、ホバー対象が `Acceptable` 状態のインレットか）を確認します。
    3.  有効な場合、対象の `DiceInletView` の `EnterHoveringState()` を呼び出します。
    4.  `DiceInletView` は、ドロップを待っていることを示すアニメーション (`_dropWaitingAnimation`) を再生します。
    5.  ダイスがインレット上から離れた場合 (`SpriteUnhoverCommand`)、`DiceInletView` は `EnterAcceptableState()` を再度呼び出され、元の受け入れ可能状態の表示に戻ります。

### 3. ダイスドロップ時

-   **トリガー**: プレイヤーが `Acceptable` 状態の `DiceInletView` 上でマウスボタンを離し、`SpriteInputHandler` が `SpriteDropCommand` を発行。
-   **処理の流れ**:
    1.  `DiceInteractionOrchestrator` は `OnDrop` メソッドでコマンドを受信し、`DiceInteractionStrategy.ChkDiceDrop` で操作が有効か確認します。
    2.  有効な場合、`UIStateMachine` を `DropedDice` 状態に遷移させ、`IsDroppedSuccessfully` フラグを立てます。
    3.  `DiceInletManager` を介して、対応する `IDiceInlet` の `OnDiceDropped()` メソッドが呼び出され、カウントダウンの更新や能力発動などの論理処理が実行されます。
    4.  詳細は `sys_dice_inlet_management.md` の「全体フロー (ダイス投入から効果発動まで)」に記載されています。

### 4. ドラッグ操作完了時

-   **トリガー**: 一連のドラッグ操作が完了し、`DiceInteractionOrchestrator` が `SpriteDragOperationCompletedCommand` を受信。
-   **処理の流れ**:
    1.  `DiceInteractionOrchestrator` は `OnSpriteDragOperationCompleted` メソッドを処理します。
    2.  `UIActivationPolicy.ResetToDiceActivations()` を呼び出します。
    3.  `UIActivationPolicy` は、全ての `DiceInletView` に対して `EnterNormalState()` を呼び出し、インレットを操作前の初期状態に戻します。
    4.  `UIStateMachine` の状態が `Idle` に戻り、UI操作が再度有効化されます。

---

## 状態遷移メソッド詳細

`DiceInletView` は、外部の `DiceInteractionOrchestrator` や `UIActivationPolicy` からの指示に応じて自身の状態と見た目を変更するため、以下の `public` メソッドを実装します。

-   **`EnterNormalState()`**: 通常状態に遷移します。コライダーは無効化され、見た目は初期状態に戻ります。
-   **`EnterAcceptableState()`**: ダイスを受け入れ可能な状態に遷移します。コライダーは有効化され、受け入れ可能アニメーションが再生されます。
-   **`EnterHoveringState()`**: ダイスがホバーしている状態に遷移します。ドロップ待ちアニメーションが再生されます。
-   **`EnterInactiveState()`**: 非アクティブ状態に遷移します。コライダーは無効化され、操作不能であることが視覚的に示されます。

---

## 関連ファイル

-   [../sys/sys_dice_inlet_management.md](../sys/sys_dice_inlet_management.md)
-   [../gdd/gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
-   [../guide/guide_ui_interaction_design.md](../guide/guide_ui_interaction_design.md)
-   [../sys/sys_creature_management.md](../sys/sys_creature_management.md)

---

## 更新履歴

-   2025-08-17: 最新のソースコードと設計に合わせて、全体を全面的に更新 (AI Document Specialist)
-   2025-08-13: 最新のソースコードと設計に合わせて、全体を全面的に更新 (Gemini)
-   2025-08-08: 初版 (Gemini - Technical Writer for Game Development)
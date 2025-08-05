# ui_card_slot_interaction.md - カードスロットインタラクション設計書

---

## 概要

このドキュメントは、カードスロット (`CardSlotView`) のUIインタラクションに関する仕様を定義します。

`CardSlotView` のインタラクションは、`UIInteractionOrchestrator` と `UIActivationPolicy` によって完全に制御されます。`CardSlotView` 自身が状態を判断して振る舞いを変えることはなく、外部からの指示に応じて、対応する状態に遷移し、指定されたアニメーションを再生する責務のみを持ちます。

---

## 共通の前提・制約

- `CardSlotView` は `BaseSpriteView` を継承します。
- アニメーションの期間は、`BaseSpriteView` の `_animationDuration` フィールドを尊重します。
- `CompositeObjectId` を持つGameObjectは、必ず `IdentifiableGameObject` コンポーネントを持ちます。
- `MultiRendererVisualController` は、`CardSlotView` にアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`CardSlotView.cs`**: カードスロットの視覚表現と、状態遷移メソッドの実行を担当するViewコンポーネント。
- **`SpriteInputHandler.cs`**: スロットに対するマウスイベントを検知し、`SpriteHoverCommand` や `SpriteDropCommand` を発行します。
- **`UIInteractionOrchestrator.cs`**: UIインタラクション全体の司令塔。`SpriteInputHandler` からのコマンドを受け取り、`UIActivationPolicy` に基づいて `CardSlotView` への指示を出します。
- **`UIActivationPolicy.cs`**: UIの状態（`UIStateMachine.CurrentState`）に基づき、各スロットがどの状態（Acceptable, Inactive等）になるべきかを決定するポリシークラス。
- **アニメーションScriptableObject一覧**:
    - `_normalAnimation`: 通常時や非アクティブ時のアニメーション。
    - `_acceptableAnimation`: ドラッグされたカードを受け入れ可能な状態を示すアニメーション。
    - `_dropWaitingAnimation`: ドラッグされたカードがホバーされ、ドロップを待っている状態のアニメーション。

---

## インタラクションフロー

### 1. カードドラッグ開始時

- **トリガー**: プレイヤーがカードのドラッグを開始し、`UIInteractionOrchestrator` が `OnBeginDrag` を処理するタイミング。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy.DraggingCardToCardSlotActivations()` を呼び出します。
    2. `UIActivationPolicy` は、全ての `CardSlotView` に対して、以下のルールに基づき状態遷移を指示します。
        - **敵チームのスロット**: `EnterInactiveState()` を呼び出し、非アクティブ状態にします。
        - **ドラッグ元が場（Top/Bottom Line）の場合**: 全てのハンドスロットは `EnterAcceptableState()` を呼び出し、受け入れ可能状態になります。
        - **ドラッグ元が手札の場合**: 全てのハンドスロットは `EnterInactiveState()` を呼び出し、非アクティブ状態になります。
        - **上記以外のプレイヤースロット**: `EnterAcceptableState()` を呼び出し、受け入れ可能状態になります。

### 2. カードドラッグ中のホバー時

- **トリガー**: ドラッグ中のカードが、`Acceptable` 状態のスロット上でホバーされる (`OnHover`)。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は、`CardInteractionStrategy` によるチェックを経て、`CardSlotManager.OnCardHoveredOnSlot()` を呼び出し、リフローのプレビュー処理を開始させます。
    2. 同時に、ホバーされた `CardSlotView` の `EnterHoveringState()` を呼び出します。
    3. `CardSlotView` は、ドロップを待っていることを示すアニメーション (`_dropWaitingAnimation`) を再生します。

### 3. ドラッグ終了時

- **トリガー**: カードのドラッグ＆ドロップ操作が完了し、`UIInteractionOrchestrator` が `OnSpriteDragOperationCompleted` を処理するタイミング。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy.ResetToCardSlotActivations()` を呼び出します。
    2. `UIActivationPolicy` は、全ての `CardSlotView` に対して `EnterInactiveState()` を呼び出し、スロットを操作不可能な初期状態に戻します。

---

## 状態遷移メソッド詳細

`CardSlotView` は、外部からの指示に応じて自身の状態と見た目を変更するため、以下の `public` メソッドを実装します。

- **`EnterNormalState()`**
  - **役割:** 通常状態に遷移します。
  - **処理:** コライダーを無効化し、通常のアニメーション (`_normalAnimation`) を再生します。

- **`EnterAcceptableState()`**
  - **役割:** ドラッグ中のカードを受け入れ可能な状態に遷移します。
  - **処理:** コライダーを有効化し、受け入れ可能アニメーション (`_acceptableAnimation`) を再生します。

- **`EnterHoveringState()`**
  - **役割:** ドラッグ中のカードがホバーしている状態に遷移します。
  - **処理:** コライダーは有効なまま、ドロップ待ちアニメーション (`_dropWaitingAnimation`) を再生します。

- **`EnterInactiveState()`**
  - **役割:** インタラクションの対象外であることを示す非アクティブ状態に遷移します。
  - **処理:** コライダーを無効化し、通常のアニメーション (`_normalAnimation`) を再生します。

---

## 関連ファイル

- [guide_rules.md](../../guide/guide_rules.md)
- [guide_files.md](../../guide/guide_files.md)
- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [gdd_reflow_system.md](../gdd/gdd_reflow_system.md)
- [sys_card_slot_manager.md](../sys/sys_card_slot_manager.md)

---

## 更新履歴
- 2025-08-04: `UIInteractionOrchestrator` と `UIActivationPolicy` による外部制御のフローを具体的に記述 (Gemini - Codebase Analyst)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)
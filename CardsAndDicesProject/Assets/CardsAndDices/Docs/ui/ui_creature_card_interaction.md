# ui_creature_card_interaction.md - クリーチャーカードインタラクション設計書

---

## 概要

このドキュメントは、クリーチャーカード (`CreatureCardView`) のUIインタラクションに関する仕様を定義します。

`CreatureCardView` のインタラクションは、`UIInteractionOrchestrator` を中心としたクラス群によって完全に制御されます。`CreatureCardView` 自身が複雑な状態判断を行うことはなく、外部からの指示に応じて、対応する状態に遷移し、指定されたアニメーションを再生する責務のみを持ちます。

---

## 共通の前提・制約

- `CreatureCardView` は `BaseSpriteView` を継承します。
- アニメーションの期間は、`BaseSpriteView` の `_animationDuration` フィールドを尊重します。
- `CompositeObjectId` を持つGameObjectは、必ず `IdentifiableGameObject` コンポーネントを持ちます。
- `MultiRendererVisualController` は、`CreatureCardView` にアタッチされていることを前提とします。

---

## 主要コンポーネントと関連クラス

- **`CreatureCardView.cs`**: クリーチャーカードの視覚表現と、状態遷移メソッドの実行を担当するViewコンポーネント。
- **`SpriteInputHandler.cs`**: カードに対するマウスイベントを検知し、`SpriteHoverCommand` や `SpriteBeginDragCommand` などを発行します。
- **`CardInteractionOrchestrator.cs`**: UIインタラクション全体の司令塔。UI関連コマンドを購読し、`Strategy` や `Policy` に基づいて `CreatureCardView` への指示を出します。
- **`CardInteractionStrategy.cs`**: 「アイドル状態のカードはホバー可能か？」など、特定の状況でインタラクションが可能かを判断する戦略クラス。
- **`UIActivationPolicy.cs`**: 「カードドラッグ開始時、ドラッグされていない他のカードを非アクティブにする」といった、UI要素の有効/無効ルールを定義するポリシークラス。
- **アニメーションScriptableObject一覧**:
    - `_normalAnimation`: 通常状態のアニメーション。
    - `_hoverAnimation`: ホバー状態のアニメーション。
    - `_dragAnimation`: ドラッグ開始時のアニメーション。

---

## インタラクションフロー

### 1. アイドル時のホバー

- **トリガー**: `UIStateMachine` が `Idle` の状態で、プレイヤーがカード上にマウスポインターを乗せる。
- **処理の流れ**:
    1. `SpriteInputHandler` が `SpriteHoverCommand` を発行します。
    2. `UIInteractionOrchestrator` はコマンドを受信し、`CardInteractionStrategy.ChkCardHover` を呼び出してホバーが可能か確認します。
    3. `Strategy` が `true` を返した場合、`Orchestrator` は対象の `CreatureCardView` の `EnterHoveringState()` を呼び出します。
    4. `CreatureCardView` はホバーアニメーション (`_hoverAnimation`) を再生します。

### 2. カードドラッグ開始時

- **トリガー**: プレイヤーがカードのドラッグを開始する。
- **処理の流れ**:
    1. `SpriteInputHandler` が `SpriteBeginDragCommand` を発行します。
    2. `UIInteractionOrchestrator` は `CardInteractionStrategy.ChkCardBeginDrag` でドラッグ開始が可能か確認します。
    3. `Strategy` が `true` を返した場合、`Orchestrator` は以下の処理を行います。
        a. `UIStateMachine` の状態を `DraggingCard` に設定します。
        b. ドラッグされた `CreatureCardView` の `EnterDraggingState()` を呼び出します。
        c. `UIActivationPolicy.DraggingCardToCardActivations()` を呼び出し、ドラッグされているカード以外の全ての `CreatureCardView` の `EnterInactiveState()` を実行させ、非アクティブ化します。

### 3. リフローアニメーション

- **トリガー**: `UIInteractionOrchestrator` が `ReflowCompletedCommand` または `DragReflowCompletedCommand` を受信する。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は、コマンド内の移動情報 (`CardMovements`) に含まれる `CreatureCardView` を `ViewRegistry` から取得します。
    2. 該当する `CreatureCardView` それぞれの `MoveToAnimated(Vector3 targetPosition)` メソッドを呼び出し、指定された位置への移動アニメーションを並行して実行させます。

### 4. ドラッグ終了時

- **トリガー**: ドラッグ＆ドロップ操作が完了し、`UIInteractionOrchestrator` が `OnSpriteDragOperationCompleted` を処理するタイミング。
- **処理の流れ**:
    1. `UIInteractionOrchestrator` は `UIActivationPolicy.ResetToCardActivations()` を呼び出します。
    2. `UIActivationPolicy` は、全ての `CreatureCardView` に対して `EnterNormalState()` を呼び出し、インタラクション可能な通常状態に戻します。

---

## 状態遷移メソッド詳細

`CreatureCardView` は、外部からの指示に応じて自身の状態と見た目を変更するため、以下の `public` メソッドを実装します。

- **`EnterNormalState()`**: 通常状態に遷移します。コライダーは有効化され、通常アニメーションが再生されます。
- **`EnterHoveringState()`**: ホバー状態に遷移します。ホバーアニメーションが再生されます。
- **`EnterDraggingState()`**: ドラッグ開始状態に遷移します。コライダーは無効化され、ドラッグ開始アニメーションが再生されます。
- **`EnterDraggingInProgressState()`**: ドラッグ中状態に遷移します。マウス追従のため、個別のアニメーションは再生しません。
- **`EnterInactiveState()`**: 非アクティブ状態に遷移します。コライダーは無効化され、通常（非アクティブ）アニメーションが再生されます。
- **`MoveTo(Vector3 targetPosition)`**: 指定された位置へ即座に移動します。
- **`MoveToAnimated(Vector3 targetPosition)`**: 指定された位置へアニメーション付きで移動します。`UniTask` を返し、アニメーションの完了を待機可能にします。
- **`SetGrayscale(bool enabled)`**: カードの見た目をグレースケール（プレイ不可など）に切り替えます。
- **`SetInteractionProfile(InteractionProfile profile)`**: `SpriteInputHandler` が参照する `InteractionProfile` を切り替え、ドラッグ可否などを動的に制御します。

---

## 関連ファイル

- [guide_rules.md](../../guide/guide_rules.md)
- [guide_files.md](../../guide/guide_files.md)
- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [gdd_reflow_system.md](../gdd/gdd_reflow_system.md)
- [sys_card_slot_manager.md](../sys/sys_card_slot_manager.md)
- [ui_card_slot_interaction.md](./ui_card_slot_interaction.md)

---

## 更新履歴
- 2025-08-04: `UIInteractionOrchestrator` による外部制御のフローを具体的に記述し、コマンド購読に関する記述を削除 (Gemini - Codebase Analyst)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)
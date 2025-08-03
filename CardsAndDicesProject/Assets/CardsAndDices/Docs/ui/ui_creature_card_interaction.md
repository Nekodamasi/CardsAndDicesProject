# ui_creature_card_interaction.md - クリーチャーカードインタラクション設計書

---

## 概要

このドキュメントの目的：クリーチャーカードのUIインタラクション（マウスイベント、発行されるコマンド、`CreatureCardView` および関連コンポーネントの処理、視覚・聴覚フィードバック）の仕様を定義します。
スコープ：カードのホバー、アンホバー、ドラッグ開始、ドラッグ中、ドラッグ終了、クリック、ドロップに関する挙動。
`UIStateMachine` による状態管理との連携について詳述します。

---

## 共通の前提・制約

- 全てのUI要素は、`BaseSpriteView` を継承していることを前提とする。
- アニメーションの期間は、`BaseSpriteView` の `_animationDuration` フィールドを尊重する。
- SEの再生は、`FXManager` を通じて行うことを前提とする。
- `CompositeObjectId` を持つGameObjectは、必ず `IdentifiableGameObject` コンポーネントを持つ。
- `MultiRendererVisualController` は、`BaseSpriteView` を継承するUI要素にアタッチされていることを前提とする。

--- 

## 主要コンポーネント

- `CreatureCardView.cs`: クリーチャーカードの視覚表現とインタラクションの起点となるViewコンポーネント。
- `BaseSpriteView.cs`: `CreatureCardView` の基底クラス。共通のUIインタラクションロジックとコマンド購読機能を提供。
- `SpriteInputHandler.cs`: マウスイベントを検知し、対応するコマンドを`SpriteCommandBus`に発行する。
- `SpriteCommandBus.cs`: UI関連コマンドの伝達ハブ。
- `UIStateMachine.cs`: UIの全体的なインタラクション状態（例: `Idle`, `DraggingCard`）を一元管理するScriptableObject。
- `_cardSlotManager.cs`: カードスロットの論理的な状態とリフロー処理を管理するScriptableObject。
- **コマンド一覧**:
-  `SpriteHoverCommand`
-  `SpriteUnhoverCommand`
-  `SpriteBeginDragCommand`
-  `SpriteDragCommand`
-  `SpriteEndDragCommand`
-  `SpriteClickCommand`
-  `SpriteDropCommand`
-  `ReflowCompletedCommand`
- **アニメーションScriptableObject一覧**:
-  `BaseAnimationSO` (基底)
-  `HoverAnimationSO`
-  `NormalAnimationSO`
-  `DragAnimationSO`

---

## インタラクションフロー

`CreatureCardView` のインタラクションは、`UIInteractionOrchestrator` からの指示によって制御されます。
`CreatureCardView` 自身は、コマンドを購読して複雑な状態判断を行うのではなく、`UIInteractionOrchestrator` からの指示に従って自身の状態を変更し、アニメーションを再生する責務を持ちます。

### プレイ不可状態（満員時）

- **条件:** `CardSlotManager` がPlayerZoneが「満員」であると判断した場合。
- **対象:** HandZoneに存在するクリーチャーカード。
- **視覚的フィードバック:**
    - カードの見た目がグレーアウトする。
- **インタラクションの制限:**
    - **ドラッグ:** 不可になる。
    - **ホバー/アンホバー:** 可能。視覚的なフィードバック（拡大など）は通常通り行われる。
    - **クリック:** 可能。（将来的にカードの詳細表示などに利用できる）
- **状態の復帰:** PlayerZoneの「満員」状態が解消されると、自動的に通常のインタラクションが可能になり、見た目も元に戻る。

### 状態遷移メソッド

`CreatureCardView` は、`UIInteractionOrchestrator` から呼び出される以下の `public` メソッドを持ちます。

- **`EnterNormalState()`**
  - **役割:** 通常状態に遷移します。
  - **処理:** コライダーを有効化し、通常のアニメーション (`_normalAnimation`) を再生します。

- **`EnterHoveringState()`**
  - **役割:** マウスカーソルがホバーしている状態に遷移します。
  - **処理:** ホバーアニメーション (`_hoverAnimation`) を再生します。

- **`EnterDraggingState()`**
  - **役割:** ドラッグされている状態に遷移します。
  - **処理:** コライダーを無効化し、ドラッグアニメーション (`_dragAnimation`) を再生します。

- **`MoveTo(Vector3 targetPosition)`**
  - **役割:** 指定された位置へ移動するアニメーションを実行します。
  - **トリガー:** 主に `ReflowCompletedCommand` を受けた `UIInteractionOrchestrator` から呼び出されます。
  - **処理:** DOTweenなどを用いて、`targetPosition` への移動アニメーションを再生します。

- **`SetGrayscale(bool enabled)`**
  - **役割:** カードの見た目をグレースケール（または通常色）に切り替えます。
  - **トリガー:** 主に `CreatureCardController` から、PlayerZoneの満員状態に応じて呼び出されます。

- **`SetInteractionProfile(InteractionProfile profile)`**
  - **役割:** `SpriteInputHandler` が参照する `InteractionProfile` を動的に切り替えます。
  - **トリガー:** 主に `CreatureCardController` から呼び出され、カードのインタラクション（ドラッグ可否など）を制御します。

### 状態遷移の管理

- **状態遷移の責任:** どのタイミングでどの状態に遷移するかは、全て `UIInteractionOrchestrator` が決定します。
- **アニメーション:** 各状態に対応するアニメーションは、`CreatureCardView` 自身が `ScriptableObject` として保持し、指示に応じて再生します。
- **コマンド発行:** `CreatureCardView` は、`SpriteInputHandler` からのイベントをトリガーとしてコマンドを発行しますが、そのコマンドを自身で解釈することはありません。

### 7. リフロー完了時

- **トリガー**: `CardSlotManager` がリフロー計算を終え、`ReflowCompletedCommand` を発行した時。
- **処理**:
    - `OnReflowCompleted` メソッドが呼び出される。
    - コマンド内の移動情報に自身のIDが含まれているか確認する。
    - 含まれている場合、現在の位置と目標位置に差があれば、DOTweenを使って目標位置への移動アニメーションを開始する。
    - アニメーション中は自身のコライダーを無効化する。
    - アニメーション完了後、`SpriteDragOperationCompletedCommand` を発行する。

### 8. ドラッグ操作完了時

- **トリガー**: `SpriteDragOperationCompletedCommand` を受け取った時。
- **処理**:
    - `OnSpriteDragOperationCompleted` メソッドが呼び出される。
    - 自身の状態を `Normal` に戻す。
    - 自身のコライダーを有効に戻す。
    - 通常状態のアニメーション (`_normalAnimation`) を開始する。

---

## 考慮事項と補足

- **`UIStateMachine` との連携**: 各UI要素は`UIStateMachine`の`CurrentState`を参照し、自身のインタラクション挙動を適切に制御します。これにより、UIの競合や意図しない操作を防ぎます。
- **コライダーの制御**: ドラッグ中のカードやスロットのコライダーの有効/無効を適切に切り替えることで、正確なインタラクションとパフォーマンスを確保します。
- **リフロー処理との関連**: ドロップイベントはリフロー処理をトリガーし、カードの視覚的な再配置を促します。
- **アニメーションのキャンセル**: 新しいアニメーションが開始される際、既存のアニメーションは`KillCurrentAnimation()`によって適切に停止されます。

---

## 関連ファイル

-   [guide_design-principles.md](../../guide/guide_design-principles.md)
-   [guide_unity-cs.md](../../guide/guide_unity-cs.md)
-   [gdd_combat_system_mockup.md](../../gdd/gdd_combat_system_mockup.md)
-   [gdd_sprite_ui_design.md](../../gdd/gdd_sprite_ui_design.md)
-   [sys_domain-model.md](../../sys/sys_domain-model.md)

---

## 更新履歴
- 2025-07-25: 初版 (Gemini - Technical Writer for Game Development)

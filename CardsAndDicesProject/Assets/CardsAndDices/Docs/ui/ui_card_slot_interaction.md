# ui_card_slot_interaction.md - カードスロットインタラクション設計書

---

## 概要

このドキュメントの目的：カードスロットのUIインタラクション（マウスイベント、発行されるコマンド、`CardSlotView` および関連コンポーネントの処理、視覚・聴覚フィードバック）の仕様を定義します。
スコープ：スロットのホバー、アンホバー、ドラッグ開始、ドラッグ中、ドラッグ終了、クリック、ドロップに関する挙動。
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

- `CardSlotView.cs`: カードスロットの視覚表現とインタラクションの起点となるViewコンポーネント。
- `BaseSpriteView.cs`: `CardSlotView` の基底クラス。共通のUIインタラクションロジックとコマンド購読機能を提供。
- `SpriteInputHandler.cs`: マウスイベントを検知し、対応するコマンドを`SpriteCommandBus`に発行する。
- `SpriteCommandBus.cs`: UI関連コマンドの伝達ハブ。
- `UIStateMachine.cs`: UIの全体的なインタラクション状態（例: `Idle`, `DraggingCard`）を一元管理するScriptableObject。
- `_cardSlotManager.cs`: カードスロットの論理的な状態とリフロー処理を管理するScriptableObject。
- **コマンド一覧**:
- `SpriteHoverCommand`
- `SpriteUnhoverCommand`
- `SpriteBeginDragCommand`
- `SpriteDragCommand`
- `SpriteEndDragCommand`
- `SpriteClickCommand`
- `SpriteDropCommand`
- `ReflowCompletedCommand`
- **アニメーションScriptableObject一覧**:
- `BaseAnimationSO` (基底)
- `HoverAnimationSO`
- `NormalAnimationSO`
- `AcceptableAnimationSO` (CardSlotView用)
- `DropWaitingAnimationSO` (CardSlotView用 - ドロップ待ち)

---

## インタラクションフロー

`CardSlotView` のインタラクションは、`UIInteractionOrchestrator` からの指示によって完全に制御されます。
`CardSlotView` 自身は、コマンドを購読したり、複雑な状態判断を行ったりしません。

### 状態遷移メソッド

`CardSlotView` は、`UIInteractionOrchestrator` から呼び出される以下の `public` メソッドを持ち、自身の状態と見た目を変更します。

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
  - **役割:** ドラッグ中のカードが元々配置されていたスロットなど、インタラクションの対象外であることを示す状態に遷移します。
  - **処理:** コライダーを無効化し、通常のアニメーション (`_normalAnimation`) または専用の非アクティブアニメーションを再生します。

### 状態遷移の管理

- **状態遷移の責任:** どのタイミングでどの状態に遷移するかは、全て `UIInteractionOrchestrator` が決定します。
- **アニメーション:** 各状態に対応するアニメーションは、`CardSlotView` 自身が `ScriptableObject` として保持し、指示に応じて再生します。
- **コマンド発行:** `CardSlotView` は、`SpriteInputHandler` からのイベントをトリガーとしてコマンドを発行しますが、そのコマンドを自身で解釈することはありません。

---

## 考慮事項と補足

- `UIStateMachine` との連携: 各UI要素は`UIStateMachine`の`CurrentState`を参照し、自身のインタラクション挙動を適切に制御します。これにより、UIの競合や意図しない操作を防ぎます。
- コライダーの制御: ドラッグ中のカードやスロットのコライダーの有効/無効を適切に切り替えることで、正確なインタラクションとパフォーマンスを確保します。
- リフロー処理との関連: ドロップイベントは`_cardSlotManager`のリフロー処理をトリガーし、カードの視覚的な再配置を促します。
- アニメーションのキャンセル: 新しいアニメーションが開始される際、既存のアニメーションは`KillCurrentAnimation()`によって適切に停止されます。

---

## 関連ファイル

- [guide_design-principles.md](../../guide/guide_design-principles.md)
- [guide_unity-cs.md](../../guide/guide_unity_cs.md)
- [gdd_combat_system_mockup.md](../../gdd/gdd_combat_system_mockup.md)
- [gdd_sprite_ui_design.md](../../gdd/gdd_sprite_ui_design.md)
- [sys_domain-model.md](../../sys/sys_domain_model.md)
- [ui_creature_card_interaction.md](../ui/ui_creature_card_interaction.md)

---

## 更新履歴
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)

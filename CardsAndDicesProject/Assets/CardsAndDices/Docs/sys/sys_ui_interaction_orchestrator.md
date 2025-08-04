# sys_ui_interaction_orchestrator.md - UIインタラクションオーケストレーター設計書

---

## 1. 概要

このドキュメントは、UI全体のインタラクションを一元的に管理・指揮する `UIInteractionOrchestrator` クラスの設計を定義します。

本クラスは、UIイベントのハブとして機能し、インタラクションの条件チェックと、それに応じたロジックの実行という2つの大きな責務を持ちます。

---

## 2. 役割と責務

`UIInteractionOrchestrator` は、UIイベントの購読、インタラクション条件の検証、および具体的な処理の実行を統括します。

- **UIイベントの一元的な購読:** `SpriteCommandBus` を流れる全てのUI関連コマンド（`SpriteBeginDragCommand`, `SpriteHoverCommand`など）を購読します。
- **インタラクション条件の検証:** `CardInteractionStrategy` を利用して、受け取ったイベントを現在のゲーム状況で処理すべきかどうかの条件をチェックします。
- **具体的な処理の実行:** 条件が満たされた場合、`UIStateMachine` の状態変更、`CardSlotManager` への処理依頼、`View` への視覚的更新指示など、イベントに応じた具体的なロジックを実行します。
- **リフロー中のホバーイベント制御:** 高速な連続ホバー操作による過剰なリフローを防ぐため、デバウンスとキューイングを組み合わせた制御を行います。

---

## 3. 主要な依存コンポーネント

- **`SpriteCommandBus`**: UIイベントを受け取るためのコマンドハブ。
- **`UIStateMachine`**: UIのグローバルな状態（`Idle`, `DraggingCard`など）を参照するためのステートマシン。
- **`CardSlotManager`**: スロットの論理的な状態を参照し、リフロー処理を依頼するためのマネージャー。
- **`ReflowService`**: リフロー計算ロジックを提供するサービス。
- **`ViewRegistry`**: `CompositeObjectId` をキーとして、シーン上に存在する `BaseSpriteView` のインスタンスを検索・取得するためのレジストリクラス。
- **`CardInteractionStrategy`**: インタラクションを実行するための**条件**をチェックする責務を持つクラス。具体的な処理ロジックは持たず、「いつ（When）イベントを処理すべきか」の判断のみを行います。
- **`UIActivationPolicy`**: UI要素の活性/非活性ルールを管理するクラス。
- **`SystemReflowController`**: システム起因のカード移動アニメーションを管理するクラス。

---

## 4. インタラクションフローの例

### 例1：カードのドラッグ開始時 (`OnBeginDrag`)

1.  `SpriteBeginDragCommand` を受け取ります。
2.  `_cardInteractionStrategy.ChkCardBeginDrag()` を呼び出し、ドラッグを開始できる状況か検証します。
3.  `true` が返された場合、`UIStateMachine` の状態を `DraggingCard` に変更し、関連するViewの更新やUIの活性化状態の変更など、具体的なドラッグ開始処理を実行します。

### 例2：リフロー中のホバーイベント (`OnHover`)

1.  `SpriteHoverCommand` を受け取ります。
2.  `_currentReflowState` が `InProgress`（リフロー処理中）かチェックします。
3.  **リフロー中の場合:** 受け取ったコマンドを `_nextHoverCommand` 変数に保存（キューイング）し、処理を中断します。新しいホバーイベントが来た場合は、常にこの変数を上書きします。
4.  **アイドル状態の場合:** `ExecuteHover()` を呼び出し、通常のリフロー処理を開始します。
5.  リフローアニメーション完了後、`OnReflowOperationCompleted` が呼び出されます。
6.  `OnReflowOperationCompleted` 内で `_nextHoverCommand` をチェックし、キューにコマンドが存在すれば、それを実行してデバウンス処理を実現します。キューが空であれば、UI操作を有効化して一連の処理を完了します。

---

## 5. 各Viewに要求されるインターフェース

`UIInteractionOrchestrator` が正しく動作するために、`BaseSpriteView` およびその派生クラスは、以下のような状態遷移メソッドを `public` で公開する必要があります。

- `EnterNormalState()`
- `EnterHoveringState()`
- `EnterDraggingState()`
- `EnterDraggingInProgressState()`
- `EnterAcceptableState()`
- `EnterInactiveState()`
- `MoveTo(Vector3 position)`
- `MoveToAnimated(Vector3 targetPosition)`: **戻り値が `UniTask` に変更されました。**
- `SetColliderEnabled(bool enable)`

---

## 6. UI操作制限モード (UI Operation Restriction Mode)

イベントシーンや戦闘中など、プレイヤーからのUI入力を一時的に無効化する必要がある場合、UI操作制限モードが使用されます。このモードはシステムからのみ呼び出され、プレイヤーが任意で操作するポーズ機能とは異なります。

### 動作原理

- **無効化**: `DisableUIInteractionCommand` が発行されると、全ての `BaseSpriteView` は自身の `BoxCollider2D` を無効化し、UI操作を受け付けなくなります。
- **有効化**: `EnableUIInteractionCommand` が発行されると、各 `BaseSpriteView` は、モードに入る前の自身の `BoxCollider2D` の状態（有効/無効）を復元します。これにより、UI操作制限モード解除時に、元のインタラクション状態が再現されます。

### 実装詳細

- **`BaseSpriteView`**: `Awake` メソッドで自身の `BoxCollider2D` の初期状態を `_originalColliderEnabled` に記憶します。`EnableUIInteractionCommand` と `DisableUIInteractionCommand` を購読し、それぞれ `OnEnableUIInteraction` と `OnDisableUIInteraction` メソッドでコライダーの状態を制御します。

---

## 関連ファイル

- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [sys_card-reflow.md](./sys_card-reflow.md)
- `Assets/CardsAndDices/Scripts/Systems/CardInteractionStrategy.cs`
- `Assets/CardsAndDices/Scripts/Systems/SystemReflowController.cs`

---

## 更新履歴

- 2025-08-03: `IInteractionStrategy` の廃止と `CardInteractionStrategy` の責務変更を反映。リフロー中のホバーイベント制御（デバウンス）に関する設計を追記。
- 2025-08-01: UI操作制限モードの導入を反映。
- 2025-07-26: 初版作成

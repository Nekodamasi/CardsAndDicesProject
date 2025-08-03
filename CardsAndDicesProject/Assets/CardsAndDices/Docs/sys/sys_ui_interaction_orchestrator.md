# sys_ui_interaction_orchestrator.md - UIインタラクションオーケストレーター設計書

---

## 1. 概要

このドキュメントは、UI全体のインタラクションを一元的に管理・指揮する `UIInteractionOrchestrator` クラスの設計を定義します。

本クラスは、UIインタラクションの複雑化に対応するため、**Strategyパターン**と**Policyパターン**を導入し、責務の分離と拡張性の向上を図りました。

--- 

## 2. 役割と責務

`UIInteractionOrchestrator` は、UIイベントのハブとして機能し、現在のUI状態とゲームの論理状態に基づいて、適切なインタラクション戦略と活性化ポリシーを適用する**コンテキスト**としての役割を担います。

- **UIイベントの一元的な購読:** `SpriteCommandBus` を流れる全てのUI関連コマンド（`SpriteBeginDragCommand`, `SpriteHoverCommand`など）を購読します。
- **インタラクション戦略の選択と委譲:** ドラッグされたオブジェクトの種類（カード、ダイスなど）に応じて、適切な `IInteractionStrategy` 実装を選択し、具体的なインタラクションロジックの実行を委譲します。
- **UI活性化ポリシーの適用:** `UIActivationPolicy` を利用して、現在のゲーム状態に基づき、UI要素の活性/非活性状態を更新します。
- **Viewへの具体的な指示:** `ViewRegistry` を通じて各Viewの公開メソッドを呼び出し、視覚的な更新を指示します。

---

## 3. 主要な依存コンポーネント

- **`SpriteCommandBus`**: UIイベントを受け取るためのコマンドハブ。
- **`UIStateMachine`**: UIのグローバルな状態（`Idle`, `DraggingCard`など）を参照するためのステートマシン。
- **`CardSlotManager`**: スロットの論理的な状態を参照し、リフロー処理を依頼するためのマネージャー。
- **`ReflowService`**: リフロー計算ロジックを提供するサービス。
- **`ViewRegistry`**: `CompositeObjectId` をキーとして、シーン上に存在する `BaseSpriteView` のインスタンスを検索・取得するためのレジストリクラス。
- **`IInteractionStrategy` (インターフェース)**: インタラクションの具体的な振る舞いを定義する戦略インターフェース。
    - **`CardInteractionStrategy`**: `IInteractionStrategy` の実装の一つで、カードに関するインタラクションロジックをカプセル化します。
    - (将来的に `DiceInteractionStrategy` などが追加される可能性があります。)
- **`UIActivationPolicy`**: UI要素の活性/非活性ルールを管理するクラス。

---

## 4. インタラクションフローの例

### 例1：カードのドラッグ開始時 (`OnBeginDrag`)

1.  `SpriteBeginDragCommand` を受け取ります。
2.  ドラッグされたオブジェクトの `ObjectType` を判断し、対応する `IInteractionStrategy` （例: `CardInteractionStrategy`）をアクティブな戦略として設定します。
3.  アクティブな戦略の `OnBeginDrag` メソッドを呼び出し、具体的なドラッグ開始処理を委譲します。
4.  `UIActivationPolicy` の `UpdateActivations` メソッドを呼び出し、現在の状態に基づいてUI要素の活性/非活性を更新します。

### 例2：スロットへのホバー時 (`OnHover`)

1.  `SpriteHoverCommand` を受け取ります。
2.  アクティブな戦略の `OnHover` メソッドを呼び出し、具体的なホバー処理を委譲します。
    -   戦略内部では、`UIStateMachine` の状態や `CardSlotManager` の情報に基づいて、リフロープレビューの要求などを行います。

### 例3：ドロップ完了時 (`OnDrop`)

1.  `SpriteDropCommand` を受け取ります。
2.  アクティブな戦略の `OnDrop` メソッドを呼び出し、具体的なドロップ処理を委譲します。
    -   戦略内部では、`CardSlotManager` にカード配置を依頼し、ドロップ成功フラグを更新します。
3.  `UIActivationPolicy` の `UpdateActivations` メソッドを呼び出し、UI要素の活性/非活性を更新します。

---

## 5. 各Viewに要求されるインターフェース

`UIInteractionOrchestrator` およびその戦略クラスが正しく動作するために、`BaseSpriteView` およびその派生クラスは、以下のような状態遷移メソッドを `public` で公開する必要があります。

- `EnterNormalState()`
- `EnterHoveringState()`
- `EnterDraggingState()`
- `EnterDraggingInProgressState()`
- `EnterAcceptableState()`
- `EnterInactiveState()`
- `MoveTo(Vector3 position)`
- `MoveToAnimated(Vector3 targetPosition, ICommand commandToEmitOnComplete = null)`
- `SetColliderEnabled(bool enable)`

これにより、各Viewは自身の状態を判断することなく、Orchestratorからの指示に従って振る舞うことができます。

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
- [ui_card_slot_interaction.md](../ui/ui_card_slot_interaction.md)
- [ui_creature_card_interaction.md](../ui/ui_creature_card_interaction.md)
- `Assets/CardsAndDices/Scripts/Systems/IInteractionStrategy.cs`
- `Assets/CardsAndDices/Scripts/Systems/CardInteractionStrategy.cs`
- `Assets/CardsAndDices/Scripts/Systems/UIActivationPolicy.cs`
- `Assets/CardsAndDices/Scripts/Commands/EnableUIInteractionCommand.cs`
- `Assets/CardsAndDices/Scripts/Commands/DisableUIInteractionCommand.cs`

---

## 更新履歴

- 2025-08-01: UI操作制限モードの導入を反映。 (Gemini)
- 2025-08-01: `UIInteractionOrchestrator`のリファクタリング（Strategyパターン、Policyパターンの導入）を反映。 (Gemini)
- 2025-07-26: 初版作成 (Gemini - Technical Writer for Game Development)

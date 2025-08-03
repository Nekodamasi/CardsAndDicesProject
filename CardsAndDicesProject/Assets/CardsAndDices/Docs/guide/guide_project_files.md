# guide_project_files.md - プロジェクトファイル一覧

## Docs

### component

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `component_card_slot_prefab.md` | カードスロットPrefabのコンポーネント仕様。 | `Assets/CardsAndDices/Docs/component/component_card_slot_prefab.md` |
| `component_creature_card_prefab.md` | クリーチャーカードPrefabのコンポーネント仕様。 | `Assets/CardsAndDices/Docs/component/component_creature_card_prefab.md` |
| `component_dice_inlet_prefab.md` | ダイスインレットPrefabのコンポーネント仕様。 | `Assets/CardsAndDices/Docs/component/component_dice_inlet_prefab.md` |

### gdd

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `gdd_combat_system.md` | 戦闘システムのコンセプトや概要。 | `Assets/CardsAndDices/Docs/gdd/gdd_combat_system.md` |
| `gdd_composite_object_id.md` | 複合オブジェクト識別子の基本設計。 | `Assets/CardsAndDices/Docs/gdd/gdd_composite_object_id.md` |
| `gdd_game_object_specs.md` | ゲームオブジェクトの仕様定義。 | `Assets/CardsAndDices/Docs/gdd/gdd_game_object_specs.md` |
| `gdd_main.md` | ゲームの要件定義と概要。 | `Assets/CardsAndDices/Docs/gdd/gdd_main.md` |
| `gdd_reflow_system.md` | カードリフローシステムの設計。 | `Assets/CardsAndDices/Docs/gdd/gdd_reflow_system.md` |
| `gdd_sprite_ui_design.md` | SpriteUIの基本設計。 | `Assets/CardsAndDices/Docs/gdd/gdd_sprite_ui_design.md` |
| `gdd_unity_specs.md` | Unityエンジン技術要件設計書。 | `Assets/CardsAndDices/Docs/gdd/gdd_unity_specs.md` |

### guide

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `guide_design-principles.md` | プロジェクトの核心的な設計原則。 | `Assets/CardsAndDices/Docs/guide/guide_design-principles.md` |
| `guide_developer-cookbook.md` | 開発者が特定のタスクを実行するための実践的な手順集。 | `Assets/CardsAndDices/Docs/guide/guide_developer-cookbook.md` |
| `guide_file_management.md` | ファイル一覧の管理方法を定義するガイド。 | `Assets/CardsAndDices/Docs/guide/guide_file_management.md` |
| `guide_files.md` | ドキュメントファイル命名・管理ルール。 | `Assets/CardsAndDices/Docs/guide/guide_files.md` |
| `guide_overview.md` | プロジェクト全体の概要と開発の手引き。 | `Assets/CardsAndDices/Docs/guide/guide_overview.md` |
| `guide_project_files.md` | プロジェクトファイル一覧。 | `Assets/CardsAndDices/Docs/guide/guide_project_files.md` |
| `guide_rules.md` | ドキュメント作成・記述ルール。 | `Assets/CardsAndDices/Docs/guide/guide_rules.md` |
| `guide_sys_classes_creation.md` | `sys_classes.md` の作成手順ガイド。 | `Assets/CardsAndDices/Docs/guide/guide_sys_classes_creation.md` |
| `guide_ui_interaction_design.md` | UIインタラクションの設計ガイド。 | `Assets/CardsAndDices/Docs/guide/guide_ui_interaction_design.md` |
| `guide_unity-cs.md` | Unity C# コード出力ガイド。 | `Assets/CardsAndDices/Docs/guide/guide_unity-cs.md` |

### sys

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `sys_card_slot_manager.md` | CardSlotManagerのシステム設計書。 | `Assets/CardsAndDices/Docs/sys/sys_card_slot_manager.md` |
| `sys_card-reflow.md` | カードリフローシステム設計書。 | `Assets/CardsAndDices/Docs/sys/sys_card-reflow.md` |
| `sys_classes.md` | 戦闘システムモックアップのクラス一覧。 | `Assets/CardsAndDices/Docs/sys/sys_classes.md` |
| `sys_domain-model.md` | ドメインモデル設計書。 | `Assets/CardsAndDices/Docs/sys/sys_domain-model.md` |
| `sys_ui_interaction_orchestrator.md` | UIInteractionOrchestratorのシステム設計書。 | `Assets/CardsAndDices/Docs/sys/sys_ui_interaction_orchestrator.md` |

### ui

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `ui_card_slot_interaction.md` | カードスロットのUIインタラクション仕様。 | `Assets/CardsAndDices/Docs/ui/ui_card_slot_interaction.md` |
| `ui_creature_card_interaction.md` | クリーチャーカードのUIインタラクション仕様。 | `Assets/CardsAndDices/Docs/ui/ui_creature_card_interaction.md` |

---

## Scripts

### Commands

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `CardPlacedInSlotCommand.cs` | カードがスロットに配置されたことを通知するコマンド。 | `Assets/CardsAndDices/Scripts/Commands/CardPlacedInSlotCommand.cs` |
| `DragReflowCompletedCommand.cs` | ドラッグ操作に起因するリフロー完了を通知するコマンド。 | `Assets/CardsAndDices/Scripts/Commands/DragReflowCompletedCommand.cs` |
| `ExecuteFrontLoadCommand.cs` | 前詰め処理の実行を通知するコマンド。 | `Assets/CardsAndDices/Scripts/Commands/ExecuteFrontLoadCommand.cs` |
| `EnableUIInteractionCommand.cs` | UI操作制限モードを解除し、UI操作を有効にするコマンド。 | `Assets/CardsAndDices/Scripts/Commands/EnableUIInteractionCommand.cs` |
| `DisableUIInteractionCommand.cs` | UI操作制限モードを有効にし、UI操作を無効にするコマンド。 | `Assets/CardsAndDices/Scripts/Commands/DisableUIInteractionCommand.cs` |
| `ICommand.cs` | コマンドパターンの基本インターフェース。 | `Assets/CardsAndDices/Scripts/Commands/ICommand.cs` |
| `ReflowCompletedCommand.cs` | カードのリフロー完了通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/ReflowCompletedCommand.cs` |
| `ReflowOperationCompletedCommand.cs` | リフロー操作の完了を通知するコマンド。 | `Assets/CardsAndDices/Scripts/Commands/ReflowOperationCompletedCommand.cs` |
| `SpriteBeginDragCommand.cs` | ドラッグ開始イベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteBeginDragCommand.cs` |
| `SpriteClickCommand.cs` | クリックイベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteClickCommand.cs` |
| `SpriteDragCommand.cs` | ドラッグ中イベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteDragCommand.cs` |
| `SpriteDragOperationCompletedCommand.cs` | ドラッグ操作完了通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteDragOperationCompletedCommand.cs` |
| `SpriteDropCommand.cs` | ドロップイベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteDropCommand.cs` |
| `SpriteEndDragCommand.cs` | ドラッグ終了イベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteEndDragCommand.cs` |
| `SpriteHoverCommand.cs` | ホバーイベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteHoverCommand.cs` |
| `SpriteUnhoverCommand.cs` | アンホバーイベント通知コマンド。 | `Assets/CardsAndDices/Scripts/Commands/SpriteUnhoverCommand.cs` |
| `PlayerZoneStateChangedCommand.cs` | PlayerZoneのスロットが満員かどうかの状態変化を通知するコマンド。 | `Assets/CardsAndDices/Scripts/Commands/PlayerZoneStateChangedCommand.cs` |
| `SystemReflowCommand.cs` | システム起因のカードリフローを指示するコマンド。 | `Assets/CardsAndDices/Scripts/Commands/SystemReflowCommand.cs` |

### Data

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `CardSlotData.cs` | カードスロットの状態を保持するデータクラス（Model）。 | `Assets/CardsAndDices/Scripts/Data/CardSlotData.cs` |

### Installers

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `GameLifetimeScope.cs` | ゲームのライフタイムスコープを管理するインストーラー。 | `Assets/CardsAndDices/Scripts/Installers/GameLifetimeScope.cs` |

### Shared

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `BaseAnimationSO.cs` | アニメーション戦略の基底ScriptableObject。 | `Assets/CardsAndDices/Scripts/Shared/BaseAnimationSO.cs` |
| `CompositeObjectId.cs` | ゲームオブジェクトの複合識別子。 | `Assets/CardsAndDices/Scripts/Shared/CompositeObjectId.cs` |
| `CompositeObjectIdManager.cs` | 複合オブジェクト識別子の生成・管理。 | `Assets/CardsAndDices/Scripts/Shared/CompositeObjectIdManager.cs` |
| `DragAnimationSO.cs` | 汎用ドラッグアニメーション定義。 | `Assets/CardsAndDices/Scripts/Shared/DragAnimationSO.cs` |
| `DropWaitingAnimationSO.cs` | ドロップ待機アニメーションの定義。 | `Assets/CardsAndDices/Scripts/Shared/DropWaitingAnimationSO.cs` |
| `HoverAnimationSO.cs` | 汎用ホバーアニメーション定義。 | `Assets/CardsAndDices/Scripts/Shared/HoverAnimationSO.cs` |
| `IAnimationStrategy.cs` | UIアニメーション戦略のインターフェース。 | `Assets/CardsAndDices/Scripts/Shared/IAnimationStrategy.cs` |
| `IdentifiableGameObject.cs` | 複合オブジェクト識別子を持つGameObject。 | `Assets/CardsAndDices/Scripts/Shared/IdentifiableGameObject.cs` |
| `LinePosition.cs` | スロットが存在する大まかなライン（エリア）を定義。 | `Assets/CardsAndDices/Scripts/Shared/LinePosition.cs` |
| `Team.cs` | カードスロットやカードが所属するチームを定義。 | `Assets/CardsAndDices/Scripts/Shared/Team.cs` |
| `MultiRendererVisualController.cs` | 複数レンダラーの視覚プロパティ制御。 | `Assets/CardsAndDices/Scripts/Shared/MultiRendererVisualController.cs` |
| `NormalAnimationSO.cs` | 汎用ノーマルアニメーション定義。 | `Assets/CardsAndDices/Scripts/Shared/NormalAnimationSO.cs` |
| `ReturnToPositionAnimationSO.cs` | 元の位置に戻るアニメーションの定義。 | `Assets/CardsAndDices/Scripts/Shared/ReturnToPositionAnimationSO.cs` |
| `SlotLocation.cs` | ライン内でのスロットの具体的な役割や位置を定義。 | `Assets/CardsAndDices/Scripts/Shared/SlotLocation.cs` |
| `SpriteLayerController.cs` | 複数の描画順序を一括で制御するコンポーネント。 | `Assets/CardsAndDices/Scripts/Shared/SpriteLayerController.cs` |
| `InteractionProfile.cs` | ゲームオブジェクトのインタラクションの振る舞いを定義するScriptableObject。 | `Assets/CardsAndDices/Scripts/Shared/InteractionProfile.cs` |

### Systems

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `CardSlotManager.cs` | 全てのカードスロット関連の処理の窓口となるファサードクラス。 | `Assets/CardsAndDices/Scripts/Systems/CardSlotManager.cs` |
| `CardSlotStateRepository.cs` | 全てのカードスロットの状態を管理するリポジトリクラス。 | `Assets/CardsAndDices/Scripts/Systems/CardSlotStateRepository.cs` |
| `ReflowService.cs` | リフローの計算ロジックを担当するサービスクラス。 | `Assets/CardsAndDices/Scripts/Systems/ReflowService.cs` |
| `CardPlacementService.cs` | カードの配置ロジックを担当するサービスクラス。 | `Assets/CardsAndDices/Scripts/Systems/CardPlacementService.cs` |
| `CardSlotInteractionHandler.cs` | UIからのスロット関連のインタラクションを処理するクラス。 | `Assets/CardsAndDices/Scripts/Systems/CardSlotInteractionHandler.cs` |
| `CardSlotDebug.cs` | スロット関連のデバッグ機能を提供するクラス。 | `Assets/CardsAndDices/Scripts/Systems/CardSlotDebug.cs` |
| `IInteractionStrategy.cs` | UIインタラクション戦略のインターフェース。 | `Assets/CardsAndDices/Scripts/Systems/IInteractionStrategy.cs` |
| `CardInteractionStrategy.cs` | カードのUIインタラクション戦略を実装するクラス。 | `Assets/CardsAndDices/Scripts/Systems/CardInteractionStrategy.cs` |
| `UIActivationPolicy.cs` | UI要素の活性/非活性ルールを管理するクラス。 | `Assets/CardsAndDices/Scripts/Systems/UIActivationPolicy.cs` |
| `UIStateMachine.cs` | UIのインタラクション状態管理。 | `Assets/CardsAndDices/Scripts/Systems/UIStateMachine.cs` |
| `ViewRegistry.cs` | すべてのViewを登録・管理するクラス。 | `Assets/CardsAndDices/Scripts/Systems/ViewRegistry.cs` |
| `SystemReflowCommandHandler.cs` | `SystemReflowCommand`を購読し、カードのリフローアニメーションを実行するハンドラー。 | `Assets/CardsAndDices/Scripts/Systems/SystemReflowCommandHandler.cs` |

### Tester

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `DebugViewer.cs` | デバッグ情報を表示するUI。 | `Assets/CardsAndDices/Scripts/Tester/DebugViewer.cs` |
| `PlacementCardTester.cs` | カード配置のテストを行うクラス。 | `Assets/CardsAndDices/Scripts/Tester/PlacementCardTester.cs` |

### UI

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| `SpriteInputHandler.cs` | SpriteUIのマウスイベントを検知し、対応するコマンドを発行するハンドラー。 | `Assets/CardsAndDices/Scripts/UI/SpriteInputHandler.cs` |
| `CreatureCardController.cs` | クリーチャーカードのロジックを管理するコントローラー。 | `Assets/CardsAndDices/Scripts/UI/CreatureCardController.cs` |
| `CreatureCardView.cs` | クリーチャーカードの視覚的な表示を管理するコンポーネント。 | `Assets/CardsAndDices/Scripts/UI/CreatureCardView.cs` |

---

## 関連ファイル

- [guide_file_management.md](./guide_file_management.md)

---

## 更新履歴

- 2025-07-30: プロジェクトルールへの準拠 (Gemini - Technical Writer for Game Development)
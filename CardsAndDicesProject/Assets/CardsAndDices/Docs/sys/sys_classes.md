# sys_classes.md - クラス一覧とリンク

---

## クラス一覧

### 1. Commands

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| `CardPlacedInSlotCommand` | `ICommand` | | カードがスロットに配置されたことを通知するコマンド。 |
| `DragReflowCompletedCommand` | `ICommand` | | ドラッグ操作に起因するリフロー完了を通知するコマンド。 |
| `ExecuteFrontLoadCommand` | `ICommand` | | 前詰め処理の実行を通知するコマンド。 |
| `ICommand` | なし | | コマンドパターンの基本インターフェース。 |
| `ReflowCompletedCommand` | `ICommand` | | カードのリフローが完了し、カードが新しい位置へ移動する必要があることを通知するコマンド。 |
| `SpriteBeginDragCommand` | `ICommand` | | SpriteUI要素のドラッグ操作が開始された時のコマンド。 |
| `SpriteClickCommand` | `ICommand` | | SpriteUI要素がクリックされたことを通知するコマンド。 |
| `SpriteDragCommand` | `ICommand` | | SpriteUI要素がドラッグ中に移動したことを通知するコマンド。 |
| `SpriteDragOperationCompletedCommand` | `ICommand` | | ドラッグ操作が完全に終了し、その結果（成功/失敗）に関わらず、コライダーを有効に戻すなどの後処理を行うためのコマンド。 |
| `SpriteDropCommand` | `ICommand` | | SpriteUI要素がスロットに正常に配置されたことを通知するコマンド。または、ドラッグ操作が終了し、ドロップされたことを通知するコマンド。 |
| `SpriteEndDragCommand` | `ICommand` | | ドラッグ終了イベント通知コマンド。 |
| `SpriteHoverCommand` | `ICommand` | | マウスカーソルがSpriteUI要素上に入った時のコマンド。 |
| `SpriteUnhoverCommand` | `ICommand` | | マウスカーソルがSpriteUI要素から離れた時のコマンド。 |
| `PlayerZoneStateChangedCommand` | `ICommand` | | PlayerZoneのスロットが満員かどうかの状態変化を通知するコマンド。 |
| `EnableUIInteractionCommand` | `ICommand` | | UI操作制限モードを解除し、UI操作を有効にするコマンド。 |
| `DisableUIInteractionCommand` | `ICommand` | | UI操作制限モードを有効にし、UI操作を無効にするコマンド。 |

### 2. Data

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| `CardSlotData` | なし | | カードスロットの状態を保持するデータクラス（Model）。 |

### 3. Shared

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| `BaseAnimationSO` | `ScriptableObject`, `IAnimationStrategy` | | アニメーション戦略の基底ScriptableObject。 |
| `CompositeObjectId` | `IEquatable<CompositeObjectId>` | [gdd_composite_object_id.md](../gdd/gdd_composite_object_id.md) | ゲームオブジェクトを一意に識別するための複合IDクラス。 |
| `CompositeObjectIdManager` | `ScriptableObject` | [gdd_composite_object_id.md](../gdd/gdd_composite_object_id.md) | 複合オブジェクト識別子の生成・管理。 |
| `DragAnimationSO` | `BaseAnimationSO` | | 汎用ドラッグアニメーション定義。 |
| `DropWaitingAnimationSO` | `BaseAnimationSO` | | ドロップ待機中のアニメーション定義。 |
| `HoverAnimationSO` | `BaseAnimationSO` | | 汎用ホバーアニメーション定義。 |
| `IAnimationStrategy` | なし | | UIアニメーションの戦略を定義するインターフェース。 |
| `IdentifiableGameObject` | `MonoBehaviour` | [gdd_composite_object_id.md](../gdd/gdd_composite_object_id.md) | 複合オブジェクト識別子を持つGameObject。 |
| `LinePosition` | `Enum` | | スロットが存在する大まかなライン（エリア）を定義します。 |
| `Team` | `Enum` | | カードスロットやカードが所属するチームを定義します。 |
| `MultiRendererVisualController` | `MonoBehaviour` | | 複数レンダラーの視覚プロパティ制御。 |
| `NormalAnimationSO` | `BaseAnimationSO` | | 汎用ノーマルアニメーション定義。 |
| `SlotLocation` | `Enum` | | ライン内でのスロットの具体的な役割や位置を定義します。 |
| `SpriteLayerController` | `MonoBehaviour` | | 複数のSortingGroup、Canvas、および子階層のSpriteLayerControllerの描画順序（sortingOrder）を一括で制御するコンポーネントです。 |

### 4. Systems

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| `CardSlotManager` | `ScriptableObject` | [sys_card_slot_manager.md](./sys_card_slot_manager.md) | カードスロット関連システムのファサードクラス。 |
| `CardSlotStateRepository` | `ScriptableObject` | [sys_card_slot_manager.md](./sys_card_slot_manager.md) | 全てのスロットの状態を管理するリポジトリ。 |
| `CardPlacementService` | `ScriptableObject` | [sys_card_slot_manager.md](./sys_card_slot_manager.md) | カードの配置・削除ロジックを担当するサービス。 |
| `ReflowService` | `ScriptableObject` | [sys_card_slot_manager.md](./sys_card_slot_manager.md) | リフローの計算ロジックを担当するサービス。 |
| `CardSlotInteractionHandler` | `ScriptableObject` | [sys_card_slot_manager.md](./sys_card_slot_manager.md) | UIイベントを処理し、各サービスを連携させるハンドラー。 |
| `CardSlotDebug` | `ScriptableObject` | [sys_card_slot_manager.md](./sys_card_slot_manager.md) | デバッグ機能を提供するサービス。 |
| `IInteractionStrategy` | なし | [sys_ui_interaction_orchestrator.md](./sys_ui_interaction_orchestrator.md) | UIインタラクション戦略のインターフェース。 |
| `CardInteractionStrategy` | `ScriptableObject`, `IInteractionStrategy` | [sys_ui_interaction_orchestrator.md](./sys_ui_interaction_orchestrator.md) | カードのUIインタラクション戦略を実装するクラス。 |
| `UIActivationPolicy` | `ScriptableObject` | [sys_ui_interaction_orchestrator.md](./sys_ui_interaction_orchestrator.md) | UI要素の活性/非活性ルールを管理するクラス。 |
| `UIStateMachine` | `ScriptableObject` | | UIの全体的なインタラクション状態を管理するステートマシン。 |

### 5. Tester

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| `PlacementCardTester` | `MonoBehaviour` | | ゲームの初期状態をセットアップし、テスト用のカード配置を行うデバッグ用クラス。 |

### 6. UI

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| `BaseSpriteView` | `MonoBehaviour` | | SpriteUI要素の基底クラス。 |
| `CardSlotView` | `BaseSpriteView` | [ui_card_slot_interaction.md](../ui/ui_card_slot_interaction.md) | カードスロットの視覚的な振る舞いを管理するクラス（View）。 |
| `CreatureCardController` | `MonoBehaviour` | | クリーチャーカードのロジック（状態）を管理するコントローラー。 |
| `CreatureCardView` | `BaseSpriteView` | [ui_creature_card_interaction.md](../ui/ui_creature_card_interaction.md) | クリーチャーカードの視覚表示管理。 |
| `SpriteCommandBus` | `ScriptableObject` | | SpriteUIコマンドの中央ハブ。 |
| `SpriteInputHandler` | `MonoBehaviour`, `IPointerEnterHandler`, `IPointerExitHandler`, `IPointerDownHandler`, `IPointerUpHandler`, `IBeginDragHandler`, `IDragHandler`, `IEndDragHandler`, `IDropHandler` | | SpriteUIのマウスイベントを検知し、対応するコマンドを発行するハンドラー。 |

---

## 関連ファイル

- [guide_project_files.md](../guide/guide_project_files.md)
- [guide_sys_classes_creation.md](../guide/guide_sys_classes_creation.md)
- [sys_card_slot_manager.md](./sys_card_slot_manager.md)
- [ui_card_slot_interaction.md](../ui/ui_card_slot_interaction.md)
- [ui_creature_card_interaction.md](../ui/ui_creature_card_interaction.md)
- [gdd_composite_object_id.md](../gdd/gdd_composite_object_id.md)

---

## 更新履歴

- 2025-07-25: クラス一覧を格納フォルダごとに整理し、関連ドキュメントを修正 (Gemini - Technical Writer for Game Development)

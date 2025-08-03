# sys_classes.md - クラス一覧とリンク

---

## クラス一覧

### 1. Commands

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| CardPlacedInSlotCommand | ICommand | | カードがスロットに配置されたことを通知するコマンド。 |
| DisableUIInteractionCommand | ICommand | | UI操作制限モードを有効にし、UI操作を無効にするコマンド。 |
| DragReflowCompletedCommand | ICommand | | ドラッグ操作に起因するカードのリフローが完了し、カードが新しい位置へ移動する必要があることを通知するコマンド。 |
| EnableUIInteractionCommand | ICommand | | UI操作制限モードを解除し、UI操作を有効にするコマンド。 |
| ExecuteFrontLoadCommand | ICommand | | 前詰め処理の実行を通知するコマンド。 |
| ICommand | なし | | イベント駆動アーキテクチャにおけるイベントメッセージの基本契約を定義するインターフェース。 |
| PlayerZoneStateChangedCommand | ICommand | | PlayerZoneのスロットが満員かどうかの状態変化を通知するコマンド。 |
| ReflowCompletedCommand | ICommand | | カードのリフローが完了し、カードが新しい位置へ移動する必要があることを通知するコマンド。 |
| ReflowOperationCompletedCommand | ICommand | | リフロー操作が完全に終了したことを通知するコマンド。 |
| SpriteBeginDragCommand | ICommand | | SpriteUI要素のドラッグ操作が開始された時のコマンド。 |
| SpriteClickCommand | ICommand | | SpriteUI要素がクリックされたことを通知するコマンド。 |
| SpriteDragCommand | ICommand | | SpriteUI要素がドラッグ中に移動したことを通知するコマンド。 |
| SpriteDragOperationCompletedCommand | ICommand | | ドラッグ操作が完全に終了し、その結果（成功/失敗）に関わらず、コライダーを有効に戻すなどの後処理を行うためのコマンド。 |
| SpriteDropCommand | ICommand | | SpriteUI要素がスロットに正常に配置されたことを通知するコマンド。 |
| SpriteEndDragCommand | ICommand | | SpriteUI要素のドラッグ操作が終了したことを通知するコマンド。 |
| SpriteHoverCommand | ICommand | | マウスカーソルがSpriteUI要素上に入った時のコマンド。 |
| SpriteUnhoverCommand | ICommand | | マウスカーソルがSpriteUI要素から離れた時のコマンド。 |

### 2. Data

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| CardSlotData | なし | | カードスロットの状態を保持するデータクラス（Model）。 |

### 3. Installers

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| GameLifetimeScope | LifetimeScope | | DIコンテナのセットアップと、ゲーム全体の依存関係の解決を行うクラス。 |

### 4. Shared

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| BaseAnimationSO | ScriptableObject, IAnimationStrategy | | アニメーション戦略の基底ScriptableObject。 |
| CompositeObjectId | IEquatable<CompositeObjectId> | | ゲームオブジェクトを一意に識別するための複合IDクラス。 |
| CompositeObjectIdManager | ScriptableObject | | CompositeObjectIdを生成・管理するマネージャークラス。 |
| DragAnimationSO | BaseAnimationSO | | ドラッグ中のアニメーションを定義するScriptableObject。 |
| DropWaitingAnimationSO | BaseAnimationSO | | ドロップを待っている状態のアニメーションを定義するScriptableObject。 |
| HoverAnimationSO | BaseAnimationSO | | ホバー時のアニメーションを定義するScriptableObject。 |
| IAnimationStrategy | なし | | UIアニメーションの戦略を定義するインターフェース。 |
| IdentifiableGameObject | MonoBehaviour | | CompositeObjectIdを持つMonoBehaviourクラス。 |
| InteractionProfile | ScriptableObject | | ゲームオブジェクトのインタラクションの振る舞いを定義するScriptableObject。 |
| LinePosition | Enum | | スロットが存在する大まかなライン（エリア）を定義します。 |
| MultiRendererVisualController | MonoBehaviour | | 複数のSpriteRendererとTextMeshProUGUIの視覚的プロパティを一括で制御するコンポーネント。 |
| NormalAnimationSO | BaseAnimationSO | | 通常状態のアニメーションを定義するScriptableObject。 |
| ReturnToPositionAnimationSO | BaseAnimationSO | | オブジェクトを所定のターゲット位置に戻すアニメーション戦略。 |
| SlotLocation | Enum | | ライン内でのスロットの具体的な役割や位置を定義します。 |
| SpriteLayerController | MonoBehaviour | | 複数のSortingGroup、Canvas、および子階層のSpriteLayerControllerの描画順序を一括で制御するコンポーネント。 |
| Team | Enum | | カードスロットやカードが所属するチームを定義します。 |

### 5. Systems

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| CardInteractionStrategy | ScriptableObject, IInteractionStrategy | | カードのUIインタラクション戦略を実装するクラス。 |
| CardPlacementService | ScriptableObject | | カードの配置ロジックを担当するサービスクラス。 |
| CardSlotDebug | ScriptableObject | | スロット関連のデバッグ機能を提供するクラス。 |
| CardSlotInteractionHandler | ScriptableObject | | UIからのスロット関連のインタラクションを処理するクラス。 |
| CardSlotManager | ScriptableObject | | 全てのカードスロット関連の処理の窓口となるファサードクラス。 |
| CardSlotStateRepository | ScriptableObject | | 全てのカードスロットの状態を管理するリポジトリクラス。 |
| IInteractionStrategy | なし | | UIインタラクションの戦略を定義するインターフェース。 |
| ReflowService | ScriptableObject | | カードのリフロー（再配置）および前詰め処理の計算ロジックを担当するサービスクラス。 |
| UIActivationPolicy | ScriptableObject | | UI要素のアクティブ/非アクティブ状態を管理するクラス。 |
| UIInteractionOrchestrator | ScriptableObject | | UIインタラクション全体を統括し、適切な戦略に処理を委譲するクラス。 |
| UIStateMachine | ScriptableObject | | UIの全体的なインタラクション状態を管理するステートマシン。 |
| ViewRegistry | なし | | シーン上の全てのBaseSpriteViewインスタンスを管理し、IDによる検索機能を提供するレジストリ。 |

### 6. Tester

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| DebugViewer | MonoBehaviour | | 実行中の各種データをインスペクターに表示するためのデバッグ用クラス。 |
| PlacementCardTester | MonoBehaviour | | ゲームの初期状態をセットアップし、テスト用のカード配置を行うデバッグ用クラス。 |

### 7. UI

| クラス名 | 継承元 | 関連ドキュメント | 解説 |
| :--- | :--- | :--- | :--- |
| BaseSpriteView | MonoBehaviour | | 全てのSpriteベースのViewの基底クラス。 |
| CardSlotView | BaseSpriteView | | カードスロットの視覚的な表示を管理するコンポーネント。 |
| CreatureCardView | BaseSpriteView | | クリーチャーカードの視覚的な表示を管理するコンポーネント。 |
| SpriteCommandBus | ScriptableObject | | SpriteUIに関連するイベントの登録、配信、解除を一元管理する中央ハブ。 |
| SpriteInputHandler | MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler | | SpriteUIのマウスイベントを検知し、対応するコマンドを発行するハンドラー。 |

---

## 関連ファイル

- [guide_sys_classes_creation.md](./guide_sys_classes_creation.md): このドキュメントの作成手順ガイド
---

## 更新履歴

- 2025-07-25: クラス一覧を格納フォルダごとに整理し、関連ドキュメントを修正 (Gemini - Technical Writer for Game Development)
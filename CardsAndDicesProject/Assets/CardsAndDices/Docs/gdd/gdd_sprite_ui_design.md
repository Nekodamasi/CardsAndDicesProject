# gdd_sprite_ui_design.md - SpriteUI基本設計

---

## 概要

この設計書は、UnityにおけるSpriteベースのUI要素（SpriteUI）の基本的なインタラクションと構造を定義します。
マウスイベントの検知、コマンドの発行、およびそれに応じたUIの視覚的変化を管理するための主要なクラスとその役割を記述します。
また、オブジェクトプールによるUI要素の再利用と、その表示状態の管理についても詳述します。

---

## 主要クラスの定義

### 1. SpriteInputHandler

- **役割:** Unityの`IPointerEnterHandler`などのインターフェースを実装し、マウスイベントを検知して対応するコマンドを発行します。また、`InteractionProfile` ScriptableObjectを介して、ホバー、クリック、ドラッグといった各インタラクションの有効/無効を動的に制御します。
- **検知するマウスイベントと処理:**
    - `OnPointerEnter`:
        - `InteractionProfile`で許可されており、かつドラッグ中でなければ `SpriteHoverCommand` を発行します。
    - `OnPointerExit`:
        - `InteractionProfile`で許可されており、かつホバー中であれば `SpriteUnhoverCommand` を発行します。
    - `OnBeginDrag`:
        - `InteractionProfile`で許可されていれば `SpriteBeginDragCommand` を発行します。
    - `OnPointerUp`:
        - `InteractionProfile`で許可されており、かつドラッグ中でなければ `SpriteClickCommand` を発行します。
    - `OnEndDrag`:
        - ドラッグ中であれば `SpriteEndDragCommand` を発行します。
    - `OnDrag`:
        - `InteractionProfile`で許可されており、かつドラッグ中であれば `SpriteDragCommand` を発行します。
    - `OnDrop`:
        - `InteractionProfile`でドロップターゲットとして許可されていれば `SpriteDropCommand` を発行します。

### 2. SpriteCommandBus

- **役割:** SpriteUIに関連するイベントの登録（`on`）、配信（`emit`）、解除（`off`）を一元管理する中央ハブです。
    - 各UI要素は、このバスを通じてイベントを購読・発行します。

### 3. BaseSpriteView

- **役割:** すべてのSpriteUI要素の基底クラスとなり、共通の表示ロジック、コマンド購読機能、およびオブジェクトプールにおける再利用状態管理機能を提供します。
    - `Awake()`時に `UIInteractionOrchestrator` の `ViewRegistry` に自身を登録し、`OnDestroy()`で登録解除します。
    - `EnableUIInteractionCommand` と `DisableUIInteractionCommand` を購読し、自身のコライダーの有効/無効を切り替えます。
    - `UIInteractionOrchestrator` から呼び出される状態遷移メソッド（`EnterNormalState`, `EnterHoveringState` など）を持ち、自身の状態を管理します。
    - アニメーションの定義は `ScriptableObject` として外部化され、このクラスに注入されます。
    - **`IsSpawned` プロパティ:** このViewが現在オブジェクトプールから取り出され、ゲーム内で使用中であるかを示します。
    - **`SetSpawnedState(bool state)` メソッド:** `IsSpawned` プロパティの状態を設定します。オブジェクトがプールから取り出される際に`true`に、プールに戻される際に`false`に設定されます。
    - **`_displayRootGameObject` フィールド:** 表示に関わるコンポーネントをすべて配下に持つGameObjectへの参照です。このGameObjectの`SetActive`を切り替えることで、View全体の表示/非表示を制御します。
    - **`SetDisplayActive(bool active)` メソッド:** `_displayRootGameObject` のアクティブ状態を設定し、Viewの表示/非表示を切り替えます。`Awake()`時に`SetDisplayActive(false)`が呼ばれ、初期状態では非表示になります。

---

## アニメーション機能の分離

DOTweenを用いたアニメーションの定義と実行ロジックを、`ScriptableObject`ベースの戦略パターンとして分離します。これにより、アニメーションの再利用性、柔軟性、およびデザイナーによる調整の容易性を高めます。

### 1. IAnimationStrategy

- **役割:** すべてのアニメーション戦略が実装すべきインターフェースです。`PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration, Vector3 targetPosition)`メソッドを定義します。`targetPosition`はアニメーションの目標位置を指定します。

### 2. BaseAnimationSO

- **役割:** `IAnimationStrategy`を実装する`ScriptableObject`の抽象基底クラスです。`PlayAnimation`メソッドは、`GameObject`、`MultiRendererVisualController`、`originalScale`、`originalColor`, `duration`, `targetPosition`を引数として受け取ります。共通の`_animationDuration`フィールドやヘルパーメソッド（例: `GetBrightenedColor`）も提供します。

### 3. MultiRendererVisualController

- **役割:** 
    - 複数の`SpriteRenderer`と`TextMeshProUGUI`の視覚的プロパティ（透明度、色）を一括で制御するコンポーネントです。複雑なUI要素全体のフェードイン/アウトや色変更に利用します。インスペクターで対象となるレンダラーや子階層の`MultiRendererVisualController`を割り当てることができ、階層的なフェードや色変更に対応します。
- **メソッド:**
    - `FadeToAlpha(float alpha, float duration)`: 透明度をTweenします。
    - `SetAlpha(float alpha)`: 現在の透明度を設定します。
    - `ColorTo(Color targetColor, float duration)`: 色をTweenします。
    - `SetColor(Color targetColor)`: 現在の色を設定します。

### 4. 具体的なアニメーションScriptableObject

`BaseAnimationSO`を継承し、特定のアニメーションロジックをカプセル化します。これらはUnityエディタでアセットとして作成され、`BaseSpriteView`やその派生クラスにインスペクターから割り当てられます。

- **HoverAnimationSO / CardHoverAnimationSO:** ホバー時の拡大、明るさ変更、カード固有の回転・浮上アニメーション。
- **NormalAnimationSO / CardNormalAnimationSO:** 通常状態への復帰アニメーション。
- **DragAnimationSO / CardDragAnimationSO:** ドラッグ開始時の見た目変更アニメーション。

---

## 描画順序の一元管理

SpriteUIは、複数の`SortingGroup`、`Canvas`で構成されることがあります。これらの描画順序（`Order in Layer`）を一元的に管理するため、以下のクラスを導入します。

### 1. SpriteLayerController

- **役割:** 複数の`SortingGroup`、`Canvas`、および子階層の`SpriteLayerController`の描画順序（`sortingOrder`）を一括で制御するコンポーネントです。
- **インスペクター設定:**
    - `SortingGroup[] _sortingGroups`: 制御対象の`SortingGroup`の配列。
    - `Canvas[] _canvases`: 制御対象の`Canvas`の配列。
    - `SpriteLayerController[] _childControllers`: 制御対象の子`SpriteLayerController`の配列。
- **メソッド:**
    - `SetOrderInLayer(int order)`: 登録されたすべての要素の`sortingOrder`を指定された値に設定します。子の`SpriteLayerController`にも再帰的に適用されます。

### 2. BaseSpriteViewへの統合

- `BaseSpriteView`は、`SpriteLayerController`のインスタンスをインスペクター経由で受け取ります。
- `BaseSpriteView`に`public void SetOrderInLayer(int order)`メソッドを実装し、内部で`SpriteLayerController.SetOrderInLayer(order)`を呼び出します。
これにより、`BaseSpriteView`を持つオブジェクトから、関連するすべてのUI要素の描画順序を簡単に変更できます。

---

## コマンドクラスの定義

以下のコマンドクラスは、`SpriteInputHandler`によって発行され、`SpriteCommandBus`を通じて配信されます。

### 1. SpriteHoverCommand

- **役割:**
    - マウスカーソルがSpriteUI要素上に入ったことを通知します。
- **項目:**
    - `TargetObjectId`: ホバーされたオブジェクトの`CompositeObjectId`。

### 2. SpriteUnhoverCommand

- **役割:** 
    - マウスカーソルがSpriteUI要素上から離れたことを通知します。
- **項目:**
    - `TargetObjectId`: アンホバーされたオブジェクトの`CompositeObjectId`。

### 3. SpriteBeginDragCommand

- **役割:**
    - SpriteUI要素のドラッグ操作が開始されたことを通知します。
- **項目:**
    - `TargetObjectId`: ドラッグ開始されたオブジェクトの`CompositeObjectId`。

### 4. SpriteDragCommand

- **役割:**
    - SpriteUI要素がドラッグ中に移動したことを通知します。
- **項目:**
    - `TargetObjectId`: ドラッグ中のオブジェクトの`CompositeObjectId`。
    - `NewPosition`: ドラッグ中の新しいワールド座標。

### 5. SpriteClickCommand

- **役割:**
    - SpriteUI要素がクリックされたことを通知します。
- **項目:**
    - `TargetObjectId`: クリックされたオブジェクトの`CompositeObjectId`。

### 6. SpriteEndDragCommand

- **役割:**
    - SpriteUI要素のドラッグ操作が終了したことを通知します。
- **項目:**
    - `TargetObjectId`: ドラッグ終了したオブジェクトの`CompositeObjectId`。

### 7. SpriteDropCommand

- **役割:**
    - SpriteUI要素のドラッグ操作が終了し、ドロップされたことを通知します。
- **項目:**
    - `DroppedObjectId`: ドロップされたSpriteUI要素の`CompositeObjectId`。
    - `TargetSlotObjectId`: 要素を受け入れたスロットの`CompositeObjectId`。ドロップターゲットがない場合は`null`。

### 8. SpriteDragOperationCompletedCommand

- **役割:**
    - ドラッグ操作が完全に終了したことを通知するグローバルなイベント。このコマンドは、ドラッグの成功・失敗に関わらず、関連する全てのUI要素に最終的な後処理（コライダーの有効化、状態の正常化など）を行わせるためのトリガーとなる。
- **主な処理:**
    - **コライダーの有効化:** `BaseSpriteView` は、無効化されていたコライダーを再度有効にする。
    - **状態の正常化:** 各Viewは、自身の状態を `Normal` に戻す。
    - **UI操作の有効化:** `UIInteractionOrchestrator` は `EnableUIInteractionCommand` を発行する。
- **項目:**
    - なし

---

## ドラッグ＆ドロップとUIインタラクションの全体フロー

ドラッグ＆ドロップ操作における成否判定、UIの見た目の変化、リフロー処理は、複数のクラスが連携して実現されます。中心的な役割を担うのが `UIInteractionOrchestrator` です。

### 関連クラスと責務

-   **`UIInteractionOrchestrator`**: UIインタラクション全体の司令塔。`SpriteCommandBus` を通じて各種UIイベントを受け取り、`UIStateMachine` の状態を更新し、各ViewやManagerに具体的な指示を出します。
-   **`UIStateMachine`**: UI全体の現在の状態（例: `Idle`, `DraggingCard`, `DropedCard`）を管理します。
-   **`CardInteractionStrategy`**: 特定の状況下でカードやスロットがどのようなインタラクション（ドラッグ、ホバー等）を許可するかを判断する戦略クラス。
-   **`CardSlotManager`**: 全ての `CardSlotView` を管理し、カードの配置、リフロー処理の実行依頼などを行います。
-   **`ReflowService`**: カードが配置または移動した際の、新しいレイアウト（リフロー）の計算を担当します。
-   **`UIActivationPolicy`**: `UIStateMachine` の状態に基づき、各UI要素（カード、スロットなど）のインタラクション（コライダーの有効/無効）を制御します。
-   **`ViewRegistry`**: シーン内に存在する全ての `BaseSpriteView` のインスタンスを管理し、`CompositeObjectId` からViewを取得する機能を提供します。
    *   `GetNextAvailableCreatureCardView()` メソッドは、`IsSpawned` フラグが `false` の `CreatureCardView` を探し、見つかった場合はそのViewの `SetSpawnedState(true)` を呼び出してから返します。これにより、オブジェクトプールから再利用可能なViewを効率的に取得します。
-   **`CreatureCardView` / `CardSlotView`**: それぞれカードとスロットの見た目と振る舞いを担当するViewクラス。`Orchestrator` からの指示に従い、状態遷移やアニメーションを実行します。

### インタラクションフロー詳細

1.  **ドラッグ開始時 (`OnBeginDrag`)**
    -   `SpriteInputHandler` が `SpriteBeginDragCommand` を発行します。
    -   `Orchestrator` は `CardInteractionStrategy` に問い合わせ、ドラッグが可能か確認します。
    -   可能な場合、`UIStateMachine` を `DraggingCard` 状態に遷移させます。
    -   `Orchestrator` はドラッグされたカードのID (`_draggedId`) を記録します。
    -   ドラッグされた `CreatureCardView` は `EnterDraggingState` に遷移します。
    -   `UIActivationPolicy` が、現在の状態に合わせて他のカードやスロットのインタラクションを更新します。

2.  **ドラッグ中 (`OnDrag`)**
    -   `SpriteInputHandler` が `SpriteDragCommand` を発行します。
    -   `Orchestrator` はドラッグ中の `CreatureCardView` に `MoveTo` を指示し、マウスに追従させます。

3.  **スロットへのホバー時 (`OnHover`)**
    -   `SpriteInputHandler` が `SpriteHoverCommand` を発行します。
    -   `Orchestrator` は `CardInteractionStrategy` に問い合わせ、ホバーが可能か確認します。
    -   可能な場合、`CardSlotManager` に `OnCardHoveredOnSlot` を通知し、リフローのプレビュー（仮配置）を開始させます。この処理は非同期で行われます。

4.  **ドロップ時 (`OnDrop`)**
    -   `SpriteInputHandler` が `SpriteDropCommand` を発行します。
    -   `Orchestrator` は `CardInteractionStrategy` に問い合わせ、ドロップが可能か確認します。
    -   可能な場合、`UIStateMachine` を `DropedCard` 状態に遷移させ、UI操作を一時的に無効化します (`DisableUIInteractionCommand`)。
    -   `Orchestrator` はドロップ成功フラグ (`_isDroppedSuccessfully`) を立てます。
    -   `CardSlotManager` に `OnCardDroppedOnSlot` を通知し、カード配置の確定と、`ReflowService` を利用した最終的なリフロー計算を依頼します。

5.  **ドラッグ終了時 (`OnEndDrag`)**
    -   `SpriteInputHandler` が `SpriteEndDragCommand` を発行します。
    -   `Orchestrator` は `UIStateMachine` を `DropedCard` 状態に設定します。
    -   `UniTask.Delay` を用いて短時間待機し、その間に `OnDrop` が呼ばれて成功フラグが立っているかを確認します。
    -   **ドロップが成功しなかった場合**、`CardSlotManager.OnDropFailed()` を呼び出し、リフローのキャンセルとカードを元の位置に戻す処理を依頼します。
    -   成功・失敗に関わらず、成功フラグはリセットされます。

6.  **リフローアニメーションと後処理**
    -   `CardSlotManager` は `ReflowService` の計算結果に基づき、影響を受ける各カードに対して `ReflowCompletedCommand` または `DragReflowCompletedCommand` を発行します。
    -   `Orchestrator` はこれらのコマンドを受け取り、影響を受ける `CreatureCardView` に `MoveToAnimated` を指示して、新しい位置へアニメーションさせます。
    -   全てのアニメーションが完了した後、`Orchestrator` は `SpriteDragOperationCompletedCommand` を発行します。
    -   この最終コマンドを受け、`Orchestrator` は `UIStateMachine` を `Idle` に戻し、`UIActivationPolicy` を通じて全てのUIのインタラクションを正常化し、`EnableUIInteractionCommand` を発行してUI操作を再開させます。

このフローにより、複雑なUIインタラクションが状態機械と責務分離されたクラス群によって、堅牢かつ拡張可能に管理されます。

---

## 関連ファイル

- [guide_rules.md](../guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](../guide/guide_files.md): ドキュメントファイル命名・管理ルール

---

## 更新履歴

- 2025-08-14: SpriteUIの表示状態管理（`_displayRootGameObject`, `SetDisplayActive`）とオブジェクトプールにおける再利用（`IsSpawned`, `SetSpawnedState`）に関する記述を追加。`ViewRegistry`の`GetNextAvailableCreatureCardView`の変更を反映。(Gemini - Document Specialist)
- 2025-08-04: ソースコードとの乖離を修正し、UIInteractionOrchestratorを中心とした全体フローを実装に合わせて更新 (Gemini - Codebase Analyst)
- 2025-07-21: `SpriteDragOperationCompletedCommand` の導入と、それによるドラッグ＆ドロップフローの変更を追記 (Gemini - Technical Writer for Game Development)
- 2025-07-20: アニメーション機能の分離において、IAnimationStrategyとBaseAnimationSOのPlayAnimationメソッドがMultiRendererVisualControllerを引数として受け取るように修正
- 2025-07-20: PlacedOnSlotCommandをSpriteDropCommandに統合し、ドロップ成否判定フローをSpriteDropCommandで完結するように修正
- 2025-07-20: ドロップ成否判定フローの記述を、BaseSpriteViewでの_isDroppedSuccessfullyの管理と遅延処理のキャンセルを明確化
- 2025-07-20: CardPlacedOnSlotCommandをPlacedOnSlotCommandに変更し、ドロップ成否判定フローの記述をBaseSpriteViewに汎用化
- 2025-07-20: アニメーション機能の分離とドロップの失敗と成功の判定方法に関する設計を追加
- 2025-07-18: コマンドクラスのTargetObjectId変数名を統一し、設計書に反映
- 2025-07-18: SpriteInputHandlerのOnBeginDrag/OnDragにマウス追随処理を追加、SpriteClickCommandとSpriteEndDragCommandを追加
- 2025-07-18: SpriteInputHandlerの修正とSpriteDragCommandの追加
- 2025-07-18: 初版作成 (Gemini - Technical Writer for Game Development)
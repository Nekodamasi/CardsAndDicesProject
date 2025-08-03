# gdd_sprite_ui_design.md - SpriteUI基本設計

---

## 概要

この設計書は、UnityにおけるSpriteベースのUI要素（SpriteUI）の基本的なインタラクションと構造を定義します。
マウスイベントの検知、コマンドの発行、およびそれに応じたUIの視覚的変化を管理するための主要なクラスとその役割を記述します。

---

## 主要クラスの定義

### 1. SpriteInputHandler

- **役割:** Unityの`IPointerEnterHandler`などのインターフェースを実装し、マウスイベントを検知して対応するコマンドを発行します。
- **検知するマウスイベントと処理:**
    - `OnPointerEnter`:
        - ドラッグ中でなければ `SpriteHoverCommand` を発行します。
    - `OnPointerExit`:
        - ホバー中であれば `SpriteUnhoverCommand` を発行します。
    - `OnBeginDrag`:
        - `SpriteBeginDragCommand` を発行します。
    - `OnPointerUp`:
        - ドラッグ中でなければ `SpriteClickCommand` を発行します。
    - `OnEndDrag`:
        - `SpriteEndDragCommand` を発行します。
    - `OnDrag`:
        - `SpriteDragCommand` を発行します。
    - `OnDrop`:
        - `SpriteDropCommand` を発行します。

### 2. SpriteCommandBus

- **役割:** SpriteUIに関連するイベントの登録（`on`）、配信（`emit`）、解除（`off`）を一元管理する中央ハブです。
    - 各UI要素は、このバスを通じてイベントを購読・発行します。

### 3. BaseSpriteView

- **役割:** すべてのSpriteUI要素の基底クラスとなり、共通の表示ロジックとコマンド購読機能を提供します。
    - 各種Spriteコマンド（`SpriteHoverCommand`など）を購読します。
    - **アニメーションの定義はScriptableObjectとして外部化され、このクラスに注入されます。**
    - 内部の`enum`である`SpriteStatus`を更新し、その状態に基づいてSpriteの見た目（色、スケールなど）を変更します。

---

## アニメーション機能の分離

DOTweenを用いたアニメーションの定義と実行ロジックを、`ScriptableObject`ベースの戦略パターンとして分離します。これにより、アニメーションの再利用性、柔軟性、およびデザイナーによる調整の容易性を高めます。

### 1. IAnimationStrategy

- **役割:** すべてのアニメーション戦略が実装すべきインターフェースです。`PlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration)`メソッドを定義します。

### 2. BaseAnimationSO

- **役割:** `IAnimationStrategy`を実装する`ScriptableObject`の抽象基底クラスです。`PlayAnimation`メソッドは、`GameObject`、`MultiRendererVisualController`、`originalScale`、`originalColor`を引数として受け取り、具体的なアニメーションロジックを`protected abstract Sequence DoPlayAnimation(GameObject targetObject, MultiRendererVisualController targetVisualController, Vector3 originalScale, Color originalColor, float duration)`で派生クラスに委譲します。共通の`_animationDuration`フィールドやヘルパーメソッド（例: `GetBrightenedColor`）も提供します。

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
- `BaseSpriteView`に`public void SetOrderInLayer(int order)`メソッドを実装し、内部で`SpriteLayerController.SetOrderInLayer(order)`を呼び出します。これにより、`BaseSpriteView`を持つオブジェクトから、関連するすべてのUI要素の描画順序を簡単に変更できます。

---

## コマンドクラスの定義

以下のコマンドクラスは、`SpriteInputHandler`によって発行され、`SpriteCommandBus`を通じて配信されます。
**各コマンドは、対象となるオブジェクトの`CompositeObjectId`を`TargetObjectId`プロパティとして持ちます。**

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
    - ドラッグ操作が完全に終了したことを通知する。このコマンドは、ドラッグの成功・失敗に関わらず、関連する全てのUI要素に最終的な後処理を行わせるためのトリガーとなる。
- **主な処理:**
    - **リフロー配置の確定:** `CardSlotView` は、このコマンドを受けて `ReflowPlacedCardId`（仮配置）を `PlacedCardId`（確定配置）に上書きする。
    - **コライダーの有効化:** `BaseSpriteView` は、無効化されていたコライダーを再度有効にする。
    - **状態の正常化:** 各Viewは、自身の状態を `Normal` に戻す。
- **項目:**
    - `TargetObjectId`: ドラッグ操作が完了したオブジェクトの`CompositeObjectId`。
    - `IsDropSuccessful`: ドロップが成功したかどうか。

---

## ドロップの失敗と成功の判定方法

ドラッグ＆ドロップ操作におけるドロップの成否判定と、それに応じたUIの振る舞いは、`UIInteractionOrchestrator` が中心となって以下のフローで実現します。

1.  **ドラッグ開始時 (`Orchestrator.OnBeginDrag`)**
    - `UIStateMachine` の状態を `DraggingCard` に設定します。
    - ドラッグされたカードの `CreatureCardView` に対し、`EnterDraggingState()` を呼び出し、`DraggingStarted` 状態に遷移させます。
    - 全ての `CardSlotView` に対し、ドラッグされたカードを乗せているスロットなら `Inactive` 状態に、それ以外なら `Acceptable` 状態になるよう指示します。

2.  **ドラッグ中 (`Orchestrator.OnDrag`)**
    - `SpriteDragCommand` を受け取ります。
    - ドラッグ中のカードの `CreatureCardView` に対し、`transform.position` を `command.NewPosition` に設定し、`EnterDraggingInProgressState()` を呼び出し、`DraggingInProgress` 状態に遷移させます。

3.  **スロットへのホバー時 (`Orchestrator.OnHover`)**
    - ホバーされた `CardSlotView` に対し、`Hovering` 状態になるよう指示します。
    - `CardSlotManager` にリフローのプレビューを要求します。

4.  **スロットからのアンホバー時 (`Orchestrator.OnUnhover`)**
    - アンホバーされた `CardSlotView` に対し、`Acceptable` 状態に戻るよう指示します。
    - `CardSlotManager` にリフローのプレビューキャンセルを要求します。

5.  **ドロップ時 (`Orchestrator.OnDrop`)**
    - ドロップを受け取った `CardSlotView` が存在する場合、`CardSlotManager.OnCardDroppedOnSlot()` を呼び出し、カード配置の確定とリフロー処理を依頼します。

6.  **ドラッグ終了時 (`Orchestrator.OnEndDrag`)**
    - `UIStateMachine` の状態を `Idle` に戻します。
    - **ドロップが成功しなかった場合** (`SpriteDropCommand` が直前に発行されていない場合)、`CardSlotManager.OnDropFailed()` を呼び出し、リフローのキャンセルとカードを元の位置に戻す処理を依頼します。
    - 全ての `CardSlotView` に対し、`Normal` 状態に戻るよう指示します。

7.  **リフロー完了と後処理 (`CreatureCardView.OnReflowCompleted` -> `Orchestrator.OnSpriteDragOperationCompleted`)**
    - `CardSlotManager` から `ReflowCompletedCommand` が発行されると、各 `CreatureCardView` は指示された位置へ移動アニメーションを行います。
    - アニメーション完了後、`SpriteDragOperationCompletedCommand` が発行されます。
    - `Orchestrator` はこのコマンドを受け取り、関係する全てのView（カードとスロット）のコライダーを有効化し、状態を `Normal` に確定させる指示を出します。

このフローにより、複雑なインタラクションの判断は `UIInteractionOrchestrator` に集約され、各Viewは自身の見た目の変更に専念できます。

---

## 関連ファイル

- [guide_rules.md](../guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](../guide/guide_files.md): ドキュメントファイル命名・管理ルール

---

## 更新履歴

- 2025-07-18: 初版作成 (Gemini - Technical Writer for Game Development)
- 2025-07-18: SpriteInputHandlerの修正とSpriteDragCommandの追加
- 2025-07-18: SpriteInputHandlerのOnBeginDrag/OnDragにマウス追随処理を追加、SpriteClickCommandとSpriteEndDragCommandを追加
- 2025-07-18: コマンドクラスのTargetObjectId変数名を統一し、設計書に反映
- 2025-07-20: アニメーション機能の分離とドロップの失敗と成功の判定方法に関する設計を追加
- 2025-07-20: CardPlacedOnSlotCommandをPlacedOnSlotCommandに変更し、ドロップ成否判定フローの記述をBaseSpriteViewに汎用化
- 2025-07-20: ドロップ成否判定フローの記述を、BaseSpriteViewでの_isDroppedSuccessfullyの管理と遅延処理のキャンセルを明確化
- 2025-07-20: PlacedOnSlotCommandをSpriteDropCommandに統合し、ドロップ成否判定フローをSpriteDropCommandで完結するように修正
- 2025-07-20: アニメーション機能の分離において、IAnimationStrategyとBaseAnimationSOのPlayAnimationメソッドがSpriteRendererを引数として受け取るように修正
- 2025-07-20: アニメーション機能の分離において、IAnimationStrategyとBaseAnimationSOのPlayAnimationメソッドがMultiRendererVisualControllerを引数として受け取るように修正
- 2025-07-21: `SpriteDragOperationCompletedCommand` の導入と、それによるドラッグ＆ドロップフローの変更を追記 (Gemini - Technical Writer for Game Development)

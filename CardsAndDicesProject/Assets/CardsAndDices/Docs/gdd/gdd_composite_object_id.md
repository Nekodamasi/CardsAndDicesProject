# gdd_composite_object_id.md - 複合オブジェクト識別子 GDD

---

## 1. 概要

この設計書は、プレイヤーの操作対象や、ゲーム内で個別に識別する必要がある全てのゲームオブジェクトを一意に管理するための「複合オブジェクト識別子（Composite Object ID）」システムのゲームデザインを定義します。

本システムは、オブジェクトのID管理（**生成**、**登録**、**検索**）だけでなく、それらのオブジェクトに対するユーザーの入力（クリック、ドラッグ等）をハンドリングし、状態を管理する仕組みまでを提供します。

---

## 2. 主要コンポーネントと役割

本システムは、責務が明確に分離された以下のコンポーネントから構成されます。

### 2.1. ID管理コンポーネント

-   **CompositeObjectId （データ構造）:** 「IDそのもの」を表現するデータクラス。
-   **CompositeObjectIdManager （ID製造工場）:** `CompositeObjectId` を **生成** することに特化した `ScriptableObject`。
-   **CompositeObjectRegistry （ID登録名簿）:** シーン上に **存在する** `CompositeObjectId` を **登録・管理** する `ScriptableObject`。

### 2.2. 相互作用（インタラクション）コンポーネント

-   **IIdentifiableView （識別可能なオブジェクト）:** `CompositeObjectId` を持ち、自身が識別可能であることを示すインターフェース。
-   **IdentifiableInputHandler （入力受付）:** `IIdentifiableView` を持つGameObjectにアタッチされ、Unityの入力イベント（`OnPointerEnter`など）を検知し、`IdentifiableCommandBus` へ具体的なコマンド（`IdentifiableHoverCommand`など）を発行します。
-   **IdentifiableUIStateMachine （交通整理役）:** UI全体のインタラクション状態（`Idle`, `Dragging`など）を管理するステートマシン。`IdentifiableCommandBus` を流れるコマンドを監視し、状態の競合（例: ドラッグ中に別のオブジェクトをクリック）が起きないように、発行されるコマンドを制御します。
-   **BaseIdentifiableStateOperator （専門の処理実行役）:** 特定の `CompositeObjectId` に対する状態変化コマンド（`IdentifiableStateHoverCommand`など）を購読する `ScriptableObject`。コマンドを受け取ると、具体的なリアクション（例: アニメーション再生、エフェクト表示）を実行します。

---

## 3. IDのライフサイクル

1.  **生成 (Creation):** `IIdentifiableView` を持つオブジェクトが生成される際、`CompositeObjectIdManager` にIDの発行を要求します。
2.  **登録 (Registration):** オブジェクトは有効化されると、自身のIDを `CompositeObjectRegistry` に登録します。
3.  **利用 (Utilization):** 他のシステムは `CompositeObjectRegistry` を通じて、現在アクティブなオブジェクトのIDを安全に検索・利用します。
4.  **登録解除 (Unregistration):** オブジェクトが無効化・破壊されると、自身のIDを `CompositeObjectRegistry` から登録解除します。

---

## 4. イベント駆動による相互作用フロー

ユーザーの入力からオブジェクトの反応までは、以下のイベント駆動フローで処理されます。

1.  **入力検知:** ユーザーがマウスカーソルをオブジェクトに乗せると、そのオブジェクトの `IdentifiableInputHandler` が `OnPointerEnter` イベントを検知します。
2.  **コマンド発行（入力）:** `IdentifiableInputHandler` は、自身の `CompositeObjectId` を含んだ `IdentifiableHoverCommand` を `IdentifiableCommandBus` に発行します。
3.  **状態判定:** `IdentifiableUIStateMachine` が `IdentifiableHoverCommand` を受信します。現在のUI状態が `Idle` であれば、状態を `Hover` に遷移させ、新たな状態変化コマンド `IdentifiableStateHoverCommand` を発行することを許可します。
4.  **コマンド発行（状態変化）:** `IdentifiableUIStateMachine` は `IdentifiableStateHoverCommand` を発行します。
5.  **処理実行:** `BaseIdentifiableStateOperator` が `IdentifiableStateHoverCommand` を受信します。コマンド内の `CompositeObjectId` が自身の監視対象であれば、`OnStateHover` メソッドを実行し、オブジェクトをハイライトさせるなどの具体的な処理を行います。

このフローにより、入力、状態管理、具体的な処理が疎結合に保たれ、拡張性の高いインタラクションを実現します。

---

## 5. 関連ファイル

-   [sys_identifiable-views.md](../sys/sys_identifiable-views.md)
-   [sys_domain-model.md](../sys/sys_domain-model.md)

---

## 6. 更新履歴

-   2025-09-14: インタラクション関連クラス（InputHandler, StateMachine, Operator）の役割とフローを追記 (Gemini)
-   2025-09-13: 状態管理（State Management）に関するセクションを追加 (Gemini)
-   2025-09-13: `IdentifiableGameObject` の記述を現状に合わせて追加 (Gemini)
-   2025-09-13: `CompositeObjectRegistry` の追加とライフサイクルの明確化など、現状のソースコードに合わせて全面更新 (Gemini)
-   2025-09-12: ソースコードの現状に合わせて初期化フローを更新。関連ファイルを追加。 (Gemini - Technical Writer for Game Development)
-   2025-07-18: IdentifiableGameObjectクラスの追加 (Gemini - Technical Writer for Game Development)
-   2025-07-18: 初版作成 (Gemini - Technical Writer for Game Development)
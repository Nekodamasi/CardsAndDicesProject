# gdd_composite_object_id.md - 複合オブジェクト識別子 GDD

---

## 1. 概要

この設計書は、プレイヤーの操作対象や、ゲーム内で個別に識別する必要がある全てのゲームオブジェクトを一意に管理するための「複合オブジェクト識別子（Composite Object ID）」システムのゲームデザインを定義します。

本システムは、オブジェクトに一意のIDを付与するだけでなく、オブジェクトの**生成**、**登録**、**検索**、**登録解除**という一連のライフサイクルと、それらのオブジェクトの**状態管理**を行う仕組みを提供します。

---

## 2. 主要コンポーネントと役割

本システムは、責務が明確に分離された以下のコンポーネントから構成されます。

### 2.1. CompositeObjectId （データ構造）

-   **役割:** 「IDそのもの」を表現するデータクラス。
-   **責務:**
    -   `UniqueId`: ゲーム実行中に決して重複しない一意なIDを保持します。
    -   `ObjectType`: オブジェクトの種類（カード、ダイスなど）を示す `CompositeObjectIdTypeEntity` を保持します。
    -   `Owner`: 親オブジェクトのIDを保持し、階層構造を表現します。

### 2.2. CompositeObjectIdManager （ID製造工場）

-   **役割:** `CompositeObjectId` を **生成（Create）** することに特化した `ScriptableObject`。
-   **責務:**
    -   ゲーム全体でユニークな `UniqueId` を採番し、新しい `CompositeObjectId` を生成して返します。
    -   このクラスは、IDが「現在使われているか」については関知しません。あくまでIDを発行するだけの役割です。

### 2.3. CompositeObjectRegistry （ID登録名簿）

-   **役割:** シーン上に **存在する（アクティブな）** `CompositeObjectId` を **登録・管理** する `ScriptableObject`。
-   **責務:**
    -   `Register(id)`: オブジェクトが生成・有効化された際に、そのIDを台帳に登録します。
    -   `Unregister(id)`: オブジェクトが破壊・無効化された際に、IDを台帳から削除します。
    -   `GetIdsByType(type)`: 特定のタイプのオブジェクトIDリストを検索・取得する機能を提供します。

### 2.4. IIdentifiableView （識別可能なオブジェクト）

-   **役割:** `CompositeObjectId` を持ち、自身が識別可能であることを示すインターフェース。
-   **責務:**
    -   自身の `CompositeObjectId` を公開します。
    -   `SetSpawnedState(bool)` メソッドを持ち、自身の状態（有効/無効）を管理します。
    -   このインターフェースを実装したオブジェクト（例: `CreatureCardView`）が、自身のライフサイクルに応じて `CompositeObjectRegistry` への登録・登録解除を行います。

### 2.5. IdentifiableGameObject （責務実装の基底クラス）

-   **役割:** `IIdentifiableView` が要求する責務の多くを実装した、具体的な `MonoBehaviour` の基底クラス。
-   **責務:**
    -   **IDの取得:** `OnAwake()` ライフサイクルメソッド内で、DIコンテナから注入された `CompositeObjectIdManager` を通じて自身の `CompositeObjectId` を取得します。
    -   **IDの登録:** ID取得後、同じく注入された `CompositeObjectRegistry` に自身のIDを `Register` します。
-   **目的:** このクラスを継承することで、各ViewコンポーネントがIDの取得と登録に関する定型的な処理を毎回実装する手間を省きます。

---

## 3. IDのライフサイクル

1.  **生成 (Creation):**
    -   `IIdentifiableView` を実装したオブジェクトが生成される際、`CompositeObjectIdManager` にIDの発行を要求します。

2.  **登録 (Registration):**
    -   オブジェクトは、生成されたIDを自身の `CompositeObjectId` として保持します。
    -   オブジェクトが有効化される（例: `OnEnable`）と、自身のIDを `CompositeObjectRegistry` に `Register` します。

3.  **利用 (Utilization):**
    -   他のシステムは `CompositeObjectRegistry` を参照し、「現在存在するカード一覧」や「特定の敵オブジェクト」などをIDベースで安全に検索・操作します。

4.  **登録解除 (Unregistration):**
    -   オブジェクトが無効化・破壊される（例: `OnDisable`, `OnDestroy`）と、自身のIDを `CompositeObjectRegistry` から `Unregister` します。

このライフサイクルにより、常に「シーンに実際に存在するオブジェクトのID」だけが `CompositeObjectRegistry` に登録されている状態が保証され、無効なオブジェクトへのアクセス（NullReferenceException）を防ぎます。

---

## 4. 状態管理 (State Management)

識別可能オブジェクトの現在の状態（例: `Hovered`, `Selected`など）を管理するための仕組みです。

### 4.1. 識別可能オブジェクトステータスインスタンス

-   **役割:** `識別可能オブジェクトステータス` の現在値を保持するインスタンス。
-   **責務:**
    -   特定の `CompositeObjectId` に紐付き、そのオブジェクトの現在の `Status` を保持・更新します。

### 4.2. 識別可能オブジェクトステータスマネージャー

-   **役割:** 全ての識別可能オブジェクトの「ステータスインスタンス」を管理するマネージャークラス。
-   **責務:**
    -   ゲーム開始時などに `CompositeObjectRegistry` から現在アクティブな `CompositeObjectId` の一覧を取得します。
    -   取得したIDごとに `識別可能オブジェクトステータスインスタンス` を生成し、一元管理します。

---

## 5. 関連ファイル

-   [sys_identifiable-views.md](../sys/sys_identifiable-views.md)
-   [sys_domain-model.md](../sys/sys_domain-model.md)

---

## 6. 更新履歴

-   2025-09-13: 状態管理（State Management）に関するセクションを追加 (Gemini)
-   2025-09-13: `IdentifiableGameObject` の記述を現状に合わせて追加 (Gemini)
-   2025-09-13: `CompositeObjectRegistry` の追加とライフサイクルの明確化など、現状のソースコードに合わせて全面更新 (Gemini)
-   2025-09-12: ソースコードの現状に合わせて初期化フローを更新。関連ファイルを追加。 (Gemini - Technical Writer for Game Development)
-   2025-07-18: IdentifiableGameObjectクラスの追加 (Gemini - Technical Writer for Game Development)
-   2025-07-18: 初版作成 (Gemini - Technical Writer for Game Development)

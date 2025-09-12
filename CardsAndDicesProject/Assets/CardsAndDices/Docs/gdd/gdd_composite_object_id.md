# gdd_composite_object_id.md - 複合オブジェクト識別子基本設計

---

## 概要

この設計書は、プレイヤーの操作が影響するゲームオブジェクトを識別するための「複合オブジェクト識別子」の基本設計を定義します。
各オブジェクトに一意のIDとタイプを付与し、さらに階層構造を表現することで、オブジェクト間の関係性を簡易に比較・管理する機能を提供します。

---

## 実装クラス

### 1. CompositeObjectId - 複合オブジェクト識別子

#### 概要

- オブジェクトを識別するIDやタイプを持ち、自身を所有するオブジェクトの識別子を`owner`として所有することで階層構造を表したクラスです。

#### 項目

- **ユニークID (Unique ID)**
  - 各オブジェクトに割り当てられる一意の識別子（シーケンス番号）。
- **オブジェクトタイプ (Object Type)**
  - オブジェクトの種類を示す `CompositeObjectIdTypeEntity` 型のScriptableObject。
- **オーナー (Owner)**
  - 自身が所属する親オブジェクトの`CompositeObjectId`。これにより、オブジェクトの階層構造を表現します。

### 2. CompositeObjectIdManager - 複合オブジェクト識別子マネージャー

#### 概要

- `CompositeObjectId`を製造・管理するためのマネージャークラスです（`ScriptableObject`として実装）。
- オブジェクトタイプを指定すると、新しい`CompositeObjectId`を生成して返します。
- ユニークIDを採番し、生成されるすべての`CompositeObjectId`がユニークなIDを持つように管理します。
- DIコンテナによって`Initialize`メソッドが呼び出され、IDの採番がリセットされます。

### 3. IdentifiableGameObject - 識別可能なゲームオブジェクト

#### 概要

- Unityの`MonoBehaviour`を継承し、自身の`CompositeObjectId`を保持するクラスです。
- VContainerによる依存性注入と、Unityのライフサイクルイベントを組み合わせて初期化が行われます。

#### 初期化シーケンス

1.  **依存性注入 (Constructメソッド):**
    -   DIコンテナ（VContainer）によって`Construct`メソッドが呼び出され、`CompositeObjectIdManager`のインスタンスが注入されます。
2.  **ID生成 (OnAwakeメソッド):**
    -   `IGameInitializable`の`OnAwake`メソッドで、インスペクターから設定された`ObjectType`（`CompositeObjectIdTypeEntity`）と、注入済みの`CompositeObjectIdManager`を使って、自身の`CompositeObjectId`を生成・保持します。

この2段階の初期化により、DIコンテナによる依存関係の解決と、Unityエディタ上でのデータ設定（`[SerializeField]`）が両立されます。

---

## 関連ファイル

- [sys_domain-model.md](../sys/sys_domain-model.md): 関連するドメインモデルの設計
- [guide_rules.md](../guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](../guide/guide_files.md): ドキュメントファイル命名・管理ルール

---

## 更新履歴

- 2025-09-12: ソースコードの現状に合わせて初期化フローを更新。関連ファイルを追加。 (Gemini - Technical Writer for Game Development)
- 2025-07-18: IdentifiableGameObjectクラスの追加 (Gemini - Technical Writer for Game Development)
- 2025-07-18: 初版作成 (Gemini - Technical Writer for Game Development)

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
  - そのオブジェクトを端的に表す文字列（例: `Button`, `Card`, `Dice`など）。

- **オーナー (Owner)**
  - 自身が所属する親オブジェクトの`CompositeObjectId`。これにより、オブジェクトの階層構造を表現します。

### 2. CompositeObjectIdManager - 複合オブジェクト識別子マネージャー

#### 概要

- `CompositeObjectId`を製造・管理するためのマネージャークラスです（`ScriptableObject`として実装）。
- オブジェクトタイプを指定すると、新しい`CompositeObjectId`を生成して返します。
- ユニークIDを採番し、生成されるすべての`CompositeObjectId`がユニークなIDを持つように管理します。

### 3. IdentifiableGameObject - 識別可能なゲームオブジェクト

#### 概要

- Unityの`MonoBehaviour`を継承し、自身の`CompositeObjectId`を保持するクラスです。
- インスペクターから`ObjectType`を設定でき、VContainerを通じて`CompositeObjectIdManager`から`CompositeObjectId`を取得します。
- 必要に応じて、自身の`CompositeObjectId`の`Owner`を設定する機能を提供します。

---

## 関連ファイル

- [guide_rules.md](../guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](../guide/guide_files.md): ドキュメントファイル命名・管理ルール

---

## 更新履歴

- 2025-07-18: 初版作成 (Gemini - Technical Writer for Game Development)
- 2025-07-18: IdentifiableGameObjectクラスの追加 (Gemini - Technical Writer for Game Development)
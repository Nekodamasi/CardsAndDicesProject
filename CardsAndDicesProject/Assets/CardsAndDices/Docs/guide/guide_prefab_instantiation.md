# guide_prefab_instantiation.md - Prefabインスタンス化ガイド

---

## 概要

本ドキュメントは、プロジェクトにおける`Prefab`のインスタンス化に関する統一されたルールを定めます。DIコンテナ（VContainer）のライフサイクルと連携し、一貫性のあるオブジェクト生成を実現することを目的とします。

---

## 依存関係の登録

`Prefab`のインスタンスは、直接シーンに配置するのではなく、DIコンテナを介して動的に生成します。そのために、まず`Prefab`自体をDIコンテナに登録する必要があります。

-   `LifetimeScope`を継承したコンポーネント内で、`[SerializeField]`属性を用いて`Prefab`への参照を保持します。
-   `Configure`メソッド内で、`builder.RegisterInstance`を使用して`Prefab`をインスタンスとして登録します。
-   この際、`WithId`を用いて、どの`Prefab`であるかを識別するための明示的なIDを必ず付与します。

-   登録コード例:
    -   `builder.RegisterInstance(enemyPrefab1).As<GameObject>().WithId("Enemy1");`

---

## 実装コンポーネント

`Prefab`のインスタンス化は、主に`Factory`クラスと`Spawner`クラスの2つのコンポーネントによって責務を分離して行います。

### 1. Factoryクラス

`Factory`クラスは、DIコンテナに登録された単一の`Prefab`をインスタンス化する責務のみを持ちます。

-   **責務:**
    -   原則として、`Prefab`一つにつき、一つの`Factory`クラスを作成します。
    -   `Create`メソッドを実装し、`Prefab`のインスタンスを一つ生成して返します。
    -   DIコンテナの`IObjectResolver`を利用し、`resolver.Instantiate(prefab)`を呼び出すことで、生成されるインスタンス内の依存関係を解決します。

### 2. Spawnerクラス

`Spawner`クラスは、`Factory`を介して生成された`GameObject`の初期化とセットアップを行う責務を持ちます。

-   **責務:**
    -   `IStartable`インターフェースを継承し、DIコンテナの起動時にインスタンス生成処理が実行されるようにします。
    -   `Factory`クラスをコンストラクタでインジェクションし、インスタンスの生成を依頼します。
    -   複数のインスタンス生成や、個別の初期値設定など、`Factory`から返された`GameObject`に対するセットアップ処理を担当します。
    -   生成する数や変更内容といった設定値を管理するために、静的なデータクラスを参照することがあります。
    -   生成されたインスタンスが持つ`IInitialize`インターフェースを実装したコンポーネントに対し、初期化処理を呼び出します。

---

## 関連ファイル

-   [guide_rules.md](./guide_rules.md)
-   [guide_files.md](./guide_files.md)

---

## 更新履歴

-   YYYY-MM-DD: 初版 (Gemini - Technical Writer for Game Development)

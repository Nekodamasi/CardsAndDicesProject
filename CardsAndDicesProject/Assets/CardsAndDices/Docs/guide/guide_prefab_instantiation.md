# guide_prefab_instantiation.md - Prefabインスタンス化ガイド

---

## 概要

本ドキュメントは、プロジェクトにおける`Prefab`のインスタンス化に関する統一されたルールを定めます。DIコンテナ（VContainer）のライフサイクルと連携し、一貫性のあるオブジェクト生成を実現することを目的とします。
`Prefab` のインスタンス化は、`Prefab` ごとに個別の実装となるため、ここに記載する実装方式にそって、それぞれの個別のコンポーネントとして実装する必要があり、本ドキュメントは、実装方式の手引きとなる。

---

## 依存関係の登録

-   `Prefab` のインスタンス化は、直接シーンに配置するのではなく、DIコンテナを介して動的に生成します。そのために、まず `Prefab` 自体をDIコンテナに登録する必要があります。

-   `LifetimeScope` を継承したコンポーネント内で、`[SerializeField]` 属性を用いて `Prefab` への参照を保持します。

---

## 実装コンポーネント

### 1. Factory

-   `Prefab`のインスタンス化は、`LifetimeScope` 内の `builder.RegisterFactory` によって行います。
-   この際、`Prefab` ごとに `SpawnInfo` クラスを `builder.RegisterFactory` の引数として渡します。
-   これにより、`builder.RegisterFactory` がどの `Prefab` を生成するかを決定します。

### 2. SpawnInfoクラス

-   `Prefab` の種類ごとに個別のクラスを作成します。
-   これは、`builder.RegisterFactory` の引数として渡すことで、どの `Prefab` から生成する `Factory` かを明確にします。
-   また、同じ `Prefab` から複数生成する場合、生成する `GameObject` ごとに個別の値を渡せます。

### 2. SpawnInfoManagerクラス

-   `SpawnInfo` の種類ごとに個別のクラスを作成します。
-   `SpawnInfo` を管理し、管理する `SpawnInfo` の数が、`GameObject` の生成数になります。
-   `Spawner` に参照され、生成数（`SpawnInfo`の数）と `SpawnInfo` を渡します。

### 3. Spawnerクラス

-   `Spawner` クラスは、`Factory` を介して `Prefab` から `GameObject` を生成します。
-   `MonoBehaviour` を継承しており、アタッチされた `GameObject` の配下に生成した `GameObject` を配置します。

---

## 関連ファイル

-   [guide_rules.md](./guide_rules.md)
-   [guide_files.md](./guide_files.md)

---

## 更新履歴

-   2025-10-25: 初版

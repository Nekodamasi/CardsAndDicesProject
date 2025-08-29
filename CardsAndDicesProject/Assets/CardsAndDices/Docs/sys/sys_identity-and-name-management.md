# sys_identity-and-name-management.md - ID・名称管理システム設計書

---

## 概要

このドキュメントは、ゲーム内に登場する全てのエンティティ（クリーチャー、アイテム等）の識別IDと、多言語対応された「名称」を管理するためのシステムを設計する。

本システムは、タイプセーフなID管理と、デザイナーにとって直感的でメンテナンスしやすいデータ構造を提供することを目的とする。

---

## 設計目標

- **タイプセーフなID管理:** 文字列によるID指定を廃し、コンパイル時にチェック可能な`ScriptableObject`参照によってIDの安全性を保証する。
- **スケーラビリティ:** 将来的にエンティティの数が数百規模に増加しても、パフォーマンスや管理の複雑さが問題にならない構造とする。
- **多言語対応:** `AssetBundle`等によるリソース切り替えを想定し、英語、日本語などのリージョンに容易に対応可能な設計とする。
- **高い保守性とデザイナーへの配慮:** デザイナーがインスペクター上でドラッグ＆ドロップによって直感的にIDを設定できるワークフローを提供する。
- **既存アーキテクチャとの整合性:** プロジェクトで採用されているDIコンテナ`VContainer`と、`GameLifetimeScope`における手動のメソッドインジェクション方式を尊重し、それに準拠する。

---

## 主要コンポーネント

本システムは、責務の異なる以下の`ScriptableObject`とインターフェース、およびそれを利用する`MonoBehaviour`で構成される。

### 1. EntityDefinition

- **種別:** `ScriptableObject`
- **役割:** ゲーム内エンティティの「ID」そのものとして機能するアセット。このアセットへの参照が、タイプセーフな一意なIDとなる。
- **詳細:** 内部にデータはほとんど持たず、アセット自体の存在がIDとなる。セーブデータ等への永続化のために、一意な文字列ID（例: `creature_goblin_001`）を保持することはある。

### 2. NameDatabase

- **種別:** `ScriptableObject`
- **役割:** 特定の言語（例: 日本語）における、全ての`EntityDefinition`と、それに対応する表示名・説明文のペアを保持するデータベース。
- **詳細:** `EntityDefinition`をキー、表示名などを含む`NameData`を値とする辞書的な構造を持つ。このアセットが言語ごとに作成され、`AssetBundle`の対象となる。

### 3. INameService

- **種別:** Interface
- **役割:** 名称解決サービスの振る舞いを定義するインターフェース。
- **メソッド（例）:** `string GetDisplayName(EntityDefinition entityDef)`

### 4. NameService

- **種別:** `ScriptableObject`
- **役割:** `INameService`インターフェースの具体的な実装。`NameDatabase`を内部に保持し、`EntityDefinition`から対応する名称を検索して返す責務を持つ。

### 5. 利用者 (MonoBehaviour)

- **種別:** `MonoBehaviour` (例: `CreatureView`)
- **役割:** `VContainer`によって`INameService`のインスタンスを注入される。インスペクターで設定された`EntityDefinition`と、注入された`INameService`を利用して、自身の表示名を解決する。

---

## 依存関係と初期化フロー

本システムは、`GameLifetimeScope`で定義された既存のDI・初期化フローに完全に準拠する。

1.  **アセットの準備:** `NameService.asset`と、デフォルト言語の`NameDatabase.asset`（例: `NameDatabase_ja.asset`）を作成する。
2.  **`GameLifetimeScope`での設定:** `GameLifetimeScope`のインスペクターに、作成した`NameService.asset`と`NameDatabase_ja.asset`をドラッグ＆ドロップで設定する。
3.  **DIコンテナへの登録:** `GameLifetimeScope`の`Configure`メソッド内で、`builder.RegisterInstance(_nameService).As<INameService>()`のように`NameService`をコンテナに登録する。
4.  **サービスの初期化:** 続けて`Configure`メソッド内で、`_nameService.Initialize(_nameDatabase)`を呼び出し、`NameService`が使用するデータベースを確定させる。
5.  **`MonoBehaviour`への注入:** `CreatureView`などの`MonoBehaviour`は、コンストラクタインジェクションを通じて`INameService`を受け取る。
6.  **名称の解決:** `CreatureView`は、自身がデータとして保持する`EntityDefinition`を引数として、注入された`INameService`の`GetDisplayName`メソッドを呼び出し、表示名を取得する。

---

## フォルダ構成案

### 1. スクリプト

- `Assets/CardsAndDices/Scripts/Core/`: `INameService.cs`
- `Assets/CardsAndDices/Scripts/Service/`: `NameService.cs`
- `Assets/CardsAndDices/Scripts/Data/`: `EntityDefinition.cs`, `NameDatabase.cs`

### 2. ScriptableObjectアセット

- `Assets/CardsAndDices/ScriptableObjects/Definitions/`: `EntityDefinition`アセットを配置。
  - `Creatures/`
  - `Items/`
- `Assets/CardsAndDices/ScriptableObjects/Names/`: 名称管理関連のアセットを配置。
  - `Services/`: `NameService.asset`
  - `Databases/`:
    - `ja-JP/`: `NameDatabase_ja.asset`
    - `en-US/`: `NameDatabase_en.asset`

---

## 関連ファイル

- [guide_rules.md](../guide/guide_rules.md)
- [guide_files.md](../guide/guide_files.md)

---

## 更新履歴

- 2025-08-28: 初版 (Gemini)

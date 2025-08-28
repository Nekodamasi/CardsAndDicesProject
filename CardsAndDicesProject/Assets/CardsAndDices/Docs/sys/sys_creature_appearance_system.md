# sys_creature_appearance_system.md - クリーチャー外観システム設計書

---

## 概要

本ドキュメントは、クリーチャーカードの視覚的な外観を動的に構成・制御するための「クリーチャー外観システム」の設計を定義する。
このシステムは、ScriptableObjectベースのデータ駆動設計を採用し、キャラクタークリエイトのような柔軟な外観の組み合わせを実現することを目的とする。

---

## データ構造

システムのデータは、主に `PartId` enum と `AppearanceProfile` ScriptableObject によって定義される。

### 1. PartId (enum)

クリーチャーの外観を構成するパーツの種類を識別するためのID。

-   **ファイル名**: `PartId.cs`
-   **パス**: `Assets/CardsAndDices/Scripts/Core`
-   **責務**: 外観パーツの種類を静的に定義する。
-   **定義例**:
    ```csharp
    public enum PartId
    {
        Body,
        Hair,
        Face,
        Armor,
        Weapon
    }
    ```

### 2. AppearanceProfile (ScriptableObject)

クリーチャーの「見た目」一式を定義するデータアセット。

-   **ファイル名**: `AppearanceProfile.cs`
-   **パス**: `Assets/CardsAndDices/Scripts/Data`
-   **責務**: `PartId` とそれに対応する `Sprite` を紐づけ、一つの完全な外観セットを定義する。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `PartSprites` | `List<PartSprite>` | パーツIDとスプライトのペアのリスト |

#### 2.1. PartSprite

`AppearanceProfile`内で使用される、`PartId` と `Sprite` を紐づけるためのシリアライズ可能なクラス。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `PartId` | `PartId` (enum) | パーツの識別ID |
| `Sprite` | `Sprite` | そのパーツに設定するスプライト |

### 3. CardInitializationDataの拡張

カード生成時に外観情報を渡すため、`CardInitializationData` に `AppearanceProfile` への参照を追加する。

-   **対象ファイル**: `CardInitializationData.cs`
-   **変更内容**: `public AppearanceProfile Appearance;` フィールドを追加する。

---

## コンポーネント (MonoBehaviour)

### 1. CreatureAppearanceController

`AppearanceProfile` に基づき、実際にゲームオブジェクトの表示を制御するコンポーネント。

-   **ファイル名**: `CreatureAppearanceController.cs`
-   **パス**: `Assets/CardsAndDices/Scripts/UI`
-   **アタッチ先**: `CreatureCard` Prefab内の、外観を管理するGameObject。
-   **責務**: `AppearanceProfile` のデータを受け取り、管理下にある各パーツの `SpriteRenderer` の表示/非表示と `Sprite` の割り当てを行う。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `_parts` | `List<AppearancePart>` | Inspector上で設定する、管理対象のパーツリスト |

-   **メソッド**:
    -   `UpdateAppearance(AppearanceProfile profile)`
        -   **入力**: `AppearanceProfile`
        -   **処理**: `_parts` リストをループし、各パーツの `PartId` に対応する `Sprite` を `profile` から検索する。`Sprite` が見つかれば `SpriteRenderer` に設定して表示し、見つからなければ `GameObject` を非表示にする。

#### 1.1. AppearancePart

`CreatureAppearanceController` 内で使用される、`PartId` と `SpriteRenderer` を紐づけるためのシリアライズ可能なクラス。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `PartId` | `PartId` (enum) | パーツの識別ID |
| `Renderer` | `SpriteRenderer` | 対応する `SpriteRenderer` コンポーネントへの参照 |

---

## Prefab構成

`CreatureCard` Prefabは以下のような階層構造を持つ。

-   `CreatureCard` (Prefab)
    -   ... (既存のコンポーネント)
    -   `Visuals` (GameObject)
        -   `CreatureAppearance` (GameObject)
            -   `CreatureAppearanceController.cs` (Script)
            -   `Body` (SpriteRenderer, AppearancePartで `PartId.Body` に紐づけ)
            -   `Hair` (SpriteRenderer, AppearancePartで `PartId.Hair` に紐づけ)
            -   `Face` (SpriteRenderer, AppearancePartで `PartId.Face` に紐づけ)
            -   ... (他のパーツも同様)

---

## データフロー

1.  `CardLifecycleService` が `CardInitializationData` を使ってカードを初期化する。
2.  `CardInitializationData` に含まれる `AppearanceProfile` が `CreatureCardView` に渡される。
3.  `CreatureCardView` は、自身の子要素である `CreatureAppearanceController` の `UpdateAppearance` メソッドを呼び出し、`AppearanceProfile` を渡す。
4.  `CreatureAppearanceController` は、受け取った `AppearanceProfile` に基づいて、管理下の各 `SpriteRenderer` の表示を更新する。

---

## 関連ファイル

- [sys_creature_card_lifecycle_design.md](./sys_creature_card_lifecycle_design.md)
- [component_creature_card_prefab.md](../component/component_creature_card_prefab.md)

---

## 更新履歴

- 2025-08-27: 初版 (Gemini)

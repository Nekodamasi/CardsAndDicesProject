# sys_domain-model.md - ドメインモデル設計書

---

## 概要

このドキュメントは、「CardsAndDices」プロジェクトにおける主要な概念とデータ構造、すなわち**ドメインモデル**を定義します。
ここで定義されるオブジェクトやシステムは、ゲームロジックの根幹をなすものです。

---

## CompositeObjectId: オブジェクト識別システム

### 目的

全てのインタラクティブなゲームオブジェクト（カード、スロットなど）を一意に識別するために使用します。
これは、コマンドやデータモデルにおいて、特定のオブジェクトを参照するための主要な手段です。

### 構造

-   **`UniqueId` (long):**
    -   実行時に採番される、オブジェクト固有のユニークID。
-   **`ObjectType` (string):**
    -   オブジェクトの種類を示す文字列です (例: "Card", "Slot")。これにより、IDだけでオブジェクトの種類を判別できます。
-   **`Owner` (CompositeObjectId):**
    -   階層構造を表現するための、親オブジェクトのIDです。ルートオブジェクトの場合は `null` となります。

### 管理

-   **`IdentifiableGameObject`:**
    -   `CompositeObjectId` を保持する `MonoBehaviour` コンポーネント。全てのインタラクティブなPrefabにアタッチされます。
-   **`CompositeObjectIdManager`:**
    -   全ての `CompositeObjectId` の生成と追跡を担当するシングルトンクラスです。

---

## CardSlot System: カードスロット管理システム

### 状態管理 (Model)

-   **`CardSlotData`:**
    -   カードスロットの状態を保持するデータクラス（Model）です。
    -   どのカードが配置されているか (`PlacedCardId`)、スロットの場所 (`LinePosition`, `SlotLocation`) といった、ゲームロジックに必要な情報のみを保持します。

-   **`CardSlotManager`:**
    -   全てのスロットの状態を一元管理するシングルトンクラス（Controller）です。
    -   どのスロットにどのカードがあるかという「信頼できる唯一の情報源」として機能し、カードの配置、リフローといったロジックを実行します。

### 場所の定義

スロットの戦略的な場所は、2つのenumによって階層的に定義されます。

-   **`LinePosition`:**
    -   スロットが存在する広域なエリアを示します (例: `TopLine`, `BottomLine`, `Hand`)。
-   **`SlotLocation`:**
    -   そのライン内での具体的な役割や位置を示します (例: `Vanguard`, `Center`, `Hand0`)。

### ViewとDataの役割分担

-   **`CardSlotView`:**
    -   スロットの視覚表現と、ユーザーからのドロップイベントの検知のみを担当します。
    -   状態は持たず、ロジックの処理は即座に `CardSlotManager` に委任します。

---

## 関連ファイル

-   [guide_overview.md](../guide/guide_overview.md)
-   [guide_design-principles.md](../guide/guide_design-principles.md)

---

## 更新履歴

-   2025-07-21: `gemini.md` から分離して初版作成 (Gemini - Technical Writer for Game Development)

# guide_overview.md - プロジェクト全体概要と開発の手引き

---

## 概要

このドキュメントは、「CardsAndDices」プロジェクトに関わる全ての開発者とAIアシスタントが、**最初に読むべき出発点**です。
**このドキュメントに記載されている内容は、プロジェクト開発における最上位の指針であり、常に厳守されなければなりません。**

プロジェクトの核心的な思想、ドキュメント全体の構成、そして開発を進める上での基本的なワークフローを定義します。このガイドに従うことで、設計と実装の一貫性を保ち、開発を円滑に進めることを目的とします。

---

## プロジェクトの核心思想

このプロジェクトは、以下の3つの思想を柱としています。機能の追加や変更を行う際は、常にこれらの思想を念頭に置いてください。
**各原則の具体的な実装方法やルールについては、[guide_design-principles.md](./guide_design-principles.md) を参照してください。**

1.  **関心の分離 (Separation of Concerns):**
    -   Model (データ), View (表示), Controller (ロジック) の役割を明確に分離します。

2.  **データ駆動 (Data-Driven):**
    -   ゲームの状態や振る舞いは、UIの見た目ではなく、純粋なデータクラス（Model）によって駆動されます。

3.  **イベント駆動 (Event-Driven):**
    -   コンポーネント間のやり取りは、疎結合を保つために Command Bus を介したイベントとして伝達されます。

---

## ドキュメントの歩き方

プロジェクトのドキュメントは、目的別に以下のディレクトリに整理されています。

-   `Docs/gdd/` (**Game Design Document**):
    -   **目的:** 機能の「何を」「なぜ」作るかを定義します。仕様、要件、設計思想がまとめられています。
    -   **使い方:** 新しい機能に着手する際は、まずここにある関連ドキュメントを読んでください。
    -   **特に重要なドキュメント:**
        -   [gdd_combat_system.md](../gdd/gdd_combat_system.md): 戦闘システムのコンセプトや概要。

-   `Docs/guide/` (**Guide**):
    -   **目的:** 「どのように」作るかを定義します。コーディング規約、ファイル命名規則、そしてこの概要ドキュメントが含まれます。
    -   **使い方:** 実装やドキュメント作成の際は、常にここのガイドラインに従ってください。

-   `Docs/sys/` (**System Design**):
    -   **目的:** 複雑なシステムのクラス構造やシーケンス図など、技術的な詳細を定義します。
    -   **使い方:** 特定のシステムの詳細な実装を理解する必要がある場合に参照してください。

-   `guide_project_files.md` (`Docs/guide/`内):
    -   **目的:** プロジェクト内の主要な全ファイル（`.md`, `.cs`）の一覧です。プロジェクト全体の構造を把握するための索引として機能します。
    -   **使い方:** 新しいファイルを作成した際は、**必ずこの一覧に追加する必要があります。** 更新方法はファイル内に記述されたガイドに従ってください。

-   `sys_domain-model.md` (`Docs/sys/`内):
    -   **目的:** `CompositeObjectId` や `CardSlot` システムなど、ゲームの核となるデータ構造（ドメインモデル）の設計を定義します。
    -   **使い方:** ゲームの基本的なデータ構造や、それらを管理するシステムの仕組みを理解したい場合に参照してください。

---

## AIアシスタントの役割と振る舞い

AIアシスタントは、このプロジェクトにおいて「**ゲーム開発技術ライター (Technical Writer for Game Development)**」として振る舞います。

### 基本的な振る舞い:

1.  **常にこの `guide_overview.md` を最初に読み、プロジェクトの全体像を把握します。**
2.  タスクに着手する前に、関連する `gdd` ドキュメントを読み、仕様を理解します。
3.  コードを生成・修正する際は、`guide_unity-cs.md` のコーディング規約を厳守します。
4.  ドキュメントを生成・修正する際は、`guide_rules.md` と `guide_files.md` に従います。
5.  いきなり実装するのではなく、設計案を提示し、ユーザーの承認を得てから作業を進めます。

### 基本的なプロンプト例:

-   **新しいクラスを作成する場合:**
    ```
    「guide_overview.md」と「gdd_xxxx.md」を参考に、「guide_unity-cs.md」の規約に従って、〇〇クラスを作成してください。
    ```

-   **ドキュメントを更新する場合:**
    ```
    「guide_rules.md」と「guide_files.md」に従って、〇〇.mdを更新してください。
    ```

---

## ドキュメントの基本ルール

**全てのドキュメントを新規作成・修正する際は、以下のガイドに従ってください。**

-   **[guide_rules.md](./guide_rules.md):** ドキュメントの書式、スタイル、言葉遣いに関するルール。
-   **[guide_files.md](./guide_files.md):** ファイルの命名規則と管理方法に関するルール。

---

## コーディングスタイルと規約

**常に[guide_unity-cs.md](./guide_unity-cs.md) を参照してください。**
主要なポイントは以下の通りです。

-   **Namespace:** 全てのコードは `CardsAndDice` namespace内に配置します。
-   **コメント:** 全てのpublicメンバーに `<summary>` XMLコメントを使用します。複雑なロジックには内部コメントを追加します。
-   **命名規則:**
    -   クラス: `PascalCase`
    -   Interface: `IPascalCase`
    -   Private field: `_camelCase`
-   **ファイル構造:** 定義済みのフォルダ構造 (`/Scripts/Data`, `/Scripts/UI`, `/Scripts/Systems` など) に従います。

---

## 関連ファイル

-   [guide_design-principles.md](./guide_design-principles.md)
-   [guide_project_files.md](./guide_project_files.md)
-   [sys_domain-model.md](../sys/sys_domain-model.md)
-   [guide_rules.md](./guide_rules.md)
-   [guide_files.md](./guide_files.md)
-   [guide_unity-cs.md](./guide_unity-cs.md)
-   [gdd_combat_system.md](../gdd/gdd_combat_system.md)

---

## 更新履歴

-   2025-07-21: 初版作成 (Gemini - Technical Writer for Game Development)
-   2025-07-21: 各セクションを規約に準拠させ、内容を整理 (Gemini - Technical Writer for Game Development)
-   2025-07-21: `gemini.md` への言及を削除し、AIに関する記述を一般化 (Gemini - Technical Writer for Game Development)
-   2025-07-21: `sys_domain-model.md` への言及を追加 (Gemini - Technical Writer for Game Development)
-   2025-07-21: ドキュメント冒頭に厳守する旨を追記 (Gemini - Technical Writer for Game Development)
-   2025-07-21: `gdd_combat_system.md` への言及を追加 (Gemini - Technical Writer for Game Development)

# guide_ui_interaction_design.md - UIインタラクション設計書作成ガイド

---

## 概要

このドキュメントは、プロジェクトにおけるUIインタラクション設計書を作成するためのガイドラインを提供します。
各UI要素（例: カード、スロット、ボタンなど）のインタラクションに関する仕様を、一貫したフォーマットで記述することを目的とします。

---

## アーキテクチャ概要：インタラクション設計の基本方針

本プロジェクトのUIインタラクションは、単一のコンポーネントがすべてのロジックを抱えるのではなく、複数のクラスが責務を分担するアーキテクチャを採用しています。設計書を作成する際は、必ず以下のクラス群との連携を前提としてください。

-   **`UIInteractionOrchestrator` (司令塔)**: UIインタラクション全体の司令塔です。UIイベント（コマンド）を受け取り、現在の状態を管理し、他のクラスに処理を委譲します。
-   **`CardInteractionStrategy` (判断戦略)**: 「この状況で、この操作は可能か？」という判断ロジックをカプセル化します。`Orchestrator` は、具体的な処理を行う前に必ずこの `Strategy` に問い合わせます。
-   **`UIActivationPolicy` (有効化ポリシー)**: `UIStateMachine` の状態に基づき、どのUI要素がインタラクション可能（Acceptable）で、どれが不可能（Inactive）かを決定するポリシーです。
-   **`UIStateMachine` (状態機械)**: UI全体の現在の状態（例: `Idle`, `DraggingCard`）を保持します。

したがって、個々のUI要素の設計書では、「`UIStateMachine` の状態が `Idle` ならばホバー可能」といったロジックを記述するのではなく、「`Orchestrator` が `Strategy` に問い合わせ、許可された場合にホバー処理を実行する」というように、**クラス間の連携**として記述する必要があります。

---

## ドキュメントの構成

UIインタラクション設計書は、以下のセクションで構成されます。

### 1. 最初の見出し

-   `# [ファイル名(.md)] - [UI要素名]インタラクション設計書`

### 2. 概要

-   ドキュメントの目的とスコープを簡潔に記述します。

### 3. 共通の前提・制約

-   このUI要素に共通する実装上の前提条件を記述します。（例: `BaseSpriteView` の継承、`IdentifiableGameObject` のアタッチなど）

### 4. 主要コンポーネントと関連クラス

-   このUI要素のインタラクションに直接関わるコンポーネント、コマンド、アニメーションSO、そして連携する主要な外部クラス（`Orchestrator`, `Strategy`等）をリストアップします。

### 5. インタラクションフロー

各インタラクションについて、以下の形式で詳細を記述します。

#### 5.1. [インタラクション名] (例: ホバー)

-   **トリガー**: `SpriteInputHandler` が `OnPointerEnter` を検知。
-   **発行コマンド**: `SpriteHoverCommand`

##### 5.1.1. **`UIInteractionOrchestrator` の処理**

1.  `OnHover` メソッドで `SpriteHoverCommand` を受信します。
2.  `CardInteractionStrategy.ChkCardHover` を呼び出し、現在の状況でホバー処理が可能か問い合わせます。
3.  `Strategy` から `true` が返された場合、ホバー対象の `CreatureCardView` を `ViewRegistry` から取得し、`EnterHoveringState()` メソッドを呼び出してホバー状態への遷移を指示します。

##### 5.1.2. **`CreatureCardView` の処理**

-   `EnterHoveringState()` が呼び出された際の処理を記述します。
    -   自身の状態を `Hover` に更新します。
    -   `_hoverAnimation` ScriptableObject を使用して、拡大・発光などのアニメーションを実行します。
    -   ホバーSEを再生します。

### 6. 考慮事項と補足

-   インタラクション全体に関する追加の考慮事項や補足情報を記述します。
    -   **例**: このインタラクションは、`UIActivationPolicy` によって `Acceptable` 状態に設定されている場合にのみ発生します。

### 7. 関連ファイル

-   このドキュメントに関連する他のMarkdownドキュメントへのリンクを相対パスで記述します。

### 8. 更新履歴

-   `YYYY-MM-DD: [変更内容の要約] (変更者名またはAI)` の形式で記述します。

---

## 関連ファイル

- [guide_rules.md](./guide_rules.md)
- [guide_files.md](./guide_files.md)
- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [gdd_reflow_system.md](../gdd/gdd_reflow_system.md)

---

## 更新履歴

- 2025-08-04: `UIInteractionOrchestrator`, `CardInteractionStrategy`, `UIActivationPolicy` を中心とした現在の設計思想を反映し、ガイドラインを更新 (Gemini - Codebase Analyst)
- 2025-07-25: 初版作成 (Gemini - Technical Writer for Game Development)

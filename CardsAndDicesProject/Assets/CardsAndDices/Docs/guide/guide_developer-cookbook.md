# guide_developer-cookbook.md - 開発者クックブック

---

## 概要

このドキュメントは、開発者が特定のタスクを実行するための、実践的な手順をまとめた「レシピ集」です。
新しい機能を追加したり、既存の機能を変更したりする際は、ここで示されるワークフローに従ってください。

---

## レシピ1: 新しいインタラクティブUI要素の追加

### 手順

1.  **Prefabの作成:**
    -   `GameObject` の階層を構築します。

2.  **コアコンポーネントの追加:**
    -   `IdentifiableGameObject`
    -   `SpriteInputHandler`
    -   `BaseSpriteView` から継承した新しいViewスクリプト (例: `MyNewElementView.cs`)

3.  **Viewスクリプトの実装:**
    -   必要な `On...` コマンドハンドラ (例: `OnHover`, `OnClick`) をオーバーライドします。
    -   Inspectorで、対応するアニメーション `ScriptableObject` アセットを割り当てます。

4.  **Managerの更新 (必要な場合):**
    -   もし新しい要素が複雑な状態（例: オン/オフ、カウントなど）を持つ場合は、関連するManagerクラス (例: `CardSlotManager`) を更新して、その状態管理とロジックを追加します。

---

## レシピ2: 新しいカード効果の実装

### 手順

1.  **効果ロジックの定義:**
    -   効果のロジックは、必ず `View` ではなく、`System` や `Manager` クラス内に配置します。

2.  **効果のトリガー:**
    -   Command Bus を使用します。例えば、カードの `OnClick` ハンドラが、効果の詳細（例: 対象、ダメージ量）を含む新しいコマンド (`ExecuteEffectCommand` など) を発行します。

3.  **ロジックの実装:**
    -   `EffectManager` のようなシステムが、その新しいコマンドを購読します。
    -   コマンドを受け取った `EffectManager` は、関連する `CardData` (Model) を取得し、効果を実行し、ゲーム状態を更新します。
    -   最後に、必要に応じて `View` にアニメーションの再生などを指示します。

---

## 関連ファイル

-   [guide_overview.md](./guide_overview.md)
-   [guide_design-principles.md](./guide_design-principles.md)
-   [sys_domain-model.md](../sys/sys_domain-model.md)

---

## 更新履歴

-   2025-07-21: `gemini.md` から分離して初版作成 (Gemini - Technical Writer for Game Development)

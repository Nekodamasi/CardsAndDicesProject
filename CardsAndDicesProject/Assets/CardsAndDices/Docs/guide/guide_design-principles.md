# guide_design-principles.md - 設計原則ガイド

---

## 概要

このドキュメントは、「CardsAndDices」プロジェクトにおけるソフトウェア設計の核心的な原則を詳述します。
全てのコードは、ここで定義される原則に基づいて設計・実装される必要があります。これにより、プロジェクト全体の保守性、拡張性、および品質を維持します。

---

## 原則1: 関心の分離 (Separation of Concerns) - MVCパターン

当プロジェクトでは、**Model-View-Controller (MVC)** パターンを厳格に適用し、責務を明確に分離します。

### 1. Model (モデル)

-   **役割:** ゲームの状態（データ）を保持します。アプリケーションの「何を」表す部分です。
-   **配置場所:** `/Scripts/Data`
-   **実装ルール:**
    -   `MonoBehaviour` を継承しない、純粋なC#クラス（POCO）でなければなりません。
    -   ゲームロジックや、Viewに関するいかなる参照も持ってはいけません。
    -   **例:** `CardSlotData` は、どのカードが配置されているかというID情報を持ちますが、そのカードの `GameObject` や `SpriteRenderer` を直接参照しません。

### 2. View (ビュー)

-   **役割:** Modelのデータを視覚的に表現し、ユーザーからの入力を受け取ります。アプリケーションの「見た目」の部分です。
-   **配置場所:** `/Scripts/UI`
-   **実装ルール:**
    -   `MonoBehaviour` を継承し、`SpriteRenderer` や `Button` などのUnityコンポーネントへの参照を持ちます。
    -   自身の状態を持つべきではありません。表示は、常にControllerから渡されたModelのデータに基づいている必要があります。
    -   複雑なゲームロジックを含んではいけません。入力イベントを受け取ったら、それを解釈せずにController（またはCommand Bus）に通知する責務のみを持ちます。
    -   **例:** `CardSlotView` は、`CardSlotManager` からの指示を受けてスロットをハイライト表示しますが、カードが配置可能かどうかを自身で判断しません。

### 3. Controller (コントローラー)

-   **役割:** Viewからの入力を受け取り、Modelを更新し、その結果をViewに反映させる、アプリケーションの「振る舞い」を司る部分です。
-   **配置場所:** `/Scripts/Systems`
-   **実装ルール:**
    -   `CardSlotManager` のような、特定のドメインロジックを管理するManagerクラスとして実装されます。
    -   Modelのインスタンスを所有・管理し、ゲームのルールに基づいてそれを変更する唯一の場所です。
    -   Viewを直接操作することもありますが、基本的にはModelの変更を通じてViewの更新を間接的に引き起こします。

---

## 原則2: データ駆動 (Data-Driven)

ゲームの全ての状態変化は、UIの操作から直接引き起こされるのではなく、必ずModelデータの変更によって駆動されなければなりません。

-   **正しいフロー:**
    1.  `View` がユーザー入力を検知する。
    2.  `View` が `Controller` にイベントを通知する。
    3.  `Controller` がゲームルールに基づき、`Model` のデータを更新する。
    4.  `Controller`（または `Model` の変更を検知した別のシステム）が、`View` に更新を指示する。
    5.  `View` が新しい `Model` の状態に基づいて自身の見た目を更新する。

-   **アンチパターン（禁止）:**
    -   `Button` の `OnClick` イベントから、他のViewコンポーネントの表示を直接書き換える。
    -   `View` が自身のローカル変数で「選択中」などの状態を管理する。

---

## 原則3: イベント駆動 (Event-Driven) - Command Busパターン

コンポーネント間のやり取りは、直接的なメソッド呼び出しを避け、**Command Bus** を介したイベント（コマンド）の発行・購読によって行います。これにより、各コンポーネントは疎結合に保たれます。

-   **`ICommand`:** プロジェクト内の全てのコマンドが実装するインターフェース。
-   **`SpriteCommandBus`:** `ScriptableObject` として実装された中央イベントハブ。
-   **発行者 (`SpriteInputHandler`など):** ユーザー入力などを検知し、`TargetObjectId` などの関連データを含んだコマンドオブジェクトを生成して `SpriteCommandBus.Emit()` を呼び出します。
-   **購読者 (`BaseSpriteView`, Managerクラスなど):** `SpriteCommandBus.On<T>()` で特定のコマンドを購読し、コールバック関数内で処理を実行します。

このパターンにより、例えば「カードがクリックされた」というイベントに対して、UI（効果音の再生）、ゲームロジック（カードの選択状態変更）、分析システム（クリックログの送信）といった複数の独立したシステムが、互いを意識することなく反応できます。

> **アーキテクチャ上の注記:**
> このプロジェクトでは、インターフェース名を `ICommand` としていますが、その実装と利用方法は、伝統的な「コマンドパターン」（オブジェクト自身が実行ロジックを持つ）とは異なります。
>
> ここでの `ICommand` は、実際にはシステム間で状態やイベントを伝達するための**イベントメッセージ (Event Message)** として機能します。そして、`SpriteCommandBus` は、これらのメッセージを中継する**イベントバス (Event Bus)** の役割を担っています。
>
> したがって、このプロジェクトのアーキテクチャは、実質的に**イベント駆動アーキテクチャ (Event-Driven Architecture)** として構築されています。新しい機能を追加する際も、この設計思想に従ってください。

---

## 原則4: ScriptableObject の参照と管理

ScriptableObject は、設定データやシステムの中央管理ポイントとして強力ですが、その参照方法には一貫したルールを設けます。

### 1. `Resources` フォルダからのロードの禁止

-   **理由:**
    -   `Resources.Load` は、ビルド時に全ての `Resources` フォルダ内のアセットをバンドルするため、アプリケーションの起動時間やメモリ使用量に影響を与える可能性があります。
    -   コードからアセットへの参照が隠蔽され、どの ScriptableObject がどこで使われているか把握しにくくなります。
    -   アセットの依存関係が不明瞭になり、リファクタリングや削除が困難になります。
-   **代替案:**
    -   **インスペクターでの直接参照:** `MonoBehaviour` や他の ScriptableObject から、`[SerializeField]` を用いて ScriptableObject アセットを直接インスペクターに設定します。これにより、参照関係が明確になり、Unity エディタ上での管理が容易になります。
    -   **DI (Dependency Injection) ツール:** VContainer のような DI ツールを導入し、依存関係を自動的に解決・注入します。これにより、コードの疎結合性が高まり、テスト容易性も向上します。

### 2. `Instance` プロパティによるシングルトン参照の制限

-   **理由:**
    -   `Instance` プロパティによるシングルトンパターンは、グローバルなアクセスポイントを提供するため便利ですが、依存関係が隠蔽され、テストが困難になる傾向があります。
    -   特に ScriptableObject の場合、複数のインスタンスが存在する可能性があり、意図しない挙動を引き起こすことがあります。
-   **代替案:**
    -   **インスペクターでの直接参照:** 上記と同様に、必要な ScriptableObject をインスペクターで直接参照します。
    -   **DI (Dependency Injection) ツール:** DI ツールを使用して、必要な場所で ScriptableObject のインスタンスを注入します。これにより、依存関係が明示的になり、単一責任の原則を維持しやすくなります。

### 3. モックアップ段階での運用と将来的な DI ツールへの移行

-   **現状:** モックアップ段階では、一部の ScriptableObject (例: `UIStateMachine`, `SpriteCommandBus`) は、便宜上 `Instance` プロパティによるシングルトンアクセスを提供している場合があります。これは開発の迅速化を目的とした一時的な措置です。
-   **将来的な方針:** 正式な開発段階では、`Instance` プロパティによるアクセスを廃止し、VContainer などの DI ツールを導入して、全ての ScriptableObject の依存関係を管理します。これにより、より堅牢で保守性の高いコードベースを構築します。

---

## 関連ファイル

-   [guide_overview.md](./guide_overview.md)
-   [guide_unity-cs.md](./guide_unity-cs.md)

---

## 更新履歴

-   2025-07-21: 初版作成 (Gemini - Technical Writer for Game Development)
-   2025-07-21: 各セクションを規約に準拠させ、内容を整理 (Gemini - Technical Writer for Game Development)
-   2025-07-21: `gemini.md` への言及を削除し、AIに関する記述を一般化 (Gemini - Technical Writer for Game Development)
-   2025-07-21: `sys_domain-model.md` への言及を追加 (Gemini - Technical Writer for Game Development)
-   2025-07-21: ドキュメント冒頭に厳守する旨を追記 (Gemini - Technical Writer for Game Development)
-   2025-07-21: `gdd_combat_system.md` への言及を追加 (Gemini - Technical Writer for Game Development)
-   2025-07-25: ScriptableObjectの参照と管理に関する原則を追加 (Gemini - Technical Writer for Game Development)

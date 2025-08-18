<!-- markdownlint-disable MD024 -->
# sys_status_icon_design.md - ステータスアイコン設計書

---

## 概要

このドキュメントは、ゲーム内のクリーチャーカードに表示されるステータスアイコンの設計を定義します。
ステータスアイコンは、アイコン画像と2桁の数値で構成され、ステータス変更時にはShakeアニメーションを実行し、更新タイミングを制御することで視覚的な一貫性を保ちます。

---

## 主要コンポーネント

### 1. `StatusIconView` (MonoBehaviour)

-   **責務**: ステータスアイコンの視覚的な表示とアニメーションの実行。インスペクターで設定された `StatusIconData` に基づいて自身の表示を初期化する。
-   **配置**: `Assets/CardsAndDices/Scripts/UI/StatusIconView.cs`

### 2. `StatusIconData` (ScriptableObject)

-   **責務**: ステータスアイコンの基本情報（アイコンID、表示形式など）のマスターデータ。
-   **配置**: `Assets/CardsAndDices/Scripts/Data/StatusIconData.cs`

### 3. `StatusIconPresenter` (純粋なC#クラス)

-   **責務**: `ICreature` (Model) と `StatusIconView` (View) の間の仲介役。ステータス値の変更イベントを購読し、外部からの指示でViewの表示を更新するタイミングを制御する。
-   **配置**: `Assets/CardsAndDices/Scripts/Presenter/StatusIconPresenter.cs`

---

## `StatusIconView` の仕様

### 1. 責務

-   `StatusIconData` に基づいてアイコン画像と数値の表示を初期化・更新する。
-   ステータス値が変更された際にShakeアニメーションを実行する。

### 2. フィールド

-   `SpriteSelector _faceSpriteSelector`: アイコン画像を表示するための `SpriteSelector`。
-   `TextMeshProUGUI _valueText`: 数値を表示するための `TextMeshProUGUI`。
-   `StatusIconData _statusIconData`: このアイコンが表すステータスの定義データ。インスペクターから設定される。
-   `StatusIconData StatusIconData { get; }`: `_statusIconData` を外部に公開するためのプロパティ。

### 3. 主なメソッド

-   `void Initialize()`: `Awake()` から呼び出され、`_statusIconData` を基に `_faceSpriteSelector` のスプライトを設定し、`_valueText` の表示/非表示を切り替える。
-   `void UpdateDisplay(int value)`:
    -   表示する数値を更新する。
    -   もし表示される数値が前回の値から変更された場合、`PlayShakeAnimation()` を呼び出す。
-   `void PlayShakeAnimation()`:
    -   DOTweenを使用して、アイコンがShakeするアニメーションを実行する。

---

## `StatusIconData` の仕様

### 1. 責務

-   特定のステータスアイコンに関する静的な情報を提供します。

### 2. フィールド

-   `string IconSpriteId`: `SpriteSelector` で使用される、ステータスアイコンのスプライトID。
-   `string FormatString`: 数値の表示形式を定義する文字列（例: `"{0}"`）。`{0}` が数値に置き換えられます。
-   `bool ShowValue`: 数値を表示するかどうかを制御するフラグ。
-   `EffectTargetType TargetType`: このアイコンがどの `EffectTargetType` (例: `Health`, `Attack`) に対応するかを示す。

### 3. アセットメニュー

-   `[CreateAssetMenu(fileName = "NewStatusIconData", menuName = "CardsAndDices/Status Icon Data")]`

---

## `StatusIconPresenter` の仕様

### 1. 責務

-   `ICreature` インスタンスのステータス変更コマンドを購読する。
-   「更新のタイミングは任意で、元データを即座に反映しない」という要件を制御する。
-   外部からの要求に応じて、`StatusIconView` に表示更新とアニメーション実行を指示する。

### 2. 依存関係とコンストラクタ

-   `public StatusIconPresenter(ICreature creature, StatusIconView view, SpriteCommandBus commandBus)`
-   `ICreature`: 監視対象のクリーチャーインスタンス。
-   `StatusIconView`: 更新対象のView。`StatusIconData` はこのViewから取得する。
-   `SpriteCommandBus`: ステータス変更イベントを購読するため。

### 3. 主なメソッド

-   `void SubscribeToEvents()`:
    -   コンストラクタ内で呼び出される。
    -   `_view.StatusIconData.TargetType` に応じて、`SpriteCommandBus` の適切なステータス変更コマンド（`CreatureHealthChangedCommand` など）を購読する。
-   `void Handle...Change(Command cmd)`:
    -   各ステータス変更コマンドに対応するイベントハンドラ。
    -   コマンドの `TargetId` が自身の `_creature.Id` と一致する場合、コマンドから新しいステータス値を取得し、内部変数 `_currentValue` に保持する。**この時点ではViewの更新は行わない。**
-   `void ForceUpdateDisplay()`:
    -   外部のコントローラー（例: `CreatureCardPresenter`）によって、任意のタイミングで呼び出されることを想定。
    -   `_creature` から最新のステータス値を直接取得し直し、`_view.UpdateDisplay()` を呼び出して、Viewの表示を更新させる。
-   `void Dispose()`:
    -   `UnsubscribeFromEvents()` を呼び出し、`SpriteCommandBus` へのイベント購読をすべて解除する。

---

## 全体フロー

1.  **配置と設定**: `CreatureCardView` プレハブの子要素として `StatusIconView` コンポーネントを持つGameObjectを配置する。インスペクターで、各 `StatusIconView` に対応する `StatusIconData` アセットを割り当てる。
2.  **Presenter生成**: `CreatureCardPresenter` が生成される際、自身の `CreatureCardView` が持つ `StatusIconView` のリストを取得し、そのそれぞれに対して `StatusIconPresenter` を生成・初期化する。
3.  **イベント購読**: `StatusIconPresenter` は、自身の `StatusIconData` に設定された `TargetType` に応じて、適切なステータス変更コマンドの購読を開始する。
4.  **ステータス変更と値の内部保持**: ゲームロジックにより `ICreature` のステータスが変更されると、対応するコマンドが `SpriteCommandBus` を介して発行される。`StatusIconPresenter` はこれを受信し、新しい値を内部の `_currentValue` 変数に保存する。
5.  **Viewの更新とアニメーション**:
    -   ゲームの特定のタイミング（例: ダメージ計算完了後、ターン終了時など）で、`CreatureCardPresenter` が管理下のすべての `StatusIconPresenter` の `ForceUpdateDisplay()` メソッドを呼び出す。
    -   `StatusIconPresenter` は、`ICreature` から最新のステータス値を取得し、`StatusIconView.UpdateDisplay()` を呼び出す。
    -   `StatusIconView` は、値が前回表示した値から変更されていれば、`PlayShakeAnimation()` を実行して視覚的なフィードバックを返す。

---

## 関連ファイル

-   [./sys_creature_management.md](./sys_creature_management.md)
-   [./sys_effect_management.md](./sys_effect_management.md)
-   [../gdd/gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)

---

## 更新履歴

-   2025-08-17: ソースコードの現状に合わせて、各コンポーネントの責務と全体フローを全面的に更新 (AI Document Specialist)
-   2025-08-15: 初版 (Gemini)
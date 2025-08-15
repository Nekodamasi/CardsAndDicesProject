# sys_status_icon_design.md - ステータスアイコン設計書

---

## 概要

このドキュメントは、ゲーム内のクリーチャーカードに表示されるステータスアイコンの設計を定義します。
ステータスアイコンは、アイコン画像と2桁の数値で構成され、ステータス変更時にはShakeアニメーションを実行し、更新タイミングを制御することで視覚的な一貫性を保ちます。

---

## 1. 主要コンポーネント

### 1.1. `StatusIconView` (MonoBehaviour)

-   **責務**: ステータスアイコンの視覚的な表示とアニメーションの実行。
-   **配置**: `Assets/CardsAndDices/Scripts/UI/StatusIconView.cs`

### 1.2. `StatusIconData` (ScriptableObject)

-   **責務**: ステータスアイコンの基本情報（アイコン画像、表示形式など）のマスターデータ。
-   **配置**: `Assets/CardsAndDices/Scripts/Data/StatusIconData.cs`

### 1.3. `StatusIconPresenter` (純粋なC#クラス)

-   **責務**: `ICreature` (またはステータスを持つModel) と `StatusIconView` の間の仲介役。ステータス値の変更を購読し、更新タイミングを制御してViewに指示を出す。
-   **配置**: `Assets/CardsAndDices/Scripts/Presenter/StatusIconPresenter.cs`

---

## 2. `StatusIconView` の仕様

### 2.1. 責務

-   アイコン画像と数値を表示する。
-   ステータス値が変更された際にShakeアニメーションを実行する。

### 2.2. フィールド

-   `SpriteRenderer _iconRenderer`: アイコン画像を表示するための `SpriteRenderer`。
-   `TextMeshProUGUI _valueText`: 2桁の数値を表示するための `TextMeshProUGUI`。
-   `StatusIconData _statusIconData`: このアイコンが表すステータスの定義データ。

### 2.3. 主なメソッド

-   `void Initialize(StatusIconData data)`: アイコンの初期設定を行います。`_iconRenderer.sprite` を設定し、`_valueText` の表示形式を準備します。
-   `void UpdateDisplay(int value)`:
    -   表示する数値を更新します。
    -   もし表示される数値が前回の値から変更された場合、`PlayShakeAnimation()` を呼び出します。
-   `void PlayShakeAnimation()`:
    -   DOTweenなどのTweeningライブラリを使用して、アイコンがShakeするアニメーションを実行します。
    -   アニメーションの完了後、アイコンを元の状態に戻します。

---

## 3. `StatusIconData` の仕様

### 3.1. 責務

-   特定のステータスアイコンに関する静的な情報を提供します。

### 3.2. フィールド

-   `Sprite IconSprite`: ステータスアイコンとして表示される画像。
-   `string FormatString`: 数値の表示形式を定義する文字列（例: `"HP: {0}"`）。`{0}` が数値に置き換えられます。
-   `bool ShowValue`: 数値を表示するかどうかを制御するフラグ。
-   `EffectTargetType TargetType`: このアイコンがどの `EffectTargetType` (例: `Health`, `Attack`) に対応するかを示す。

### 3.3. アセットメニュー

-   `[CreateAssetMenu(fileName = "NewStatusIconData", menuName = "CardsAndDices/Status Icon Data")]`

---

## 4. `StatusIconPresenter` の仕様

### 4.1. 責務

-   `ICreature` インスタンスのステータス変更を購読する。
-   「更新のタイミングは任意で、元データを即座に反映しない」という要件を制御する。
-   制御されたタイミングで `StatusIconView` に表示更新とアニメーション実行を指示する。

### 4.2. 依存関係

-   `ICreature`: 監視対象のクリーチャーインスタンス。
-   `StatusIconView`: 更新対象のView。
-   `StatusIconData`: どのステータスアイコンを扱うかを示すデータ。
-   `EffectManager`: エフェクトによるステータス変動を取得するため。
-   `SpriteCommandBus`: ステータス変更イベントを購読するため。

### 4.3. 主なメソッド

-   `void Initialize(ICreature creature, StatusIconView view, StatusIconData data)`: プレゼンターを初期化し、必要な参照を設定します。`StatusIconView.Initialize(data)` を呼び出します。
-   `void OnStatusChanged(StatusChangedCommand command)`:
    -   `SpriteCommandBus` から発行されるステータス変更イベントを購読します。
    -   イベントから対象のステータス値を取得し、内部に保持します。
    -   **更新タイミングの制御**:
        -   即座にViewを更新せず、タイマー、ターン終了イベント、または特定のゲームイベントをトリガーとして更新をキューイングします。
        -   例: `TurnEndCommand` を購読し、ターン終了時にまとめて `StatusIconView.UpdateDisplay()` を呼び出す。
-   `void ForceUpdateDisplay()`: 制御されたタイミングで、内部に保持している最新のステータス値で `StatusIconView.UpdateDisplay()` を呼び出します。

---

## 5. 全体フロー

1.  **ステータスアイコンの配置**: クリーチャーカードのプレハブに `StatusIconView` コンポーネントを配置し、インスペクターで `StatusIconData` を設定します。
2.  **プレゼンターの生成**: `CreatureManager` (または `CreatureCardPresenter` の内部) で、`ICreature` インスタンス、`StatusIconView`、`StatusIconData` を引数に `StatusIconPresenter` を生成し、初期化します。
3.  **ステータス変更の検知**: `ICreature` インスタンスのステータス（例: 体力）が変更されると、`SpriteCommandBus` を介して `StatusChangedCommand` (または `HealthChangedCommand` など) が発行されます。
4.  **更新の制御**: `StatusIconPresenter` がこのコマンドを購読し、変更されたステータス値を内部に保持します。この時点ではViewは更新されません。
5.  **Viewの更新とアニメーション**: 
    -   ゲームの特定のタイミング（例: ターン終了時、ダメージ計算完了後など）で、`StatusIconPresenter` が `ForceUpdateDisplay()` を呼び出します。
    -   `StatusIconPresenter` は、`ICreature` の現在のステータス値と `EffectManager` からの変動値を計算し、`StatusIconView.UpdateDisplay(calculatedValue)` を呼び出します。
    -   `StatusIconView` は、値が変更されたことを検知し、`PlayShakeAnimation()` を実行します。

---

## 関連ファイル

-   [gdd_sprite_ui_design.md](../../gdd/gdd_sprite_ui_design.md)
-   [sys_sprite_selector_design.md](../sys_sprite_selector_design.md)
-   [sys_creature_management.md](../sys_creature_management.md)
-   [sys_effect_management.md](../sys_effect_management.md)
-   [CreatureData.cs](../../Scripts/Domain/CreatureData.cs)
-   [EffectManager.cs](../../Scripts/Manager/EffectManager.cs)
-   [ICreature.cs](../../Scripts/Domain/ICreature.cs)
-   [CreatureCardView.cs](../../Scripts/UI/CreatureCardView.cs)
-   [SpriteCommandBus.cs](../../Scripts/UI/SpriteCommandBus.cs)
-   [SpriteSelector.cs](../../Scripts/UI/SpriteSelector.cs)

---

## 更新履歴

-   2025-08-15: 初版 (Gemini)

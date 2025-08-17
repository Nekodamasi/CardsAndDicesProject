# sys_creature_management.md - クリーチャー管理設計書

---

## 概要

このドキュメントは、ゲーム内のクリーチャーの実行時インスタンスの管理システムについて定義します。
クリーチャーのステータス変動、ライフサイクル、およびViewとの連携を管理するための主要なクラスとその責務を詳述します。

---

## 1. クリーチャー実行時インスタンス (`ICreature` / `Creature`)

ゲーム中に実際に存在するクリーチャーの論理的な表現であり、変動するステータス値を保持します。

### 1.1. 責務

-   `CreatureData` (マスターデータ) への参照を保持し、基本ステータスを取得する。
-   現在の体力、現在の攻撃力など、ゲーム中に変動するステータス値の保持。
-   `EffectManager` からエフェクトによるステータス変動量を取得し、現在の最大体力などを計算する。
-   ダメージを受ける、回復する、バフ/デバフが適用されるなどのロジック。
-   自身のステータス変更をイベントとして発行し、関連するシステムに通知する。

### 1.2. インターフェース (`ICreature`)

-   **役割**: クリーチャーの実行時インスタンスが実装すべき共通インターフェース。
-   **主なプロパティ**:
    -   `CompositeObjectId Id`: クリーチャーの一意な識別子。Viewから取得する。
    -   `int CurrentHealth`: 現在の体力値。
    -   `int BaseHealth`: エフェクトによる変動を考慮した現在の最大体力。
    -   `int Attack`: 現在の攻撃力。
    -   `int CurrentShield`: 現在のシールド値。
    -   `int BaseShield`: エフェクトによる変動を考慮した現在の最大シールド値。
    -   `int CurrentCooldown`: 現在のカウントダウン値。
    -   `int BaseCooldown`: エフェクトによる変動を考慮した現在の最大カウントダウン値。
    -   `int Energy`: 現在のエネルギー。
    -   
-   **主なメソッド**:
    -   `void TakeDamage(int amount)`: ダメージを受ける処理。
    -   `void ApplyEffect(EffectInstance effect)`: エフェクトを適用する処理。
    -   `void RemoveEffect(EffectInstance effect)`: エフェクトを解除する処理。

### 1.3. 実装クラス (`Creature`)

-   **役割**: `ICreature` インターフェースの具体的な実装。
-   **依存関係**: `CreatureData` (基本データ)、`EffectManager` (エフェクト管理)。これらはDIによって注入されます。
-   **ステータス計算**:
    -   `MaxHealth` は、`CreatureData.Health` (基本最大体力) に `EffectManager.GetTotalEffectValue(Id, EffectTargetType.Health)` (エフェクトによる体力変動量) を加算して動的に計算されます。
    -   `CurrentHealth` は、`TakeDamage` や回復処理によって直接変動します。`BaseHealth` を超えることがあります。
-   **イベント発行**: 体力変更などの重要なステータス変更時には、`SpriteCommandBus` を介してイベント（例: `HealthChangedCommand`）を発行し、Viewや他のシステムに通知します。

---

## 2. クリーチャーファクトリー (`CreatureFactory`)

`Creature` インスタンスの生成ロジックをカプセル化するクラスです。

### 2.1. 責務

-   `CreatureData` や必要な依存関係（`EffectManager` など）を受け取り、`ICreature` インスタンスを生成する。
-   生成ロジックを抽象化し、呼び出し元が `Creature` の具体的なコンストラクタを知る必要がないようにする。

### 2.2. 依存関係

-   `EffectManager`: 生成する `Creature` インスタンスに注入するため。

### 2.3. 主なメソッド

-   `ICreature Create(CompositeObjectId id, CreatureData baseData)`: 指定されたIDと基本データに基づいて新しい `ICreature` インスタンスを生成し、返します。

---

## 3. クリーチャーマネージャー (`CreatureManager`)

ゲーム内に存在するすべての `ICreature` インスタンスを一元的に管理するクラスです。

### 3.1. 責務

-   `ICreature` インスタンスのリストを保持し、管理する。
-   `CreatureFactory` を使用して `ICreature` インスタンスを生成する。
-   `ICreature` インスタンスに対する操作（ダメージ適用、エフェクト適用、状態更新など）を調整する。
-   `ICreature` インスタンスのライフサイクル（フィールドへの配置、死亡、除去など）を管理する。
-   `ICreature` インスタンスと `CreatureCardView` の間の `CreatureCardPresenter` を生成・管理し、両者の連携を確立する。

### 3.2. 依存関係

-   `CreatureFactory`: `Creature` インスタンスの生成のため。
-   `ViewRegistry`: 対応する `CreatureCardView` を取得するため。
-   `SpriteCommandBus`: イベントの購読や発行のため。

### 3.3. 主なメソッド

-   `ICreature SpawnCreature(CompositeObjectId id, CreatureData baseData, CompositeObjectId viewId)`: 新しいクリーチャーを生成し、管理リストに追加します。対応するViewが存在すれば、Presenterを生成して紐付けます。
-   `ICreature GetCreature(CompositeObjectId id)`: 指定されたIDのクリーチャーを取得します。
-   `void RemoveCreature(CompositeObjectId id)`: 指定されたIDのクリーチャーをゲームから除去します。

---

## 4. クリーチャーカードプレゼンター (`CreatureCardPresenter`)

`ICreature` (Model) と `CreatureCardView` (View) の間の仲介役です。

### 4.1. 責務

-   `ICreature` インスタンスのステータス変更イベントを購読する。
-   購読したイベントに基づいて `CreatureCardView` の表示を更新する。
-   Viewからのユーザー入力を `ICreature` インスタンスに伝達する（必要に応じて）。

### 4.2. 依存関係

-   `ICreature`: 監視対象のクリーチャーインスタンス。
-   `CreatureCardView`: 更新対象のView。
-   `SpriteCommandBus`: イベント購読のため。

---

## 5. 全体フロー

1.  **クリーチャーの生成**: `CreatureManager` が `CreatureFactory.Create()` を呼び出し、新しい `ICreature` インスタンスを生成します。
2.  **Viewとの紐付け**: `CreatureManager` は、対応する `CreatureCardView` を `ViewRegistry` から取得し、`ICreature` インスタンスと `CreatureCardView` を引数に `CreatureCardPresenter` を生成します。
3.  **ステータス変更**: `ICreature` インスタンスがダメージを受けるなどしてステータスが変更されると、`SpriteCommandBus` を介してイベントを発行します。
4.  **Viewの更新**: `CreatureCardPresenter` がこのイベントを購読し、`CreatureCardView` の `UpdateHealthDisplay()` などのメソッドを呼び出して表示を更新します。
5.  **エフェクトの適用**: `ICreature` インスタンスにエフェクトが適用されると、`EffectManager` に登録され、`MaxHealth` などの動的なステータス計算に影響を与えます。

---

## 関連ファイル

-   [gdd_combat_system.md](../../gdd/gdd_combat_system.md)
-   [sys_effect_management.md](../sys_effect_management.md)
-   [guide_design-principles.md](../../guide/guide_design-principles.md)
-   [CreatureData.cs](../../Scripts/Domain/CreatureData.cs)
-   [EffectManager.cs](../../Scripts/Manager/EffectManager.cs)
-   [CreatureCardView.cs](../../Scripts/UI/CreatureCardView.cs)
-   [SpriteCommandBus.cs](../../Scripts/UI/SpriteCommandBus.cs)
-   [ViewRegistry.cs](../../Scripts/Manager/ViewRegistry.cs)

---

## 更新履歴

-   2025-08-15: 初版 (Gemini)

# sys_dice_inlet_management.md - ダイスインレット管理設計書

---

## 概要

このドキュメントは、ゲーム内のダイスインレットの実行時インスタンスの管理システムについて定義します。
ダイスインレットの値やステータス変動、ライフサイクル、およびViewとの連携を管理するための主要なクラスとその責務を詳述します。

---

## データ定義

### 1. `InletAbilityProfile`

- **役割**: ダイスインレットの静的な識別情報と基本設定を保持します。このプロファイルは、`PlayerCardDataProvider` や `EnemyCardDataProvider` といった `ICardDataProvider` インターフェースを実装したクラスによって生成される `CardInitializationData` に含まれる形で、カードのライフサイクル初期に`DiceInletManager` へ登録されます。
- **主なフィールド**:
    - `DiceInletConditionSO Condition`: ダイス投入時の発動条件。
        - `int InitialCountdownValue`: インレットの初期カウントダウン値。
        - `int InitialUsageCount`: インレットの初期使用可能回数。
        - `UsageCountResetType`: 使用可能回数がリセットされるタイミング（TurnEnd, CooldownReset）
    - `BaseInletAbilitySO Ability`: インレットが発動する能力。

### 2. `DiceInletConditionSO` (ScriptableObject)

- **役割**: ダイス投入時の発動条件を定義します。
- **主なフィールド**:
    - `AllowedDiceFacesSO AllowedDiceFaces`: 投入許可されたダイスの目（１～６）。
- **主なメソッド**:
    - `bool CanAccept(DiceData diceData)`: 投入されたダイスの値が条件を満たすかチェックします。

### 3. `BaseInletAbilitySO` (ScriptableObject)

- **役割**: インレットが発動する「効果」を定義するすべてのScriptableObjectの基底クラス。
- **主なメソッド**:
    - `void ExecuteAbility(ICreature targetCreature, DiceData placedDice)`: インレットの能力を実行します。具体的な効果に応じたコマンドを `SpriteCommandBus` を介して発行します。

---

## ダイスインレット実行時インスタンス (`DiceInlet`)

ゲーム中に存在するダイスインレットの論理的な表現であり、`CurrentCountdownValue` などの変動する状態を保持します。

### 1. 責務

- `InletAbilityProfile` への参照を保持し、基本設定を取得する。
- `CurrentCountdownValue` を保有し、ダイス投入時に更新する。
- ダイスドラッグの条件チェックと、能力発動のトリガー。
- カウントダウン値の変更をイベントとして発行する。

### 2. 主なプロパティ

- `CompositeObjectId Id`: インレットの一意な識別子。
- `CompositeObjectId CardId`: インレットが所属するクリーチャーカードの一意な識別子。
- `int CurrentCountdownValue`: 現在のカウントダウン値。
- `Int CurrentUsageCount` 現在の使用可能回数

### 3. 主なメソッド

- `void OnDiceDropped(DiceData diceData, ICreature targetCreature)`: ダイスがインレットに投入された際の処理。投入された `diceData` に応じてカウントダウン値を減少させます。カウントダウン値が0以下になった場合、能力を発動し、カウントダウン値を初期カウントダウン値 (`InitialCountdownValue`) に戻します。
- `bool CanAccept(DiceData diceData)`:ダイスドラッグ時にインレットを活性化させるかをチェックします。`DiceInletConditionSO`の`bool CanAccept(DiceData diceData)`メソッドと`CurrentUsageCount`元に現在インレットにダイス投入が可能かチェックします。

---

## DiceInletFactory

`DiceInlet`インスタンスの生成ロジックに特化したFactoryクラス。

### 1. 責務

- `InletAbilityProfile` や必要な依存関係（`DiceInletManager` など）を受け取り、`DiceInlet` インスタンスを生成する。
- 生成ロジックを抽象化し、呼び出し元が `DiceInlet` の具体的なコンストラクタを知る必要がないようにする。

### 2. 依存関係

- `EffectManager`: 生成する `DiceInlet` インスタンスに注入するため。
- `AbilityManager`: 生成する `DiceInlet` インスタンスに注入するため。

### 3. 主なメソッド

- `DiceInlet Create(CompositeObjectId Inletid, CompositeObjectId Cardid,InletAbilityProfile profile)`: 提供されたIDと能力プロファイルを用いて、新しい`DiceInlet`インスタンスを生成し、初期化して返します。

---

## ダイスインレットマネージャー (`DiceInletManager`)

ゲーム内に存在するすべての `DiceInlet` インスタンスを一元的に管理するクラスです。

### 1. 責務

-   `DiceInletFactory` を介して `DiceInlet` インスタンスを生成し、管理する。
-   ダイス投入イベントを受け取り、適切な `DiceInlet` インスタンスに処理を委譲する。

---

## ダイスインレットプレゼンター (`DiceInletPresenter`)

`DiceInlet` (Model) と `DiceInletView` (View) の間の仲介役です。

### 4.1. 責務

-   `DiceInlet` インスタンスのステータスや値変更イベントを購読する。
-   購読したイベントに基づいて `DiceInletView` の表示を更新する。
-   Viewからのユーザー入力を `DiceInlet` インスタンスに伝達する（必要に応じて）。

### 4.2. 依存関係

-   `DiceInlet`: 監視対象のクリーチャーインスタンス。
-   `DiceInletView`: 更新対象のView。
-   `SpriteCommandBus`: イベント購読のため。

---

## 効果発動担当システム

ダイスインレットの効果発動は、`BaseInletAbilitySO` を拡張し、イベント駆動の原則に沿ってコマンドを発行することで実現します。

### 1. `BaseInletAbilitySO` の役割

-   インレットが発動する具体的な効果ロジックをカプセル化します。
-   `ExecuteAbility(ICreature targetCreature, int diceValue, SpriteCommandBus commandBus)` 抽象メソッドを実装し、内部で適切なコマンド（例: `BuffApplyCommand`, `ApplyDamageCommand`）を `SpriteCommandBus` を介して発行します。

---

## 全体フロー (ダイス投入から効果発動まで)

1.  **ダイス投入**: プレイヤーがダイスをインレットにドロップする。
2.  **イベント発行**: `SpriteInputHandler` が `SpriteDropCommand` を発行し、`DiceInteractionOrchestrator` がこれを受け取る。
3.  **インレット特定**: `DiceInteractionOrchestrator` は、ドロップされたインレットのIDから `DiceInletManager` を介して対応する `DiceInlet` インスタンスを取得する。
4.  **能力発動トリガー**: `DiceInteractionOrchestrator` は、取得した `DiceInlet` インスタンスの `OnDiceDropped(diceValue, targetCreature)` メソッドを呼び出す。
5.  **カウントダウン減少と効果実行**: `DiceInlet.OnDiceDropped()` 内で、投入された `diceValue` に応じて `CurrentCountdownValue` が減少します。この減少により `CurrentCountdownValue` が0以下になった場合、能力を発動し、カウントダウン値を初期値 (`InitialCountdownValue`) に戻します。
6.  **コマンド発行**: `ExecuteAbility` メソッド内で、具体的な効果に応じたコマンド（例: `BuffApplyCommand`, `ApplyDamageCommand`）が `SpriteCommandBus` を介して発行される。
7.  **コマンド処理**: 各コマンドの購読者（例: `EffectManager`、`CombatManager`）がコマンドを受け取り、それぞれの責務に応じた処理を実行する。

---

## 全体フロー（ダイスインレット生成）

1.  **ダイスインレットの生成**: `DiceInletManager` が `DiceInletFactory.Create()` を呼び出し、新しい `DiceInlet` インスタンスを生成します。
2.  **Viewとの紐付け**: `DiceInletManager` は、対応する `DiceInletView` を `ViewRegistry` から取得し、`DiceInlet` インスタンスと `DiceInletView` を引数に `DiceInletPresenter` を生成します。
3.  **ステータス変更**: `DiceInlet` インスタンスがカウントダウンするなどステータスや値が変更されると、`SpriteCommandBus` を介してイベントを発行します。
4.  **Viewの更新**: `DiceInletPresenter` がこのイベントを購読し、`DiceInletView` の `UpdateCountdownDisplay()` などのメソッドを呼び出して表示を更新します。
5.  **エフェクトの適用**: `DiceInlet` インスタンスにエフェクトが適用されると、`EffectManager` に登録され、`CurrentCountdownValue` などの動的なステータス計算に影響を与えます。

---

## 関連ファイル

-   [gdd_combat_system.md](../gdd/gdd_combat_system.md)
-   [sys_effect_management.md](./sys_effect_management.md)
-   [sys_creature_management.md](./sys_creature_management.md)
-   [guide_design-principles.md](../guide/guide_design-principles.md)
-   [sys_creature_card_lifecycle_design.md](./sys_creature_card_lifecycle_design.md)
-   [InletAbilityProfile.cs](../../Scripts/Data/InletAbilityProfile.cs)
-   [DiceInletConditionSO.cs](../../Scripts/Data/DiceInletConditionSO.cs)
-   [BaseInletAbilitySO.cs](../../Scripts/Data/BaseInletAbilitySO.cs)
-   [DiceInletManager.cs](../../Scripts/Manager/DiceInletManager.cs)
-   [ICreature.cs](../../Scripts/Domain/ICreature.cs)
-   [SpriteCommandBus.cs](../../Scripts/UI/SpriteCommandBus.cs)

---

## 更新履歴

-   2025-08-17: 初版 (Nekodamasi)

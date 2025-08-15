# sys_dice_inlet_design.md - ダイスインレット設計書

---

## 概要

このドキュメントは、ダイスインレットの技術仕様を定義します。
ダイスインレットは、クリーチャーカードに付随する特殊能力であり、ダイスを投入することで発動します。

---

## 1. データ定義

### 1.1. `DiceInletData` (ScriptableObject)

-   **役割**: ダイスインレットの静的な識別情報と基本設定を保持します。
-   **主なフィールド**:
    -   `string Id`: インレットの一意な識別子。
    -   `DiceInletConditionSO Condition`: ダイス投入時の発動条件。
    -   `BaseInletAbilitySO Ability`: インレットが発動する能力。
    -   `int InitialCountdownValue`: インレットの初期カウントダウン値。
    -   `int ResetCountdownValue`: 能力発動後のカウントダウンリセット値。

### 1.2. `DiceInletConditionSO` (ScriptableObject)

-   **役割**: ダイス投入時の発動条件を定義します。
-   **主なメソッド**:
    -   `bool Check(int diceValue)`: 投入されたダイスの値が条件を満たすかチェックします。

### 1.3. `BaseInletAbilitySO` (ScriptableObject)

-   **役割**: インレットが発動する「効果」を定義するすべてのScriptableObjectの基底クラス。
-   **主なメソッド**:
    -   `void ExecuteAbility(ICreature targetCreature, int diceValue, SpriteCommandBus commandBus)`: インレットの能力を実行します。具体的な効果に応じたコマンドを `SpriteCommandBus` を介して発行します。

---

## 2. ダイスインレット実行時インスタンス (`DiceInlet`)

ゲーム中に存在するダイスインレットの論理的な表現であり、`CurrentCountdownValue` などの変動する状態を保持します。

### 2.1. 責務

-   `DiceInletData` への参照を保持し、基本設定を取得する。
-   `CurrentCountdownValue` を保有し、ダイス投入時に更新する。
-   ダイス投入時の条件チェックと、能力発動のトリガー。
-   カウントダウン値の変更をイベントとして発行する。

### 2.2. 主なプロパティ

-   `string Id`: インレットの一意な識別子。
-   `int CurrentCountdownValue`: 現在のカウントダウン値。

### 2.3. 主なメソッド

-   `void OnDiceDropped(int diceValue, ICreature targetCreature)`: ダイスがインレットに投入された際の処理。条件チェック後、投入された `diceValue` に応じてカウントダウン値を減少させます。カウントダウン値が0以下になった場合、能力を発動し、カウントダウン値をリセット値 (`ResetCountdownValue`) に戻します。

---

## 3. ダイスインレットマネージャー (`DiceInletManager`)

ゲーム内に存在するすべての `DiceInlet` インスタンスを一元的に管理するクラスです。

### 3.1. 責務

-   `DiceInlet` インスタンスの生成と管理。
-   ダイス投入イベントを受け取り、適切な `DiceInlet` インスタンスに処理を委譲する。

---

## 4. 効果発動担当システム

ダイスインレットの効果発動は、`BaseInletAbilitySO` を拡張し、イベント駆動の原則に沿ってコマンドを発行することで実現します。

### 4.1. `BaseInletAbilitySO` の役割

-   インレットが発動する具体的な効果ロジックをカプセル化します。
-   `ExecuteAbility(ICreature targetCreature, int diceValue, SpriteCommandBus commandBus)` 抽象メソッドを実装し、内部で適切なコマンド（例: `BuffApplyCommand`, `ApplyDamageCommand`）を `SpriteCommandBus` を介して発行します。

### 4.2. 全体フロー (ダイス投入から効果発動まで)

1.  **ダイス投入**: プレイヤーがダイスをインレットにドロップする。
2.  **イベント発行**: `SpriteInputHandler` が `SpriteDropCommand` を発行し、`DiceInteractionOrchestrator` がこれを受け取る。
3.  **インレット特定**: `DiceInteractionOrchestrator` は、ドロップされたインレットのIDから `DiceInletManager` を介して対応する `DiceInlet` インスタンスを取得する。
4.  **能力発動トリガー**: `DiceInteractionOrchestrator` は、取得した `DiceInlet` インスタンスの `OnDiceDropped(diceValue, targetCreature)` メソッドを呼び出す。
5.  **カウントダウン減少と効果実行**: `DiceInlet.OnDiceDropped()` 内で、条件が満たされていれば、投入された `diceValue` に応じて `CurrentCountdownValue` が減少します。この減少により `CurrentCountdownValue` が0以下になった場合、能力を発動し、カウントダウン値をリセット値 (`ResetCountdownValue`) に戻します。
6.  **コマンド発行**: `ExecuteAbility` メソッド内で、具体的な効果に応じたコマンド（例: `BuffApplyCommand`, `ApplyDamageCommand`）が `SpriteCommandBus` を介して発行される。
7.  **コマンド処理**: 各コマンドの購読者（例: `EffectManager`、`CombatManager`）がコマンドを受け取り、それぞれの責務に応じた処理を実行する。
8.  **カウントダウンリセット**: 能力が発動した場合、`DiceInlet.OnDiceDropped()` 内で `CurrentCountdownValue` がリセット値 (`ResetCountdownValue`) に戻されます。

---

## 関連ファイル

-   [gdd_combat_system.md](../../gdd/gdd_combat_system.md)
-   [sys_effect_management.md](../sys_effect_management.md)
-   [sys_creature_management.md](../sys_creature_management.md)
-   [guide_design-principles.md](../../guide/guide_design-principles.md)
-   [DiceInletData.cs](../../Scripts/Data/DiceInletData.cs)
-   [DiceInletConditionSO.cs](../../Scripts/Data/DiceInletConditionSO.cs)
-   [BaseInletAbilitySO.cs](../../Scripts/Data/BaseInletAbilitySO.cs)
-   [DiceInletManager.cs](../../Scripts/Manager/DiceInletManager.cs)
-   [ICreature.cs](../../Scripts/Domain/ICreature.cs)
-   [SpriteCommandBus.cs](../../Scripts/UI/SpriteCommandBus.cs)

---

## 更新履歴

-   2025-08-15: 初版 (Gemini)
-   2025-08-15: CurrentCountdownValueの保有者と効果発動担当システムの設計を追加 (Gemini)
-   2025-08-15: 投入されたダイスの目によるカウントダウン値の減少ロジックを明確化 (Gemini)
-   2025-08-15: 全体フローにおけるカウントダウン処理の記述を修正 (Gemini)
-   2025-08-15: ターン経過によるカウントダウン減少の記述を削除 (Gemini)

# sys_ability_management.md - 固有能力管理設計書

---

## 概要

本設計は、「Cards and Dices」プロジェクトにおける「固有能力（Ability）」機能の技術的な実装を定義します。固有能力は、特定の条件で発動し、様々な効果をゲームプレイにもたらす要素です。本設計は、プロジェクトの既存の設計原則とシステムとの整合性を保ちつつ、拡張性と保守性の高いシステムを構築することを目的とします。

---

## クラスおよびコンポーネント設計

### 1. データ定義 (Model - ScriptableObject)

-   **`BaseAbilityDataSO` (抽象クラス)**:
    -   全ての固有能力の定義データが継承する抽象基底クラス。
    -   **プロパティ**: `Id`, `TargetSelector`, `TriggerCondition`, `EffectDefinition`, `Duration` を保持し、能力の構成要素を定義します。

-   **`BaseAbilityTargetSelectorSO` (抽象クラス)**:
    -   能力の効果対象を選択するロジックを定義します。
    -   **メソッド**: `SelectTarget(ownerId, creatureManager, diceManager)` でターゲットのリストを返します。

-   **`BaseAbilityTriggerConditionSO` (抽象クラス)**:
    -   能力の発動条件を定義します。
    -   **メソッド**: `Check(ownerId, creatureManager, diceManager, abilityManager)` で、ゲームの状態を基に発動条件を判定します。

-   **`BaseAbilityEffectDefinitionSO` (抽象クラス)**:
    -   能力が発動した際の効果内容を定義します。
    -   **メソッド**: `Execute(context, commandBus, ...)` で効果を実行し、必要に応じてコマンドを発行します。

-   **`BaseAbilityDurationSO` (抽象クラス)**:
    -   能力の有効期限や使用回数制限を定義します。
    -   **メソッド**: `OnUse(instance)` で使用後の状態を更新し、`OnReset(instance)` で初期状態にリセットします。

### 2. ランタイムインスタンス (Model - Pure C# Class)

-   **`AbilityInstance`**:
    -   ゲーム中に存在する固有能力の実行時インスタンス。アビリティに関するロジックの実行主体です。
    -   **プロパティ**: `OwnerId`, `Data` (参照元データ), `SubOwnerId`, `CurrentCooldown`, `RemainingUsages`, `IsSuppressed`。
    -   **責務**: 自身の状態管理と、アビリティ実行の全プロセスを担当します。
    -   **メソッド `ExecuteAbility(...)`**: アビリティ実行のコアロジック。以下の処理を順次実行します。
        1.  `IsSuppressed` (抑止状態) や `RemainingUsages` (残り使用回数) をチェック。
        2.  `Data.TriggerCondition.Check(...)` を呼び出し、発動条件を判定。
        3.  `Data.TargetSelector.SelectTarget(...)` でターゲットを決定。
        4.  `Data.EffectDefinition.Execute(...)` で効果を実行。
        5.  `Data.Duration.OnUse(this)` で使用回数やクールダウンを更新。

### 3. 管理クラス (Controller)

-   **`AbilityManager` (ScriptableObject)**:
    -   全ての `AbilityInstance` を一元的に管理するレジストリ兼ディスパッチャー。
    -   **責務**:
        -   `AbilityFactory` を用いて `AbilityInstance` を生成し、リストに登録・解除します (`RegisterAbility`, `UnregisterAbilitiesForOwner`)。
        -   `SpriteCommandBus` を購読し、`ExecuteAbilityEffectCommand` を監視します。
        -   コマンド受信時、管理下の全ての `AbilityInstance` に `ExecuteAbility` メソッドの実行を指示します。

---

## 主要な処理フロー

### 1. 固有能力の初期化と登録

1.  システムの適切な箇所（例: クリーチャー生成時など）で、`AbilityManager.RegisterAbility(abilityData, ownerId, subOwnerId)` が呼び出されます。
2.  `AbilityManager` は `AbilityFactory` を通じて `AbilityInstance` を生成し、管理リストに追加します。
3.  `AbilityInstance` のコンストラクタ内で `Data.Duration.OnReset(this)` が呼ばれ、使用回数やクールダウンが初期化されます。

### 2. 固有能力の発動

1.  ゲーム内でアビリティを発動すべきタイミングになると、`ExecuteAbilityEffectCommand` が `SpriteCommandBus` を介して発行されます。
2.  `AbilityManager` はこのコマンドを受信し、管理している全ての `AbilityInstance` の `ExecuteAbility()` メソッドを呼び出します。
3.  各 `AbilityInstance` は、自身の `ExecuteAbility()` メソッド内で、発動条件のチェックから効果の実行、使用後の状態更新までの一連の処理を自己完結的に行います。

---

## 既存システムとの連携

-   **クリーチャーシステム**: `CreatureData` が `BaseAbilityDataSO` のリストを保持することで、クリーチャーに能力を定義します。`AbilityInstance` は `OwnerId` を通じて自身の所有者であるクリーチャーを認識します。
-   **エフェクトシステム**: 能力の効果がステータス変更やバフ/デバフの場合、`BaseAbilityEffectDefinitionSO` が `ApplyEffectCommand` を発行し、`EffectManager` が処理を引き継ぎます。

---

## 関連ファイル

-   [gdd_combat_system.md](../gdd/gdd_combat_system.md)
-   [gdd_composite_object_id.md](../gdd/gdd_composite_object_id.md)
-   [sys_creature_management.md](./sys_creature_management.md)
-   [sys_effect_management.md](./sys_effect_management.md)
-   [sys_dice_inlet_management.md](./sys_dice_inlet_management.md)
-   [guide_design-principles.md](../guide/guide_design-principles.md)

---

## 更新履歴

-   2025-08-22: ソースコードの現状に合わせ、クラスの責務と処理フローを全面的に更新 (Gemini)
-   2025-08-21: 関連ファイルと不要なクラス記述を更新 (Gemini)
-   2025-08-15: 初版 (Gemini)

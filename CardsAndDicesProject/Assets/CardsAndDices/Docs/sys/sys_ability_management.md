# sys_ability_management.md - 固有能力管理設計書

---

## 概要

本設計は、「Cards and Dices」プロジェクトにおける「固有能力（Ability）」機能の技術的な実装案を提案します。固有能力は、特定の条件で発動し、様々な効果をゲームプレイにもたらす要素です。本設計では、プロジェクトの既存の設計原則（MVC、データ駆動、イベント駆動）と既存システム（クリーチャー管理、エフェクト管理、複合オブジェクト識別子、ダイスインレット）との整合性を保ちつつ、高い拡張性と保守性を持つシステムを構築することを目指します。

---

## クラスおよびコンポーネント設計

### 1. データ定義 (Model - ScriptableObject)

-   **`BaseAbilityDataSO` (ScriptableObject)**:
    -   全ての固有能力の定義データが継承する抽象基底クラス。
    -   共通プロパティ: `string Id`。
    -   抽象メソッド: `GetTriggerCondition()`, `GetEffectDefinition()` など、具体的な能力で実装されるべきインターフェースを定義。
-   **`BaseAbilityTriggerConditionSO` (ScriptableObject)**:
    -   能力の発動条件を定義する基底クラス。
    -   例: `OnAttackTriggerConditionSO`, `OnPlacementTriggerConditionSO`, `OnTurnEndTriggerConditionSO`。
    -   メソッド: `bool Check(ICommand command, AbilityInstance abilityInstance)`: 発行されたコマンドと能力インスタンスの状態に基づいて条件をチェック。
-   **`BaseAbilityEffectDefinitionSO` (ScriptableObject)**:
    -   能力が発動した際の効果内容を定義する基底クラス。
    -   例: `ApplyEffectAbilityEffectSO`, `DealDamageAbilityEffectSO`, `ModifyBasicAttackEffectSO`。
    -   メソッド: `void Execute(AbilityContext context, SpriteCommandBus commandBus)`: 効果を実行し、必要に応じてコマンドを発行。
-   **`BaseAbilityDurationSO` (ScriptableObject)**:
    -   能力の有効期限や使用回数制限を定義する基底クラス。
    -   例: `CooldownDurationSO`, `TurnLimitedDurationSO`, `CombatLongDurationSO`, `UsageLimitedDurationSO`。
    -   プロパティ: `int InitialValue`, `int ResetValue` など。
    -   メソッド: `void OnEvent(AbilityInstance instance, ICommand command)`: イベントに応じた寿命管理ロジック。

### 2. ランタイムインスタンス (Model - Pure C# Class)

-   **`AbilityInstance`**:
    -   ゲーム中に存在する固有能力のランタイムインスタンス。
    -   プロパティ: `string Id` (CompositeObjectId), `BaseAbilityDataSO Data` (参照元の定義データ), `ICreature OwnerCreature` (能力を持つクリーチャー), `int CurrentCooldown`, `int RemainingUsages`, `bool IsSuppressed`。
    -   責務: 自身の状態（クールダウン、使用回数、抑止状態）を管理。
    -   イベント発行: 状態変化時に`SpriteCommandBus`を介してイベントを発行（例: `AbilityCooldownChangedCommand`）。

### 3. 管理クラス (Controller)

-   **`AbilityManager`(ScriptableObject)**:
    -   全ての`AbilityInstance`を一元的に管理するクラス。
    -   責務:
        -   `AbilityInstance`の生成と破棄。
        -   `SpriteCommandBus`を購読し、ゲームイベント（コマンド）を監視。
        -   購読したコマンドを各`AbilityInstance`の発動条件に渡し、条件を満たす`AbilityInstance`を特定。
        -   発動条件を満たした`AbilityInstance`の`BaseAbilityEffectDefinitionSO`を呼び出し、効果を実行。
        -   `AbilityInstance`のクールダウンや使用回数制限の更新（`AbilityDurationSO`のロジックを呼び出す）。
        -   `CompositeObjectIdManager`と連携し、`AbilityInstance`に`CompositeObjectId`を付与。
-   **`CreatureAbilityBinder`**:
    -   `CreatureManager`によって生成され、`ICreature`と`AbilityManager`を連携させる。
    -   `ICreature`に紐づく`BaseAbilityDataSO`から`AbilityInstance`を生成し、`AbilityManager`に登録。
    -   `ICreature`のライフサイクルイベント（死亡など）に応じて、関連する`AbilityInstance`を`AbilityManager`から削除。

---

## データ構造

固有能力のデータは、ScriptableObjectとして定義します。これにより、Unityエディタ上での管理と、デザイナーによる柔軟な調整が可能になります。

```
// BaseAbilityDataSO.cs
public abstract class BaseAbilityDataSO : ScriptableObject
{
    public string Id;
    public string Name;
    public string Description;

    // 発動条件を定義するScriptableObjectへの参照
    public BaseAbilityTriggerConditionSO TriggerCondition;

    // 効果内容を定義するScriptableObjectへの参照
    public BaseAbilityEffectDefinitionSO EffectDefinition;

    // 有効期限や使用回数制限を定義するScriptableObjectへの参照
    public AbilityDurationSO Duration;
}

// BaseAbilityTriggerConditionSO.cs
public abstract class BaseAbilityTriggerConditionSO : ScriptableObject
{
    // コマンドと能力インスタンスの状態に基づいて条件をチェック
    public abstract bool Check(ICommand command, AbilityInstance abilityInstance);
}

// BaseAbilityEffectDefinitionSO.cs
public abstract class BaseAbilityEffectDefinitionSO : ScriptableObject
{
    // 能力実行時のコンテキスト（発動源、ターゲットなど）
    public class AbilityContext
    {
        public CompositeObjectId SourceId; // 能力の発動源（例: クリーチャーカードのID）
        public CompositeObjectId TargetId; // 能力のターゲット（例: 攻撃対象のクリーチャーID）
        public int DiceValue; // ダイスインレットの場合のダイス値など
        // その他、能力実行に必要な情報
    }
    // 効果を実行し、必要に応じてコマンドを発行
    public abstract void Execute(AbilityContext context, SpriteCommandBus commandBus);
}

// BaseAbilityDurationSO.cs
public abstract class BaseAbilityDurationSO : ScriptableObject
{
    // 初期値（クールダウンの初期値、使用回数の初期値など）
    public int InitialValue;
    // リセット値（クールダウンが終了した後の値、使用回数がリセットされた後の値など）
    public int ResetValue;

    // イベントに応じた寿命管理ロジック
    public abstract void OnEvent(AbilityInstance instance, ICommand command);
}
```

---

## 主要な処理フロー

### 1. 固有能力の初期化と登録

1.  `CreatureManager`が`ICreature`を生成する際、`CreatureAbilityBinder`を介して`ICreature`に紐づく`BaseAbilityDataSO`のリストを取得。
2.  `CreatureAbilityBinder`は各`BaseAbilityDataSO`から`AbilityInstance`を生成し、`AbilityManager.RegisterAbility(abilityInstance)`を呼び出して登録。
3.  `AbilityInstance`は自身の`CompositeObjectId`を`ICreature`の`CompositeObjectId`に設定。

### 2. 固有能力の発動

1.  ゲーム内で特定のイベント（例: クリーチャーが攻撃された、ターンが終了した）が発生。
2.  イベントに対応するコマンド（例: `CreatureAttackedCommand`, `TurnEndCommand`）が`SpriteCommandBus`を介して発行される。
3.  `AbilityManager`は`SpriteCommandBus`を購読しており、発行されたコマンドを受信。
4.  `AbilityManager`は登録されている全ての`AbilityInstance`をループし、各`AbilityInstance.Data.TriggerCondition.Check(command, abilityInstance)`を呼び出して発動条件をチェック。
5.  条件を満たし、かつクールダウン中や使用回数制限を超えていない`AbilityInstance`を特定。
6.  `AbilityManager`は、特定された`AbilityInstance`の`BaseAbilityEffectDefinitionSO.Execute(context, SpriteCommandBus)`を呼び出し、能力の効果を実行。`AbilityContext`には、発動源（例: 攻撃されたクリーチャーのID）、ターゲット（例: 攻撃してきたクリーチャーのID）、その他必要な情報を含める。
7.  `BaseAbilityEffectDefinitionSO.Execute()`内で、具体的な効果に応じたコマンド（例: `ApplyDamageCommand`, `BuffApplyCommand`）が`SpriteCommandBus`を介して発行される。
8.  能力が発動した場合、`AbilityManager`は`AbilityInstance.Data.Duration.OnEvent(abilityInstance, command)`を呼び出し、クールダウンや使用回数制限の更新ロジックを実行。

### 3. 固有能力の寿命管理（クールダウン、使用回数制限）

1.  `AbilityManager`は`TurnStartCommand`や`TurnEndCommand`などの時間経過を示すコマンドを購読。
2.  これらのコマンドを受信した際、`AbilityManager`は登録されている全ての`AbilityInstance`の`BaseAbilityDurationSO.OnEvent(abilityInstance, command)`を呼び出す。
3.  `BaseAbilityDurationSO.OnEvent()`内で、`AbilityInstance`の`CurrentCooldown`を減少させたり、`RemainingUsages`をリセットしたりするロジックが実行される。
4.  `AbilityInstance`の状態変化（クールダウン終了、使用回数ゼロなど）は、`AbilityPresenter`を介してUIに反映される。
5.  発動抑止は、`AbilityInstance`の`IsSuppressed`フラグを操作するコマンド（例: `SuppressAbilityCommand`）を発行することで制御。`BaseAbilityTriggerConditionSO.Check()`内で`IsSuppressed`フラグもチェックする。

---

## 既存システムとの連携

### 1. クリーチャーシステム

-   **能力の付与**: `ICreature`のデータ（`CreatureData`）に`BaseAbilityDataSO`のリストを持たせることで、クリーチャーごとに固有能力を定義します。
-   **ランタイム管理**: `CreatureManager`が`ICreature`を生成する際に、`CreatureAbilityBinder`を介して対応する`AbilityInstance`を生成し、`AbilityManager`に登録します。
-   **ターゲット指定**: 固有能力のターゲットがクリーチャーである場合、`CompositeObjectId`を使用して`ICreature`を特定します。

### 2. エフェクトシステム

-   **効果の適用**: 固有能力の効果がステータス変更やバフ/デバフである場合、`BaseAbilityEffectDefinitionSO.Execute()`内で`BuffApplyCommand`や`DebuffApplyCommand`を発行し、`EffectManager`を介して処理されます。
-   **基礎攻撃の変更**: `gdd_combat_system.md`で言及されている基礎攻撃の`使用能力値`や`効果範囲`を変更する固有能力は、特殊な`EffectData`タイプを定義し、それが`ICreature`に適用されることで基礎攻撃のプロパティが一時的に変更されるように実装します。これにより、`EffectManager`の寿命管理の仕組みを再利用できます。

---

## 関連ファイル

-   [../gdd/gdd_combat_system.md](../../gdd/gdd_combat_system.md)
-   [../guide/guide_design-principles.md](../../guide/guide_design-principles.md)
-   [../guide/guide_ui_interaction_design.md](../../guide/guide_ui_interaction_design.md)
-   [../gdd/gdd_composite_object_id.md](../../gdd/gdd_composite_object_id.md)
-   [./sys_creature_management.md](./sys_creature_management.md)
-   [./sys_effect_management.md](./sys_effect_management.md)
-   [./sys_dice_inlet_design.md](./sys_dice_inlet_design.md)

---

## 更新履歴

-   2025-08-15: 初版 (Gemini)

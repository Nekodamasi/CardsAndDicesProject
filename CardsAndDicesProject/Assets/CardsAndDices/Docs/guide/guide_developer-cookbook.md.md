# guide_developer-cookbook.md - 開発者クックブック

---

## 概要

このドキュメントは、開発者が特定のタスクを実行するための、実践的な手順をまとめた「レシピ集」です。
新しい機能を追加したり、既存の機能を変更したりする際は、ここで示されるワークフローに従ってください。

---

## レシピ

### 1. レシピ1: 新しいAbilityの追加

1.  **概要:**
    -   `Ability` は、`AbilityDataEntity` に各データを紐づける事で作成する
    -   アビリティのデータには、`効果範囲` 、`発動させる条件` 、`実行される効果` 、`使用制限` で構成される
    -   構成されるデータは、`ScriptableObject` や `enum` によりインスペクター上で設定できる

2.  **効果範囲:**
    -   `AreaOfEffect(enum）` を設定する。

3.  **発動させる条件:**
    -   `BaseAbilityTriggerConditionSO` を継承したクラスを作成する。

    -   `ActivationTiming` により、`Ability` の起動タイミングを設定し、`CheckCondition` メソッド内で、起動条件のcheckを行う。

4.  **実行される効果:**
    -   `BaseAbilityEffectDefinitionSO` を継承したクラスを作成する。
    -   `Execute` メソッドに、発動時の効果を記載する。
    -   主な効果として以下の `ScriptableObject` が用意されている
        - `BuffDebuffEffectSO:`
            - バフデバフの値を指定して対象に付与する
        - `CreatureAttackEffectSO:`
            - `Health` や `Shield` といった現在値で処理する値にたいする変更を行う。
        - `CreatureAttackEffectSO:`
            - 特定の対象に攻撃を行う。

5.  **使用制限:**
    -   `BaseAbilityDurationSO` を継承したクラスを作成する。

    -   派生クラスである `AbilityDurationSO` で基本的なすべてのAbilityを作成可能である。

### 2. レシピ2: 新しいDiceInletの追加

1.  **概要:**
    -   `DiceInlet` は インレットの固有データと、インレットの発動により、起動する（またはロックされる） `Ability` により構成される。
    -   インレットの固有データは、 `InletProfileIdEntity` に含まれており、関連するAbilityもこの中に設定する

2.  **インレットの固有データ:**
    -   `InletProfileIdEntity` をインスペクター上で作成し、固有データを設定する。
    -   `AllowedDiceFacesEntity` をインスペクター上で作成し、ドロップ可能なサイコロの目を設定する。
    -   `InletEffectType` により、設定されたAbilityが起動するのかロックされるのかを決める。
        - `Ability Executor:` インレット発動時、Abilityを起動する
        - `Ability Sealer:` インレット発動時、Abilityをロックする

3.  **関連Abilityの設定:**
    -   `InletProfileIdEntity` の `Abilities` にインレットの起動時に影響を受けるAbilityを設定する。
    -   `Abilities`に設定されたAbilityは、以下２つの効果を受ける。
        - `BaseAbilityTriggerConditionSO` の `ActivationTiming` に `Inlet` を指定した場合、自身が設定されたインレットの発動が起動タイミングになる
        - `Ability Sealer` に設定されている場合は、インレットの発動時に、Abilityはロックされる。
    -   設定されたAbilityは上記２つ以外の効果を受けず、それ以外は通常のAbilityと同じである。
    -   Inlet側が `Ability Executor` である場合、`ActivationTiming` に `Inlet` が指定されていなければ、インレットの影響は何も受けない。
    -   インレットに複数のAbilityを設定可能だが、`Ability Executor` であれば、メインの効果は、`ActivationTiming` に `Inlet` に設定されていることを想定している。
    -   逆に `Ability Sealer` が設定されているInletに`ActivationTiming` が `Inlet` のAbilityを設定しても効果を得れれない。

### 3. レシピ3: 新しいエネミーの追加

1.  **概要:**
    -   エネミーデータは、 `FixedCardInitializer` で実装される。
    -   生成した `FixedCardInitializer` を `Wave` データに追加することで、エネミーをゲームに追加できる。

2.  **FixedCardInitializer:**
    -   `FixedCardInitializer` をインスペクター上で作成し、固有データを設定する。
    -   `CreatureNameEntity` をインスペクター上で作成し、エネミーの固有IDを設定する。
    -   `Ability` や `DiceInlet` を設定することで、エネミー固有の能力を与えることが出来る。
    -   エネミーの `DiceInlet` は、 `Ability Sealer:` を想定しており、紐づけるAbilityは、 `TurnEnd` に発動する強力なAbilityを想定している。

---

## ScriptableObject 格納先

### 1. Ability関連

1.  **AbilityDataEntity:**
-   `Combats/Data/EntityDefinition/AbilityDataEntity`

2.  **BaseAbilityTriggerConditionSO（派生）:**
-   `Combats/Data/Abilities/Triggers`

3.  **BaseAbilityEffectDefinitionSO（派生）:**
-   `Combats/Data/Abilities/Effects`

4.  **BaseAbilityDurationSO（派生）:**
-   `Combats/Data/Abilities/Durations`

### 2. DiceInlet関連

1.  **InletProfileIdEntity:**
-   `Combats/Data/EntityDefinition/InletProfileIdEntity`

2.  **AllowedDiceFacesEntity:**
-   `Combats/Data/EntityDefinition/AllowedDiceFacesEntity`

### 3. Enemy関連

1.  **FixedCardInitializer:**
-   `Combats/Creatures`

2.  **CreatureNameEntity:**
-   `Combats/Data/EntityDefinition/CreatureNameEntity`

---

## 関連ファイル

-   [sys_ability_system.md](../sys/sys_ability_system.md)
-   [sys_dice_inlet_management.md](../sys/sys_dice_inlet_management.md)
-   [sys_wave_system.md](../sys/sys_wave_system.md)

---

## 更新履歴

-   2025-10-19: 初版

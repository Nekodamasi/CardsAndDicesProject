# sys_tubutubu_management.md - 〇〇管理設計書

---

## 概要

本設計は、「Cards and Dices」プロジェクトにおける「つぶつぶ（Tubutubu）」機能の技術的な実装を定義します。つぶつぶは、特定の条件で発動し、〇〇〇を管理するための主要なクラスとその責務を詳述します。

---

## クラスおよびコンポーネント設計

### 1. データ定義 (Model)

- **`BaseNekonekoSO` (抽象クラス)**:
    - 全てのネコネコの定義データが継承する抽象基底クラス。
    - **継承**: `ScriptableObject`
    - **プロパティ**: `Id`, `xxxx` を保持し、〇〇の構成要素を定義します。


### 2. ランタイムインスタンス (Model)

- **`NekonekoInstance`**:
    - **継承**: `Pure C# Class`
    - ゲーム中に存在するネコネコの実行時インスタンス。アビリティに関するロジックの実行主体です。
    - **プロパティ**: `OwnerId`, `Data` (参照元データ)。
    - **責務**: 自身の状態管理を担当します。
    - **メソッド `ExecuteTubutubu(...)`**: 〇〇実行のコアロジック。以下の処理を順次実行します。
        1.  `IsSuppressed` (抑止状態) や `RemainingUsages` (残り使用回数) をチェック。
        2.  `Data.TriggerCondition.Check(...)` を呼び出し、発動条件を判定。


### 3. 仲介クラス (Presenter)

- **`NekonekoPresenter`**:
    - **継承**: `Pure C# Class`
    - `NekonekoView` と `NekonekoInstance` を繋ぐ仲介クラス。
    - **プロパティ**: `OwnerId`, `Data` (参照元データ)。
    - **責務**: 購読したイベントに基づいて `NekonekoView` の表示を更新する。。
    - **メソッド `ExecuteTubutubu(...)`**: 〇〇実行のコアロジック。以下の処理を順次実行します。
        1.  `IsSuppressed` (抑止状態) や `RemainingUsages` (残り使用回数) をチェック。
        2.  `Data.TriggerCondition.Check(...)` を呼び出し、発動条件を判定。

### 4. 管理クラス (Controller)

-   **`TubutubuManager`**:
    - **継承**: `ScriptableObject`
    - 全ての `NekonekoInstance` を一元的に管理するレジストリ兼ディスパッチャー。
    - **責務**:
        - `NekonekoInstance` を生成し、リストに登録・解除します (`RegisterAbility`, `UnregisterAbilitiesForOwner`)。
        - `SpriteCommandBus` を購読し、`ExecuteAbilityEffectCommand` を監視します。
        - コマンド受信時、管理下の全ての `AbilityInstance` に `ExecuteAbility` メソッドの実行を指示します。

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

- **クリーチャーシステム**: `CreatureData` が `BaseAbilityDataSO` のリストを保持することで、クリーチャーに能力を定義します。`AbilityInstance` は `OwnerId` を通じて自身の所有者であるクリーチャーを認識します。
- **エフェクトシステム**: 能力の効果がステータス変更やバフ/デバフの場合、`BaseAbilityEffectDefinitionSO` が `ApplyEffectCommand` を発行し、`EffectManager` が処理を引き継ぎます。

---

## 関連ファイル

- [gdd_xxxxxx.md](../gdd/gdd_xxxxxx.md)

---

## 更新履歴

- 2025-08-15: 初版 (Gemini)

# sys_effect_management.md - エフェクト管理設計書

---

## 概要

このドキュメントは、ゲーム内におけるバフ・デバフといった継続的な効果（エフェクト）のデータ定義、ライフサイクル管理、および関連システムとの連携方法について設計するものです。

---

## データ定義

### 1. EffectData

バフ・デバフ効果の静的な情報を定義するScriptableObjectです。

-   **主なフィールド:**
    -   `BuffDebuffType`: エフェクトの種類を識別するEnum型。
    -   `UpdateTiming`: 効果の持続時間を更新するタイミング（例: ターン開始時、終了時）を定義する`TriggerTiming` Enum型。
    -   `DurationValue`: 効果が持続するターン数。

### 2. EffectInstance

`EffectData`に基づき、特定のターゲットに対して適用されたエフェクトの実行時インスタンスです。

-   **主なプロパティ:**
    -   `TargetObjectId`: エフェクトが適用されている対象のID。
    -   `Data`: 参照する`EffectData`。
    -   `CurrentValue`: 現在の効果量（例: 攻撃力+5の「5」の部分）。
    -   `TargetType`: 効果が影響を与えるステータスの種類（例: 攻撃力、体力）。
    -   `RemainingTurns`: 効果が持続する残りターン数。
    -   `IsExpired`: 効果が期限切れになったかどうかを示すフラグ。

-   **主なメソッド:**
    -   `Initialize(effectData, cardId, effectTargetType, currentValue)`: インスタンスを初期化します。
    -   `CheckExpired(triggerTiming)`: 指定されたタイミングに基づき、残りターン数を更新し、期限切れかどうかを判定します。

---

## EffectManager

全てのエフェクトインスタンスを一元管理するScriptableObjectです。

-   **責務:**
    -   現在アクティブな`EffectInstance`のリストを管理します。
    -   `SpriteCommandBus`を購読し、`ApplyEffectCommand`と`UpdateEffectExpiredCommand`を処理します。
    -   `EffectFactory`を使用して新しい`EffectInstance`を生成します。
    -   `RegisterEffect(effectInstance)`: 新しいエフェクトをリストに追加します。
    -   `RemoveEffect(effectInstance)`: 指定されたエフェクトをリストから削除します。
    -   `GetTotalEffectValue(targetObjectId, targetType)`: 特定のターゲットと効果タイプに対する合計効果量を計算して返します。

---

## コマンドフロー

本システムのエフェクト適用と更新は、`SpriteCommandBus`を介したコマンドによって駆動されます。

1.  **エフェクト適用**:
    -   アビリティなどが発動されると、`BuffDebuffEffectSO`のようなクラスが`ApplyEffectCommand`を生成します。
    -   このコマンドには、ターゲットID、`EffectData`、効果対象タイプ、効果量が格納されます。
    -   `SpriteCommandBus.Emit()`を通じてコマンドが発行されます。
    -   `EffectManager`がコマンドを購読し、`OnApplyEffect`メソッドで`EffectFactory`を使い`EffectInstance`を生成・登録します。

2.  **エフェクト更新・期限切れ処理**:
    -   ターン管理システムなどが、特定のタイミング（例: ターン開始時）で`UpdateEffectExpiredCommand`を発行します。
    -   `EffectManager`がこのコマンドを購読し、`OnUpdateEffectExpired`メソッドで全アクティブエフェクトの`CheckExpired`を呼び出します。
    -   `IsExpired`フラグが立ったエフェクトは、`RemoveExpiredEffects`メソッドによってリストから削除されます。

---

## コマンド連携

### 1. SpriteCommandBus

-   `ICommand`インターフェースを実装したコマンドを一元的に配信するイベントバスです。
-   本システムでは、`CommandInvoker`の役割を果たし、各マネージャ間の疎結合な連携を実現します。

### 2. ApplyEffectCommand

-   新しいエフェクトをターゲットに適用する際に使用されるコマンドです。
-   `EffectManager`がこれを処理し、エフェクトインスタンスを生成・登録します。

### 3. UpdateEffectExpiredCommand

-   エフェクトの残りターン数を更新し、期限切れをチェックするタイミングを通知するコマンドです。
-   `EffectManager`がこれを処理し、全エフェクトのライフサイクルを管理します。

---

## 関連ファイル

-   [gdd_combat_system.md](../gdd/gdd_combat_system.md)
-   [sys_ability_management.md](./sys_ability_management.md)
-   [sys_creature_management.md](./sys_creature_management.md)
-   [guide_design-principles.md](../guide/guide_design-principles.md)

---

## 更新履歴

-   2025-08-22: ソースコードの現状に合わせ、データ定義、管理クラスの責務、コマンドフローを全面的に更新 (Gemini)
-   2025-08-21: 関連ファイルとコマンド名を最新化 (Gemini)
-   2025-08-15: EffectManagerをScriptableObjectとして再定義 (Gemini)
-   2025-07-12: 初版 (Technical Writer)
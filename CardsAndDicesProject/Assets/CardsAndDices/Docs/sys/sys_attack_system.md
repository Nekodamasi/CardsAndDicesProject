# sys_attack_system.md - 攻撃システム設計書

---

## 概要

本設計は、`gdd_combat_system.md`で定義された「攻撃」機能を、プロジェクトの三大原則（関心の分離、データ駆動、イベント駆動）に則って実装するためのシステム設計を定義します。攻撃のロジックをUIから完全に分離し、再利用性と保守性の高い、データ駆動型のアーキテクチャを構築します。

---

## 提案クラスと責務

| クラス名 | 種別 | 責務 |
| :--- | :--- | :--- |
| `PerformAttackCommand` | コマンド (新規) | 「誰が」「どの攻撃定義で」「どのターゲットを（任意）」攻撃する、という意図をシステムに伝達するデータクラス。UIや他のシステム（例: 基礎攻撃）から発行される。 |
| `CombatManager` | マネージャ (修正) | `PerformAttackCommand`を購読し、攻撃処理全体のフロー（対象決定→ダメージ計算→結果適用）を統括する。`TargetSelector`や`DamageCalculator`といった専門サービスを利用する。 |
| `TargetSelector` | サービス (新規) | `PerformAttackCommand`で渡された攻撃の`効果範囲`と`攻撃処理のルール`に基づき、攻撃対象となるクリーチャーのリストを決定して返す責務を持つ。 |
| `DamageCalculator` | サービス (新規) | 攻撃者、防御者、攻撃定義（使用能力値、追加攻撃力など）、および`EffectManager`から取得したバフ/デバフ情報を基に、最終的なダメージ量を計算する責務を持つ。 |
| `ApplyDamageCommand` | コマンド (新規) | 計算済みのダメージ量と追加効果を、特定のターゲットに適用するよう指示するデータクラス。`CombatManager`によって発行される。 |
| `CreatureManager` | マネージャ (修正) | `ApplyDamageCommand`を購読し、管理下のクリーチャーモデルのHealthを実際に減少させる。Healthが0以下になった場合、`CreatureDiedEvent`を発行する責務を持つ。 |
| `AttackData` | データ (新規) | 攻撃の静的な定義を保持するScriptableObject。「使用能力値」「追加攻撃力」「効果範囲」「追加効果」などの情報を格納する。 |

---

## 処理フロー

ユーザーの操作やシステムの自動処理（基礎攻撃）によって攻撃が発生した場合、以下のイベント駆動フローで処理が実行されます。

### 1. 攻撃要求の発行
-   UI（例: `DiceInletView`）やシステム（例: `CombatManager`のクールダウン処理）が、攻撃者のID、使用する`AttackData`、任意でターゲットのIDを含む`PerformAttackCommand`を生成し、`CommandBus`に発行する。

### 2. 攻撃プロセスの開始
-   `CombatManager`が`PerformAttackCommand`を受信する。

### 3. 攻撃対象の決定
-   `CombatManager`は`TargetSelector`サービスを呼び出し、`PerformAttackCommand`内の`AttackData`（効果範囲）と攻撃者の位置情報を基に、攻撃対象となるクリーチャーIDのリストを取得する。

### 4. ダメージ計算
-   `CombatManager`は対象リストの各クリーチャーに対してループ処理を行う。
-   ループ内で`DamageCalculator`サービスを呼び出す。この際、攻撃者情報、対象クリーチャー情報、`AttackData`を渡し、最終的なダメージ量を計算させる。`DamageCalculator`は内部で`EffectManager`に問い合わせ、バフ/デバフを考慮に入れる。

### 5. ダメージ適用の要求
-   `CombatManager`は、計算されたダメージ量、追加効果、および対象クリーチャーのIDを含む`ApplyDamageCommand`を生成し、`CommandBus`に発行する。

### 6. ダメージの適用と結果の伝播
-   `CreatureManager`が`ApplyDamageCommand`を受信する。
-   該当するクリーチャーのHealth（Model）を指定されたダメージ量だけ減少させる。
-   `CreatureHealthChangedEvent`を発行し、UI（HPバーなど）の更新をトリガーする。
-   もしクリーチャーのHealthが0以下になった場合、さらに`CreatureDiedEvent`を発行し、戦場からの除外など後続処理をトリガーする。
-   追加効果がある場合は、`EffectManager`にエフェクト適用を要求するコマンドを発行する。

---

## 関連ファイル

- [gdd_combat_system.md](../gdd/gdd_combat_system.md)
- [guide_design-principles.md](../guide/guide_design-principles.md)
- [sys_domain-model.md](./sys_domain-model.md)
- [sys_effect_management.md](./sys_effect_management.md)
- [sys_cooldown_processing.md](./sys_cooldown_processing.md)

---

## 更新履歴

- 2025-09-02: 初版 (Gemini)
# sys_dice_inlet_design.md - ダイスインレット設計書

---

## 概要

このドキュメントは、「CardsAndDices」プロジェクトにおけるダイスインレットの技術的な仕様を定義します。
ダイスインレットは、クリーチャーカードに付属し、ダイスを投入することで特定の能力を発動させるためのインタラクティブな要素です。

---

## インレットの基本機能

### 1. ダイス投入による能力発動

プレイヤーは、ダイスをドラッグ＆ドロップすることでインレットに投入します。
インレットは、投入されたダイスが発動条件を満たした場合に、固有の能力を発動します。

### 2. 投入可能なダイスの目の制限

各インレットには、投入を許可するダイスの目のリストが設定されます。
例えば、「偶数のみ」「3以上」といった条件を定義できます。

### 3. カウントダウンによる発動条件

インレットは、能力発動までに必要なカウントダウン値を持ちます。
ダイスが投入されると、その目の数だけカウントダウン値が減少し、0以下になった時点で能力が発動します。

### 4. クールタイム

一度能力が発動したインレットは、特定の期間（例: 1ターン）再発動できなくなるクールタイム状態になります。
クールタイム中のインレットは、視覚的に無効であることがプレイヤーに示されます。

### 5. エネミーインレット

敵クリーチャーのインレットは、プレイヤーがダイスを投入することで、そのターンにおける敵の特定の能力を封じる役割を持ちます。
これは、敵の強力な能力に対する対抗策として機能します。

---

## データ構造とプロパティ

インレットの状態と振る舞いは、複数のデータオブジェクトの組み合わせによって管理されます。中心となるのは `DiceInletData` であり、その発動条件は `DiceInletConditionSO` によって定義されます。

### 1. DiceInletData (クラス)

インレットのインスタンスごとにユニークな状態を管理するデータクラスです。

-   **`UniqueId` (`CompositeObjectId`):**
    -   このインレットを一意に識別するためのID。
-   **`Condition` (`DiceInletConditionSO`):**
    -   このインレットの発動条件を定義する `ScriptableObject` への参照。
-   **`CurrentCountdownValue` (int):**
    -   現在のカウントダウン値。ダイス投入により減少します。`Condition` の `InitialCountdownValue` で初期化されます。
-   **`IsInCooldown` (bool):**
    -   クールタイム中かどうかを示すフラグ。

### 2. DiceInletConditionSO (ScriptableObject)

インレットの発動条件を定義する `ScriptableObject` です。これにより、複数のインレットで同じ発動条件を再利用できます。

-   **`AllowedDiceFaces` (`AllowedDiceFacesSO`):**
    -   投入を許可するダイスの目を定義する `ScriptableObject` への参照。
-   **`InitialCountdownValue` (int):**
    -   発動に必要なカウントダウンの初期値。
-   **`ActivationType` (`InletActivationType`):**
    -   プレイヤーに提示される発動条件の見た目とロジックを定義するenum。
-   **`SuppressAbilityOnActivation` (bool):**
    -   エネミーインレット用のフラグ。`true` の場合、インレット発動時に、紐づくアビリティを次のターンまで無効にします。

### 3. AllowedDiceFacesSO (ScriptableObject)

投入可能なダイスの目の組み合わせを定義する `ScriptableObject` です。「奇数のみ」「偶数のみ」「3以上」といった共通の条件をアセットとして作成し、複数の `DiceInletConditionSO` で再利用することを目的とします。

-   **`IsFaceAllowed` (`List<bool>` サイズ6):**
    -   どの目が許可されているかを管理するboolのリスト。リストのインデックスがダイスの目に対応します（例: `IsFaceAllowed[0]` は1の目、`IsFaceAllowed[5]` は6の目）。

### 4. InletActivationType (enum)

-   **`SingleMatchTrigger`:**
    -   特定の目に一致するダイスが投入されたら即座に発動するタイプ。
-   **`TotalSumTrigger`:**
    -   投入されたダイスの目の合計値がカウントダウン値に達したら発動するタイプ。

---

## 発動条件の種別

### 1. Single Match Trigger

-   **動作ロジック:**
    -   `DiceInletConditionSO` の `AllowedDiceFaces` で許可されているいずれかの目のダイスが投入された時点で、即座に能力が発動します。
    -   このタイプの場合、`InitialCountdownValue` は通常1として扱われます。
-   **必要なデータ (`DiceInletConditionSO`):**
    -   `AllowedDiceFaces`: 発動トリガーとなる目のリストを持つ `AllowedDiceFacesSO` を参照。

### 2. Total Sum Trigger

-   **動作ロジック:**
    -   ダイスが投入されるたびに、`DiceInletData` の `CurrentCountdownValue` がその目の数だけ減少します。
    -   `CurrentCountdownValue` が0以下になった時点で、能力が発動します。
    -   UI上では、カウントダウン値が徐々に減っていく様子が視覚的に表現されます。
-   **必要なデータ (`DiceInletConditionSO`):**
    -   `AllowedDiceFaces`: 投入可能な目のリストを持つ `AllowedDiceFacesSO` を参照（通常は全て許可）。
    -   `InitialCountdownValue`: 発動に必要な合計値。

---

## インレット能力アーキテクチャ

### 設計目標

インレットの能力は、それが付属するクリーチャーカードによって動的に決定される。また、カードオブジェクトはプールされ再利用されるため、カードのライフサイクルと能力のライフサイクルを明確に分離し、動的な紐付け替えを可能にする必要がある。同時に、UIシステム（どのインレットを有効化するか）やゲームロジック（能力の発動）など、複数のシステムがインレットの現在の能力（条件と効果）を安全に参照できる仕組みを構築する。

この目標を達成するため、以下の「関心の分離」に基づいたアーキテクチャを採用する。

### 責務の分離（クラスの役割）

| クラス名 | 役割 | 種別 | 解説 |
| :--- | :--- | :--- | :--- |
| `DiceInletData` | **静的な識別子** | `struct` | インレットViewを一意に識別する`CompositeObjectId`のみを保持する。不変。 |
| `CreatureCardData` | **能力の所有者** | `class` | 自身の能力として`InletAbilityProfile`のインスタンスを保持する。カード初期化時に生成。 |
| `InletAbilityProfile` | **能力のパッケージ** | `class` | 「条件(`Condition`)」と「効果(`Ability`)」をペアで保持するデータコンテナ。 |
| `DiceInletAbilityRegistry` | **現在の担当者名簿** | `ScriptableObject` | どのインレットIDに、現在どの`InletAbilityProfile`が割り当てられているかを動的に記録する。 |
| `CardLifecycleService` | **ライフサイクル管理** | `ScriptableObject` | カードの生成・初期化・破棄を管理し、`Registry`への能力の登録・解除を行う。 |
| `UIActivationPolicy` | **判定の利用者** | `ScriptableObject` | `Registry`に問い合わせ、現在の`Condition`に基づいてUIの有効/無効を判断する。 |
| `(効果発動担当)` | **効果の実行者** | - | ダイス配置成功後、`Registry`に問い合わせ、現在の`Ability`を実行する。 |

### データフロー

#### 1. 能力の登録フロー（カード初期化時）

1.  **`（カード生成システム）`**: オブジェクトプールなどからカードを取得し、初期化を開始する。
2.  **`CardLifecycleService`**: `InitializeCard(cardId)`が呼び出される。
3.  **`CreatureCardView`**: `cardId`からViewが特定され、そのViewが持つ`InletAbilityProfile`と`DiceInletView`が取得される。
4.  **`DiceInletAbilityRegistry`**: `Register(inletId, profile)`が呼び出され、インレットのIDとカードの能力プロファイルが紐付けられる。

#### 2. 判定・実行フロー（ダイス操作時）

1.  **`Player`**: ダイスをドラッグする。
2.  **`UIActivationPolicy`**: `ShouldActivateInlet`が各インレットに対して呼び出される。
    a. **`DiceInletAbilityRegistry`**: `GetProfile(inletId)`で現在の能力プロファイルを取得。
    b. **`InletAbilityProfile`**: `profile.Condition`にアクセスし、ドラッグ中のダイスで条件を満たすか判定。
    c. 条件を満たすインレットのみUIが有効化される。
3.  **`Player`**: 有効化されたインレットにダイスをドロップする。
4.  **`（効果発動担当システム）`**: ドロップを検知し、配置を確定させる。
    a. **`DiceInletAbilityRegistry`**: `GetProfile(inletId)`で再度プロファイルを取得。
    b. **`InletAbilityProfile`**: `profile.Ability.Execute()`を呼び出し、効果を発動する。

---

## 関連ファイル

-   [gdd_combat_system.md](../gdd/gdd_combat_system.md)
-   [guide_rules.md](../guide/guide_rules.md)
-   [guide_files.md](../guide/guide_files.md)

---

## 更新履歴

-   2025-08-14: コードスニペットを削除し、メンテナンス性を向上 (Gemini)
-   2025-08-14: データフローの記述をソースコードと一致するように修正 (Gemini)
-   2025-08-13: インレット能力アーキテクチャの章を追記 (Gemini)
-   2025-08-13: 発動条件をScriptableObject化し、データ構造を更新 (Gemini)
-   2025-08-08: 初版 (Gemini - Technical Writer for Game Development)

# sys_creature_card_lifecycle_design.md - クリーチャーカードのライフサイクル設計書

---

## 概要

このドキュメントは、クリーチャーカードの生成から除去までのライフサイクル全体を定義します。
クリーチャーカードは、プレイヤー、エネミーといった異なる情報源からカードの基礎情報を生成し、それをカードオブジェクトに適用することでその性質を決定します。
このプロセスは、戦闘管理、データ供給、ライフサイクル管理、能力登録といった複数のサービスやマネージャーが連携して実現されます。

---

## クリーチャーカード生成フロー

クリーチャーカードは、戦闘開始時や新たなウェーブの発生時に生成されます。このフローは `CombatManager` を起点とし、カードのデータ構造、View、能力が段階的に構築・設定されます。

### 1. シーケンス

1.  **ユーザー操作** が **CombatManager** の `InitializeCombatField()` を呼び出します。
2.  **CombatManager** は **Player/EnemyCardDataProvider** の `GetCardDataList()` を呼び出し、`List<CardInitializationData>` を取得します。
3.  取得したリストの各 `CardInitializationData` に対して、以下の処理をループ実行します。
    1.  **CombatManager** が **ViewRegistry** の `GetNextAvailableCreatureCardView()` を呼び出し、利用可能な **CreatureCardView** を取得します。
    2.  **CombatManager** が **CardLifecycleService** の `InitializeCard()` を呼び出します。
        1.  **CardLifecycleService** は **CreatureCardView** の `ApplyData()` を呼び出し、クリーチャーの基本データを適用します。
        2.  **CardLifecycleService** は **CreatureCardView** の `GetInletViews()` を呼び出し、インレットのリストを取得します。
        3.  各インレットに対して、**CardLifecycleService** は **DiceInletAbilityRegistry** の `Register()` を呼び出し、能力を登録します。
    3.  **CombatManager** が **CardSlotManager** の `PlaceCardAsSystem()` を呼び出し、カードをスロットに配置します。

### 2. 各コンポーネントの役割

| コンポーネント | 役割 |
| :--- | :--- |
| **CombatManager** | 戦闘全体の流れを制御する司令塔。カード生成のトリガーとなり、各サービスに必要な指示を出します。 |
| **Player/EnemyCardDataProvider** | プレイヤーやエネミーのデータソース（マスターデータ、装備、ウェーブ情報など）に基づき、カードの初期化情報(`CardInitializationData`)を生成する責務を負います。 |
| **CardInitializationData** | カード生成に必要な情報（`CreatureData`、`InletAbilityProfile`リスト）を集約したデータ転送オブジェクト(DTO)。これにより、データソースと生成ロジックを疎結合に保ちます。 |
| **CardLifecycleService** | カードのライフサイクルを直接管理するサービス。`CardInitializationData` を受け取り、カードのViewへのデータ適用や、能力の登録・解除処理を実行します。 |
| **DiceInletAbilityRegistry** | 全てのアクティブなインレットの能力(`InletAbilityProfile`)を一元管理するレジストリ。インレットIDをキーとして能力を登録・取得・解除する機能を提供します。 |
| **ViewRegistry** | シーン上のViewコンポーネント（`CreatureCardView`など）を管理するレジストリ。オブジェクトプールとして機能し、非アクティブなViewを再利用のために管理します。 |
| **CreatureCardView** | クリーチャーカードの視覚表現を担当するコンポーネント。`CreatureData`に基づき、自身の見た目を更新します。また、自身に紐づく`DiceInletView`のリストを保持します。 |
| **CardSlotManager** | カードスロットの状態を管理し、カードの配置処理を実行します。 |

---

## クリーチャーカード除去フロー

クリーチャーカードが戦闘不能になった場合など、盤面から取り除かれる際のフローです。カードの能力を無効化し、オブジェクトをプールに返却して再利用に備えます。

### 1. シーケンス

1.  **Trigger**（例: `CombatManager`）が、カード除去の必要性を判断し、**CardLifecycleService** の `TeardownCard()` を呼び出します。
2.  **CardLifecycleService** は **CreatureCardView** の `GetInletViews()` を呼び出し、関連する全てのインレットのリストを取得します。
3.  各インレットに対して、以下の処理をループ実行します。
    1.  **CardLifecycleService** は **DiceInletAbilityRegistry** の `Unregister()` を呼び出し、能力の登録を解除します。
4.  **CardLifecycleService** は **CreatureCardView** の `SetDisplayActive(false)` を呼び出し、カードを非表示にします。
5.  **CardLifecycleService** は **ViewRegistry** の `ReturnCreatureCardView()` を呼び出し、カードのViewをオブジェクトプールに返却します。

### 2. 各コンポーネントの役割

| コンポーネント | 役割 |
| :--- | :--- |
| **Trigger** | カードの除去を判断するコンポーネント（例: `CombatManager`、カード自身のロジックなど）。`CardLifecycleService`に処理を依頼します。 |
| **CardLifecycleService** | カードの除去処理を統括します。カードに紐づく全てのインレット能力の登録を解除し、Viewをオブジェクトプールへ返却するよう指示します。 |
| **DiceInletAbilityRegistry** | 除去されるカードに関連する全てのインレットの能力プロファイルをレジストリから削除します。 |
| **CreatureCardView** | 自身の表示を非アクティブにし、オブジェクトプールに返却される準備をします。 |
| **ViewRegistry** | 非アクティブになった`CreatureCardView`を回収し、再利用可能なオブジェクトとしてプールします。 |

---

## データクラス参照

- **`CardInitializationData`**: `CreatureData`と`List<InletAbilityProfile>`を保持します。
- **`InletAbilityProfile`**: `DiceInletConditionSO`（条件）と`BaseInletAbilitySO`（効果）を保持します。

---

## 関連ファイル

- [sys_classes.md](./sys_classes.md)
- [sys_domain-model.md](./sys_domain-model.md)
- [class_CombatManager.md](../class/class_CombatManager.md)
- [class_CardLifecycleService.md](../class/class_CardLifecycleService.md)
- [sys_dice_inlet_design.md](./sys_dice_inlet_design.md)

---

## 更新履歴

- 2025-08-15: シーケンス図をテキストベースの記述に修正 (Gemini)
- 2025-08-15: ソースコードとの同期。シーケンス図の正確性を向上させ、各コンポーネントの役割を明確化。 (Gemini)
- 2025-08-15: 初版 (Gemini)

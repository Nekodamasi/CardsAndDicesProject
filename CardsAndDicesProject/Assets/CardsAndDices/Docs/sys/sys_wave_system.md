# sys_wave_system.md - ウェーブシステム設計書

## 概要

本ドキュメントは、戦闘における敵の出現パターンを管理する「ウェーブシステム」の設計を定義する。
このシステムは、ScriptableObjectベースのデータ駆動設計を採用し、拡張性と保守性を高めることを目的とする。

---

## データ構造 (ScriptableObject)

システムのデータは「敵単体」「敵グループ」「ウェーブ」「戦闘」の4階層のScriptableObjectによって定義される。

### 1. EnemyProfile

敵一体の戦闘における特性を定義する。

-   **ファイル名**: `EP_{EnemyName}.asset`
-   **パス**: `Assets/CardsAndDices/ScriptableObjects/EnemyProfiles`
-   **責務**:
    -   敵一体の静的なパラメータと、カードとして生成される際の基礎データを紐づける。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `FixedCardInitializer` | `FixedCardInitializer` | カード生成のベースとなるデータへの参照 |
| `AreaId` | `AreaId` (enum) | 主な出現エリア |
| `ChallengeRating` | `ChallengeRating` (enum) | 敵の強さの指標 |
| `EnemyRoleId` | `EnemyRoleId` (enum) | 戦闘における役割（アタッカー、ディフェンダー等） |
| `PowerLevel` | `int` | 敵の総合的な強さを示す数値。グループ選出の際の重み付けなどに使用する。 |

### 2. EnemyGroup

特定のテーマや条件でまとめられた `EnemyProfile` の集合。

-   **ファイル名**: `EG_{GroupName}.asset`
-   **パス**: `Assets/CardsAndDices/ScriptableObjects/EnemyGroups`
-   **責務**:
    -   ウェーブ生成時に、特定の役割やテーマを持つ敵を動的に選出するための候補リストを定義する。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `GroupName` | `string` | グループの識別名（例: "ゴブリン突撃兵", "オーク守備隊"） |
| `EnemyProfiles` | `List<EnemyProfile>` | グループに属する `EnemyProfile` のリスト |

### 3. WaveData

1ウェーブ分の敵の構成と配置を定義する。

-   **ファイル名**: `WD_{CombatId}_{WaveNumber}.asset`
-   **パス**: `Assets/CardsAndDices/ScriptableObjects/Waves`
-   **責務**:
    -   1回のウェーブで出現する敵の種類と配置場所を定義する。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `WaveNumber` | `int` | 戦闘内でのウェーブ番号 (1-indexed) |
| `EnemyPlacements` | `List<EnemyPlacement>` | このウェーブの敵配置情報のリスト |

#### 3.1. EnemyPlacement

`WaveData`内で使用される、敵一体の配置情報を定義するシリアライズ可能なクラス。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `EnemyGroup` | `EnemyGroup` | 配置する敵の候補が含まれる `EnemyGroup` への参照 |
| `LinePosition` | `LinePosition` (enum) | 配置先のLine位置（例: `TopLine`, `BottomLine`） |
| `SlotLocation` | `SlotLocation` (enum) | 配置先のスロット位置（例: `Vanguard`, `Center`） |

### 4. CombatData

一連の戦闘全体を定義する。

-   **ファイル名**: `CD_{CombatId}.asset`
-   **パス**: `Assets/CardsAndDices/ScriptableObjects/Combats`
-   **責務**:
    -   一つの戦闘における全ウェーブのシーケンスと共通ルールを定義する。

| フィールド名 | 型 | 説明 |
| :--- | :--- | :--- |
| `CombatId` | `string` | 戦闘のユニークな識別ID |
| `Waves` | `List<WaveData>` | この戦闘で発生する `WaveData` のリスト |
| `WaveInterval` | `float` | 次のウェーブが出現するまでの待機時間（秒） |
| `IsInfinite` | `bool` | 無限にウェーブが続くかどうかのフラグ |

---

## ロジック・管理クラス

ScriptableObjectで定義されたデータを解釈し、ゲームプレイを制御する。

### 1. WaveGeneratorService

`CombatData`に基づき、具体的な出現エネミーを決定する。

-   **クラス名**: `WaveGeneratorService`
-   **責務**:
    -   現在のウェーブ情報から、実際にフィールドに生成すべきカード (`FixedCardInitializer`) とその配置位置のリストを生成する。
-   **メソッド**:
    -   `GenerateWaveEnemies(CombatData combatData, int waveNumber)`
        -   **入力**: `CombatData`, `waveNumber`
        -   **処理**:
            1.  `combatData`から該当する`WaveData`を取得する。
            2.  `WaveData`内の各`EnemyPlacement`を評価する。
            3.  `EnemyGroup`の中から`EnemyProfile`を1体ランダムに選出する。
        -   **出力**: `List<(FixedCardInitializer, LinePosition)>` - 生成すべきカードの初期化データと配置位置のタプルのリスト。

### 2. CombatManager

戦闘全体のフローとウェーブの進行を管理する。

-   **クラス名**: `CombatManager`
-   **責務**:
    -   戦闘の開始から終了までの状態遷移を管理する。
    -   `WaveGeneratorService`と連携し、ウェーブを進行させる。
-   **処理フロー**:
    1.  戦闘開始時に`CombatData`をロードする。
    2.  `currentWave = 1`として最初のウェーブを開始する。
    3.  `WaveGeneratorService.GenerateWaveEnemies`を呼び出し、出現する敵のリストを取得する。
    4.  `CardGeneratorService`と`CardSlotManager`を介して、敵カードをフィールドに生成・配置する。
    5.  フィールド上の敵の全滅を検知する。
    6.  `WaveInterval`タイマーを開始する。
    7.  タイマー完了後、次のウェーブがあれば`currentWave`をインクリメントして3に戻る。なければ戦闘勝利処理へ移行する。

---

## 関連ドキュメント

-   [gdd_combat_system.md](./gdd/gdd_combat_system.md)
-   [sys_card_slot_manager.md](./sys/sys_card_slot_manager.md)

---

## 更新履歴

-   2025-08-25: 初版 (Gemini)
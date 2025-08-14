# class_CombatManager.md - コンバットマネージャー設計書

---

## 概要

このドキュメントは、`CombatManager` クラスの設計を定義します。
`CombatManager` は、ゲームの戦闘フェーズ全体の流れを制御する責務を負います。
特に、戦闘の開始、プレイヤーおよびエネミーカードの生成と配置、ウェーブの管理など、高レベルなゲームロジックを統括します。

---

## 責務

-   戦闘フェーズの開始と終了の管理。
-   プレイヤーおよびエネミーカードの生成フローのトリガーと制御。
-   ウェーブの進行管理と、それに伴うエネミーカードの追加投入。
-   カードの生成後、`CardSlotManager` を介したボード上への配置指示。

---

## プロパティ

-   **`_cardLifecycleService` (`CardLifecycleService`):**
    -   カードの生成と初期化を行うサービス。
-   **`_cardSlotManager` (`CardSlotManager`):**
    -   カードをボード上のスロットに配置するマネージャー。
-   **`_playerCardDataProvider` (`PlayerCardDataProvider`):**
    -   プレイヤーカードの初期化データを生成するプロバイダー。
-   **`_enemyCardDataProvider` (`EnemyCardDataProvider`):**
    -   エネミーカードの初期化データを生成するプロバイダー。

---

## 主要メソッド

### 1. `InitializeCombatField()`

-   **目的:** 戦闘フィールドを初期化し、戦闘を開始します。プレイヤーカードと最初のウェーブのエネミーカードを生成・配置します。
-   **引数:** なし
-   **戻り値:** なし
-   **処理フロー:**
    1.  `_playerCardDataProvider.GetCardDataList()` を呼び出し、プレイヤーカードの初期化データリストを取得します。
    2.  取得した各 `CardInitializationData` を引数として `_cardLifecycleService.CreateAndInitializeCard()` を呼び出し、プレイヤーカードを生成・初期化します。
    3.  生成された `CreatureCardView` を `_cardSlotManager.PlaceCardToEmptySlot()` を介して、プレイヤー側のスロットに配置します。
    4.  `SpawnNewWave(1)` を呼び出し、最初の敵ウェーブを生成します。

### 2. `SpawnNewWave(int waveNumber)`

-   **目的:** 指定されたウェーブ番号のエネミーカードを生成し、ボードに配置します。
-   **引数:**
    -   `waveNumber` (`int`): 生成するウェーブの番号。
-   **戻り値:** なし
-   **処理フロー:**
    1.  `_enemyCardDataProvider.GetCardDataListForWave(waveNumber)` を呼び出し、指定ウェーブのエネミーカード初期化データリストを取得します。
    2.  取得した各 `CardInitializationData` を引数として `_cardLifecycleService.CreateAndInitializeCard()` を呼び出し、エネミーカードを生成・初期化します。
    3.  生成された `CreatureCardView` を `_cardSlotManager.PlaceCardToEmptySlot()` を介して、エネミー側のスロットに配置します。

---

## 関連ファイル

- [sys_domain-model.md](../../sys/sys_domain-model.md)
- [class_CardLifecycleService.md](./class_CardLifecycleService.md)
- [class_PlayerCardDataProvider.md](./class_PlayerCardDataProvider.md)
- [class_EnemyCardDataProvider.md](./class_EnemyCardDataProvider.md)

---

## 更新履歴

- 2025-08-13: 初版 (Gemini)

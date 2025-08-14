# class_EnemyCardDataProvider.md - エネミーカードデータプロバイダー設計書

---

## 概要

このドキュメントは、`EnemyCardDataProvider` クラスの設計を定義します。
`EnemyCardDataProvider` は、エネミーのデータ（マスターデータ、ウェーブ情報、難易度設定など）から、カード生成システムが利用する `CardInitializationData` を生成する責務を負います。

---

## 責務

-   エネミーのマスターデータやウェーブ情報を読み込み、解析する。
-   解析したデータに基づき、`CardInitializationData` のリストを生成する。
-   難易度設定やイベント、ウェーブのペナルティなどによる動的なデータ変更を適用する。

---

## プロパティ

-   **`_enemyMaster` (`EnemyMasterData`):**
    -   エネミーの基本情報を保持するマスターデータ。
-   **`_combatState` (`CombatState`):**
    -   現在の戦闘状態（ウェーブ数、難易度など）を保持するデータ。

---

## 主要メソッド

### 1. `GetCardDataListForWave(int waveNumber)`

-   **目的:** 指定されたウェーブ番号に基づいて、生成すべきエネミーカードの初期化データリストを取得します。
-   **引数:**
    -   `waveNumber` (`int`): 現在のウェーブ番号。
-   **戻り値:**
    -   `List<CardInitializationData>`: エネミーのカード初期化データを含むリスト。
-   **処理フロー:**
    1.  `_enemyMaster` から、指定されたウェーブに出現するエネミーの基本情報を取得します。
    2.  `_combatState` から、現在の難易度設定やイベントによる補正情報を取得します。
    3.  取得した情報に基づき、各エネミーカードに対応する `CreatureData` と `InletAbilityProfile` を生成し、必要に応じて動的な変更（例: ステータス補正）を適用します。
    4.  生成した `CreatureData` と `InletAbilityProfile` を使用して `CardInitializationData` のインスタンスを作成します。
    5.  作成した `CardInitializationData` をリストに追加し、返します。

---

## 関連ファイル

- [sys_domain-model.md](../../sys/sys_domain-model.md)
- [class_CardLifecycleService.md](./class_CardLifecycleService.md)
- [class_CombatManager.md](./class_CombatManager.md)

---

## 更新履歴

- 2025-08-13: 初版 (Gemini)

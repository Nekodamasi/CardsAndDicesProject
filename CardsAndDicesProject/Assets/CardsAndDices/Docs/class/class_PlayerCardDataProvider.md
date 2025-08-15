# class_PlayerCardDataProvider.md - プレイヤーカードデータプロバイダー設計書

---

## 概要

このドキュメントは、`PlayerCardDataProvider` クラスの設計を定義します。
`PlayerCardDataProvider` は、プレイヤーの現在の状態（装備、キャラクターデータなど）から、カード生成システムが利用する `CardInitializationData` を生成する責務を負います。

---

## 責務

-   プレイヤーのゲームデータ（例: `PlayerProfile`）を読み込み、解析する。
-   解析したデータに基づき、`CardInitializationData` のリストを生成する。

---

## プロパティ

-   **`_playerProfile` (`PlayerProfile`):**
    -   プレイヤーのキャラクター情報や装備状態などを保持するデータソース。

---

## 主要メソッド

### 1. `GetCardDataList()`

-   **目的:** プレイヤーの現在の状態から、生成すべきカードの初期化データリストを取得します。
-   **引数:** なし
-   **戻り値:**
    -   `List<CardInitializationData>`: プレイヤーのカード初期化データを含むリスト。
-   **処理フロー:**
    1.  `_playerProfile` から、プレイヤーが現在装備しているキャラクターやアイテムの情報を取得します。
    2.  取得した情報に基づき、各カードに対応する `CreatureData` と `InletAbilityProfile` を生成します。
    3.  生成した `CreatureData` と `InletAbilityProfile` を使用して `CardInitializationData` のインスタンスを作成します。
    4.  作成した `CardInitializationData` をリストに追加し、返します。

---

## 関連ファイル

- [sys_domain-model.md](../../sys/sys_domain-model.md)
- [class_CardLifecycleService.md](./class_CardLifecycleService.md)
- [class_CombatManager.md](./class_CombatManager.md)

---

## 更新履歴

- 2025-08-13: 初版 (Gemini)

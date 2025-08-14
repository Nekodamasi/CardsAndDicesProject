# class_CardLifecycleService.md - カードライフサイクルサービス設計書

---

## 概要

このドキュメントは、`CardLifecycleService` クラスの設計を定義します。
`CardLifecycleService` は、ゲーム内のカード（`CreatureCardView`）の生成、初期化、および破棄といったライフサイクル全般を管理する責務を負います。
特に、`CardInitializationData` を受け取り、カードのインスタンス化とそれに伴うインレット能力の登録を行う高レベルなインターフェースを提供します。

---

## 責務

-   `CardInitializationData` に基づく `CreatureCardView` の生成と初期化。
-   生成されたカードに紐づくインレット能力の `DiceInletAbilityRegistry` への登録。
-   カードのオブジェクトプールからの取得と返却の管理。

---

## プロパティ

-   **`_inletRegistry` (`DiceInletAbilityRegistry`):**
    -   インレット能力の登録を行うためのレジストリ。
-   **`_cardPool` (`IObjectPool<CreatureCardView>`):**
    -   `CreatureCardView` のインスタンスを効率的に管理するためのオブジェクトプール。

---

## 主要メソッド

### 1. `CreateAndInitializeCard(CardInitializationData initData)`

-   **目的:** `CardInitializationData` に基づいて新しいカードを生成し、初期化します。
-   **引数:**
    -   `initData` (`CardInitializationData`): カード生成に必要な全ての情報を含むDTO。
-   **戻り値:**
    -   `CreatureCardView`: 初期化が完了したカードのViewコンポーネント。
-   **処理フロー:**
    1.  `_cardPool` から `CreatureCardView` のインスタンスを取得します。
    2.  `initData.CreatureData` を使用して、取得した `CreatureCardView` にクリーチャーデータを適用します。
    3.  `CreatureCardView` が持つ各インレットの `ObjectId` と、`initData.InletAbilityProfiles` に含まれる対応する `InletAbilityProfile` を取得します。
    4.  取得した `ObjectId` と `InletAbilityProfile` を引数として、`_inletRegistry.Register()` を呼び出し、インレット能力を登録します。
    5.  初期化が完了した `CreatureCardView` を返します。

### 2. `ReturnCardToPool(CreatureCardView cardView)`

-   **目的:** 使用済みのカードをオブジェクトプールに戻し、再利用可能にします。
-   **引数:**
    -   `cardView` (`CreatureCardView`): プールに戻すカードのViewコンポーネント。
-   **戻り値:** なし
-   **処理フロー:**
    1.  `cardView` に関連付けられたインレット能力を `_inletRegistry` から解除します。
    2.  `cardView` の状態をリセットし、`_cardPool` に返却します。

---

## 関連ファイル

- [sys_dice_inlet_design.md](../../sys/sys_dice_inlet_design.md)
- [sys_domain-model.md](../../sys/sys_domain-model.md)
- [class_PlayerCardDataProvider.md](./class_PlayerCardDataProvider.md)
- [class_EnemyCardDataProvider.md](./class_EnemyCardDataProvider.md)
- [class_CombatManager.md](./class_CombatManager.md)

---

## 更新履歴

- 2025-08-13: 初版 (Gemini)

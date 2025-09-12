# sys_identifiable-views.md - 識別可能Viewシステム設計書

---

## 1. 概要

本ドキュメントは、`CompositeObjectId` を利用してUI要素やゲームオブジェクトを個別に識別し、それらを操作するための「識別可能Viewシステム」の設計を定義する。

このシステムは、ユーザーインタラクションの対象となるUIや、個別の状態を持つ表示オブジェクト（例: フィールドのカード）を一意に識別することを目的とする。`CompositeObjectId` は、シーン上の特定のGameObjectインスタンスを指し示すIDであり、そのGameObjectが表示しているデータ（例: キャラクター情報）のIDとは分離される。

これにより、View（見た目）とModel（データ）の関心を分離し、データ駆動およびイベント駆動のアーキテクチャを維持したまま、複雑なViewの連携を疎結合で実現する。

---

## 2. 主要な概念

本システムは、`IIdentifiableView`, `BaseIdentifiableView`, `Presenter`, `Controller` の4つの主要な概念で構成される。

### 2.1. IIdentifiableView

-   **役割:** `CompositeObjectId` を通じて識別可能であることを示すインターフェース。
-   **責務:**
    -   `CompositeObjectId` プロパティを公開する。
    -   このインターフェースを実装するクラスは、必ず `CompositeObjectId` コンポーネントをアタッチする必要がある。

### 2.2. BaseIdentifiableView

-   **役割:** `IIdentifiableView` を実装した抽象基底クラス。具体的なView（例: `HealthView`, `AnimationView`）は、このクラスを継承して作成する。
-   **責務:**
    -   Viewの振る舞いを定義するメソッドを提供する。これらのメソッドは、外部の `Presenter` や `Controller` から呼び出されることを前提とする。
    -   自身のイベント購読は行わない。状態の変更やイベントの受信は `Presenter` または `Controller` に委任する。

### 2.3. Presenter

-   **役割:** **特定のデータモデル（Model）**と、それに対応する **`IIdentifiableView`（View）** を仲介する。
-   **責務:**
    -   Modelの状態変化を監視するイベントを購読する。
    -   Modelから受け取った情報に基づき、担当するViewの表示を更新するためのメソッドを呼び出す。
    -   一つのPresenterは、原則として一つのViewと一つのModelの関心事に集中する。

### 2.4. Controller

-   **役割:** **特定のデータモデルに直接紐付かない**イベント（例: ユーザー入力、アニメーション完了通知）を処理し、`IIdentifiableView` の振る舞いを制御する。
-   **責務:**
    -   Command Busから発行されるシステムワイドなイベントを購読する。
    -   イベントの内容に応じて、対象となるViewのメソッド（例: アニメーションの再生、ハイライトの表示）を呼び出す。

---

## 3. アーキテクチャパターン

本システムは、MVP (Model-View-Presenter) パターンとイベント駆動アーキテクチャを組み合わせた設計を採用する。

-   **Model:** ゲームの純粋なデータとビジネスロジック。
-   **View (`IIdentifiableView`):** `MonoBehaviour` を継承し、UnityのGameObjectとして表示を担当。自身のロジックは最小限に留め、状態を持たない。
-   **Presenter:** Modelの変更をViewに反映する。
-   **Controller:** Modelに依存しないイベントを処理し、Viewを操作する。
-   **Command Bus:** 各コンポーネント間の疎結合な通信を実現するイベントバス。

この構成により、「どのオブジェクトか (`CompositeObjectId`)」「何のデータか (Model)」「どう見せるか (View)」「いつ更新するか (Presenter/Controller)」が明確に分離される。

---

## 4. 実装例: クリーチャーへのダメージ適用

### 4.1. シナリオ

プレイヤーがクリーチャーカードにダメージを与える。対象のクリーチャーはHPが減少し、ダメージエフェクトが再生される。

### 4.2. 登場コンポーネント

-   **GameObject:**
    -   `CreatureCard_Prefab`: `CompositeObjectId` を持つルートオブジェクト。
        -   `HealthBar_View (HP表示用GameObject)`: `HealthView` コンポーネントを持つ。
        -   `Effect_View (エフェクト再生用GameObject)`: `DamageEffectView` コンポーネントを持つ。
-   **Model:**
    -   `CreatureModel`: HPや攻撃力などのデータを持つ純粋なC#クラス。
-   **View:**
    -   `HealthView` (`BaseIdentifiableView`): HPバーの表示を更新する `UpdateHealth(int currentHp, int maxHp)` メソッドを持つ。
    -   `DamageEffectView` (`BaseIdentifiableView`): ダメージエフェクトを再生する `PlayDamageEffect()` メソッドを持つ。
-   **Presenter:**
    -   `CreatureHealthPresenter`: `CreatureModel` の `OnHpChanged` イベントを購読。イベント受信時、対応する `HealthView` の `UpdateHealth` メソッドを呼び出す。
-   **Controller:**
    -   `CreatureEffectController`: Command Busから `DamageAppliedEvent` を購読。イベント受信時、`CompositeObjectId` をキーにして対象の `DamageEffectView` を特定し、`PlayDamageEffect` メソッドを呼び出す。

### 4.3. 処理フロー

1.  何らかのアクションにより、特定の `CreatureModel` のHPが減少する。
2.  `CreatureModel` が `OnHpChanged` イベントを発行する。
3.  `CreatureHealthPresenter` がイベントを検知し、担当する `HealthView` の `UpdateHealth` メソッドを呼び出してHPバーの表示を更新する。
4.  同時に、システムが `DamageAppliedEvent` をCommand Busに発行する。このイベントには、対象の `CompositeObjectId` が含まれる。
5.  `CreatureEffectController` がイベントを検知し、`CompositeObjectId` を使って `DamageEffectView` を探し、その `PlayDamageEffect` メソッドを呼び出してエフェクトを再生する。

---

## 5. 関連ファイル

-   [sys_domain-model.md](../sys/sys_domain-model.md)
-   [gdd_combat_system.md](../gdd/gdd_combat_system.md)

---

## 6. 更新履歴

-   2025-09-12: 初版 (Gemini)

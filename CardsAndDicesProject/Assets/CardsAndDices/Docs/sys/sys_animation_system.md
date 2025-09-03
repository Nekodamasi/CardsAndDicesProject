# sys_animation_system.md - アニメーションシステム設計書

---

## 概要

本ドキュメントは、UI要素（特に`SpriteView`）のアニメーション機能を、拡張性・保守性の高い形で実装するためのシステム設計を定義します。
アニメーションの振る舞いを「戦略（Strategy）」としてカプセル化し、データ駆動で管理することを基本方針とします。

---

## 設計方針

### 1. Strategyパターンの採用
「どのアニメーションを再生するか」という決定と、「どのようにアニメーションを再生するか」という具体的な処理を分離するため、Strategyパターンを採用します。
これにより、`SpriteView`などのクラスを変更することなく、新しいアニメーション（攻撃、被ダメージなど）を容易に追加できます。

### 2. データ駆動
アニメーションのパラメータ（再生時間、拡縮率、色など）は、すべて`ScriptableObject`として定義します。これにより、プログラマ以外（デザイナー、プランナー）でも、コードに触れることなくアニメーションの調整が可能になります。

### 3. 責務の分離
- **View (`SpriteView`など):** アニメーションの再生をトリガーし、各戦略に必要なコンポーネント群（コンテキスト）を提供する責務を持ちます。
- **Strategy (`HoverAnimationStrategy`など):** `ScriptableObject`からパラメータを読み取り、具体的なアニメーション処理（Tween操作、シェーダー変更など）を実行する責務を持ちます。
- **Profile (`HoverAnimationProfile`など):** 個々のアニメーションのパラメータを保持するデータコンテナとしての責務を持ちます。

---

## 主要コンポーネント

### 1. `AnimationContext` (クラス)
アニメーションの実行に必要なコンポーネントへの参照を集約したコンテキストクラスです。Strategyはこのクラスを介して対象のGameObjectを操作します。

| プロパティ名 | 型 | 解説 |
| :--- | :--- | :--- |
| `CoroutineRunner` | `MonoBehaviour` | アニメーション内でコルーチンを実行するための参照 |
| `TargetTransform` | `Transform` | アニメーション対象のTransform |
| `TargetRenderer` | `SpriteRenderer` | アニメーション対象のSpriteRenderer |
| `MaterialPropertyBlock` | `MaterialPropertyBlock` | シェーダーパラメータを変更するためのブロック |

### 2. `IAnimationStrategy` (インターフェース)
すべてのアニメーション戦略クラスが実装する共通のインターフェースです。

| メソッド名 | 戻り値 | 引数 | 解説 |
| :--- | :--- | :--- | :--- |
| `ExecuteAsync` | `UniTask` | `AnimationContext` | アニメーションを実行する |

### 3. `AnimationProfile` (ScriptableObject)
各アニメーションのパラメータを定義する`ScriptableObject`の基底クラス、またはその総称です。具体的なアニメーションごとに派生クラスを作成します。（例: `HoverAnimationProfile`）

### 4. 具体的な戦略クラス
`IAnimationStrategy`を実装し、特定の`AnimationProfile`と組み合わせてアニメーションを実行します。（例: `HoverAnimationStrategy`）

---

## 処理フロー例：ホバーアニメーション

1. `SpriteView`がユーザーのホバーイベントを検知します。
2. `SpriteView`は、インスペクターに設定された`HoverAnimationProfile`を元に`HoverAnimationStrategy`のインスタンスを生成します。
3. `SpriteView`は、自身のコンポーネントから`AnimationContext`を生成します。
4. `SpriteView`は、生成した`AnimationContext`を引数として、`HoverAnimationStrategy`の`ExecuteAsync`メソッドを呼び出します。
5. `HoverAnimationStrategy`は、`HoverAnimationProfile`から再生時間や拡縮率などのパラメータを読み取ります。
6. `HoverAnimationStrategy`は、`AnimationContext`を通じて`Transform`や`SpriteRenderer`を操作し、DOTweenなどを用いて実際のアニメーション処理を実行します。

---

## 関連ファイル
- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [guide_design-principles.md](../guide/guide_design-principles.md)

---

## 更新履歴
- 2025-09-03: 初版 (Gemini)

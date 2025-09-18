# sys_animation_system.md - アニメーションシステム設計書

---

## 概要

本ドキュメントは、UI要素（特に`SpriteView`やそれに類するコンポーネント）のアニメーション機能を、拡張性・保守性の高い形で実装するためのシステム設計を定義します。
アニメーションの振る舞いを「戦略（Strategy）」としてカプセル化し、データ駆動で管理することを基本方針とします。

---

## 設計方針

### 1. Strategyパターンの採用
「どのアニメーションを再生するか」という決定と、「どのようにアニメーションを再生するか」という具体的な処理を分離するため、Strategyパターンを採用します。
これにより、呼び出し元のクラス（`SpriteView`など）を変更することなく、新しいアニメーション（ホバー、ドラッグ、通常状態など）を容易に追加できます。

### 2. データ駆動
アニメーションのパラメータ（再生時間、拡縮率、色など）は、すべて`ScriptableObject`として定義します。これにより、プログラマ以外（デザイナー、プランナー）でも、コードに触れることなくアニメーションの調整が可能になります。

### 3. 責務の分離
- **呼び出し元 (`SpriteView`など):** アニメーションの再生をトリガーし、各戦略に必要なコンポーネント群（コンテキスト）を提供する責務を持ちます。
- **Strategy (`HoverAnimationStrategy`など):** `ScriptableObject`からパラメータを読み取り、具体的なアニメーション処理（DOTweenのSequence生成）を実行する責務を持ちます。
- **Profile (`HoverAnimationProfile`など):** 個々のアニメーションのパラメータを保持するデータコンテナとしての責務を持ちます。
- **Context (`AnimationContext`):** アニメーションの実行に必要なコンポーネントへの参照を集約し、Strategyに渡す責務を持ちます。

---

## 主要コンポーネント

### 1. `AnimationContext` (MonoBehaviour)
アニメーションの実行に必要なコンポーネントへの参照を集約したコンテキストクラスです。Strategyはこのクラスを介して対象のGameObjectや関連コンポーネントを操作します。`MonoBehaviour`を継承しており、自身がアタッチされたGameObjectに紐づくコンポーネントを管理します。

| プロパティ名 | 型 | 解説 |
| :--- | :--- | :--- |
| `MultiRendererVisualController` | `MultiRendererVisualController` | 複数のRendererの色や透明度を一括で制御するコントローラー |
| `TargetTransform` | `Transform` | アニメーション対象のTransform |
| `SpriteView` | `BaseSpriteView` | アニメーションの起点となるViewコンポーネント |
| `MaterialPropertyBlock` | `MaterialPropertyBlock` | シェーダーパラメータを変更するためのブロック |
| `TargetPosition` | `Vector3` | 主に移動アニメーションで利用される目標座標 |

### 2. `IAnimationStrategy` (インターフェース)
すべてのアニメーション戦略クラスが実装する共通のインターフェースです。

| メソッド名 | 戻り値 | 引数 | 解説 |
| :--- | :--- | :--- | :--- |
| `ExecuteAsync` | `Sequence` | `AnimationContext` | DOTweenの`Sequence`オブジェクトを生成して返す |

### 3. `BaseAnimationProfile` (ScriptableObject)
各アニメーションのパラメータを定義する`ScriptableObject`の抽象基底クラスです。具体的なアニメーションごとにこのクラスを継承したプロファイルを作成します。

**主な派生クラス:**
- `NormalAnimationProfile`: 通常状態のアニメーション設定
- `HoverAnimationProfile`: ホバー状態のアニメーション設定
- `DragAnimationProfile`: ドラッグ状態のアニメーション設定

### 4. 具体的な戦略クラス
`IAnimationStrategy`を実装し、特定の`AnimationProfile`と組み合わせてアニメーションの`Sequence`を生成します。

- `NormalAnimationStrategy`: 通常状態（例：非ホバー時）に戻すアニメーションを定義します。
- `HoverAnimationStrategy`: ホバー時の拡大や発光などのアニメーションを定義します。
- `DragAnimationStrategy`: ドラッグ中の拡縮やフェードなどのアニメーションを定義します。

---

## 処理フロー例：ホバーアニメーション

1. `SpriteView`がユーザーのホバーイベントを検知します。
2. `SpriteView`は、インスペクターに設定された`HoverAnimationProfile`を元に`HoverAnimationStrategy`のインスタンスを生成します。
3. `SpriteView`は、自身が持つ`AnimationContext`（または関連する`AnimationContext`）を引数として、`HoverAnimationStrategy`の`ExecuteAsync`メソッドを呼び出します。
4. `HoverAnimationStrategy`は、コンストラクタで受け取った`HoverAnimationProfile`から再生時間、拡縮率、目標色などのパラメータを読み取ります。
5. `HoverAnimationStrategy`は、`AnimationContext`を通じて`TargetTransform`や`MultiRendererVisualController`を操作し、DOTweenのAPIを用いてアニメーションの`Sequence`を構築します。
6. `ExecuteAsync`メソッドは、構築した`Sequence`オブジェクトを呼び出し元（`SpriteView`）に返却します。呼び出し元は、返された`Sequence`を再生します。

---

## 関連ファイル
- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [guide_design-principles.md](../guide/guide_design-principles.md)

---

## 更新履歴
- 2025-09-04: 現行ソースコードとの同期 (Gemini)
- 2025-09-03: 初版 (Gemini)
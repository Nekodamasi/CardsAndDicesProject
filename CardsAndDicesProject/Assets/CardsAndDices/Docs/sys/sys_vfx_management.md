# sys_vfx_management.md - VFX管理システム設計書

## 概要
本提案は、ユースケースで要求される視覚効果（パーティクル等）および音響効果（以下、VFX）の再生機能を実現するための、拡張性と再利用性に優れたシステム設計を定義する。
プロジェクトの核心思想である「関心の分離」「データ駆動」「イベント駆動」を遵守し、パフォーマンスとメンテナンス性を両立させることを目的とする。

---

## クラス設計

| クラス名 | 責務 | 主要なプロパティ/メソッド |
| :--- | :--- | :--- |
| `VfxManager` (ScriptableObject) | `VfxDefinition`に基づいたVFX再生の受付、オブジェクトプールの管理を行う。DIコンテナによって注入・管理される。 | `Initialize()`<br>`UniTask PlayVfxAsync(VfxDefinition, Vector3, Quaternion)`<br>`void StopVfx(int instanceId)` |
| `VfxPlayer` (MonoBehaviour) | プールされる個々のVFX用Prefabにアタッチされるコンポーネント。自身の再生/停止、再生完了時の`VfxManager`への通知（自動返却）といったライフサイクルを管理する。 | `Initialize(Action<VfxPlayer>)`<br>`Play(VfxDefinition)`<br>`Stop()` |
| `VfxDefinition` (ScriptableObject) | `BaseEntityDefinition`を継承。VFXのIDと設定データを兼ねる。パーティクルPrefab、AudioClip、ループ設定、再生時間などを保持する。 | `ParticlePrefab` (GameObject)<br>`AudioClip`<br>`IsLooping` (bool)<br>`Duration` (float) |
| `VfxTrigger` (MonoBehaviour) | 特定のVFXを、アタッチされたGameObjectの位置で再生する責務を持つ。他のコンポーネントやUnityEventから利用されることを想定。 | `Play()` |

---

## シーケンス解説
### 1. ユースケース: 「火属性ダメージ時にVFXを再生する」
1.  `CombatSystem` などのロジック層が、再生したい`VfxDefinition`アセット（例: `FireDamageVfx.asset`）への参照を取得する。
2.  `CombatSystem` は、その`VfxDefinition`と再生座標を指定して、イベントバスに `PlayVfxCommand` を送信する。
3.  DIコンテナから注入された`VfxManager`が `PlayVfxCommand` を受信、または直接呼び出される。
4.  `VfxManager` は、コマンドに含まれる`VfxDefinition`に基づき、対応するオブジェクトプールから `VfxPlayer` のインスタンスを取得する。
5.  `VfxManager` は、取得した `VfxPlayer` に `VfxDefinition` と一意のインスタンスIDを渡して `Play` メソッドを呼び出す。
6.  `VfxPlayer` は、`VfxDefinition`内の情報（Prefab, AudioClip）を使い、`ParticleSystem` と `AudioSource` を再生する。
7.  `VfxDefinition` がループ再生でない場合、`VfxPlayer` は再生完了後、コールバック経由で`VfxManager`に通知する。
8.  `VfxManager` は通知を受け、その `VfxPlayer` インスタンスをプールに返却する。

---

## 設計の根拠
### 1. 関心の分離 (Separation of Concerns)
-   `VfxManager`（いつ、どこで再生するか）、`VfxPlayer`（どのように再生し、終了するか）、`VfxDefinition`（何を再生するか）の責務を明確に分離した。

### 2. データ駆動 (Data-Driven)
-   VFXの振る舞いを`VfxDefinition`という`ScriptableObject`によって定義することで、コードの変更なしに新しいVFXの追加や調整が可能になる。`BaseEntityDefinition`を継承することで、既存のエンティティシステムとの一貫性も保たれる。

### 3. イベント駆動 (Event-Driven)
-   VFXの再生トリガーをイベントバス経由のコマンドに限定することで、再生要求元と `VfxManager` との間に直接的な依存関係が生まれない。これにより、システム全体の疎結合が保たれる。

---

## 関連ファイル
- [guide_design-principles.md](../guide/guide_design-principles.md)
- [sys_domain-model.md](./sys_domain-model.md)
- [sys_identity-and-name-management.md](./sys_identity-and-name-management.md)

---

## 更新履歴
- 2025-09-01: 初版 (Gemini)
- 2025-09-01: IDとして`BaseEntityDefinition`を継承した`VfxDefinition`を使用するよう設計を更新 (Gemini)
- 2025-09-01: `VfxTrigger`コンポーネントの追加を反映 (Gemini)
- 2025-09-01: `VfxManager`を規約に沿った`ScriptableObject`として修正し、ソースコードと完全に一致するよう全体を更新 (Gemini)
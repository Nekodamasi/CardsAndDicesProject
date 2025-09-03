# sys_vfx_management.md - VFX管理システム設計書

---

## 概要

本ドキュメントは、視覚効果（パーティクル等）および音響効果（以下、VFX）の再生機能を実現するための、拡張性と再利用性に優れたシステム設計を定義します。
プロジェクトの核心思想である「関心の分離」「データ駆動」を遵守し、パフォーマンスとメンテナンス性を両立させることを目的とします。

---

## クラス設計

| クラス名 | 責務 | 主要なプロパティ/メソッド |
| :--- | :--- | :--- |
| `VfxManager` (ScriptableObject) | `VfxDefinition`に基づいたVFX再生の受付、オブジェクトプールの管理を行う。DIコンテナによって注入・管理される。 | `Initialize(SoundManager)`<br>`UniTask PlayVfxAsync(VfxDefinition, Vector3, Quaternion)`<br>`void StopVfx(int instanceId)` |
| `VfxPlayer` (MonoBehaviour) | プールされる個々のVFX用Prefabにアタッチされるコンポーネント。自身の再生/停止、再生完了時の`VfxManager`への通知（自動返却）といったライフサイクルを管理する。 | `Initialize(Action<VfxPlayer>, SoundManager)`<br>`Play(VfxDefinition)`<br>`Stop()` |
| `VfxDefinition` (ScriptableObject) | `BaseEntityDefinition`を継承。VFXのIDと設定データを兼ねる。パーティクルPrefab、SEデータ、ループ設定、再生時間などを保持する。 | `ParticlePrefab` (GameObject)<br>`SEData` (SEData)<br>`IsLooping` (bool)<br>`Duration` (float) |
| `VfxTrigger` (MonoBehaviour) | 特定のVFXを、アタッチされたGameObjectの位置で再生する責務を持つ。他のコンポーネントやUnityEventから利用されることを想定。 | `Play()` |

---

## シーケンス解説

### 1. ユースケース: 「火属性ダメージ時にVFXを再生する」

1.  `CombatSystem` などのロジック層が、再生したい`VfxDefinition`アセット（例: `FireDamageVfx.asset`）への参照を取得します。
2.  `CombatSystem` は、DIコンテナから注入された`VfxManager`の`PlayVfxAsync`メソッドを、`VfxDefinition`と再生座標を引数にして呼び出します。
3.  `VfxManager` は、`VfxDefinition`に基づき、対応するオブジェクトプールから `VfxPlayer` のインスタンスを取得（または新規作成）します。
4.  `VfxManager` は、取得した `VfxPlayer` に再生座標と回転を設定し、`Play` メソッドを呼び出します。
5.  `VfxPlayer` は、`VfxDefinition`内の情報に基づき、`ParticleSystem` を再生します。
6.  同時に、`VfxDefinition`に設定された`SEData`を元に`AudioSource`を再生します。この時、`VfxPlayer`は初期化時に`SoundManager`から受け取った`AudioMixerGroup`を使用するため、SE全体の音量設定が適用されます。
7.  `VfxDefinition` がループ再生でない場合、`VfxPlayer` は再生完了後、コールバック経由で`VfxManager`に通知します。
8.  `VfxManager` は通知を受け、その `VfxPlayer` インスタンスを非アクティブ化し、プールに返却します。

---

## 設計の根拠

### 1. 関心の分離 (Separation of Concerns)
-   `VfxManager`（いつ、どこで再生するか）、`VfxPlayer`（どのように再生し、終了するか）、`VfxDefinition`（何を再生するか）の責務を明確に分離しました。
-   VFXの音響部分は`SoundManager`と`SEData`の責務とし、VFXシステムはそれを利用する形にすることで、音量などの全体設定を一元管理できるようにしています。

### 2. データ駆動 (Data-Driven)
-   VFXの振る舞いを`VfxDefinition`という`ScriptableObject`によって定義することで、コードの変更なしに新しいVFXの追加や調整が可能になります。
-   サウンド情報も`SEData`という別の`ScriptableObject`に分離することで、サウンドアセットの差し替えや再利用が容易になります。

### 3. 疎結合な再生トリガー
-   VFXの再生要求元（例: `CombatSystem`）は、`VfxManager`のインターフェースのみに依存します。`VfxPlayer`の存在やオブジェクトプールの内部実装を意識する必要はありません。
-   さらに、`VfxTrigger`コンポーネントを用意することで、ロジックを一切書かずにUnityEventなどからVFXを再生する口を提供し、デザイナーとプログラマーの作業を分離します。

---

## 関連ファイル
- [guide_design-principles.md](../guide/guide_design-principles.md)
- [sys_domain-model.md](./sys_domain-model.md)
- [sys_identity-and-name-management.md](./sys_identity-and-name-management.md)
- [sys_sound_system.md](./sys_sound_system.md)

---

## 更新履歴
- 2025-09-01: 初版 (Gemini)
- 2025-09-01: IDとして`BaseEntityDefinition`を継承した`VfxDefinition`を使用するよう設計を更新 (Gemini)
- 2025-09-01: `VfxTrigger`コンポーネントの追加を反映 (Gemini)
- 2025-09-01: `VfxManager`を規約に沿った`ScriptableObject`として修正し、ソースコードと完全に一致するよう全体を更新 (Gemini)
- 2025-09-03: ソースコードの現状に合わせ、SoundSystemとの連携を明記。クラス設計とシーケンス図を更新。 (Gemini)

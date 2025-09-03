# sys_sound_system.md - サウンドシステム設計書

---

## 概要

本作のサウンドシステム、特にSE（サウンドエフェクト）の管理と再生に関するアーキテクチャを定義します。音量設定を一元管理しつつ、SEの再生責務は各所に分散させることで、疎結合で拡張性の高い設計を目指します。

---

## 設計方針

### 1. 責務の分離
-   **SoundManager (ScriptableObject):** サウンド設定（特に音量）の管理に責務を集中させます。AudioMixerへの設定適用や、PlayerPrefsを介したユーザー設定の永続化を担当します。SEの再生そのものには関与しません。
-   **SE再生コンポーネント:** `AudioSource`を持ち、`SEData`に基づいてSEを再生する責務を持ちます。`SoundManager`から`AudioMixerGroup`を取得し、自身の`AudioSource`に設定します。

### 2. データ駆動
-   再生するSEの情報は`SEData`というScriptableObjectで管理します。これにより、再生するコンポーネントは具体的な`AudioClip`を意識することなく、参照を差し替えるだけで音の変更が可能になります。

---

## 主要クラスと責務

| クラス名 | 種別 | 責務 |
| :--- | :--- | :--- |
| `SoundManager` | ScriptableObject (新規) | - `AudioMixer`への参照を保持し、公開されている`SEVolume`パラメータを制御する。<br>- ユーザーの音量設定を`PlayerPrefs`で読み書きする。<br>- DIコンテナ（VContainer）を通じて、自身のインスタンスと`SEGroup`（AudioMixerGroup）を他コンポーネントに提供する。 |
| `SEData` | ScriptableObject (新規) | - 再生する`AudioClip`とその設定（ループ再生の可否など）を保持するデータコンテナ。 |
| `SEPlayer` | Component (新規) | - `AudioSource`を保持する汎用的なSE再生コンポーネント。<br>- `Play(SEData)`のようなメソッドを持ち、渡された`SEData`の`AudioClip`を再生する。<br>- 初期化時に`SoundManager`から`SEGroup`を取得し、自身の`AudioSource.outputAudioMixerGroup`に設定する。 |

---

## 処理フロー

### 1. ゲーム起動時の音量設定フロー
1.  `SoundManager`がロード（Initializeメソッド）される。
2.  `SoundManager`は`PlayerPrefs`からユーザーの音量設定を検索する。
3.  設定が存在すれば、その値を`AudioMixer`の`SEVolume`パラメータに設定する。
4.  設定が存在しなければ、デフォルト値を`AudioMixer`の`SEVolume`パラメータに設定する。
    -   *補足: AudioMixerのVolumeは対数（dB）であるため、線形（0.0〜1.0）の値を変換するロジックを`SoundManager`内に実装する。*

### 2. SE再生フロー
1.  何らかのイベント（例: ボタンクリック）をトリガーに、`SEPlayer`を持つオブジェクトが`Play(SEData)`を呼び出す。
2.  `SEPlayer`は、自身の`AudioSource`に`SEData`の`AudioClip`とループ設定を適用し、`AudioSource.Play()`を実行する。
3.  `AudioSource`の出力は`SEGroup`を経由するため、`SoundManager`が設定したマスターSE音量が自動的に適用される。

### 3. 音量変更フロー
1.  ユーザーが設定UI（例: `Slider`）を操作する。
2.  UIコンポーネントが`SoundManager`の`SetVolume(float volume)`のようなメソッドを呼び出す。
3.  `SoundManager`は受け取った値を`AudioMixer`の`SEVolume`パラメータに設定し、同時に`PlayerPrefs`に値を保存する。

---

## 関連ファイル
- [guide_asset_workflow.md](../guide/guide_asset_workflow.md)
- [guide_design-principles.md](../guide/guide_design-principles.md)
- [gdd_unity_specs.md](../gdd/gdd_unity_specs.md)

---

## 更新履歴
- 2025-09-02: 初版 (Gemini)
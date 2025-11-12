# gdd_unity_specs.md - Unityエンジン技術要件設計書

---

## プロジェクト設定

このセクションでは Unity プロジェクトの初期構成と UI／シーン設定をまとめます。

### 1. Sorting Layer

- Background、Default、UI などのレイヤーを定義
- UI 用スプライト／テキストは必ず “UI” レイヤーを使用

### 2. Layer

- 必要に応じて “UI” 用のレイヤーを追加
- カメラの cullingMask で描画制御

### 3. カメラ設定

- プロジェクション：Orthographic
- Size：デザインに合わせて調整
- Clipping Planes：Near = 0.1、Far = 100
- Clear Flags：Solid Color
- Background Color：プロジェクト共通の背景色
- Culling Mask：Default ＋ UI レイヤー
- Depth：0（単一カメラ運用時）
- （オプション）Pixel Perfect Camera コンポーネント追加

### 4. UI オブジェクト設定

#### 4.1 SpriteRenderer

- コンポーネント：SpriteRenderer
- Sorting Layer：UI
- Order in Layer：重なり順指定
- Pixel Per Unit：アセット解像度に合わせる
- Material：Sprites-Default

#### 4.2 TextMeshPro (Mesh)

- コンポーネント：TextMeshPro (Mesh)
- Auto Size：ON/OFFはフォントサイズに応じて設定
- Alignment：デザインに準拠
- Sorting Layer：UI
- Order in Layer：重なり順調整

### 5. ワールド配置

- すべての UI はシーン上の空オブジェクト “UIRoot” 配下にまとめる
- 各オブジェクトの Z 座標は 0f 前後に統一
- 表示順は Sorting Layer／Order in Layer で制御

---

## 技術要件

このセクションでは使用技術をまとめます。

### 1. 使用技術

- Unity 6.1 LTS
- C# (.NET 7 相当)
- DOTween（アニメーション）
- R3 イベントチャネル（イベント駆動）
- VContainer（DI）

### 2. AudioMixer: パラメータの公開（Expose）手順

`SoundManager` などのスクリプトからAudioMixerのVolumeなどを動的に制御するには、対象のパラメータを「公開（Expose）」して、スクリプトから参照できる名前を付ける必要があります。

#### 手順

1.  **AudioMixerウィンドウを開く:**
    -   Unityのメニューバーから `Window > Audio > Audio Mixer` を選択します。

2.  **対象のGroupを選択:**
    -   AudioMixerウィンドウで、操作したいパラメータを持つ`AudioMixerGroup`（例: `SFXGroup`）を選択します。

3.  **パラメータを公開する:**
    -   インスペクターに表示されている`Volume`スライダーの**名前（"Volume"というラベル）を右クリック**します。
    -   表示されたメニューから **"Expose 'Volume' to script"** を選択します。

4.  **名前を変更する:**
    -   AudioMixerウィンドウの右上にある **"Exposed Parameters"** ドロップダウンリストを開きます。
    -   今公開したパラメータ（例: `MyExposedParam`）がリストに追加されています。
    -   そのパラメータを右クリックして **"Rename"** を選択し、スクリプト側で使用する名前（例: `SEVolume`）に変更します。

この手順により、`AudioMixer.SetFloat("SEVolume", ...)` のように、スクリプトからパラメータを名前で指定して操作できるようになります。

---

### 3. AudioMixerの設定

#### 3.1 Snapshot
- Start Snapshot : fadeInMixerSnapshot
- fadeOutMixerSnapshot
    - Master
        - dB:0.0dB
    - SEGroup
        - dB:0.0dB
    - BGMGroup
        - dB:-80.0dB
- fadeInMixerSnapshot
    - Master
        - dB:0.0dB
    - SEGroup
        - dB:0.0dB
    - BGMGroup
        - dB:0.0dB

#### 3.2 groups
- Master
    - SEGroup
        - Pitch:100.00%
        - Volume:0.00dB
    - BGMGroup
        - Pitch:100.00%
        - Volume:0.00dB

---

## 関連ファイル

- [guide_rules.md](../guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](../guide/guide_files.md): ドキュメントファイル命名・管理ルール

---

## 更新履歴

- 2025-07-10: 初版 (Nekodamasi)
- 2025-07-20: 関連ファイルセクション追加 (Nekodamasi)
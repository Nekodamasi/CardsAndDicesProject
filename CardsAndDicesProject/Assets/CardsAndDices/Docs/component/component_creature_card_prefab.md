# component_creature_card_prefab.md – クリーチャーカード Prefab コンポーネント仕様

---

## 目的

CreatureCard Prefab の構成と各要素にアタッチされるコンポーネントを明確に示し、
開発者が正しくインスタンスを配置・編集できるようにする。

---

## Prefab 概要

- Prefab 名：CreatureCard.prefab
- 用途：バトル画面上のクリーチャーカード表現および操作を担う GameObject

---

## ゲームオブジェクト階層

CreatureCard (CreatureCard, SpriteRenderer, BoxCollider2D)
├─ WorldSpaceCanvas (Canvas)
│ ├─ NameText (TMP_Text)
│ ├─ AttackText (TMP_Text)
│ ├─ HealthText (TMP_Text)
│ ├─ ShieldText (TMP_Text)
│ ├─ CooldownText (TMP_Text)
│ └─ EnergyText (TMP_Text)
├─ DiceInlet1 (RectTransform, DiceInlet)
├─ DiceInlet2 (RectTransform, DiceInlet)
└─ Collider (BoxCollider2D)

---

## 各要素の詳細

### 1. CreatureCard (Root)

- コンポーネント
    - **`CreatureCardController` (MonoBehaviour)**
    - **`CreatureCardView` (MonoBehaviour)**
    - `SpriteInputHandler` (MonoBehaviour)
    - `IdentifiableGameObject` (MonoBehaviour)
    - `SpriteRenderer`
    - `BoxCollider2D`

#### CreatureCardController

- **役割:**
    - クリーチャーカードのロジック（状態）を管理するコントローラー。
    - PlayerZoneの満員状態を監視し、自身がHandZoneにある場合に、カードのインタラクション（ドラッグ可否など）や見た目を制御します。
- **シリアライズフィールド:**
    - `_view` (`CreatureCardView`): 制御対象のViewコンポーネント。
    - `_identifiable` (`IdentifiableGameObject`): 自身のIDを識別するためのコンポーネント。
    - `_playableProfile` (`InteractionProfile`): 通常時に使用するインタラクションプロファイル。
    - `_unplayableProfile` (`InteractionProfile`): 満員時に使用するインタラクションプロファイル（ドラッグ不可に設定）。

### 2. WorldSpaceCanvas

- 用途：ステータス（名前／攻撃力など）をカード上に表示
- コンポーネント
- Canvas (Render Mode = World Space)

#### 2.1 NameText…EnergyText

- 各 TMP_Text コンポーネント
- 内容：cardData の各プロパティを表示

### 3. DiceInlet

- 用途：ダイスドロップ受け口
- コンポーネント
- DiceInlet
- シリアライズフィールド
- triggerData : InletTriggerConditionData

### 4. Collider

- 用途：カードのドラッグ／クリック検知
- コンポーネント
- BoxCollider2D

---

## インタラクション

- mouseover
  - `CardHoverCommand` 発行
- CardHoverCommand
  - カードを1.2倍にスケールアニメーション
  - カードのヴィジュアルを強調表示
  - 効果音再生
  - ツールチップ表示
- mouseout
  - カードを1.0倍にスケールアニメーション
  - カードのヴィジュアルを通常表示
  - ツールチップ非表示
- drag
  - `CardBeginDragCommand` 発行
- CardBeginDragCommand
  - カードを1.0倍に変更
  - カードのヴィジュアルをドラッグ表示
  - 効果音再生
  - `DiceDroppedChannel` 発行
- drop
  - `CardDropCommand` 発行
  - 配置成功なら `CardSlot` に配置
  - 配置失敗なら Drag 時の `CardSlot` に移動アニメーション
  - 移動アニメーション完了後に通常表示

---

## クラス仕様

### 1. CreatureCard

- シリアライズフィールド
- cardData : CreatureCardData
- nameText : TMP_Text
- attackText : TMP_Text
- healthText : TMP_Text
- shieldText : TMP_Text
- cooldownText : TMP_Text
- energyText : TMP_Text
- diceInlet1 : DiceInlet
- diceInlet2 : DiceInlet

- Prefab 名：CreatureCard.prefab  
- データ：ScriptableObject の `CreatureCardData` 参照  
- ヴィジュアルデータ：ScriptableObject の `CreatureCardVisualData` 参照  
- 視覚要素：SpriteRenderer  
- 文字表示：TextMeshPro (Mesh)  
- 固有能力用ダイスインレット（DiceInlet.prefab）を２つ固定配置  
- コンポーネント：CreatureCard, Collider, Canvas (Render Mode=World Space)

---

## 更新履歴

- 2025-07-17: 初版 (Nekodamasi)

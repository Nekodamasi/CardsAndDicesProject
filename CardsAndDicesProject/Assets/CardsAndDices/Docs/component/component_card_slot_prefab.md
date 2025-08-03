# component_creature_card_prefab.md – クリーチャーカード Prefab コンポーネント仕様

## 目的

CardSlot Prefab の構成と各要素にアタッチされるコンポーネントを明確に示し、  
開発者が正しくインスタンスを配置・編集できるようにする。

## Prefab 概要

- Prefab 名：CardSlot.prefab  
- 用途：バトル画面上のクリーチャーカード表現および操作を担う GameObject  

## ゲームオブジェクト階層

CardSlotPrefab
└─ CardSlot (CardSlot, SpriteRenderer, BoxCollider2D)


## 各要素の詳細

### CreatureCard (Root)

- コンポーネント  
  - CreatureCard (MonoBehaviour)  
  - SpriteRenderer  
  - BoxCollider2D（ドラッグ検知用）  
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

### WorldSpaceCanvas

- 用途：ステータス（名前／攻撃力など）をカード上に表示  
- コンポーネント  
  - Canvas (Render Mode = World Space)  
  - RectTransform  

#### NameText…EnergyText

- 各 TMP_Text コンポーネント  
- 内容：cardData の各プロパティを表示  

### DiceInlet

- 用途：ダイスドロップ受け口  
- コンポーネント  
  - RectTransform  
  - DiceInlet  
- シリアライズフィールド  
  - triggerData : InletTriggerConditionData  

### Collider

- 用途：カードのドラッグ／クリック検知  
- コンポーネント  
  - BoxCollider2D  

## 注意事項

- Canvas 以下の UI 要素は Prefab 内で固定配置し、動的生成は行わない。  
- DiceInlet は Prefab 側で 1:1 マッピングし、再利用／生成負荷を抑える。  

## 更新履歴

- 2025-07-14: 初版 (Copilot)  

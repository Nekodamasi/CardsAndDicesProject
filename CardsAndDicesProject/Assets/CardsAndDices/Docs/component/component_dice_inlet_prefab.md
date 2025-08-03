# component_dice_inlet_prefab.md – ダイスインレット Prefab コンポーネント仕様

## 目的

DiceInlet Prefab の構成と各要素にアタッチされるコンポーネントを明確に示し、  
クリーチャーカード上のダイスドロップ受け口として正しく配置・編集できるようにする。

## Prefab 概要

- Prefab 名：DiceInlet.prefab  
- 用途：クリーチャーカードの能力発動ポイントとなるダイスドロップ受け口 UI 要素  

## ゲームオブジェクト階層

DiceInlet (RectTransform, CanvasRenderer, Image, DiceInlet)
└─ Highlight (RectTransform, CanvasRenderer, Image)

## 各要素の詳細

### DiceInlet (Root)

- コンポーネント  
  - RectTransform  
  - CanvasRenderer  
  - Image (raycastTarget = true)  
  - DiceInlet (MonoBehaviour)  

- シリアライズフィールド  
  - triggerData : InletTriggerConditionData  
  - highlightImage : Image  

### Highlight

- 用途：ダイスドロップ可能時の強調表示  
- コンポーネント  
  - RectTransform  
  - CanvasRenderer  
  - Image (ハイライト用スプライト)  

## 注意事項

- Image は UI.GraphicRaycaster 経由でドロップ検知に利用する。  
- Highlight は初期非表示とし、DiceInlet 内で Enable/Disable を制御する。  
- RectTransform のサイズ・アンカーは CreatureCard Prefab 側と一致させること。  

## 更新履歴

- 2025-07-14: 初版 (Copilot)  

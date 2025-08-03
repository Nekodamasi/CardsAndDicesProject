# gdd_game_object_specs.md - ゲームオブジェクト仕様

---

## ゲームオブジェクト仕様

### 1. クリーチャーカード

- Prefab 名：CreatureCard.prefab
- データ：ScriptableObject の `CreatureCardData` 参照
- ヴィジュアルデータ：ScriptableObject の `CreatureCardVisualData` 参照
- 視覚要素：SpriteRenderer
- 文字表示：TextMeshPro (Mesh)
- 固有能力用ダイスインレット（DiceInlet.prefab）を２つ固定配置
- コンポーネント：CreatureCard, Collider, Canvas (Render Mode=World Space)
- ステータスアイコン（StatusIcon.prefab）を５つ配置
　- AttackStatusIcon
　- HealthStatusIcon
　- ShieldStatusIcon
　- CooldownStatusIcon
　- EnergyStatusIcon

#### 1.1 クリーチャーカード インタラクション

- mouseover
  - `CardHoverCommand` 発行
- `CardHoverCommand` 購読
  - カードを1.2倍にスケールアニメーション
  - カードのヴィジュアルを強調表示
  - 効果音再生
  - ツールチップ表示
- mouseout
  - `CardMouseoutCommand` 発行
- `CardMouseoutCommand` 購読
  - カードを1.0倍にスケールアニメーション
  - カードのヴィジュアルを通常表示
  - ツールチップ非表示
- drag
  - `CardBeginDragCommand` 発行
- `CardBeginDragCommand` 購読
  - カードを1.0倍にスケールアニメーション
  - カードのヴィジュアルをドラッグ表示
  - 効果音再生
  - `DiceBeginDragChannel` 発行
- `DiceBeginDragChannel` 購読
  - カードを1.0倍にスケールアニメーション
- drop
  - `CardDropCommand` 発行
- `CardDropCommand` 購読
  - 配置成功なら `CardSlot` に配置
  - 配置失敗なら Drag 時の `CardSlot` に移動アニメーション
  - 移動アニメーション完了後に通常表示
  - `DiceDroppedChannel` 発行
  

### 2. カードスロット

- Prefab 名：CardSlot.prefab
- データ：ScriptableObject の `CardSlotData` 参照
- 視覚要素：SpriteRenderer
- コンポーネント：CardSlot, Collider, Canvas (Render Mode=World Space)

#### 2.1 カードスロット インタラクション

- dragenter
  - `SlotHoverCommand` 発行
  - ドロップ可能スロットとして強調表示
  - 効果音再生
- dragexit
  - `SlotUnhoverCommand` 発行
  - 強調表示を解除
- drop
  - `CardSlotReceiveCommand` 発行
  - 配置成功なら `CreatureCard` を子オブジェクトとして保持

### 3. ダイス

- Prefab 名：Dice.prefab
- データ：ScriptableObject の `DiceData` 参照
- ヴィジュアルデータ：ScriptableObject の `DiceVisualData` 参照
- 視覚要素：SpriteRenderer
- コンポーネント：Dice, Collider

#### 3.1 ダイス インタラクション

- mouseover
  - Dice を 1.2 倍にスケールアニメーション
  - Dice のヴィジュアルを強調表示
  - 効果音再生
- mouseout
  - Dice を 1.0 倍にスケールアニメーション
  - Dice のヴィジュアルを通常表示
- drag
  - `DiceBeginDragCommand` 発行
  - Dice のヴィジュアルをドラッグ表示
  - 効果音再生
- drop
  - `DiceDropCommand` 発行
  - 配置成功なら ダイスを消費
  - 配置失敗なら元位置へ戻るアニメーション

### 4. ダイススロット

- Prefab 名：DiceSlot.prefab
- 視覚要素：SpriteRenderer
- コンポーネント：DiceSlot

#### 4.1 ダイススロット インタラクション

- 置き場としての機能のみなので、インタラクションなし

### 5. ダイスインレット

- Prefab 名：DiceInlet.prefab
- データ：ScriptableObject の `InletData` 参照
- ヴィジュアルデータ：ScriptableObject の `DiceInletVisualData` 参照
- 視覚要素：SpriteRenderer
- 文字表示：TextMeshPro (Mesh)
- コンポーネント：DiceInlet, Collider, Canvas (Render Mode=World Space)

#### 5.1 ダイスインレット インタラクション

- dragenter
  - `DiceInletHoverCommand` 発行
  - ドロップ可能インレットとして強調表示
- dragexit
  - `DiceInletUnhoverCommand` 発行
  - 強調表示を解除
- drop
  - `DiceInletReceiveCommand` 発行
  - 成功なら Inletの発動可否チェック → 発動
  - 失敗なら元位置へ戻るアニメーション

### 6. ステータスアイコン

- Prefab 名：StatusIcon.prefab
- ヴィジュアルデータ：ScriptableObject の `StatusIconVisualData` 参照
- 視覚要素：SpriteRenderer
- 文字表示：TextMeshPro (Mesh)
- コンポーネント：StatusIcon, Canvas (Render Mode=World Space)

#### 6.1 ステータスアイコン インタラクション

- StatusUpdateChannel 購読
  - 表示している値とCardの現在値が異なる場合、シェイクアニメーション＋表示更新

---

## データ定義

### 1. CreatureCardData

- creatureId：クリーチャーカードを一意に識別する文字列
- attack：攻撃力
- health：体力
- shield：シールド値
- cooldown：クールダウンダイス数
- energy：特殊な能力値
- abilities：固有能力データのリスト（`CreatureAbilityData` 型）
- baseAttackRange：デフォルト攻撃範囲（`EffectRangeData`）

### 2. CreatureAbilityData

- abilityId：能力を一意に識別する文字列
- triggerType：発動タイミングを表す列挙（OnAttacked, OnKill, OnPositionChanged など）
- parameters：効果量や持続ターンなど能力パラメータを保持するリスト
- EffectRange：効果範囲（`EffectRangeData`）

### 3. InletTriggerConditionData

- inletTriggerConditionId：インレット発動条件を一意に識別する文字列
- allowedDiceRolls：6個のBooleanのリスト。1～6のダイスの目の許可状態を保持する
- inletTriggerType：発動条件のタイプ（AllowedDiceType, CountdownType）
- maxCount：発動に必要なダイス目の合計値
- IsSealed：封印フラグ。オンの場合はインレット発動時にinLetAbilitiesの固有能力をターン終了まで封印する

### 4. EffectData

- effectId：効果を一意に識別する文字列
- effectType：効果の種類を表す列挙（AttackUp, HealthRegen, ShieldBreak など）
- parameters：効果量や持続ターン数などを定義するパラメータ辞書
- durationType：持続条件の種類を表す列挙（TurnCount, OnCondition, Instant など）
- durationTurns：持続ターン数（durationType が TurnCount の場合）

### 5. DiceData

- diceId：ダイスを一意に識別する文字列

### 6. DiceVisualData

- diceSprite：ダイスのSprite

### 7. CreatureCardVisualData

- creatureSprite：カード中央に表示するメインアートワーク用のSprite

### 8. DiceInletVisualData

- diceInletSprite：ダイスインレットのSprite

### 9. CardSlotData

- cardSlotId：カードスロットを一意に識別する文字列
- lineType：Line位置情報（TopLine, BottomLine）
- slotPosition ：Position位置情報（Vanguard, Center, Rear ）

### 10. EffectRangeData

- EffectRange：効果範囲を一意に識別する文字列
- teamType：チーム（Player, Enemy）
- lineType：Line位置情報（TopLine, BottomLine）
- slotPosition ：Position位置情報（Vanguard, Center, Rear ）

### 11. InletData

- inletId：インレットを一意に識別する文字列
- inletTriggerConditionData：インレット発動条件（`inletTriggerConditionData`）
- inLetAbilities：インレットに紐づいている固有能力（`CreatureAbilityData` 型）

### 12. StatusIconVisualData

- StatusIconSprite：アイコンのSprite


---

## ゲームオブジェクト構成図

BattleScreen
├─ Managers (Empty GameObject)
│   ├─ AttackPhaseManager (AttackPhaseManager)
│   ├─ DiceSumManager (DiceSumManager)
│   ├─ EffectManager (EffectManager)
│   ├─ FXManager (FXManager)
│   ├─ VFXManager (VFXManager)
│   ├─ UIStateMachine (UIStateMachine)
│   ├─ CommandInvoker (CommandInvoker)
│   └─ AbilityManager (AbilityManager)
├─ UIRoot (GameObject, Canvas (World Space), GraphicRaycaster)
│   ├─ PlayerZone (Empty GameObject)
│   │   ├─ TopLine (Empty GameObject)
│   │   │   ├─ VanguardSlot (CardSlot)
│   │   │   ├─ CenterSlot (CardSlot)
│   │   │   └─ RearSlot (CardSlot)
│   │   └─ BottomLine (Empty GameObject)
│   │       ├─ VanguardSlot (CardSlot)
│   │       ├─ CenterSlot (CardSlot)
│   │       └─ RearSlot (CardSlot)
│   ├─ EnemyZone (Empty GameObject)
│   │   ├─ TopLine (Empty GameObject)
│   │   │   ├─ VanguardSlot (CardSlot)
│   │   │   ├─ CenterSlot (CardSlot)
│   │   │   └─ RearSlot (CardSlot)
│   │   └─ BottomLine (Empty GameObject)
│   │       ├─ VanguardSlot (CardSlot)
│   │       ├─ CenterSlot (CardSlot)
│   │       └─ RearSlot (CardSlot)
│   ├─ HandZone (Empty GameObject)
│   │   ├─ HandSlot1 (CardSlot)
│   │   ├─ HandSlot2 (CardSlot)
│   │   ├─ HandSlot3 (CardSlot)
│   │   ├─ HandSlot4 (CardSlot)
│   │   ├─ HandSlot5 (CardSlot)
│   │   ├─ HandSlot6 (CardSlot)
│   │   ├─ HandSlot7 (CardSlot)
│   │   └─ HandSlot8 (CardSlot)
│   ├─ DiceZone (Empty GameObject)
│   │   ├─ DiceSlot1 (DiceSlot)
│   │   ├─ DiceSlot2 (DiceSlot)
│   │   ├─ DiceSlot3 (DiceSlot)
│   │   ├─ DiceSlot4 (DiceSlot)
│   │   ├─ DiceSlot5 (DiceSlot)
│   │   ├─ DiceSlot6 (DiceSlot)
│   │   ├─ DiceSlot7 (DiceSlot)
│   │   └─ DiceSlot8 (DiceSlot)
│   └─ TooltipCanvas (GameObject, Canvas (World Space), TooltipController)
└─ SceneData (Empty GameObject)
    ├─ CreatureCardData (ScriptableObject)
    ├─ CreatureAbilityData (ScriptableObject)
    ├─ InletTriggerConditionData (ScriptableObject)
    ├─ EffectData (ScriptableObject)
    ├─ EffectRangeData (ScriptableObject)
    ├─ DiceData (ScriptableObject)
    ├─ DiceVisualData (ScriptableObject)
    ├─ CreatureCardVisualData (ScriptableObject)
    ├─ DiceInletVisualData (ScriptableObject)
    └─ CardSlotData (ScriptableObject)

---

## 更新履歴

- 2025-07-10: 初版 (Nekodamasi)
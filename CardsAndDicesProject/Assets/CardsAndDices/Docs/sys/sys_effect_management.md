# sys_effect_management.md - エフェクト管理設計書

## 目的

システム全体でのバフ／デバフ効果の適用および寿命管理を統一的に実装する。

---

## 範囲

- `EffectData`、`EffectInstance`、`EffectManager` の設計  
- コマンドとの連携およびイベント駆動フローの定義  

---

## データ定義

### 1. EffectData

- バフ／デバフ効果内容を定義する ScriptableObject  
- 主なフィールド  
  - `effectId`: 効果識別子  
  - `name`: 効果名  
  - `value`: ステータス増減量  
  - `targetType`: 適用対象タイプ  
  - `durationType`: 持続条件タイプ（TurnCount, CooldownReset, EventTrigger）  
  - `durationValue`: 持続ターン数または条件値  
  - `eventTriggers`: 対応するイベント名一覧  

### 2. EffectInstance

- `EffectData` を参照し、カードごとに生成される実行時エフェクトインスタンス  
- 主なプロパティ
  - `cardId`: 対象カード識別子
  - `data`: 参照先の `EffectData`  
  - `remainingTurns`: 残存ターン数  
  - `isExpired`: 有効期限切れフラグ  
- 主なメソッド  
  - `Initialize(effectData, cardId)`: インスタンス初期化  
  - `OnEvent(eventData)`: イベント受信処理  
  - `CheckExpiration()`: 寿命判定  

---

## EffectManager

- バフ／デバフ効果を一元管理する ScriptableObject  
- 責務  
  - 登録された `EffectInstance` のリスト管理  
  - `TurnStartEvent`、`CooldownResetEvent` の購読  
  - イベント受信時に `remainingTurns` を更新し、`isExpired` を更新。  
  - `RegisterEffect(effectInstance)` / `RemoveEffect(effectInstance)` の提供  

---

## イベントフロー

- `TurnStartEvent`: すべての `EffectInstance` の残存ターン数を減少  
- `CooldownResetEvent`: `CooldownCommand` 実行後、ペイロードに `cardId` を含めて発行  
- カスタムイベント (`EventTrigger`): 任意条件で効果発動または終了  

---

## コマンド連携

- `BuffApplyCommand` / `DebuffApplyCommand`  
  - `EffectInstance` を生成し、`EffectManager.RegisterEffect()` を実行  
- `EffectExpirationCommand`  
  - 寿命切れの `EffectInstance` を `EffectManager.RemoveEffect()` で破棄  
- すべてのコマンドは `CommandInvoker` を介してキューイング・実行し、履歴登録を行う  

---

## 関連ファイル

- [gdd_combat_system.md](../gdd/gdd_combat_system.md)

---

## 更新履歴

- 2025-07-12: 初版 (Technical Writer)  
- 2025-08-15: EffectManagerをScriptableObjectとして再定義 (Gemini)
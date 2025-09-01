# sys_cooldown_processing.md - クールダウン処理システム設計書

## 概要
本ドキュメントは、戦闘中のターン進行などに伴い、全てのクリーチャーのクールダウンを特定の順序で処理するためのシステム設計を定義します。
プロジェクトの設計思想、特にイベント駆動と、UIの応答性を維持するための非同期処理のベストプラクティスを遵守し、信頼性と拡張性の高い実装を目指します。

---

## クラス・コマンド設計

| 名前 | 種別 | 責務 |
| :--- | :--- | :--- |
| `ProcessAllCreaturesCooldownCommand` | コマンド (struct) | 全クリーチャーのクールダウン処理を開始するきっかけとなるイベントを通知します。このコマンド自体はデータを持つ必要はありません。 |
| `CombatManager` | マネージャ (class) | `ProcessAllCreaturesCooldownCommand`を購読し、クールダウン処理全体の非同期フローを管理・実行する責務を持ちます。 |

---

## シーケンス解説

クールダウン処理は、以下の非同期フローで実行されます。

1.  ゲームの進行を管理するシステム（例: `TurnManager`）が、クールダウン処理を開始するタイミングで`ProcessAllCreaturesCooldownCommand`をイベントバスに発行します。
2.  `CombatManager`が`ProcessAllCreaturesCooldownCommand`を受信し、関連付けられた`HandleCooldownProcessingAsync`のような非同期メソッドを実行します。
3.  `CombatManager`は`CardSlotManager`（または関連リポジトリ）に問い合わせ、`Hand`を除く全ての`CardSlot`の情報を取得します。
4.  取得したスロット情報を、以下の優先順位でソートします。
    1.  チーム (`Enemy`が先、`Player`が後)
    2.  スロット位置 (`Vanguard` → `Center` → `Rear`)
5.  `CombatManager`は、ソートされた順序でスロットのリストを`foreach`でループ処理します。
6.  ループ内の各スロットに配置されているクリーチャーに対して、以下の処理を**順番に**行います。
    a.  そのクリーチャーのクールダウンを1減少させるため、`CreatureCooldownChangedCommand`をイベントバスに発行します。
    b.  変更されたクールダウン値をUIに反映させるため、`CreatureCardUpdateDisplayCommand`をイベントバスに発行します。
    c.  `await UniTask.Delay(TimeSpan.FromSeconds(0.2));` を実行し、次のクリーチャーの処理に進む前に0.2秒間、非同期待機します。
7.  リスト内の全てのクリーチャーの処理が完了すると、`HandleCooldownProcessingAsync`メソッドが終了します。この処理中、メインスレッドはブロックされず、UIの応答性は維持されます。

---

## 設計の根拠

### 1. イベント駆動
クールダウン処理の開始を`ProcessAllCreaturesCooldownCommand`という疎結合なイベントにすることで、処理の実行者(`CombatManager`)と、処理の開始を指示する者（例: `TurnManager`）を分離しています。これにより、将来的な仕様変更（例: 特定のスキルでクールダウンを発生させる）にも柔軟に対応できます。

### 2. 関心の分離
`CombatManager`は、どのクリーチャーをどの順番で処理するかという「フローの制御」に責務を集中させています。実際のクールダウン値の変更やUIの更新は、既存のそれぞれのコマンドに委任しており、各コンポーネントが単一の責任を持つという原則を守っています。

### 3. 応答性の確保
各クリーチャー間の待機処理に`UniTask.Delay`による非同期の待機を導入することで、クールダウン処理という時間のかかるプロセス中にゲームが停止（フリーズ）することを防ぎます。これにより、プレイヤーは常に応答性の高いUI操作を維持できます。

---

## 関連ファイル
- [guide_design-principles.md](../guide/guide_design-principles.md)
- [sys_domain-model.md](./sys_domain-model.md)
- [class_CombatManager.md](../class/class_CombatManager.md)

---

## 更新履歴
- 2025-09-01: 初版 (Gemini)

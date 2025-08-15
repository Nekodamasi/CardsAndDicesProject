# guide_files.md - ドキュメントファイル命名・管理ルール

---

## 概要

このドキュメントは、プロジェクト内の**ドキュメントファイルそのものの命名、分類、およびファイルシステム上での管理方法**を定義します。ファイルの一貫した構成と識別を目的とします。

---

## ファイル命名規則

### 1. プレフィックス一覧

ドキュメントの種類に応じて、以下のプレフィックスを使用します。

| 設計書の種類           | プレフィックス | 例                                 |
| :--------------------- | :------------- | :--------------------------------- |
| 概要設計書             | `guide_`       | `guide_overview.md`, `guide_rules.md` |
| システム設計書         | `sys_`         | `sys_domain-model.md`, `sys_classes.md` |
| UI設計書               | `ui_`          | `ui_inventory.md`                  |
| フォーマット関連       | `format_`      | `format_ui.md`                     |
| GDD                    | `gdd_`         | `gdd_main.md`, `gdd_combat_system.md` |
| コンポーネント仕様書   | `component_`   | `component_card_slot_prefab.md`      |
| クラス設計書           | `class_`       | `class_PlayerController.md`        |

### 2. 一般ファイル命名規則

- スネークケース（snake_case）を使用し、ファイル名を小文字に統一します。
- 内容に応じた明確な命名を心がけます（例: `ui_mainmenu.md` はメインメニューのUI設計書）。

### 3. クラス設計書命名規則

- `class_[クラス名].md` の形式を使用します。
- クラス名はPascalCaseで記述します（例: `class_PlayerController.md`）。

### 4. 拡張子

- Markdown形式の `.md` を使用します。

---

## ファイル管理と配置

### 1. フォルダ階層の原則

ドキュメントファイルは、その内容と関連性に応じて適切なフォルダに配置します。

- `Docs/guide/`: プロジェクト全体のガイドラインや共通ルールに関するドキュメント。
- `Docs/gdd/`: ゲームデザインドキュメント（GDD）やゲーム全体の設計に関するドキュメント。
- `Docs/sys/`: 各システムの詳細設計に関するドキュメント。
- `Docs/component/`: 各コンポーネントの仕様に関するドキュメント。
- `Docs/class/`: 各クラスの詳細設計に関するドキュメント。
- `Docs/ui/`: 各UIのトインタラクション設計に関するドキュメント。 
 - `Scripts/Abilities/`: ScriptableObjectベースのアビリティ定義
 - `Scripts/Animation/`: アニメーション関連のScriptableObject
 - `Scripts/Commands/`: コマンドパターンで使用されるコマンド定義
 - `Scripts/Core/`: プロジェクト全体で共通の基盤コード（インターフェース、抽象クラス、汎用ユーティリティ）
 - `Scripts/Data/`: 純粋なデータ定義（ScriptableObject、構造体、クラス）
 - `Scripts/Domain/`: ゲームのコアロジック、データモデル、エンティティ
 - `Scripts/Factory/`: オブジェクトの生成ロジックをカプセル化
 - `Scripts/Installers/`: DIコンテナのインストールロジック
 - `Scripts/Manager/`: 複数のコンポーネントやシステムを統括・管理する高レベルなロジック
 - `Scripts/Orchestrator/`: 複数のシステムやコンポーネント間の複雑な連携を調整
 - `Scripts/Presenter/`: ViewとModelの仲介役
 - `Scripts/Repository/`: データの永続化や取得を担当
 - `Scripts/Service/`: 特定の機能を提供するサービス層（データ操作、外部API連携など）
 - `Scripts/State/`: ステートマシン関連のクラス
 - `Scripts/Strategy/`: 特定の振る舞いをカプセル化し、交換可能なアルゴリズムを提供
 - `Scripts/Tester/`: テスト関連のスクリプト
 - `Scripts/UI/`: UIコンポーネント（MonoBehaviour）

### 2. バージョン管理

- ドキュメントファイルはGitでバージョン管理されます。
- `.gitignore`ファイルを使用して、Gitの管理対象から除外すべきファイルやフォルダを適切に設定します。

---

## 関連リンク

- [guide_rules.md](./guide_rules.md): ドキュメント作成・記述ルール

---

## 更新履歴

- 2025-08-15: フォルダ階層の原則を更新しソースプロ
- 2025-07-18: ドキュメントの住み分けに伴う内容の再構成 (Gemini - Technical Writer for Game Development)
グラムのディレクトリ階層を明確化
- 2025-07-10: 初版 (Nekodamasi)

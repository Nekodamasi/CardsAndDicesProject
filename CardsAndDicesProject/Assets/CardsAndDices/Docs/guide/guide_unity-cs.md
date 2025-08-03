# guide_unity-cs.md - Unity C# コード出力ガイド

このドキュメントは、AI に Unity 用 C# ソースコードを生成させる際の共通ルールとフォーマットをまとめたガイドです。
生成されたコードは、以下のガイドラインに従って記述してください。

---

## 概要

このガイドでは、AI に以下のポイントを徹底させる方法を示します。

- Namespace の指定
- XML ドキュメンテーションコメント（`<summary>` タグ）の充実
- コード内コメントの充実
- コードの可読性を高める命名規則
- 一貫したコーディングスタイル
- プロジェクトのファイル構成

---

## 使い方

1. AI プロンプトにこのガイドを添付する
2. 「このガイドに従って、〇〇クラスを生成してください」とリクエスト
3. 生成されたコードがガイドラインに沿っているかチェック

---

## コード生成ルール

### 1. Namespace の指定

- すべてのクラスは `CardsAndDice` 名前空間内に配置する。
- ファイル冒頭で必ず以下を記述する。
```csharp
namespace CardsAndDice 
{
    // クラス定義はここから
}
```

### 2. XML ドキュメンテーションコメント

- クラス、プロパティ、メソッド、イベントには必ず `<summary>` タグを記述する。
- 引数や戻り値の説明には `<param>`、`<returns>` タグを使用する。
- コメントは具体的かつ簡潔に記述し、動作や役割が一目でわかるようにする。

### 3. コード内コメント

- 処理の意図や注意点、設計上の背景などを随所にコメントで残す。
- 複雑なアルゴリズムには、ステップごとにコメントを挿入する。

### 4. 命名規則

- **クラス名:** PascalCase を使用し、略語も大文字で統一（例: `CombatSystem`, `EffectManager`）。単一責務を示す名詞または名詞句とします。
- **インタフェース名:** `I` をプレフィックスに付与し PascalCase を使用（例: `ICommand`, `IEventSubscriber`）。
- **抽象クラス名:** `Base` をプレフィックスに付与し PascalCase を使用（例: `BaseSystem`）。
- **メソッド名:** PascalCase を使用し、動詞または動詞句とします（例: `Initialize`, `ExecuteCommand`）。
- **変数名:**
    - ローカル変数: camelCase を使用し短く表現（例: `currentWave`, `remainingTurnCount`）。
    - プライベートフィールド: `_` プレフィックスと camelCase を使用（例: `_eventBus`, `_cooldownTimer`）。
    - 定数: 全大文字とアンダースコアを使用（例: `MAX_HAND_SIZE`, `DEFAULT_COOLDOWN`）。

### 5. コーディングスタイル

- **インデント:** タブを使用します。
- **ブレース配置:** メソッド／クラスの開始位置にブレースを配置し、終わりのブレースは改行後に置きます。
- **空行:** メソッド間、および論理的な区切りのコードブロック間には1行の空行を入れます。

---

## ファイル構成

### 1. フォルダ階層

- `Assets\CardsAndDices\Scripts/Systems`：ゲームシステム関連
- `Assets\CardsAndDices\Scripts/Commands`：コマンドパターン関連
- `Assets\CardsAndDices\Scripts/Data`：データクラス
- `Assets\CardsAndDices\Scripts/UI`：UIコンポーネント
- `Assets\CardsAndDices\Scripts\Shared`：システム基盤／共通ユーティリティ

### 2. ファイル名

- クラス名に合わせた PascalCase と拡張子 `.cs` を使用します。
- テストクラスは `<クラス名>Tests.cs` と命名し、同階層の `Tests` フォルダに配置します。

---

## チェックリスト

- [ ] Namespace が `CardsAndDice` になっている
- [ ] クラス／メソッド／プロパティに `<summary>` がある
- [ ] 引数・戻り値に `<param>`・`<returns>` がある
- [ ] 命名規則に沿っている
- [ ] コーディングスタイル（インデント、ブレース、空行）が規約通りである
- [ ] ファイルが正しいフォルダに配置されている
- [ ] 内部ロジックに必要なコメントが残されている

---

以上のガイドに従って、AI へのプロンプトを作成し、一貫性と可読性の高い Unity C# コードを生成してください。

---

## 関連ファイル

- [guide_rules.md](./guide/guide_rules.md): ドキュメント作成・記述ルール
- [guide_files.md](./guide/guide_files.md): ドキュメントファイル命名・管理ルール

---

## 更新履歴

- 2025-07-10: 初版 (Nekodamasi)
- 2025-07-18: コーディング規約と統合 (Nekodamasi)
- 2025-07-18: 関連ファイルセッション修正 (Nekodamasi)
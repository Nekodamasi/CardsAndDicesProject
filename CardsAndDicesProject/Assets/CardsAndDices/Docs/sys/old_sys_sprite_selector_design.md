# sys_sprite_selector_design.md - 汎用スプライト選択機構

---

## 概要

このドキュメントは、IDに基づいてスプライトを動的に切り替えるための、汎用的で再利用可能なシステムについて定義します。
このシステムは、主に以下の2つのコンポーネントで構成されます。

-   **`SelectableSpriteSheet` (ScriptableObject)**: IDとスプライトの対応表データ。
-   **`SpriteSelector` (MonoBehaviour)**: `SelectableSpriteSheet` を参照し、指定されたIDのスプライトを `SpriteRenderer` に表示するコンポーネント。

この機構は、ダイスの面の表示だけでなく、カードの絵柄、アイテムのアイコンなど、あらゆる「データに基づいて見た目が変わるUI要素」に適用可能です。

---

## ScriptableObjectの仕様

### 1. クラス名
`SelectableSpriteSheet`

### 2. アセットメニュー
`CardsAndDices/Selectable Sprite Sheet`

### 3. 機能
IDとスプライトのペアのコレクションを保持します。インスペクターから容易にIDとスプライトの組み合わせを編集できます。
パフォーマンス向上のため、実行開始時にリストを内部で `Dictionary` に変換し、IDによる高速なスプライト検索（O(1)）を実現します。

### 4. フィールド
-   `private List<IdSpritePair> _sprites`: IDとスプライトのペアを格納するリスト。
    -   `IdSpritePair` 構造体:
        -   `public string Id`: スプライトを識別するための一意のID。
        -   `public Sprite Sprite`: 表示するスプライト画像。

### 5. パブリックメソッド
-   `public Sprite GetSprite(string id)`: 指定されたIDに対応するスプライトを返します。見つからない場合は `null` を返します。

---

## MonoBehaviourの仕様

### 1. クラス名
`SpriteSelector`

### 2. 機能
アタッチされた `SpriteRenderer` の `sprite` プロパティを、`SelectableSpriteSheet` から取得したスプライトに差し替えます。
`RequireComponent(typeof(SpriteRenderer))` を持ち、`Awake()` メソッドで自動的に `SpriteRenderer` コンポーネントを取得することで、常に存在することを保証します。

### 3. フィールド
-   `[SerializeField] private SelectableSpriteSheet _spriteSheet`: 表示するスプライトのデータソース。
-   `private SpriteRenderer _spriteRenderer`: スプライトを表示するコンポーネントへの参照。`Awake()` メソッドで自動的に取得されます。

### 4. パブリックメソッド
-   `public void SelectSprite(string id)`: 指定されたIDのスプライトを表示します。`_spriteSheet` が未設定の場合や、IDが見つからない場合はエラー/警告ログを出力します。

---

## 使用例：ダイスの面の更新

1.  ダイスの各面（"1", "2", ..., "6", "skull"など）に対応するIDとスプライト画像を定義した `DiceFaces.asset` (SelectableSpriteSheet) を作成します。
2.  ダイスのGameObjectに `SpriteSelector` コンポーネントを追加し、`Sprite Sheet` フィールドに `DiceFaces.asset` をアタッチします。
3.  `DiceView` クラスから `SpriteSelector` の参照を保持します。
4.  `DiceView.UpdateFace(int faceValue)` メソッド内で、`faceValue` を文字列に変換し、`_spriteSelector.SelectSprite(faceValue.ToString())` を呼び出すことで、ダイスの見た目を更新します。

---

## 関連ファイル

- [gdd_sprite_ui_design.md](../gdd/gdd_sprite_ui_design.md)
- [ui_dice_interaction.md](../ui/ui_dice_interaction.md)

---

## 更新履歴

- 2025-08-14: 初版 (Gemini)
- 2025-08-14: ソースコードとの整合性向上、機能説明の詳細化 (Gemini)
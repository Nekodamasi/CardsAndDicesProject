# guide_file_management.md - ファイル一覧管理ガイド

---

## 概要

このドキュメントは、プロジェクトのファイル一覧ドキュメント (`guide_project_files.md`) を管理・更新するための手順とルールを定義します。

**AIへのプロンプト例:**
`guide_file_management.md内のガイドにしたがって、guide_project_files.mdのファイル一覧を最新化してください`

---

## ファイル一覧生成ガイド

### 1. 対象ファイル

- 対象ディレクトリ直下の`*.md`と`*.cs`ファイルすべて
- 対象ディレクトリ: `D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices`

### 2. フォーマット

`guide_project_files.md` は、ファイルを格納しているディレクトリごとにセクションを分け、以下のMarkdownテーブル形式で出力してください。

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |

- **ファイル名:** 実際のファイル名（例: `gdd_sprite_ui_design.md`）
- **解説:** そのファイルの短い解説。
- **格納場所:** ファイルのフルパス。

### 3. ファイル一覧の更新手順

1. 以下のディレクトリ内の*.mdと*.csファイルをすべてリストアップし、ファイル名、短い解説、フルパスのフォーマットでMarkdownテーブルとして出力してください
    対象ディレクトリ:
    ```
     D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices
2. 出力したMarkdownテーブルを `guide_project_files.md` の内容と置き換えます。
3. 必要に応じて、各ファイルの「解説」をより詳細に記述します
4. チェックリストに従って`guide_project_files.md` の内容が適切かチェックします

---

## チェックリスト

ファイル一覧を更新する際は、以下の点を確認してください。

- [ ] 対象ディレクトリ (`D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices`) 配下のすべての`.md`ファイルと`.cs`ファイルがリストに含まれているか。
- [ ] 各ファイルの「解説」が適切で、そのファイルの役割を簡潔に表しているか。
- [ ] 「格納場所」が正しいフルパスになっているか。
- [ ] Markdownテーブルのフォーマットが崩れていないか。

---

## 関連リンク

- [guide_project_files.md](./guide_file_management.md)

---

## 更新履歴

- 2025-07-30: 初版 (Nekodamasi)
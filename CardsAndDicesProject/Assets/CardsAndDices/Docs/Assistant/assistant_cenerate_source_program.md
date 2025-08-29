  あなたは、このUnityプロジェクトのアーキテクチャに精通した、経験豊富なC#開発者です。
  あなたのタスクは、提供された設計書の内容を、既存のプロジェクトの設計と規約に完全に準拠する形で、C#スクリプトとして実装することで
  す。

  作業を開始する前に、以下のドキュメントを熟読してください。

  <design_doc>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_identity-and-name-management.md
  </design_doc>

  <related_design_docs>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_creature_card_lifecycle_design.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_domain-model.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_card_slot_manager.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_creature_management.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_ui_interaction_design.md
  </related_design_docs>

  <code_style_guide>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_unity-cs.md
  </code_style_guide>

  <project_file_manifest>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_files.md
  </project_file_manifest>

  <project_files>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_project_files.md
  </project_files>

  作業は、思考プロセスを記述する<scratchpad>と、最終的な成果物を記述する<created_files>の2つのフェーズで行います。いきなりコー
  ドを書き始めるのではなく、まず<scratchpad>内で慎重に計画を立ててください。

  フェーズ1: 思考と計画

  <scratchpad>タグ内に、以下の思考プロセスを記述してください。

   1. ドキュメントの理解:
       - design_docの内容を要約し、実装すべき要件をリストアップします。
       - related_design_docsの内容をそれぞれ要約し、design_docの要件とどのように関連しているかを分析します。
       - code_style_guideとproject_file_manifestの重要なルール（特に命名規則、ファイル配置場所、コメントスタイル）を書き出します
         。

   2. 既存コードの分析:
       - related_design_docsの内容に基づき、参照・分析すべき既存のソースコードファイルを特定します。
       - それらのソースコードの構造、クラス間の連携方法、DI（依存性注入）のパターン、初期化処理の流れなどを分析し、今回実装する
         機能がどのように統合されるべきかを考察します。

   3. 実装計画の策定:
       - 上記の分析結果を踏まえ、design_docの要件を満たすために作成する全てのクラスとインターフェースをリストアップします。
       - それぞれのクラスについて、code_style_guideに従ったフィールド、プロパティ、メソッドの具体的な設計を記述します。
       - project_file_manifestに基づき、各ファイルの正確な保存先パスを決定します。

  フェーズ2: 実装

  計画に基づいて作成した全てのファイルを、保存先パスに保存してください

  フェーズ3: ファイル一覧の更新

  保存したファイルが新規ファイルの場合、project_filesに追加してください

  重要なルール:
   - 全てのC#コードは、code_style_guideの規約（namespace、XMLコメント、命名規則など）に厳密に従う必要があります。
   - 既存のアーキテクチャや設計思想（related_design_docsや既存コードから読み取れるもの）と矛盾しないように実装してください。
   - 指示された以外のファイルを作成したり、既存のファイルを変更したりしないでください。

  それでは、始めてください。
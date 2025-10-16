  あなたは、ソースコードを分析してシステム設計書を作成する、経験豊富で優秀なシステム設計者です。あなたの仕事は、アシスタントとして、提供された情報源を注意深く分析し、一貫性のある詳細なシステム設計書を作成することです。


  あなたのタスクは、提供されたC#のソースコード群と関連ドキュメントを分析し、指定されたフォーマット例に従って、ゲーム内の「クリーチャーカード」に付属する「ダイスインレット」管理システムに関するシステム設計書をマークダウン形式で作成することです。

  作業を始める前に、以下の資料をよく読んでください。

  <related_design_docs>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_combat_system.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_composite_object_id.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_design-principles.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_domain-model.md
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_identifiable-views.md
  </related_design_docs>

  <source_code_files>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Domain\Creature\CreatureCardInstance.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Domain\Creature\CreatureCardSlotInstance.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Presenters\CreatureCardPresenter.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Presenters\CreatureCardSlotController.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Presenters\CreatureCardSlotPresenter.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Managers\CreatureCards\CreatureCardManager.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Managers\CreatureCards\CreatureCardSlotManager.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Services\ReflowService.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\UI\Views\CreatureCardSlotView.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\UI\Views\CreatureCardView.cs
  </source_code_files>

  <reference>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Managers\CreatureCards\CreatureManager.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Combats\Domain\Creature\CardInitializationData.cs
  </reference>

  <format_example>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\Example\example_system_management_document.md
  </format_example>

  以下の手順に従って、タスクを遂行してください。

   step1. 背景の理解: まず、<related_design_docs>を熟読し、このプロジェクトの設計思想、命名規則、既存のシステムアーキテクチャを完全に把握してください。これは、これから作成するドキュメントが一貫性を保つために非常に重要です。
   step2. ソース分析：<source_code_files>を熟読し、システム設計書に記載する処理内容を完全に把握します
   step3. 出力形式の把握：<format_example>を熟読し、出力するシステム設計書の編集方法を把握します
   step4. システム設計書の生成：フォーマットに従いシステム設計書を生成します
   step5. 出力結果のチェック：以下のチェックを行います
       - [ ] クラスおよびコンポーネント設計の各クラスにある、「継承」、「プロパティ」、「責務」、「メソッド」は可能な限り書き出せているか
       - [ ] <format_example>に存在しない項目を追加していないか
       - [ ] <source_code_files>で指定したファイルに存在するすべてのクラスが記載されているか

   step6. 最終的な出力: 完成したシステム設計書の全文を、以下の <output_document> タグの中に記述してください。前置きや余分な説明は一切含めないでください。
  <output_document>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_dice_management.md
  </Instructions>
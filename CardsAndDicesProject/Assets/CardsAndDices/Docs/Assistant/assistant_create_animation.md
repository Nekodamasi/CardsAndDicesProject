あなたは、UnityとC#、特にDOTweenライブラリを使用したアニメーション実装に精通した、経験豊富なソフトウェア開発者です。
あなたのタスクは、ダイスが画面外から勢いよく飛び込んでくるようなのアニメーションを実装するために必要なC#ソースコードを生成し、関連するプロジェクトファイルを更新することです。

  タスクを実行するために必要な情報は以下の通りです。

  <animation_system_design>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_animation_system.md
  </animation_system_design>

  <coding_guide>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_unity-cs.md
  </coding_guide>

  <project_files_guide>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_project_files.md
  </project_files_guide>

  <existing_animation_code>
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\AnimationExecutor.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\AnimationStrategyRegistry.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\BaseAnimationStrategySO.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\BuffAnimationStrategySO.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\DragAnimationStrategySO.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\BaseAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\BodySlamAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\BuffAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EntityDefinition\AnimationStrategyEntity.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\AnimationContext.cs

  </existing_animation_code>

  <animation_description>
  - AnimationContextのHomePositionから右に向かって1.5移動したします
  - 上の位置でDisplayIdentifiableStatusCommandコマンドを発行します
  - ここまでで0.2秒のアニメーションです
  - 移動量は、AnimationProfileで管理します
  </animation_description>

  最終的な出力を生成する前に、<scratchpad>タグの中にあなたの思考プロセスを段階的に記述してください。これにより、あなたがどのように結論に至ったかを理解することができます。思考プロセスには以下を含めてください。
   1. 提供された設計書の分析。
       - <animation_system_design>タグの中の設計書を良く読込、アニメーションシステム全体の仕様を理解します。
   2. 提供された既存のコードの分析。
       - <existing_animation_code>タグの中のソースプログラムをすべて読込、現在の実装を良く理解します。
   3. <animation_description>タグで説明されているアニメーションをDOTweenで実装するための具体的な計画。
   4. 生成する2つのクラス（AnimationProfileとAnimationStrategy）のクラス名と、それらを保存するファイルパスの決定。
   　　- `BaseAnimationStrategySO` を継承して、3で計画した、DOTweenを実装した、`AnimationStrategySO` を生成します。
   　　- `BaseAnimationProfile` を継承して、作成する `AnimationStrategySO` で使用するパラメータを設計して、`AnimationProfile` を生成します。  
   5. 生成したソースプログラムに対して以下のチェックを行います。
       - [ ] Namespace が `CardsAndDices` になっている
       - [ ] クラス／メソッド／プロパティに `<summary>` がある
       - [ ] 引数・戻り値に `<param>`・`<returns>` がある
       - [ ] 命名規則に沿っている
       - [ ] コーディングスタイル（インデント、ブレース、空行）が規約通りである
       - [ ] ファイルが正しいフォルダに配置されている
       - [ ] 内部ロジックに必要なコメントが残されている
       - [ ] コメントは全て日本語で書かれている
   6. <project_files_guide>タグのファイル一覧に追記したソースプログラムを追加します

  思考プロセスが完了したら、以下の指示に従って成果物を出力してください。

   1. 生成する各ソースコードは、以下の例のように<source_code>タグで囲み、file_path属性に完全なファイルパスを指定してください。ファイルパスはproject_files_guideのディレクトリ構造に従う必要があります。

  <example>
  <source_code file_path="D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animation\NewAnimationProfile.cs">
  // C# code for the animation profile...
  </source_code>
  <source_code file_path="D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\NewAnimationStrategySO.cs">
  // C# code for the animation strategy...
  </source_code>
  </example>

   2. 更新後のファイル一覧は、<updated_file_list>タグの中に、ファイルの全内容を記述してください。既存の内容に新しいファイルのエントリをアルファベット順で追加する必要があります。提供された設計書、コーディング規約、ファイル構造に厳密に従ってください。前置きなしで、すぐに思考プロセスから始めてください。
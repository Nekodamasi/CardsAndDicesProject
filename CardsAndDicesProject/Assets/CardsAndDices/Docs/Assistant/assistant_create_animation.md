あなたは、UnityとC#、特にDOTweenライブラリを使用したアニメーション実装に精通した、経験豊富なソフトウェア開発者です。
あなたのタスクは、クリーチャーカードが右側に向かって体当たりするアニメーションを実装するために必要なC#ソースコードを生成し、関連するプロジェクトファイルを更新することです。

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
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animation\BaseAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animation\DragAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animation\HoverAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animation\NormalAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animation\BodySlamAnimationProfile.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\IAnimationStrategy.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\DragAnimationStrategy.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\HoverAnimationStrategy.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\NormalAnimationStrategy.cs
  - D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\BodySlamAnimationStrategy.cs
  </existing_animation_code>

  <animation_description>
  - 少し左に下がってから、右側に移動（体当たり）し、0.1fほど止まってから、すっと元の位置に戻ります
  </animation_description>

  最終的な出力を生成する前に、<scratchpad>タグの中にあなたの思考プロセスを段階的に記述してください。これにより、あなたがどのように結論に至ったかを理解することがで
  きます。思考プロセスには以下を含めてください。
   1. 提供された設計書と既存のコードの分析。
   2. animation_descriptionで説明されているアニメーションをDOTweenで実装するための具体的な計画。
   3. 生成する2つのクラス（AnimationProfileとAnimationStrategy）のクラス名と、それらを保存するファイルパスの決定。
   4. project_files_guideを更新するための計画。

  思考プロセスが完了したら、以下の指示に従って成果物を出力してください。

   1. 生成する各ソースコードは、以下の例のように<source_code>タグで囲み、file_path属性に完全なファイルパスを指定してください。ファイルパスはproject_files_guideのディ
      レクトリ構造に従う必要があります。

  <example>
  <source_code file_path="D:\\Users\\ponki\\Unity\\CardsAndDicesProject\\Assets\\CardsAndDices\\Scripts\\Animation\\NewAnimationProfile.cs">
  // C# code for the animation profile...
  </source_code>
  <source_code file_path="D:\\Users\\ponki\\Unity\\CardsAndDicesProject\\Assets\\CardsAndDices\\Scripts\\Strategy\\NewAnimationStrategy.cs">
  // C# code for the animation strategy...
  </source_code>
  </example>

   2. 更新後のファイル一覧は、<updated_file_list>タグの中に、ファイルの全内容を記述してください。既存の内容に新しいファイルのエントリをアルファベット順で追加する必要
      があります。

  提供された設計書、コーディング規約、ファイル構造に厳密に従ってください。前置きなしで、すぐに思考プロセスから始めてください。
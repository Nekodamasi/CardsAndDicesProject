  ソースファイル格納ディレクトリ整理の提案

  1. 目的

   - Scripts/Shared および Scripts/Systems
     ディレクトリの肥大化を解消し、ファイルの検索性・参照性を向上させる。
   - 各ディレクトリの責務を明確化し、新しいファイルの配置基準を明確にする。
   - プロジェクト全体のコードベースの保守性と拡張性を高める。

  2. 新しいディレクトリ構造の概要

  既存の Scripts/Data, Scripts/Installers, Scripts/Tester, Scripts/UI, Scripts/Commands, Scripts/Abilities
  ディレクトリは、その責務が明確であるため維持します。
  Scripts/Shared と Scripts/Systems の内容を、より粒度の細かい責務を持つディレクトリに再分類します。

    1 Assets/CardsAndDices/Scripts/
    2 ├── Abilities/        # ScriptableObjectベースのアビリティ定義
    3 ├── Animation/        # アニメーション関連のScriptableObject
    4 ├── Commands/         # コマンドパターンで使用されるコマンド定義
    5 ├── Core/             #
      プロジェクト全体で共通の基盤コード（インターフェース、抽象クラス、汎用ユーティリティ）
    6 ├── Data/             # 純粋なデータ定義（ScriptableObject、構造体、クラス）
    7 ├── Domain/           # ゲームのコアロジック、データモデル、エンティティ
    8 ├── Factory/          # オブジェクトの生成ロジックをカプセル化
    9 ├── Installers/       # DIコンテナのインストールロジック
   10 ├── Manager/          # 複数のコンポーネントやシステムを統括・管理する高レベルなロジック
   11 ├── Orchestrator/     # 複数のシステムやコンポーネント間の複雑な連携を調整
   12 ├── Presenter/        # ViewとModelの仲介役
   13 ├── Repository/       # データの永続化や取得を担当
   14 ├── Service/          # 特定の機能を提供するサービス層（データ操作、外部API連携など）
   15 ├── State/            # ステートマシン関連のクラス
   16 ├── Strategy/         # 特定の振る舞いをカプセル化し、交換可能なアルゴリズムを提供
   17 ├── Tester/           # テスト関連のスクリプト
   18 └── UI/               # UIコンポーネント（MonoBehaviour）

  3. 各ディレクトリの責務と配置例

  Scripts/Core/
  プロジェクト全体で共通して使用される、特定のシステムに依存しない基盤となるインターフェース、抽象クラス、汎
  用ユーティリティなど。

   - 配置例（`Scripts/Shared` からの移動）:
       - CompositeObjectId.cs: 複合オブジェクトID
       - CompositeObjectIdManager.cs: 複合オブジェクトIDのマネージャー
       - IdentifiableGameObject.cs: IDを持つゲームオブジェクト
       - IAnimationStrategy.cs: アニメーション戦略のインターフェース
       - MultiRendererVisualController.cs: 複数レンダラーの表示コントローラー
       - SpriteLayerController.cs: スプライトのレイヤーコントローラー
       - SpriteStatus.cs: スプライトの状態（Enum）
       - Team.cs: チーム（Enum）
       - LinePosition.cs: ライン上の位置
       - SlotLocation.cs: スロットの位置
       - DiceSlotLocation.cs: ダイススロットの位置

  Scripts/Domain/
  ゲームのコアロジック、データモデル、エンティティなど、ビジネスロジックに密接に関連するコード。

   - 配置例（`Scripts/Data` からの移動、または新規）:
       - CreatureData.cs: クリーチャーの基本ステータス
       - DiceData.cs: 個々のダイスのデータ
       - CardInitializationData.cs: カード生成に必要な情報
       - CardSlotData.cs: カードスロットのデータ
       - DiceInletData.cs: ダイスインレットの静的な識別情報
       - DiceSlotData.cs: 個々のダイススロットのデータ

  Scripts/Service/
  特定の機能を提供するサービス層。データ操作、外部API連携、複雑な計算など。

   - 配置例（`Scripts/Systems` からの移動）:
       - CardLifecycleService.cs: カードのライフサイクル管理
       - CardPlacementService.cs: カード配置処理
       - DicePlacementService.cs: ダイス配置処理
       - ReflowService.cs: リフロー処理

  Scripts/Manager/
  複数のコンポーネントやシステムを統括・管理する高レベルなロジック。

   - 配置例（`Scripts/Systems` からの移動）:
       - CombatManager.cs: 戦闘フェーズ全体の流れを制御
       - CardSlotManager.cs: カードスロット管理
       - DiceManager.cs: 全てのダイスのロジックと状態管理
       - DiceInletManager.cs: ダイスインレットのロジックと状態管理
       - DiceSlotManager.cs: ダイススロットのロジックと状態管理

  Scripts/Presenter/
  ViewとModelの仲介役。UIロジックとデータロジックの分離。

   - 配置例（`Scripts/Systems` からの移動）:
       - DicePresenter.cs: DiceDataとDiceViewを紐づけ、状態を同期

  Scripts/Controller/
  ユーザー入力やシステムイベントを受け取り、ゲームの状態を変更するロジック。

   - 配置例（`Scripts/Systems` からの移動）:
       - SystemReflowController.cs: システム起因のリフロー処理を実行

  Scripts/Factory/
  オブジェクトの生成ロジックをカプセル化。

   - 配置例（`Scripts/Systems` からの移動）:
       - DiceFactory.cs: DiceDataインスタンスの生成ロジック

  Scripts/Repository/
  データの永続化や取得を担当。

   - 配置例（`Scripts/Systems` からの移動）:
       - CardSlotStateRepository.cs: カードスロットの状態リポジトリ
       - DiceSlotStateRepository.cs: ダイススロットの状態リポジトリ

  Scripts/Strategy/
  特定の振る舞いをカプセル化し、交換可能なアルゴリズムを提供する。

   - 配置例（`Scripts/Systems` からの移動）:
       - CardInteractionStrategy.cs: カードのUIインタラクション戦略
       - DiceInteractionStrategy.cs: ダイスのUIインタラクション戦略

  Scripts/Orchestrator/
  複数のシステムやコンポーネント間の複雑な連携を調整する。

   - 配置例（`Scripts/Systems` からの移動）:
       - CardInteractionOrchestrator.cs: カードのUIインタラクションを統括
       - DiceInteractionOrchestrator.cs: ダイスのUIインタラクションを統括
       - IUIInteractionOrchestrator.cs: UIインタラクションオーケストレーターのインターフェース

  Scripts/State/
  ステートマシン関連のクラス。

   - 配置例（`Scripts/Systems` からの移動）:
       - UIStateMachine.cs: UIのステートマシン
       - UIActivationPolicy.cs: UIのアクティベーションポリシー

  Scripts/Animation/
  アニメーション関連のScriptableObject。

   - 配置例（`Scripts/Shared` からの移動）:
       - BaseAnimationSO.cs: アニメーションの基底ScriptableObject
       - DragAnimationSO.cs: ドラッグアニメーションのScriptableObject
       - DropWaitingAnimationSO.cs: ドロップ待ちアニメーションのScriptableObject
       - HoverAnimationSO.cs: ホバーアニメーションのScriptableObject
       - NormalAnimationSO.cs: 通常アニメーションのScriptableObject
       - ReturnToPositionAnimationSO.cs: 位置に戻るアニメーションのScriptableObject

  4. 考慮事項

   - Unityの一般的な慣習: Assets/Scripts 以下に機能ごとにディレクトリを分ける一般的な慣習に沿っています。
   - 既存の命名規則との整合性: 既存の Scripts/Data, Scripts/UI
     などは維持し、新しいディレクトリも責務が明確な名詞を使用しています。
   - 将来的な拡張性: 各ディレクトリの責務を明確にすることで、新しい機能が追加される際にどこにファイルを配置す
     べきか迷いにくくなり、コードベースの拡張性が向上します。
   - 段階的な移行: 一度にすべてのファイルを移動するのではなく、段階的に移行を進めることを推奨します。

  ファイル分類リスト

  Docs/class

   - class_CardLifecycleService.md
   - class_CombatManager.md
   - class_EnemyCardDataProvider.md
   - class_PlayerCardDataProvider.md

  Docs/component

   - component_card_slot_prefab.md
   - component_creature_card_prefab.md
   - component_dice_inlet_prefab.md

  Docs/gdd

   - gdd_combat_system.md
   - gdd_composite_object_id.md
   - gdd_game_object_specs.md
   - gdd_main.md
   - gdd_reflow_system.md
   - gdd_sprite_ui_design.md
   - gdd_unity_specs.md

  Docs/guide

   - guide_design-principles.md
   - guide_developer-cookbook.md
   - guide_file_management.md
   - guide_files.md
   - guide_overview.md
   - guide_project_files.md
   - guide_rules.md
   - guide_sys_classes_creation.md
   - guide_ui_interaction_design.md
   - guide_unity-cs.md

  Docs/sys

   - sys_card_slot_manager.md
   - sys_classes.md
   - sys_dice_inlet_design.md
   - sys_dice_lifecycle_design.md
   - sys_domain-model.md
   - sys_sprite_selector_design.md

  Docs/ui

   - ui_card_slot_interaction.md
   - ui_creature_card_interaction.md
   - ui_dice_inlet_interaction.md
   - ui_dice_interaction.md
   - ui_dice_slot_interaction.md

  Scripts/Abilities

   - BaseInletAbilitySO.cs

  Scripts/Animation

   - BaseAnimationSO.cs
   - DragAnimationSO.cs
   - DropWaitingAnimationSO.cs
   - HoverAnimationSO.cs
   - NormalAnimationSO.cs
   - ReturnToPositionAnimationSO.cs

  Scripts/Commands

   - CardPlacedInSlotCommand.cs
   - DisableUIInteractionCommand.cs
   - DragReflowCompletedCommand.cs
   - EnableUIInteractionCommand.cs
   - ExecuteFrontLoadCommand.cs
   - ICommand.cs
   - PlayerZoneStateChangedCommand.cs
   - ReflowCompletedCommand.cs
   - ReflowOperationCompletedCommand.cs
   - SpriteBeginDragCommand.cs
   - SpriteClickCommand.cs
   - SpriteDragCommand.cs
   - SpriteDragOperationCompletedCommand.cs
   - SpriteDropCommand.cs
   - SpriteEndDragCommand.cs
   - SpriteHoverCommand.cs
   - SpriteUnhoverCommand.cs
   - SystemDiceReflowCommand.cs
   - SystemReflowCommand.cs

  Scripts/Core

   - CompositeObjectId.cs
   - CompositeObjectIdManager.cs
   - DiceSlotLocation.cs
   - IAnimationStrategy.cs
   - ICardDataProvider.cs
   - IdentifiableGameObject.cs
   - InteractionProfile.cs
   - IUIInteractionOrchestrator.cs
   - LinePosition.cs
   - MultiRendererVisualController.cs
   - SlotLocation.cs
   - SpriteLayerController.cs
   - SpriteStatus.cs
   - Team.cs

  Scripts/Data

   - AllowedDiceFacesSO.cs
   - DiceInletConditionSO.cs
   - FixedCardInitializer.cs
   - InletAbilityProfile.cs
   - SelectableSpriteSheet.cs

  Scripts/Domain

   - CardInitializationData.cs
   - CardSlotData.cs
   - CreatureData.cs
   - DiceData.cs
   - DiceInletData.cs
   - DiceSlotData.cs

  Scripts/Factory

   - DiceFactory.cs

  Scripts/Installers

   - GameLifetimeScope.cs

  Scripts/Manager

   - CardSlotInteractionHandler.cs
   - CardSlotManager.cs
   - CombatManager.cs
   - DiceInletAbilityRegistry.cs
   - DiceInletManager.cs
   - DiceManager.cs
   - DiceSlotInteractionHandler.cs
   - DiceSlotManager.cs
   - ViewRegistry.cs

  Scripts/Orchestrator

   - CardInteractionOrchestrator.cs
   - DiceInteractionOrchestrator.cs

  Scripts/Presenter

   - DicePresenter.cs

  Scripts/Repository

   - CardSlotStateRepository.cs
   - DiceSlotStateRepository.cs

  Scripts/Service

   - CardLifecycleService.cs
   - CardPlacementService.cs
   - DicePlacementService.cs
   - EnemyCardDataProvider.cs
   - PlayerCardDataProvider.cs
   - ReflowService.cs

  Scripts/State

   - UIActivationPolicy.cs
   - UIStateMachine.cs

  Scripts/Strategy

   - CardInteractionStrategy.cs
   - DiceInteractionStrategy.cs

  Scripts/Tester

   - DebugViewer.cs
   - PlacementCardTester.cs

  Scripts/UI

   - BaseSpriteView.cs
   - CardSlotView.cs
   - CreatureCardView.cs
   - DiceInletView.cs
   - DiceSlotView.cs
   - DiceView.cs
   - SpriteCommandBus.cs
   - SpriteInputHandler.cs
   - SpriteSelector.cs
# guide_project_files.md - プロジェクトファイル一覧

---

## 概要

このドキュメントは、プロジェクト内に存在する全てのドキュメントとソースコードのファイル一覧です。
ファイルを追加・削除した場合は、必ずこの一覧を更新してください。

---

## ドキュメント (`.md`)

### Docs/Assistant

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| assistant_cenerate_source_program.md | 設計書からC#スクリプトを実装するためのAIアシスタントへの指示書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_cenerate_source_program.md |
| assistant_create_animation.md | バフアニメーション実装タスクをAIアシスタントに指示するためのプロンプト。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_create_animation.md |
| assistant_directory.md | ソースファイルのディレクトリ構造整理に関する提案書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_directory.md |
| assistant_files copy.md | ファイル一覧ドキュメントを更新するためのAIアシスタントへの指示書（コピー）。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_files copy.md |
| assistant_files.md | ファイル一覧ドキュメントを更新するためのAIアシスタントへの指示書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_files.md |
| assistant_meta_prompt copy.md | AIアシスタントへの指示を作成するためのメタプロンプト（コピー）。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_meta_prompt copy.md |
| assistant_meta_prompt.md | AIアシスタントへの指示を作成するためのメタプロンプト。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_meta_prompt.md |
| assistant_update_document.md | 設計書を最新の状態に更新するためのドキュメントスペシャリストAIへの指示書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\assistant_update_document.md |
| dice_prompt.md | ダイス関連機能の実装検討をAIアシスタントに指示するためのプロンプト。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\dice_prompt.md |
| inlet.md | ダイスを投入して能力を発動する「インレット」機能の概要と仕様を解説。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\inlet.md |
| inret_prompt.md | ダイスインレットの仕様書作成をAIアシスタントに指示するためのプロンプト。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\inret_prompt.md |
| sekkei.md | ソフトウェアアーキテクトAIが「攻撃」機能の技術設計案を作成するための指示書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\Assistant\sekkei.md |

### Docs/class

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| class_CardLifecycleService.md | カードの生成、初期化、破棄といったライフサイクルを管理する`CardLifecycleService`クラスの設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\class\class_CardLifecycleService.md |
| class_CombatManager.md | 戦闘全体の流れを制御する`CombatManager`クラスの設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\class\class_CombatManager.md |
| class_EnemyCardDataProvider.md | 敵カードのデータを提供する`EnemyCardDataProvider`クラスの設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\class\class_EnemyCardDataProvider.md |
| class_PlayerCardDataProvider.md | プレイヤーカードのデータを提供する`PlayerCardDataProvider`クラスの設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\class\class_PlayerCardDataProvider.md |

### Docs/component

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| component_card_slot_prefab.md | `CardSlot` Prefabの構成と各コンポーネントの仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\component\component_card_slot_prefab.md |
| component_creature_card_prefab.md | `CreatureCard` Prefabの構成と各コンポーネントの仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\component\component_creature_card_prefab.md |
| component_dice_inlet_prefab.md | `DiceInlet` Prefabの構成と各コンポーネントの仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\component\component_dice_inlet_prefab.md |

### Docs/gdd

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| gdd_combat_system.md | カードとダイスを組み合わせた戦闘システムの核心的なコンセプトを定義するゲームデザインドキュメント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_combat_system.md |
| gdd_composite_object_id.md | ゲーム内の全オブジェクトを識別・管理する「複合オブジェクト識別子」システムのゲームデザインを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_composite_object_id.md |
| gdd_game_object_specs.md | クリーチャーカード、スロット、ダイスなど、主要なゲームオブジェクトの仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_game_object_specs.md |
| gdd_main.md | ゲームのコンセプト、ターゲットプラットフォーム、ユーザー層を定義する最上位の要件定義書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_main.md |
| gdd_reflow_system.md | カードのリフロー（再配置）システムのロジックとフローを定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_reflow_system.md |
| gdd_sprite_ui_design.md | SpriteベースのUI要素（SpriteUI）の基本的なインタラクションと構造を定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_sprite_ui_design.md |
| gdd_unity_specs.md | Unityエンジンの技術要件（Sorting Layer、カメラ設定、使用技術など）を定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\gdd\gdd_unity_specs.md |

### Docs/guide

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| guide_asset_workflow.md | サウンドアセットの作成・管理に関するワークフローと規約を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_asset_workflow.md |
| guide_design-principles.md | プロジェクトのソフトウェア設計における核心的な原則（MVC、データ駆動、イベント駆動など）を詳述。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_design-principles.md |
| guide_developer-cookbook.md | 開発者が特定のタスク（UI要素追加、カード効果実装など）を実行するための実践的な手順集。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_developer-cookbook.md |
| guide_file_management.md | ファイル一覧ドキュメント `guide_project_files.md` を管理・更新するための手順とルールを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_file_management.md |
| guide_files.md | ドキュメントファイルの命名規則、分類、およびファイルシステム上での管理方法を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_files.md |
| guide_overview.md | プロジェクト全体の概要、核心思想、ドキュメント構成、開発ワークフローを定義する、開発者とAIの出発点。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_overview.md |
| guide_prefab_instantiation.md | Prefabのインスタンス化に関する統一ルールを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_prefab_instantiation.md |
| guide_project_files.md | プロジェクト内に存在する全てのドキュメントとソースコードのファイル一覧。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_project_files.md |
| guide_rules.md | 全てのMarkdownドキュメントの作成および記述に関する一般的なルールとベストプラクティスを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_rules.md |
| guide_sys_classes_creation.md | `sys_classes.md` を新規作成・更新する手順を解説するガイド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_sys_classes_creation.md |
| guide_ui_interaction_design.md | UIインタラクション設計書を作成するためのガイドライン。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_ui_interaction_design.md |
| guide_unity-cs.md | Unity用C#ソースコードを生成する際の共通ルールとフォーマットを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\guide\guide_unity-cs.md |

### Docs/sys

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| sys_ability_management.md | 「固有能力（Ability）」機能の技術的な実装を定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_ability_management.md |
| sys_animation_system.md | UI要素のアニメーション機能をStrategyパターンを用いて実装するためのシステム設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_animation_system.md |
| sys_attack_system.md | 「攻撃」機能の技術的な実装を定義するシステム設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_attack_system.md |
| sys_card_slot_manager.md | カードスロット関連システムの責務分割されたアーキテクチャを定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_card_slot_manager.md |
| sys_classes.md | プロジェクトで使用される主要なクラスの一覧と、それらへのリンクを提供。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_classes.md |
| sys_cooldown_processing.md | クリーチャーのクールダウンを特定の順序で処理するためのシステム設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_cooldown_processing.md |
| sys_creature_appearance_system.md | クリーチャーカードの視覚的な外観を動的に構成・制御するためのシステム設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_creature_appearance_system.md |
| sys_creature_card_lifecycle_design.md | クリーチャーカードの生成から除去までのライフサイクル全体を定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_creature_card_lifecycle_design.md |
| sys_creature_management.md | クリーチャーの実行時インスタンスの管理システムを定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_creature_management.md |
| sys_dice_inlet_management.md | ダイスインレットの実行時インスタンスの管理システムを定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_dice_inlet_management.md |
| sys_dice_lifecycle_design.md | ダイスオブジェクトのライフサイクル（生成、利用、返却）に関する技術仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_dice_lifecycle_design.md |
| sys_domain-model.md | プロジェクトの主要な概念とデータ構造（ドメインモデル）を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_domain-model.md |
| sys_effect_management.md | バフ・デバフなどの継続的なエフェクトの管理システムを定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_effect_management.md |
| sys_identifiable-views.md | `CompositeObjectId`を利用してUI要素を個別に識別・操作するためのシステム設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_identifiable-views.md |
| sys_identity-and-name-management.md | エンティティのIDと多言語対応された名称を管理するシステムを定義する設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_identity-and-name-management.md |
| sys_initialization_flow.md | ゲーム起動時のコンポーネント初期化順序を制御し、安定した起動を実現するための仕組みを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_initialization_flow.md |
| sys_sound_system.md | SE（サウンドエフェクト）の管理と再生に関するアーキテクチャを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_sound_system.md |
| sys_sprite_selector_design.md | IDに基づいてスプライトを動的に切り替えるための汎用的なシステムを定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_sprite_selector_design.md |
| sys_status_icon_design.md | クリーチャーカードに表示されるステータスアイコンの設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_status_icon_design.md |
| sys_vfx_management.md | 視覚効果（VFX）および音響効果の再生機能を管理するシステム設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_vfx_management.md |
| sys_wave_system.md | 戦闘における敵の出現パターンを管理する「ウェーブシステム」の設計書。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\sys\sys_wave_system.md |

### Docs/ui

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| ui_card_slot_interaction.md | カードスロット(`CardSlotView`)のUIインタラクションに関する仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\ui\ui_card_slot_interaction.md |
| ui_creature_card_interaction.md | クリーチャーカード(`CreatureCardView`)のUIインタラクションに関する仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\ui\ui_creature_card_interaction.md |
| ui_dice_inlet_interaction.md | ダイスインレット(`DiceInletView`)のUIインタラクションに関する仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\ui\ui_dice_inlet_interaction.md |
| ui_dice_interaction.md | ダイス(`DiceView`)のUIインタラクションに関する仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\ui\ui_dice_interaction.md |
| ui_dice_slot_interaction.md | ダイススロット(`DiceSlotView`)のUIインタラクションに関する仕様を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Docs\ui\ui_dice_slot_interaction.md |

---

## ソースコード (`.cs`)

### Scripts/Abilities

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BaseAbilityDataSO.cs | 全てのアビリティ定義ScriptableObjectの抽象基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BaseAbilityDataSO.cs |
| BaseAbilityDurationSO.cs | アビリティの持続時間、クールダウン、使用制限ロジックの抽象基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BaseAbilityDurationSO.cs |
| BaseAbilityEffectDefinitionSO.cs | 全ての能力効果定義の抽象基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BaseAbilityEffectDefinitionSO.cs |
| BaseAbilityTargetSelectorSO.cs | アビリティの効果対象を選択するロジックの基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BaseAbilityTargetSelectorSO.cs |
| BaseAbilityTriggerConditionSO.cs | 全てのアビリティ発動条件の抽象基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BaseAbilityTriggerConditionSO.cs |
| BaseInletAbilitySO.cs | インレットが発動する「効果」を定義する全ScriptableObjectの基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BaseInletAbilitySO.cs |
| BuffDebuffAbilityDataSO.cs | バフ・デバフを与えるアビリティのデータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\BuffDebuffAbilityDataSO.cs |

### Scripts/Abilities/Durations

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| UnlimitedUsageSO.cs | アビリティが永続的に使用可能であることを示す効果期間を定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Durations\UnlimitedUsageSO.cs |

### Scripts/Abilities/Effects

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BuffDebuffEffectSO.cs | バフ・デバフ効果を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Effects\BuffDebuffEffectSO.cs |
| CurrentValueChangeEffectSO.cs | 現在値（HP, Shield, Cooldown）を直接変更する効果を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Effects\CurrentValueChangeEffectSO.cs |
| IncreaseAttackEffectSO.cs | 攻撃力を増加させる効果を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Effects\IncreaseAttackEffectSO.cs |

### Scripts/Abilities/Targets

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| SelfTargetSelectorSO.cs | アビリティの所有者自身をターゲットとして選択するロジック。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Targets\SelfTargetSelectorSO.cs |

### Scripts/Abilities/Triggers

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| ActivationTimingOnlyTriggerConditionSO.cs | `ActivationTiming`の判定のみを行うトリガー条件。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Triggers\ActivationTimingOnlyTriggerConditionSO.cs |
| OnAttackedTriggerConditionSO.cs | 所有者クリーチャーが攻撃された時に満たされるトリガー条件。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Triggers\OnAttackedTriggerConditionSO.cs |
| OnPlacementCardTriggerConditionSO.cs | カードが特定の位置（例：前衛）に配置された時に満たされるトリガー条件。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Abilities\Triggers\OnPlacementCardTriggerConditionSO.cs |

### Scripts/Animations

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BaseAnimationProfile.cs | 全てのアニメーションプロファイルの基底クラスとなるScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\BaseAnimationProfile.cs |
| BodySlamAnimationProfile.cs | 体当たりアニメーションのパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\BodySlamAnimationProfile.cs |
| BuffAnimationProfile.cs | バフ効果を受けた際のアニメーションパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\BuffAnimationProfile.cs |
| DamageAnimationProfile.cs | ダメージを受けた際のアニメーションパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\DamageAnimationProfile.cs |
| DeathAnimationProfile.cs | 死亡時のアニメーションパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\DeathAnimationProfile.cs |
| DiceJumpInAnimationProfile.cs | ダイスのジャンプインアニメーション用のパラメータを定義する。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\DiceJumpInAnimationProfile.cs |
| DragAnimationProfile.cs | ドラッグ中のアニメーションパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\DragAnimationProfile.cs |
| HoverAnimationProfile.cs | ホバー時のアニメーションパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\HoverAnimationProfile.cs |
| MoveRightAnimationProfile.cs | 右方向への移動アニメーションのパラメータを定義するプロファイル。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\MoveRightAnimationProfile.cs |
| NormalAnimationProfile.cs | 通常状態のアニメーションパラメータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\NormalAnimationProfile.cs |

### Scripts/Animations/AnimationStrategy

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AnimationExecutor.cs | アニメーション戦略(`BaseAnimationStrategySO`)を実行するサービスクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\AnimationExecutor.cs |
| AnimationStrategyRegistry.cs | `AnimationStrategyEntity`と`BaseAnimationStrategySO`のマッピングを管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\AnimationStrategyRegistry.cs |
| BaseAnimationStrategySO.cs | 全てのアニメーション戦略の基底クラスとなるScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\BaseAnimationStrategySO.cs |
| BuffAnimationStrategySO.cs | バフ効果を受けた際のアニメーション戦略を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\BuffAnimationStrategySO.cs |
| DiceJumpInAnimationStrategySO.cs | `DiceJumpInAnimationProfile` を使用して、ダイスのジャンプインアニメーションをDOTweenで実行する。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\DiceJumpInAnimationStrategySO.cs |
| DragAnimationStrategySO.cs | ドラッグ中のアニメーション戦略を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\DragAnimationStrategySO.cs |
| HoverAnimationStrategySO.cs | ホバー時のアニメーション戦略を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\HoverAnimationStrategySO.cs |
| MoveRightAnimationStrategySO.cs | 右方向へ移動するアニメーションを実行する戦略。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\MoveRightAnimationStrategySO.cs |
| NormalAnimationStrategySO.cs | 通常状態のアニメーション戦略を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Animations\AnimationStrategy\NormalAnimationStrategySO.cs |

### Scripts/Commands

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AllCreatureCardUpdateDisplayCommand.cs | 全てのクリーチャーカードの表示を最新の値に更新するよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\AllCreatureCardUpdateDisplayCommand.cs |
| ApplyEffectCommand.cs | 特定のターゲットにエフェクトを適用するよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ApplyEffectCommand.cs |
| CardPlacedInSlotCommand.cs | カードがスロットに配置されたことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CardPlacedInSlotCommand.cs |
| CooldownZeroAttacksCommand.cs | クールダウンが0になったクリーチャーに攻撃を実行させるコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CooldownZeroAttacksCommand.cs |
| CreatureAttackChangedCommand.cs | クリーチャーの攻撃力変更を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureAttackChangedCommand.cs |
| CreatureAttackedCommand.cs | クリーチャーが攻撃されたことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureAttackedCommand.cs |
| CreatureBUffEffectedCommand.cs | クリーチャーにバフエフェクトのVFXを表示するよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureBUffEffectedCommand.cs |
| CreatureCardUpdateDisplayCommand.cs | 特定のクリーチャーカードの表示を最新の値に更新するよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureCardUpdateDisplayCommand.cs |
| CreatureCooldownChangedCommand.cs | クリーチャーのクールダウン値変更を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureCooldownChangedCommand.cs |
| CreatureDamagedCommand.cs | クリーチャーがダメージを受けたことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureDamagedCommand.cs |
| CreatureEnergyChangedCommand.cs | クリーチャーのエネルギー値変更を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureEnergyChangedCommand.cs |
| CreatureHealthChangedCommand.cs | クリーチャーの体力値変更を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureHealthChangedCommand.cs |
| CreatureResetCoolDownZeroCommand.cs | クールダウンが0になったクリーチャーのステータスをリセットするコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureResetCoolDownZeroCommand.cs |
| CreatureShieldChangedCommand.cs | クリーチャーのシールド値変更を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\CreatureShieldChangedCommand.cs |
| DiceDropInInletCommand.cs | ダイスがインレットにドロップされたことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\DiceDropInInletCommand.cs |
| DiceInletCountdownCompleteCommand.cs | ダイスインレットのカウントダウンが完了したことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\DiceInletCountdownCompleteCommand.cs |
| DragReflowCompletedCommand.cs | ドラッグ操作に起因するカードのリフローが完了したことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\DragReflowCompletedCommand.cs |
| ExecuteAbilityEffectCommand.cs | 特定のタイミングでアビリティ効果の発動を試みるよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ExecuteAbilityEffectCommand.cs |
| ExecuteFrontLoadCommand.cs | カードの前詰め処理の実行を指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ExecuteFrontLoadCommand.cs |
| ICommand.cs | 全てのコマンドが実装する、イベントメッセージの基本契約を定義するインターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ICommand.cs |
| InletExecuteAbilityEffectCommand.cs | インレット発動によるアビリティ効果の実行を指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\InletExecuteAbilityEffectCommand.cs |
| PerformAttackCommand.cs | 1回分の攻撃を実行するよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\PerformAttackCommand.cs |
| PerformAttackedCommand.cs | 攻撃が完了したことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\PerformAttackedCommand.cs |
| PlayVfxCommand.cs | VFXの再生を要求するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\PlayVfxCommand.cs |
| PlayerZoneStateChangedCommand.cs | PlayerZoneのスロットが満員かどうかの状態変化を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\PlayerZoneStateChangedCommand.cs |
| ProcessAllCreaturesCooldownCommand.cs | 全クリーチャーのクールダウン処理を開始するきっかけとなるイベントコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ProcessAllCreaturesCooldownCommand.cs |
| ReflowCompletedCommand.cs | カードのリフローが完了し、カードが新しい位置へ移動する必要があることを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ReflowCompletedCommand.cs |
| ReflowOperationCompletedCommand.cs | リフロー操作が完全に終了したことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\ReflowOperationCompletedCommand.cs |
| SpriteDragOperationCompletedCommand.cs | ドラッグ操作が完全に終了し、後処理を行うためのコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\SpriteDragOperationCompletedCommand.cs |
| SystemDiceReflowCommand.cs | システム起因のダイスリフローを実行するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\SystemDiceReflowCommand.cs |
| SystemReflowCommand.cs | システム起因のカードリフローを実行するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\SystemReflowCommand.cs |
| UpdateEffectExpiredCommand.cs | エフェクトの有効期限切れをチェックするタイミングを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UpdateEffectExpiredCommand.cs |

### Scripts/Commands/Dices

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| MoveToAnimationReflowDiceCommand.cs | ダイスをリフロー後の位置へアニメーション付きで移動させるコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Dices\MoveToAnimationReflowDiceCommand.cs |
| PlacedDiceCommand.cs | ダイスを特定のスロットに配置するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Dices\PlacedDiceCommand.cs |
| ReflowPlacedDiceCommand.cs | リフロー計算のためにダイスを仮配置するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Dices\ReflowPlacedDiceCommand.cs |
| RemoveDiceCommand.cs | ダイスをスロットから取り除くコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Dices\RemoveDiceCommand.cs |

### Scripts/Commands/Display

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| DisplayIdentifiableStatusCommand.cs | 識別可能オブジェクトの現在のステータスをViewに反映させるコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Display\DisplayIdentifiableStatusCommand.cs |
| MoveToAnimationIdentifiableCommand.cs | 識別可能オブジェクトをアニメーション付きで移動させるコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Display\MoveToAnimationIdentifiableCommand.cs |
| MoveToIdentifiableCommand.cs | 識別可能オブジェクトを即座に移動させるコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\Display\MoveToIdentifiableCommand.cs |

### Scripts/Commands/IdentifiableState

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| DisableUIInteractionCommand.cs | 全てのUIインタラクションを無効化するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\DisableUIInteractionCommand.cs |
| EnableUIInteractionCommand.cs | 全てのUIインタラクションを有効化するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\EnableUIInteractionCommand.cs |
| IdentifiableStateBeginDragCommand.cs | 識別可能オブジェクトのドラッグが開始されたことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateBeginDragCommand.cs |
| IdentifiableStateClickCommand.cs | 識別可能オブジェクトがクリックされたことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateClickCommand.cs |
| IdentifiableStateClickedCommand.cs | 識別可能オブジェクトのクリック処理が完了したことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateClickedCommand.cs |
| IdentifiableStateDragCommand.cs | 識別可能オブジェクトがドラッグ中であることを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateDragCommand.cs |
| IdentifiableStateDropCommand.cs | 識別可能オブジェクトがドロップされたことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateDropCommand.cs |
| IdentifiableStateDropedCommand.cs | 識別可能オブジェクトのドロップ処理が完了したことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateDropedCommand.cs |
| IdentifiableStateEndDragCommand.cs | 識別可能オブジェクトのドラッグが終了したことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateEndDragCommand.cs |
| IdentifiableStateEndDragedCommand.cs | 識別可能オブジェクトのドラッグ終了処理が完了したことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateEndDragedCommand.cs |
| IdentifiableStateHoverCommand.cs | 識別可能オブジェクトがホバーされたことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateHoverCommand.cs |
| IdentifiableStateHoveredCommand.cs | 識別可能オブジェクトのホバー処理が完了したことを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateHoveredCommand.cs |
| IdentifiableStateUnhoverCommand.cs | 識別可能オブジェクトのアンホバーを通知するステートコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableState\IdentifiableStateUnhoverCommand.cs |

### Scripts/Commands/IdentifiableStatus

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableChangeHomePositionCommand.cs | 識別可能オブジェクトのホームポジションを変更するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableStatus\IdentifiableChangeHomePositionCommand.cs |
| IdentifiableChangeStatusCommand.cs | 識別可能オブジェクトのステータスを変更するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableStatus\IdentifiableChangeStatusCommand.cs |
| IdentifiableReturnHomePositionCommand.cs | 識別可能オブジェクトをホームポジションに戻すコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\IdentifiableStatus\IdentifiableReturnHomePositionCommand.cs |

### Scripts/Commands/UpdateManagers

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| InstanceSetUpedCommand.cs | 全てのインスタンスのセットアップが完了したことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UpdateManagers\InstanceSetUpedCommand.cs |
| SceneLoadedCommand.cs | シーンのロードが完了したことを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UpdateManagers\SceneLoadedCommand.cs |
| UpdateIdentifiableStatusCommand.cs | 識別可能オブジェクトのステータスを更新するよう指示するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UpdateManagers\UpdateIdentifiableStatusCommand.cs |

### Scripts/Commands/UserInput

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableBeginDragCommand.cs | ユーザー入力によるドラッグ開始を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableBeginDragCommand.cs |
| IdentifiableClickCommand.cs | ユーザー入力によるクリックを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableClickCommand.cs |
| IdentifiableClickedCommand.cs | ユーザー入力によるクリック処理の完了を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableClickedCommand.cs |
| IdentifiableDragCommand.cs | ユーザー入力によるドラッグ中の移動を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableDragCommand.cs |
| IdentifiableDropCommand.cs | ユーザー入力によるドロップを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableDropCommand.cs |
| IdentifiableDropedCommand.cs | ユーザー入力によるドロップ処理の完了を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableDropedCommand.cs |
| IdentifiableEndDragCommand.cs | ユーザー入力によるドラッグ終了を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableEndDragCommand.cs |
| IdentifiableEndDragedCommand.cs | ユーザー入力によるドラッグ終了処理の完了を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableEndDragedCommand.cs |
| IdentifiableHoverCommand.cs | ユーザー入力によるホバーを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableHoverCommand.cs |
| IdentifiableHoveredCommand.cs | ユーザー入力によるホバー処理の完了を通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableHoveredCommand.cs |
| IdentifiableUnhoverCommand.cs | ユーザー入力によるアンホバーを通知するコマンド。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Commands\UserInput\IdentifiableUnhoverCommand.cs |

### Scripts/Core

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AreaId.cs | 戦闘が行われるエリアを識別するためのEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\AreaId.cs |
| AreaOfEffect.cs | 攻撃や固有能力の効果範囲を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\AreaOfEffect.cs |
| BuffDebuffType.cs | バフ・デバフの種別を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\BuffDebuffType.cs |
| ChallengeRating.cs | 戦闘の難易度を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\ChallengeRating.cs |
| CompositeObjectId.cs | ゲームオブジェクトを一意に識別するための複合IDクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\CompositeObjectId.cs |
| CompositeObjectIdManager.cs | `CompositeObjectId`を生成・管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\CompositeObjectIdManager.cs |
| CreatureCardType.cs | クリーチャーカードの種別（プレイヤー／敵）を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\CreatureCardType.cs |
| DiceSlotLocation.cs | ダイススロットの具体的な位置を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\DiceSlotLocation.cs |
| EffectDurationType.cs | エフェクトの持続条件タイプを定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\EffectDurationType.cs |
| EffectTargetType.cs | エフェクトの適用対象ステータスを定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\EffectTargetType.cs |
| EnemyRoleId.cs | 敵の戦闘における役割を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\EnemyRoleId.cs |
| IAnimationStrategy.cs | アニメーションの振る舞いを定義する戦略インターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\IAnimationStrategy.cs |
| ICardDataProvider.cs | カード初期化データを提供するプロバイダーのインターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\ICardDataProvider.cs |
| ICreature.cs | クリーチャーの実行時インスタンスが実装すべき共通インターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\ICreature.cs |
| IdentifiableGameObject.cs | `CompositeObjectId`を持つMonoBehaviourの基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\IdentifiableGameObject.cs |
| IdentifiableStatus.cs | 識別可能オブジェクトが取りうる状態を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\IdentifiableStatus.cs |
| IDiceInlet.cs | ダイスインレットの実行時インスタンスが実装すべきインターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\IDiceInlet.cs |
| INameService.cs | エンティティの名称解決サービスの振る舞いを定義するインターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\INameService.cs |
| InletActivationViewType.cs | インレットの発動条件の見た目の種類を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\InletActivationViewType.cs |
| InteractionProfile.cs | ゲームオブジェクトのインタラクションの振る舞いを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\InteractionProfile.cs |
| IUIInteractionOrchestrator.cs | UIインタラクションを統括するクラスのインターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\IUIInteractionOrchestrator.cs |
| LinePosition.cs | スロットが存在する大まかなライン（エリア）を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\LinePosition.cs |
| MultiRendererVisualController.cs | 複数のレンダラーの視覚的プロパティを一括で制御するコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\MultiRendererVisualController.cs |
| PartId.cs | クリーチャーの外観を構成するパーツの種類を識別するためのEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\PartId.cs |
| SlotLocation.cs | ライン内でのスロットの具体的な役割や位置を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\SlotLocation.cs |
| SortingOrders.cs | ゲーム全体で使用される描画のソート順を一元管理する静的クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\SortingOrders.cs |
| SpriteLayerController.cs | 複数のSortingGroupやCanvasの描画順序を一括で制御するコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\SpriteLayerController.cs |
| SpriteStatus.cs | スプライトの状態を定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\SpriteStatus.cs |
| Team.cs | カードスロットやカードが所属するチームを定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\Team.cs |
| TriggerTiming.cs | アビリティの期限や発動のタイミングを定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\TriggerTiming.cs |
| UsageCountResetType.cs | ダイスインレットの使用可能回数がリセットされるタイミングを定義するEnum。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\UsageCountResetType.cs |

### Scripts/Core/Identifiable

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableCommandBus.cs | 識別可能オブジェクトに関連するイベントを一元管理する中央ハブ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Core\Identifiable\IdentifiableCommandBus.cs |

### Scripts/Data

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AllowedDiceFacesSO.cs | 投入可能なダイスの目の組み合わせを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\AllowedDiceFacesSO.cs |
| AppearanceProfile.cs | クリーチャーの「見た目」一式を定義するデータアセット。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\AppearanceProfile.cs |
| BaseEntityDefinition.cs | 全てのエンティティ定義の基底クラスとなるScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\BaseEntityDefinition.cs |
| CombatData.cs | 一連の戦闘全体（全ウェーブのシーケンスと共通ルール）を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\CombatData.cs |
| CombatScenarioRegistry.cs | 複数の`CombatData`を管理し、条件に合った戦闘データを検索するためのレジストリ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\CombatScenarioRegistry.cs |
| CreatureIdEntity.cs | クリーチャーを一意に識別するためのエンティティ定義ScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\CreatureIdEntity.cs |
| DiceInletConditionSO.cs | ダイスをインレットに配置するための条件を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\DiceInletConditionSO.cs |
| EffectData.cs | バフ・デバフ効果の静的な情報を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EffectData.cs |
| EnemyGroup.cs | 特定のテーマや条件でまとめられた`EnemyProfile`の集合を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EnemyGroup.cs |
| EnemyProfile.cs | 敵一体の戦闘における特性を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EnemyProfile.cs |
| FixedCardInitializer.cs | 固定情報から`CardInitializationData`を生成するためのScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\FixedCardInitializer.cs |
| InletAbilityProfile.cs | インレットの「条件」と「効果」をセットで保持する不変なデータクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\InletAbilityProfile.cs |
| InletProfileIdEntity.cs | インレットプロフィールのIDを定義するエンティティScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\InletProfileIdEntity.cs |
| NameDatabase.cs | `EntityDefinition`とそれに対応する表示名・説明文のペアを保持するデータベース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\NameDatabase.cs |
| SEData.cs | 再生するSE（サウンドエフェクト）のデータコンテナとなるScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\SEData.cs |
| SelectableSpriteSheet.cs | IDとスプライトのペアをコレクションとして保持するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\SelectableSpriteSheet.cs |
| StatusIconData.cs | ステータスアイコンの画像やフォーマットなどの静的なデータを定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\StatusIconData.cs |
| VfxDefinition.cs | VFXのIDと設定データを兼ねるScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\VfxDefinition.cs |
| WaveData.cs | 1ウェーブ分の敵の構成と配置を定義するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\WaveData.cs |

### Scripts/Data/EntityDefinition

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AnimationStrategyEntity.cs | アニメーション戦略を一意に識別するためのエンティティ定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EntityDefinition\AnimationStrategyEntity.cs |
| CompositeObjectIdTypeEntity.cs | `CompositeObjectId`のタイプを識別するためのエンティティ定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EntityDefinition\CompositeObjectIdTypeEntity.cs |
| DiceSlotPositionEntity.cs | ダイススロットの位置を一意に識別するためのエンティティ定義。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Data\EntityDefinition\DiceSlotPositionEntity.cs |

### Scripts/Domain

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AbilityInstance.cs | クリーチャーにアタッチされたアビリティの実行時インスタンス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\AbilityInstance.cs |
| AnimationContext.cs | アニメーション戦略に渡すための、アニメーションに必要なコンポーネントへの参照を集約したコンテキストクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\AnimationContext.cs |
| CardInitializationData.cs | カード生成に必要な全ての情報を集約したデータ転送オブジェクト(DTO)。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\CardInitializationData.cs |
| CardSlotData.cs | カードスロットの状態を保持するデータクラス（Model）。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\CardSlotData.cs |
| Creature.cs | `ICreature`インターフェースの具象実装。クリーチャーの論理的な実体。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\Creature.cs |
| CreatureData.cs | クリーチャーの基本データを定義するシリアライズ可能なクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\CreatureData.cs |
| DiceData.cs | ダイスのデータを保持するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\DiceData.cs |
| DiceInlet.cs | ダイスインレットの論理的な実行時インスタンス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\DiceInlet.cs |
| DiceInstance.cs | ダイスの状態を保持するデータクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\DiceInstance.cs |
| DiceSlotData.cs | ダイススロットのデータを保持するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\DiceSlotData.cs |
| EffectInstance.cs | 特定のターゲットに適用されたエフェクトの実行時インスタンス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\EffectInstance.cs |
| EnemyPlacement.cs | 敵一体の配置情報を定義するシリアライズ可能なクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\EnemyPlacement.cs |
| SEPlayer.cs | SE（サウンドエフェクト）の再生を担当する汎用コンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\SEPlayer.cs |
| VfxPlayer.cs | 個々のVFX再生を管理し、自身のライフサイクルを責務に持つコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\VfxPlayer.cs |
| VfxTrigger.cs | 特定のVFXを、アタッチされたGameObjectの位置で再生するトリガーコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\VfxTrigger.cs |

### Scripts/Domain/Identifiable

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| DiceSlotInstance.cs | ダイススロットを管理するインスタンス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\Identifiable\DiceSlotInstance.cs |
| IdentifiableStatusInstance.cs | 個々の識別可能オブジェクトの現在の状態を保持・管理するインスタンス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Domain\Identifiable\IdentifiableStatusInstance.cs |

### Scripts/Editor/Data

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| NameDatabaseEditor.cs | `NameDatabase` ScriptableObjectのカスタムエディタ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Editor\Data\NameDatabaseEditor.cs |

### Scripts/Factory

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AbilityFactory.cs | `AbilityInstance`の生成ロジックに特化したFactoryクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Factory\AbilityFactory.cs |
| CreatureFactory.cs | `ICreature`インスタンスの生成ロジックをカプセル化するFactoryクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Factory\CreatureFactory.cs |
| DiceFactory.cs | `DiceData`インスタンスの生成ロジックに特化したFactoryクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Factory\DiceFactory.cs |
| DiceInletFactory.cs | `DiceInlet`インスタンスの生成ロジックに特化したFactoryクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Factory\DiceInletFactory.cs |
| EffectFactory.cs | `EffectInstance`の生成ロジックに特化したFactoryクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Factory\EffectFactory.cs |

### Scripts/Initializers/LifetimeScope

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CombatLifetimeScope.cs | 戦闘シーンのDIコンテナ設定を行うVContainerのLifetimeScope。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\LifetimeScope\CombatLifetimeScope.cs |
| GameLifetimeScope.cs | ゲーム全体のDIコンテナ設定を行うVContainerのLifetimeScope。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\LifetimeScope\GameLifetimeScope.cs |

### Scripts/Initializers/PrefabInitializeManager

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CreatureCardSpawnInfoManager.cs | `CreatureCardSpawnInfo`のリストを管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\PrefabInitializeManager\CreatureCardSpawnInfoManager.cs |
| DiceSpawnInfoManager.cs | `DiceSpawnInfo`のリストを管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\PrefabInitializeManager\DiceSpawnInfoManager.cs |

### Scripts/Initializers/PrefabSpawner

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CreatureCardSpawner.cs | `CreatureCardSpawnInfo`に基づいてクリーチャーカードのPrefabをインスタンス化するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\PrefabSpawner\CreatureCardSpawner.cs |
| DiceSpawner.cs | `DiceSpawnInfo`に基づいてダイスのPrefabをインスタンス化するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\PrefabSpawner\DiceSpawner.cs |

### Scripts/Initializers/SceneInitializers

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| GameInitializer.cs | ゲーム起動時に各コンポーネントの初期化を制御するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\SceneInitializers\GameInitializer.cs |
| IGameInitializable.cs | 制御された初期化処理を必要とするコンポーネントが実装すべきインターフェース。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\SceneInitializers\IGameInitializable.cs |
| SceneInitializer.cs | シーンロード完了後に、登録された`IGameInitializable`コンポーネントの初期化を順次実行するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\SceneInitializers\SceneInitializer.cs |

### Scripts/Initializers/SpawnInfo

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CreatureCardSpawnInfo.cs | クリーチャーカードのスポーン情報を保持するデータクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\SpawnInfo\CreatureCardSpawnInfo.cs |
| DiceSpawnInfo.cs | ダイスのスポーン情報を保持するデータクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Initializers\SpawnInfo\DiceSpawnInfo.cs |

### Scripts/Managers

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| AbilityManager.cs | ゲーム内の全てのアクティブな`AbilityInstance`を一元管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\AbilityManager.cs |
| CardSlotInteractionHandler.cs | UIからのスロット関連のインタラクションを処理するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\CardSlotInteractionHandler.cs |
| CardSlotManager.cs | 全てのカードスロット関連の処理の窓口となるファサードクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\CardSlotManager.cs |
| CombatManager.cs | 戦闘フェーズ全体の流れを制御する高レベルなゲームロジックを統括するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\CombatManager.cs |
| CreatureManager.cs | ゲーム内に存在する全ての`ICreature`インスタンスを一元的に管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\CreatureManager.cs |
| DiceInletManager.cs | ゲーム内に存在する全ての`DiceInlet`インスタンスを一元的に管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\DiceInletManager.cs |
| DiceSlotInteractionHandler.cs | UIからのダイススロット関連のインタラクションを処理するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\DiceSlotInteractionHandler.cs |
| EffectManager.cs | バフ・デバフなどのエフェクトインスタンスを一元管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\EffectManager.cs |
| IdentifiableStatusManager.cs | 全ての識別可能オブジェクトの状態(`IdentifiableStatusInstance`)を一元管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\IdentifiableStatusManager.cs |
| Old_DiceSlotManager.cs | 全てのダイススロットの状態を管理し、ダイスの配置などを担当するマネージャークラス（旧版）。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\Old_DiceSlotManager.cs |
| SoundManager.cs | サウンド設定（特に音量）を一元管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\SoundManager.cs |
| VfxManager.cs | VFXの再生とオブジェクトプールを管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\VfxManager.cs |
| ViewRegistry.cs | シーン上の全ての`BaseSpriteView`インスタンスを管理し、IDによる検索機能を提供するレジストリ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\ViewRegistry.cs |

### Scripts/Managers/Dices

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| DiceManager.cs | 全てのダイスの生成、状態管理、リロールなどを一元的に行うマネージャークラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\Dices\DiceManager.cs |
| DiceSlotManager.cs | 全てのダイススロットの状態を管理し、ダイスの配置などを担当するマネージャークラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Managers\Dices\DiceSlotManager.cs |

### Scripts/Orchestrator

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CardInteractionOrchestrator.cs | カード関連のUIインタラクションを統括する司令塔クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Orchestrator\CardInteractionOrchestrator.cs |
| DiceInteractionOrchestrator.cs | ダイス関連のUIインタラクションを統括する司令塔クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Orchestrator\DiceInteractionOrchestrator.cs |

### Scripts/Presenter

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CreatureCardPresenter.cs | `ICreature`(Model)と`CreatureCardView`(View)を接続する仲介役。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Presenter\CreatureCardPresenter.cs |
| DiceInletPresenter.cs | `DiceInlet`(Model)と`DiceInletView`(View)を接続する仲介役。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Presenter\DiceInletPresenter.cs |
| StatusIconPresenter.cs | `ICreature`(Model)と`StatusIconView`(View)を接続し、表示更新タイミングを制御する仲介役。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Presenter\StatusIconPresenter.cs |

### Scripts/Presenter/Dices

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| DicePresenter.cs | `DiceInstance`(Model)と`Old_DiceView`(View)を接続し、状態を同期させる仲介役。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Presenter\Dices\DicePresenter.cs |
| DiceSlotController.cs | `DiceSlotInstance`を管理し、その変更をコマンドで通知するコントローラー。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Presenter\Dices\DiceSlotController.cs |

### Scripts/Registries

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CompositeObjectRegistry.cs | シーン上の全ての`CompositeObjectId`を登録・管理するレジストリ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Registries\CompositeObjectRegistry.cs |

### Scripts/Repository

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CardSlotStateRepository.cs | 全てのカードスロットの状態を管理するリポジトリクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Repository\CardSlotStateRepository.cs |
| DiceSlotStateRepository.cs | 全てのダイススロットの状態を管理するリポジトリクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Repository\DiceSlotStateRepository.cs |

### Scripts/Service

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CardLifecycleService.cs | カードの生成、初期化、破棄など、ライフサイクル全般を管理するサービス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\CardLifecycleService.cs |
| CardPlacementService.cs | カードの配置ロジックを担当するサービスクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\CardPlacementService.cs |
| CombatDataLoaderService.cs | `CombatScenarioRegistry`を通じて、条件に合った`CombatData`をロードする責務を持つサービス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\CombatDataLoaderService.cs |
| DicePlacementService.cs | ダイスの配置ロジックを担当するサービスクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\DicePlacementService.cs |
| EnemyCardDataProvider.cs | エネミーのデータから`CardInitializationData`を生成する責務を負うデータプロバイダー。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\EnemyCardDataProvider.cs |
| NameService.cs | `EntityDefinition`から対応する名称を検索して返す名称解決サービス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\NameService.cs |
| PlayerCardDataProvider.cs | プレイヤーのデータから`CardInitializationData`を生成する責務を負うデータプロバイダー。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\PlayerCardDataProvider.cs |
| ReflowService.cs | カードのリフロー（再配置）および前詰め処理の計算ロジックを担当するサービスクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\ReflowService.cs |
| TargetSelector.cs | 攻撃やスキルの効果範囲に基づき、対象を選択するサービスクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\TargetSelector.cs |
| WaveGeneratorService.cs | `CombatData`に基づき、具体的な出現エネミーを決定するサービス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Service\WaveGeneratorService.cs |

### Scripts/State

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| UIActivationPolicy.cs | UIの状態に基づき、各UI要素のインタラクション可否を制御するポリシークラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\State\UIActivationPolicy.cs |
| UIStateMachine.cs | UIの全体的なインタラクション状態を管理するステートマシン。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\State\UIStateMachine.cs |

### Scripts/Strategy

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BodySlamAnimationStrategy.cs | 体当たりアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\BodySlamAnimationStrategy.cs |
| BuffAnimationStrategy.cs | バフ効果を受けた際のアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\BuffAnimationStrategy.cs |
| CardInteractionStrategy.cs | カードとスロットのインタラクションが可能かどうかの条件をチェックする戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\CardInteractionStrategy.cs |
| DamageAnimationStrategy.cs | ダメージを受けた際のアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\DamageAnimationStrategy.cs |
| DeathAnimationStrategy.cs | 死亡時のアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\DeathAnimationStrategy.cs |
| DiceInteractionStrategy.cs | ダイスのUIインタラクション戦略を実装するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\DiceInteractionStrategy.cs |
| DragAnimationStrategy.cs | ドラッグ中のアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\DragAnimationStrategy.cs |
| HoverAnimationStrategy.cs | ホバー時のアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\HoverAnimationStrategy.cs |
| NormalAnimationStrategy.cs | 通常状態のアニメーションを実装する戦略クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Strategy\NormalAnimationStrategy.cs |

### Scripts/Systems

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CardSlotDebug.cs | スロット関連のデバッグ機能を提供するクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Systems\CardSlotDebug.cs |
| SystemReflowController.cs | システム起因のリフロー処理を実行するコントローラー。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Systems\SystemReflowController.cs |

### Scripts/Tester

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| DebugViewer.cs | 実行中の各種データをインスペクターに表示するためのデバッグ用クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Tester\DebugViewer.cs |
| PlacementCardTester.cs | ゲームの初期状態をセットアップし、テスト用のカード配置を行うデバッグ用クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Tester\PlacementCardTester.cs |

### Scripts/UI

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BaseSpriteView.cs | 全てのSpriteベースのViewの抽象基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\BaseSpriteView.cs |
| CardSlotView.cs | カードスロットの視覚的な表示を管理するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\CardSlotView.cs |
| CreatureAppearanceController.cs | `AppearanceProfile`に基づき、ゲームオブジェクトの表示を制御するコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\CreatureAppearanceController.cs |
| CreatureCardView.cs | クリーチャーカードの視覚的な表示を管理するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\CreatureCardView.cs |
| DiceInletView.cs | ダイスインレットの視覚表現とUIインタラクションを担当するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\DiceInletView.cs |
| DiceSlotView.cs | ダイススロットの視覚的な表示を管理するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\DiceSlotView.cs |
| Old_DiceView.cs | ダイスの視覚的な表示と状態遷移を管理するコンポーネント（旧版）。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Old_DiceView.cs |
| SpriteCommandBus.cs | SpriteUIに関連するイベントを一元管理する中央ハブ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\SpriteCommandBus.cs |
| SpriteInputHandler.cs | SpriteUIのマウスイベントを検知し、対応するコマンドを発行するハンドラー。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\SpriteInputHandler.cs |
| SpriteSelector.cs | `SelectableSpriteSheet`のデータに基づき、IDを指定してSpriteRendererの表示を切り替える汎用コンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\SpriteSelector.cs |
| StatusIconView.cs | ステータスアイコンの視覚的な表示とアニメーションの実行を担当するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\StatusIconView.cs |

### Scripts/UI/Identifiable/Operator

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BaseIdentifiableStateOperator.cs | 特定の識別可能オブジェクトの状態変化コマンドを監視し、対応する処理を実行するための基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\Operator\BaseIdentifiableStateOperator.cs |
| DragStateOperator.cs | 識別可能オブジェクトの状態変化コマンドを監視し、具体的な処理を実装するためのクラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\Operator\DragStateOperator.cs |

### Scripts/UI/Identifiable/State

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableInputHandler.cs | 識別可能オブジェクトのマウスイベントを検知し、対応するコマンドを発行するハンドラー。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\State\IdentifiableInputHandler.cs |
| IdentifiableUIState.cs | UIのインタラクション状態を表す列挙型。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\State\IdentifiableUIState.cs |
| IdentifiableUIStateMachine.cs | UIの全体的なインタラクション状態を管理するステートマシン。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\State\IdentifiableUIStateMachine.cs |

### Scripts/UI/Identifiable/ViewManagers

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableStatusViewManager.cs | 全ての識別可能オブジェクトの状態(`IdentifiableStatusInstance`)を一元管理するScriptableObject。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\ViewManagers\IdentifiableStatusViewManager.cs |

### Scripts/UI/Identifiable/ViewPresenters

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableStatusPresenter.cs | `IdentifiableStatusInstance`(Model)と`IdentifiableStatusView`(View)を接続する仲介役。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\ViewPresenters\IdentifiableStatusPresenter.cs |

### Scripts/UI/Identifiable/ViewRegistry

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| IdentifiableViewRegistry.cs | シーン上の全ての`BaseIdentifiableView`インスタンスを管理し、IDによる検索機能を提供するレジストリ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\ViewRegistry\IdentifiableViewRegistry.cs |

### Scripts/UI/Identifiable/Views

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| BaseIdentifiableView.cs | 全ての識別可能Viewの抽象基底クラス。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\Views\BaseIdentifiableView.cs |
| DiceView.cs | ダイスの視覚的な表示と状態遷移を管理するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\Views\DiceView.cs |
| IdentifiableStatusView.cs | 識別可能オブジェクトの状態を視覚的に表示するViewコンポーネント。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\UI\Identifiable\Views\IdentifiableStatusView.cs |

### Scripts/Utility

| ファイル名 | 解説 | 格納場所 |
| :--- | :--- | :--- |
| CoroutineRunner.cs | MonoBehaviourを継承しないクラスからコルーチンを実行するためのユーティリティ。 | D:\Users\ponki\Unity\CardsAndDicesProject\Assets\CardsAndDices\Scripts\Utility\CoroutineRunner.cs |

---

## 関連ファイル

- [guide_file_management.md](./guide_file_management.md)

---

## 更新履歴

- 2025-09-17: ファイル一覧を最新化し、全てのファイルの解説を追記 (Gemini)
- 2025-09-05: ファイルリストを更新 (Gemini)
- 2025-08-27: ファイルリストを最新化し、全てのファイルの解説を追記 (Gemini)
- 2025-08-21: ファイルリストを最新化 (Gemini)
- 2025-08-19: ファイル一覧を最新化し、解説を追記 (Gemini)
- 2025-08-19: `guide_files.md` を削除し、`GEMINI.md` を追加。全ファイルパスを相対パスに修正 (Gemini)
- 2025-08-15: ソースコードディレクトリ構造の提案に基づき、ファイルパスを更新 (Gemini)
- 2025-08-14: ファイルリストを最新化 (Gemini)
- 2025-08-13: ファイルリストを最新化 (Gemini)
- 2025-08-08: ファイルリストを最新化 (Gemini)
- 2025-07-30: プロジェクトルールへの準拠 (Gemini - Technical Writer for Game Development)

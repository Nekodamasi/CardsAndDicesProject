using UnityEngine;
using VContainer; // VContainer.Inject を使用するために追加
using Cysharp.Threading.Tasks;
using System;

namespace CardsAndDices
{
	/// <summary>
	/// 戦闘画面での各フェーズによるイベントの遷移を担当します
	/// </summary>
	[CreateAssetMenu(fileName = "CombatPhaseStateMachine", menuName = "CardsAndDices/Combats/Managers/States/CombatPhaseStateMachine")]
	public class CombatPhaseStateMachine : ScriptableObject, IDisposable
	{
		private GameEventBus _eventBus;
		private CombatPhase _currentCombatPhase;

		/// <summary>
		/// ScriptableObjectが初期化される時の処理。
		/// VContainerによって呼び出されます。
		/// </summary>
		[Inject]
		public void Initialize(GameEventBus eventBus)
		{
			_eventBus = eventBus;
			_currentCombatPhase = CombatPhase.None;
			_eventBus.On<ChangeCombatPhaseEvent>(OnChangeCombatPhase);
			_eventBus.On<BuffDebuffEffectEndActionEvent>(OnBuffDebuffEffectEndAction);
			_eventBus.On<CoolDownZeoAttackEndEvent>(OnCoolDownZeoAttackEnd);
			_eventBus.On<CreatureAttackEndEvent>(OnCreatureAttackEnd);
			_eventBus.On<CombatPhaseDiceOffScreenEndEvent>(OnCombatPhaseDiceOffScreenEnd);
			_eventBus.On<CreatureTurnEndExecuteAbilityEndEvent>(OnCreatureTurnEndExecuteAbilityEnd);
			_eventBus.On<CombatPhaseCardFrontLoadMovementEndEvent>(OnCombatPhaseCardFrontLoadMovementEnd);
			_eventBus.On<CombatPhaseWaveEnemySetUpEndEvent>(OnCombatPhaseWaveEnemySetUpEnd);
		}

        public void Dispose()
        {
			_eventBus.Off<ChangeCombatPhaseEvent>(OnChangeCombatPhase);
			_eventBus.Off<BuffDebuffEffectEndActionEvent>(OnBuffDebuffEffectEndAction);
			_eventBus.Off<CoolDownZeoAttackEndEvent>(OnCoolDownZeoAttackEnd);
			_eventBus.Off<CreatureAttackEndEvent>(OnCreatureAttackEnd);
			_eventBus.Off<CombatPhaseDiceOffScreenEndEvent>(OnCombatPhaseDiceOffScreenEnd);
			_eventBus.Off<CreatureTurnEndExecuteAbilityEndEvent>(OnCreatureTurnEndExecuteAbilityEnd);
			_eventBus.Off<CombatPhaseCardFrontLoadMovementEndEvent>(OnCombatPhaseCardFrontLoadMovementEnd);
			_eventBus.Off<CombatPhaseWaveEnemySetUpEndEvent>(OnCombatPhaseWaveEnemySetUpEnd);
        }

		/// <summary>
		/// 前詰め処理完了イベント
		/// </summary>
		private void OnCombatPhaseWaveEnemySetUpEnd(CombatPhaseWaveEnemySetUpEndEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.TurnEndPhase:
					Debug.Log("ここに来れたらOK");
					_eventBus.Emit(new ResetUIStatusEvent());
					break;
			}
		}

		/// <summary>
		/// 前詰め処理完了イベント
		/// </summary>
		private void OnCombatPhaseCardFrontLoadMovementEnd(CombatPhaseCardFrontLoadMovementEndEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.TurnEndPhase:
					_eventBus.Emit(new CombatPhaseWaveEnemySetUpEvent());
					break;
			}
		}

		/// <summary>
		/// ターンエンドアビリティ実行終了イベント
		/// </summary>
		private void OnCreatureTurnEndExecuteAbilityEnd(CreatureTurnEndExecuteAbilityEndEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.TurnEndPhase:
					_eventBus.Emit(new CombatPhaseCardFrontLoadMovementEvent());
					_eventBus.Emit(new ResetTurnEndEvent());
					break;
			}
		}

		/// <summary>
		/// ダイスオフスクリーンイベント
		/// </summary>
		private void OnCombatPhaseDiceOffScreenEnd(CombatPhaseDiceOffScreenEndEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.TurnEndPhase:
					_eventBus.Emit(new CreatureTurnEndExecuteAbilityEvent());
					break;
			}
		}

		/// <summary>
		/// クリーチャー攻撃処理終了
		/// </summary>
		private void OnCreatureAttackEnd(CreatureAttackEndEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.DiceInletEffectPhase:
					// クールダウンフェーズに変更
					SetCurrentCombatPhase(CombatPhase.CooldownPhase);
					_eventBus.Emit(new CoolDownZeoAttackEvent());
					break;
				case CombatPhase.TurnEndPhase:
					_eventBus.Emit(new CreatureTurnEndExecuteAbilityEvent());
					break;
			}
		}

		/// <summary>
		/// クールダウン攻撃処理終了
		/// </summary>
		private void OnCoolDownZeoAttackEnd(CoolDownZeoAttackEndEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.DiceInletEffectPhase:
					// クールダウンフェーズに変更
					SetCurrentCombatPhase(CombatPhase.CooldownPhase);
					_eventBus.Emit(new CoolDownStartEvent());
					break;
				case CombatPhase.CooldownPhase:
					// プレイヤーインプットフェーズに変更
					SetCurrentCombatPhase(CombatPhase.PlayerInputPhase);
					_eventBus.Emit(new ResetCoolDownEvent());
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// バフデバフエフェクト完了イベント
		/// </summary>
		private void OnBuffDebuffEffectEndAction(BuffDebuffEffectEndActionEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.DiceInletEffectPhase:
                	_eventBus.Emit(new CoolDownZeoAttackEvent());
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 現在現在のコンバットフェーズを設定します
		/// </summary>
		private void SetCurrentCombatPhase(CombatPhase combatPhase)
		{
			_currentCombatPhase = combatPhase;
		}

		/// <summary>
		/// コンバットフェーズ変更イベント
		/// </summary>
		private void OnChangeCombatPhase(ChangeCombatPhaseEvent evt)
		{
			SetCurrentCombatPhase(evt.CombatPhase);
			if (_currentCombatPhase == CombatPhase.PlayerInputPhase) return;
            _eventBus.Emit(new DisableUIInteractionEvent());
			
		}
	}
}

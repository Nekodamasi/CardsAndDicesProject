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
		}

        public void Dispose()
        {
			_eventBus.Off<ChangeCombatPhaseEvent>(OnChangeCombatPhase);
			_eventBus.Off<BuffDebuffEffectEndActionEvent>(OnBuffDebuffEffectEndAction);
			_eventBus.Off<CoolDownZeoAttackEndEvent>(OnCoolDownZeoAttackEnd);
			_eventBus.Off<CreatureAttackEndEvent>(OnCreatureAttackEnd);
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
		}
	}
}

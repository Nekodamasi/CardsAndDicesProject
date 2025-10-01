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
//			_eventBus.On<AttackActionStartEvent>(OnAttackActionStart);
//			_eventBus.On<AttackActionEndEvent>(OnAttackActionEnd);
		}

        public void Dispose()
        {
			_eventBus.Off<ChangeCombatPhaseEvent>(OnChangeCombatPhase);
			_eventBus.Off<BuffDebuffEffectEndActionEvent>(OnBuffDebuffEffectEndAction);
//			_eventBus.Off<AttackActionStartEvent>(OnAttackActionStart);
//			_eventBus.Off<AttackActionEndEvent>(OnAttackActionEnd);
        }

		/// <summary>
		/// バフデバフエフェクト完了イベント
		/// </summary>
		private void OnBuffDebuffEffectEndAction(BuffDebuffEffectEndActionEvent evt)
		{
			switch (_currentCombatPhase)
			{
				case CombatPhase.DiceInletEffectPhase:
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

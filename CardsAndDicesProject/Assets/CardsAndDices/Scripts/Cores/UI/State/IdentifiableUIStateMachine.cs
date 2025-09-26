using UnityEngine;
using VContainer; // VContainer.Inject を使用するために追加
using Cysharp.Threading.Tasks;
using System;

namespace CardsAndDices
{
	/// <summary>
	/// UIの全体的なインタラクション状態を管理するステートマシン。
	/// カードやダイスのドラッグ状態などを一元的に管理し、UIの競合を防ぎます。
	/// 設計書「gdd_combat_system_mockup.md」に基づき、ScriptableObjectとして機能します。
	/// </summary>
	[CreateAssetMenu(fileName = "IdentifiableUIStateMachine", menuName = "CardsAndDices/UI/Identifiable/State/UIStateMachine")]
	public class IdentifiableUIStateMachine : ScriptableObject
	{
		private GameEventBus _identifiableEventBus;

		/// <summary>
		/// ScriptableObjectが初期化される時の処理。
		/// VContainerによって呼び出されます。
		/// </summary>
		[Inject]
		public void Initialize(GameEventBus identifiableEventBus)
		{
			_identifiableEventBus = identifiableEventBus;
			CurrentState = IdentifiableUIState.Idle;
			StateObjectId = null;
			TargetObjectId = null;
			_identifiableEventBus.On<IdentifiableBeginDragEvent>(OnIdentifiableBeginDrag);
			_identifiableEventBus.On<IdentifiableClickEvent>(OnIdentifiableClick);
			_identifiableEventBus.On<IdentifiableDropEvent>(OnIdentifiableDrop);
			_identifiableEventBus.On<IdentifiableDragEvent>(OnIdentifiableDrag);
			_identifiableEventBus.On<IdentifiableEndDragEvent>(OnIdentifiableEndDrag);
			_identifiableEventBus.On<IdentifiableUnhoverEvent>(OnIdentifiableUnhover);
			_identifiableEventBus.On<IdentifiableHoverEvent>(OnIdentifiableHover);
			_identifiableEventBus.On<IdentifiableHoveredEvent>(OnIdentifiableHovered);
			_identifiableEventBus.On<DisableUIInteractionEvent>(OnDisableUIInteraction);
			_identifiableEventBus.On<EnableUIInteractionEvent>(OnEnableUIInteraction);
			_identifiableEventBus.On<ResetUIStatusEvent>(OnResetUIStatus);			
		}

		/// <summary>
		/// 現在STATEを設定します
		/// </summary>
		private void SetCurrentState(IdentifiableUIState state, CompositeObjectId id, CompositeObjectId targetId)
		{
			CurrentState = state;
			StateObjectId = id;
			TargetObjectId = targetId;
		}

		/// <summary>
		/// UIStatusをリセットします
		/// </summary>
		private void OnResetUIStatus(ResetUIStatusEvent evt)
		{
			SetCurrentState(IdentifiableUIState.Idle, null, null);
		}

		/// <summary>
		/// UI操作制限モードを有効にします
		/// </summary>
		private void OnDisableUIInteraction(DisableUIInteractionEvent evt)
		{
			SetCurrentState(IdentifiableUIState.NonResponse, null, null);
		}

		/// <summary>
		/// UI操作制限モードを解除します
		/// </summary>
		private void OnEnableUIInteraction(EnableUIInteractionEvent evt)
		{
			SetCurrentState(IdentifiableUIState.Idle, null, null);
		}

		/// <summary>
		/// ホバー
		/// </summary>
		private void OnIdentifiableHover(IdentifiableHoverEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Idle:
					SetCurrentState(IdentifiableUIState.Hover, evt.ExecutedObjectId, null);

					// ホバーコマンド
					_identifiableEventBus.Emit(new IdentifiableStateHoverEvent(evt.ExecutedObjectId));
					break;
				case IdentifiableUIState.Hovered:
					if (StateObjectId != evt.ExecutedObjectId)
					{
						SetCurrentState(IdentifiableUIState.Hover, evt.ExecutedObjectId, null);

						// ホバーコマンド
						_identifiableEventBus.Emit(new IdentifiableStateHoverEvent(evt.ExecutedObjectId));
					}
					break;
				case IdentifiableUIState.Dragging:
						// ドラッグホバーコマンド
						_identifiableEventBus.Emit(new IdentifiableStateDragedHoverEvent(evt.ExecutedObjectId, StateObjectId));
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// ホバー完了
		/// </summary>
		private void OnIdentifiableHovered(IdentifiableHoveredEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Hover:
					if (StateObjectId == evt.ExecutedObjectId)
					{
						SetCurrentState(IdentifiableUIState.Hovered, evt.ExecutedObjectId, null);
						_identifiableEventBus.Emit(new IdentifiableStateHoveredEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// アンホバー
		/// </summary>
		private void OnIdentifiableUnhover(IdentifiableUnhoverEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Hovered:
					if (StateObjectId == evt.ExecutedObjectId)
					{
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// アンホバーコマンド
						_identifiableEventBus.Emit(new IdentifiableStateUnhoverEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// ドラッグ終了
		/// </summary>
		private async void OnIdentifiableEndDrag(IdentifiableEndDragEvent evt)
		{
            // 遅延処理でドロップの成否を判定
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

			switch (CurrentState)
			{
				case IdentifiableUIState.Dragging:
					if (StateObjectId == evt.ExecutedObjectId)
					{
						Debug.Log("すてーとましん（ドラッグ終了）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.EndDrag, evt.ExecutedObjectId, null);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateEndDragEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}
/*
        /// <summary>
		/// ドラッグ終了完了
		/// </summary>
		private void OnIdentifiableEndDraged(IdentifiableEndDragedEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.EndDrag:
					if (StateObjectId == evt.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドラッグ終了完了）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateEndDragedEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}
*/
		/// <summary>
		/// ドラッグ中
		/// </summary>
		private void OnIdentifiableDrag(IdentifiableDragEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.BiginDrag:
				case IdentifiableUIState.Dragging:
					if (StateObjectId == evt.ExecutedObjectId)
					{
//            			Debug.Log("すてーとましん（ドラッグ中）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Dragging, evt.ExecutedObjectId, null);
						// ドラッグ中コマンド
						_identifiableEventBus.Emit(new IdentifiableStateDragEvent(evt.ExecutedObjectId, evt.NewPosition));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// ドロップ
		/// </summary>
		private void OnIdentifiableDrop(IdentifiableDropEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Dragging:
					if (StateObjectId == evt.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドロップ）:" + evt.ExecutedObjectId + "/" + evt.TargetObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Drop, evt.ExecutedObjectId, evt.TargetObjectId);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateDropEvent(evt.ExecutedObjectId, evt.TargetObjectId));
					}
					break;
				default:
					break;
			}
		}
/*
        /// <summary>
		/// ドロップ完了
		/// </summary>
		private void OnIdentifiableDroped(IdentifiableDropedEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Drop:
					if (StateObjectId == evt.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドロップ完了）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateDropedEvent(evt.ExecutedObjectId, evt.TargetObjectId));
					}
					break;
				default:
					break;
			}
		}
*/
        /// <summary>
		/// クリック
		/// </summary>
		private void OnIdentifiableClick(IdentifiableClickEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Idle:
            		Debug.Log("すてーとましん（クリック）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
					SetCurrentState(IdentifiableUIState.BiginClick, evt.ExecutedObjectId, null);
					// ドラッグ開始コマンド
					_identifiableEventBus.Emit(new IdentifiableStateClickEvent(evt.ExecutedObjectId));
					break;
				case IdentifiableUIState.Hovered:
				case IdentifiableUIState.Hover:
					if (StateObjectId == evt.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（クリック）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.BiginClick, evt.ExecutedObjectId, null);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateClickEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

/*
		/// <summary>
		/// クリック完了
		/// </summary>
		private void OnIdentifiableClicked(IdentifiableClickedEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.BiginClick:
					if (StateObjectId == evt.ExecutedObjectId)
					{
						Debug.Log("すてーとましん（クリック完了）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateClickedEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}
*/
		/// <summary>
		/// ドラッグ開始
		/// </summary>
		private void OnIdentifiableBeginDrag(IdentifiableBeginDragEvent evt)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Idle:
            		Debug.Log("すてーとましん（ドラッグ開始）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
					SetCurrentState(IdentifiableUIState.BiginDrag, evt.ExecutedObjectId, null);
					// ドラッグ開始コマンド
					_identifiableEventBus.Emit(new IdentifiableStateBeginDragEvent(evt.ExecutedObjectId));
					break;
				case IdentifiableUIState.Hovered:
					if (StateObjectId == evt.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドラッグ開始）:" + evt.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.BiginDrag, evt.ExecutedObjectId, null);
						// ドラッグ開始コマンド
						_identifiableEventBus.Emit(new IdentifiableStateBeginDragEvent(evt.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// 現在のUI状態を取得します。
		/// </summary>
		public IdentifiableUIState CurrentState { get; private set; } = IdentifiableUIState.Idle;

		/// <summary>
		/// Stateの変更に寄与するオブジェクトのID。
		/// </summary>
		public CompositeObjectId StateObjectId { get; private set; }

		/// <summary>
		/// Stateの変更によるターゲットオブジェクトのID。
		/// </summary>
		public CompositeObjectId TargetObjectId { get; private set; }
	}
}

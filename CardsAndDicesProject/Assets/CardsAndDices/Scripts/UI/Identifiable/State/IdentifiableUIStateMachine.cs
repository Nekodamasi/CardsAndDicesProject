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
		public IdentifiableCommandBus _identifiableCommandBus;

		/// <summary>
		/// ScriptableObjectが初期化される時の処理。
		/// VContainerによって呼び出されます。
		/// </summary>
		[Inject]
		public void Initialize(IdentifiableCommandBus identifiableCommandBus)
		{
			_identifiableCommandBus = identifiableCommandBus;
			CurrentState = IdentifiableUIState.Idle;
			StateObjectId = null;
			TargetObjectId = null;
			_identifiableCommandBus.On<IdentifiableBeginDragCommand>(OnIdentifiableBeginDrag);
			_identifiableCommandBus.On<IdentifiableClickCommand>(OnIdentifiableClick);
			_identifiableCommandBus.On<IdentifiableClickedCommand>(OnIdentifiableClicked);
			_identifiableCommandBus.On<IdentifiableDropCommand>(OnIdentifiableDrop);
			_identifiableCommandBus.On<IdentifiableDropedCommand>(OnIdentifiableDroped);
			_identifiableCommandBus.On<IdentifiableDragCommand>(OnIdentifiableDrag);
			_identifiableCommandBus.On<IdentifiableEndDragCommand>(OnIdentifiableEndDrag);
			_identifiableCommandBus.On<IdentifiableEndDragedCommand>(OnIdentifiableEndDraged);
			_identifiableCommandBus.On<IdentifiableUnhoverCommand>(OnIdentifiableUnhover);
			_identifiableCommandBus.On<IdentifiableHoverCommand>(OnIdentifiableHover);
			_identifiableCommandBus.On<IdentifiableHoveredCommand>(OnIdentifiableHovered);
			_identifiableCommandBus.On<DisableUIInteractionCommand>(OnDisableUIInteraction);
			_identifiableCommandBus.On<EnableUIInteractionCommand>(OnEnableUIInteraction);
			
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
		/// UI操作制限モードを有効にします
		/// </summary>
		private void OnDisableUIInteraction(DisableUIInteractionCommand cmd)
		{
			SetCurrentState(IdentifiableUIState.NonResponse, null, null);
		}

		/// <summary>
		/// UI操作制限モードを解除します
		/// </summary>
		private void OnEnableUIInteraction(EnableUIInteractionCommand cmd)
		{
			SetCurrentState(IdentifiableUIState.Idle, null, null);
		}

		/// <summary>
		/// ホバー
		/// </summary>
		private void OnIdentifiableHover(IdentifiableHoverCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Idle:
				case IdentifiableUIState.Hovered:
					Debug.Log("すてーとましん（ホバー）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
					SetCurrentState(IdentifiableUIState.Hover, cmd.ExecutedObjectId, null);

					// アンホバーコマンド
					_identifiableCommandBus.Emit(new IdentifiableStateHoverCommand(cmd.ExecutedObjectId));
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// ホバー完了
		/// </summary>
		private void OnIdentifiableHovered(IdentifiableHoveredCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Hover:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ホバー完了）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Hovered, cmd.ExecutedObjectId, null);
						_identifiableCommandBus.Emit(new IdentifiableStateHoveredCommand(cmd.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// アンホバー
		/// </summary>
		private void OnIdentifiableUnhover(IdentifiableUnhoverCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Hovered:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（あんホバー）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// アンホバーコマンド
						_identifiableCommandBus.Emit(new IdentifiableStateUnhoverCommand(cmd.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// ドラッグ終了
		/// </summary>
		private async void OnIdentifiableEndDrag(IdentifiableEndDragCommand cmd)
		{
            // 遅延処理でドロップの成否を判定
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));

			switch (CurrentState)
			{
				case IdentifiableUIState.Dragging:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
						Debug.Log("すてーとましん（ドラッグ終了）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.EndDrag, cmd.ExecutedObjectId, null);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateEndDragCommand(cmd.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// ドラッグ終了完了
		/// </summary>
		private void OnIdentifiableEndDraged(IdentifiableEndDragedCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.EndDrag:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドラッグ終了完了）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateEndDragedCommand(cmd.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// ドラッグ中
		/// </summary>
		private void OnIdentifiableDrag(IdentifiableDragCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.BiginDrag:
				case IdentifiableUIState.Dragging:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドラッグ中）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Dragging, cmd.ExecutedObjectId, null);
						// ドラッグ中コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateDragCommand(cmd.ExecutedObjectId, cmd.NewPosition));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// ドロップ
		/// </summary>
		private void OnIdentifiableDrop(IdentifiableDropCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Dragging:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドロップ）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Drop, cmd.ExecutedObjectId, cmd.TargetObjectId);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateDropCommand(cmd.ExecutedObjectId, cmd.TargetObjectId));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// ドロップ完了
		/// </summary>
		private void OnIdentifiableDroped(IdentifiableDropedCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Drop:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドロップ完了）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateDropedCommand(cmd.ExecutedObjectId, cmd.TargetObjectId));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// クリック
		/// </summary>
		private void OnIdentifiableClick(IdentifiableClickCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Idle:
            		Debug.Log("すてーとましん（クリック）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
					SetCurrentState(IdentifiableUIState.BiginClick, cmd.ExecutedObjectId, null);
					// ドラッグ開始コマンド
					_identifiableCommandBus.Emit(new IdentifiableStateClickCommand(cmd.ExecutedObjectId));
					break;
				case IdentifiableUIState.Hovered:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（クリック）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.BiginClick, cmd.ExecutedObjectId, null);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateClickCommand(cmd.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

        /// <summary>
		/// クリック完了
		/// </summary>
		private void OnIdentifiableClicked(IdentifiableClickedCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.BiginClick:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（クリック完了）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.Idle, null, null);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateClickedCommand(cmd.ExecutedObjectId));
					}
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// ドラッグ開始
		/// </summary>
		private void OnIdentifiableBeginDrag(IdentifiableBeginDragCommand cmd)
		{
			switch (CurrentState)
			{
				case IdentifiableUIState.Idle:
            		Debug.Log("すてーとましん（ドラッグ開始）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
					SetCurrentState(IdentifiableUIState.BiginDrag, cmd.ExecutedObjectId, null);
					// ドラッグ開始コマンド
					_identifiableCommandBus.Emit(new IdentifiableStateBeginDragCommand(cmd.ExecutedObjectId));
					break;
				case IdentifiableUIState.Hovered:
					if (StateObjectId == cmd.ExecutedObjectId)
					{
            			Debug.Log("すてーとましん（ドラッグ開始）:" + cmd.ExecutedObjectId + "/" + StateObjectId + " CurrentState:" + CurrentState);
						SetCurrentState(IdentifiableUIState.BiginDrag, cmd.ExecutedObjectId, null);
						// ドラッグ開始コマンド
						_identifiableCommandBus.Emit(new IdentifiableStateBeginDragCommand(cmd.ExecutedObjectId));
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

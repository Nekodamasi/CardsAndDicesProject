using UnityEngine;
using VContainer; // VContainer.Inject を使用するために追加

namespace CardsAndDices
{
	/// <summary>
	/// UIの全体的なインタラクション状態を管理するステートマシン。
	/// カードやダイスのドラッグ状態などを一元的に管理し、UIの競合を防ぎます。
	/// 設計書「gdd_combat_system_mockup.md」に基づき、ScriptableObjectとして機能します。
	/// </summary>
	[CreateAssetMenu(fileName = "UIStateMachine", menuName = "CardsAndDice/UI State Machine")]
	public class UIStateMachine : ScriptableObject
	{
		/// <summary>
		/// UIのインタラクション状態を表す列挙型。
		/// </summary>
		public enum UIState
		{
			/// <summary>
			/// アイドル状態。ユーザーによる主要な操作（ドラッグなど）が行われていない。
			/// </summary>
			Idle,

			/// <summary>
			/// クリーチャーカードをドラッグ中の状態。
			/// </summary>
			DraggingCard,

			/// <summary>
			/// ダイスをドラッグ中の状態。
			/// </summary>
			DraggingDice,

			/// <summary>
			/// カードがスロットにドロップされた
			/// </summary>
			DropedCard,

			/// <summary>
			/// ドロップカードの移動中
			/// </summary>
			DropedCardMove,
			/// <summary>
			/// リフロー中
			/// </summary>
			Reflow,
		}

		/// <summary>
		/// 現在のUI状態を取得します。
		/// </summary>
		public UIState CurrentState { get; private set; } = UIState.Idle;

        /// <summary>
		/// ScriptableObjectが初期化される時の処理。
		/// VContainerによって呼び出されます。
		/// </summary>
		[Inject]
		public void Initialize()
		{
			CurrentState = UIState.Idle;
            // 何もしない場合でも、空のInitializeメソッドを定義
		}

		/// <summary>
		/// UIの状態を新しい状態に遷移させます。
		/// このメソッドは、主に各種コマンド（CardBeginDragCommandなど）から呼び出されることを想定しています。
		/// </summary>
		/// <param name="newState">遷移先の新しい状態。</param>
		public void SetState(UIState newState)
		{
			if (CurrentState == newState)
			{
				return;
			}

			CurrentState = newState;

			// 状態遷移に伴うグローバルなイベント発行が必要な場合は、ここに追記します。
			// 例: R3イベントチャネルを使用して状態変更を通知する
		}
	}
}

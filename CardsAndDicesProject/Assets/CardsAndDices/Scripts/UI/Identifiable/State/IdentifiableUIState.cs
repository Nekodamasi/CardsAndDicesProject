namespace CardsAndDices
{
		/// <summary>
		/// UIのインタラクション状態を表す列挙型。
		/// </summary>
		public enum IdentifiableUIState
		{
			/// <summary>
			/// アイドル状態。ユーザーによる主要な操作（ドラッグなど）が行われていない。
			/// </summary>
			Idle,

			/// <summary>
			/// ホバー状態。
			/// </summary>
			Hover,

			/// <summary>
			/// ホバー完了状態。
			/// </summary>
			Hovered,

			/// <summary>
			/// ドラッグ開始の状態。
			/// </summary>
			BiginDrag,

			/// <summary>
			/// ドラッグ中の状態。
			/// </summary>
			Dragging,

			/// <summary>
			/// ドロップ状態
			/// </summary>
			Drop,

			/// <summary>
			/// ドロップ完了状態
			/// </summary>
			Droped,

			/// <summary>
			/// ドラッグ終了状態
			/// </summary>
			EndDrag,

			/// <summary>
			/// ドラッグ終了完了状態
			/// </summary>
			EndDraged,

			/// <summary>
			/// クリック開始
			/// </summary>
			BiginClick,

			/// <summary>
			/// クリック完了
			/// </summary>
			Clicked,

			/// <summary>
			/// ノンレスポンス
			/// </summary>
			NonResponse
		}
}

namespace CardsAndDices
{
	/// <summary>
	/// SpriteUI要素のドラッグ操作が終了したことを通知するコマンド。
	/// </summary>
	public class SpriteEndDragCommand : ICommand
	{
		/// <summary>
		/// ドラッグ終了したオブジェクトのCompositeObjectId。
		/// </summary>
		public CompositeObjectId TargetObjectId { get; private set; }

		/// <summary>
		/// SpriteEndDragCommandの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="targetObjectId">ドラッグ終了したオブジェクトのCompositeObjectId。</param>
		public SpriteEndDragCommand(CompositeObjectId targetObjectId)
		{
			TargetObjectId = targetObjectId;
		}

		/// <summary>
		/// コマンドを実行します。
		/// </summary>
		public void Execute()
		{
			// このコマンドは通知用のため、ここでは具体的な実行ロジックはありません。
			// 購読側で処理されます。
		}

		/// <summary>
		/// コマンドを元に戻します。
		/// </summary>
		public void Undo()
		{
			// このコマンドは通知用のため、ここでは具体的なUndoロジックはありません。
		}
	}
}
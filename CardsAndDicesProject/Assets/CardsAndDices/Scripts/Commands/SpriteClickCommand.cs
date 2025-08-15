namespace CardsAndDices
{
	/// <summary>
	/// SpriteUI要素がクリックされたことを通知するコマンド。
	/// </summary>
	public class SpriteClickCommand : ICommand
	{
		/// <summary>
		/// クリックされたオブジェクトのCompositeObjectId。
		/// </summary>
		public CompositeObjectId TargetObjectId { get; private set; }

		/// <summary>
		/// SpriteClickCommandの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="targetObjectId">クリックされたオブジェクトのCompositeObjectId。</param>
		public SpriteClickCommand(CompositeObjectId targetObjectId)
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
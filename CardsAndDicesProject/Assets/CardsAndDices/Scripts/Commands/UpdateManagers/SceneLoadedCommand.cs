namespace CardsAndDices
{
	/// <summary>
	/// シーンがloadされた時に呼ばれるコマンドです。
	/// </summary>
	public class SceneLoadedCommand : ICommand
	{
		public SceneLoadedCommand()
		{
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
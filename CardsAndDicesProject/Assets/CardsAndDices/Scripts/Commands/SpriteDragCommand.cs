using UnityEngine;

namespace CardsAndDices
{
	/// <summary>
	/// SpriteUI要素がドラッグ中に移動したことを通知するコマンド。
	/// </summary>
	public class SpriteDragCommand : ICommand
	{
		/// <summary>
		/// ドラッグ中のオブジェクトのCompositeObjectId。
		/// </summary>
		public CompositeObjectId TargetObjectId { get; private set; }

		/// <summary>
		/// ドラッグ中の新しいワールド座標。
		/// </summary>
		public Vector3 NewPosition { get; private set; }

		/// <summary>
		/// SpriteDragCommandの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="targetObjectId">ドラッグ中のオブジェクトのCompositeObjectId。</param>
		/// <param name="newPosition">ドラッグ中の新しいワールド座標。</param>
		public SpriteDragCommand(CompositeObjectId targetObjectId, Vector3 newPosition)
		{
			TargetObjectId = targetObjectId;
			NewPosition = newPosition;
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
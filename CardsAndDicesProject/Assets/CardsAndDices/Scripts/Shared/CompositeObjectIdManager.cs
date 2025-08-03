using UnityEngine;
using VContainer; // VContainer.Inject を使用するために追加

namespace CardsAndDice
{
	/// <summary>
	/// CompositeObjectIdを生成・管理するマネージャークラス。
	/// ScriptableObjectとして実装され、ユニークIDの採番を永続化します。
	/// </summary>
	[CreateAssetMenu(fileName = "CompositeObjectIdManager", menuName = "CardsAndDice/Composite Object ID Manager")]
	public class CompositeObjectIdManager : ScriptableObject
	{
		/// <summary>
		/// 次に割り当てるユニークID。
		/// </summary>
		[SerializeField] private long _nextUniqueId = 1;

        /// <summary>
        /// ScriptableObjectが初期化される時の処理。
        /// VContainerによって呼び出されます。
        /// </summary>
        [Inject]
        public void Initialize()
        {
            ResetManager();
        }

		/// <summary>
		/// 新しいCompositeObjectIdを生成して返します。
		/// </summary>
		/// <param name="objectType">オブジェクトのタイプ。</param>
		/// <param name="owner">このオブジェクトのオーナーとなるCompositeObjectId。ルートの場合はnull。</param>
		/// <returns>生成されたCompositeObjectId。</returns>
		public CompositeObjectId CreateId(string objectType, CompositeObjectId owner = null)
		{
			// ユニークIDを採番し、次のIDをインクリメントします。
			long currentId = _nextUniqueId;
			_nextUniqueId++;

			// ScriptableObjectの変更を保存するためにダーティフラグを立てます。
			// Unityエディタでのみ有効です。
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			#endif

			return new CompositeObjectId(currentId, objectType, owner);
		}

		/// <summary>
		/// マネージャーを初期状態にリセットします。
		/// 主にテストやデバッグ目的で使用します。
		/// </summary>
		public void ResetManager()
		{
			_nextUniqueId = 1;
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			#endif
		}
	}
}
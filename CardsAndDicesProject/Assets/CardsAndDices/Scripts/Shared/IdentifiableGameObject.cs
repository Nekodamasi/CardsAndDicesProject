using UnityEngine;
// using VContainer; // VContainer.Inject を使用しないため削除

namespace CardsAndDice
{
	/// <summary>
	/// CompositeObjectIdを持つMonoBehaviourクラス。
	/// インスペクターからObjectTypeを設定でき、自身のCompositeObjectIdを管理します。
	/// </summary>
	public class IdentifiableGameObject : MonoBehaviour
	{
        [Header("Components")]
		[SerializeField] private CompositeObjectIdManager _idManager;

		/// <summary>
		/// このオブジェクトのタイプを表す文字列。
		/// インスペクターから設定します。
		/// </summary>
		[SerializeField] private string _objectType;
	
		/// <summary>
		/// このMonoBehaviourに割り当てられたCompositeObjectId。
		/// </summary>
		public CompositeObjectId ObjectId { get; private set; }

		/// <summary>
		/// コンポーネントの初期化を行います。
		/// </summary>
		private void Awake()
		{
			// 自身のCompositeObjectIdを生成
			ObjectId = _idManager.CreateId(_objectType);
//			Debug.Log($"Initialized {gameObject.name} with ObjectId: {ObjectId}");
		}

		/// <summary>
		/// このオブジェクトのオーナー（親）を設定します。
		/// </summary>
		/// <param name="ownerId">親となるCompositeObjectId。</param>
		public void SetOwner(CompositeObjectId ownerId)
		{
			if (ObjectId != null)
			{
				ObjectId.Owner = ownerId;
				Debug.Log($"Set owner for {gameObject.name} ({ObjectId.UniqueId}) to {ownerId.UniqueId}");
			}
			else
			{
				Debug.LogWarning($"Attempted to set owner on {gameObject.name} before ObjectId was initialized.");
			}
		}
	}
}
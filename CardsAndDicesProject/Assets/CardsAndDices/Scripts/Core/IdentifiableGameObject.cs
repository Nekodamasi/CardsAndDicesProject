using UnityEngine;
using VContainer;

namespace CardsAndDices
{
	/// <summary>
	/// CompositeObjectIdを持つMonoBehaviourクラス。
	/// インスペクターからObjectTypeを設定でき、自身のCompositeObjectIdを管理します。
	/// </summary>
	public class IdentifiableGameObject : MonoBehaviour, IGameInitializable
	{
        [Header("Components")]
		[SerializeField] private CompositeObjectIdManager _idManager;
		[SerializeField] private CompositeObjectRegistry _compositeObjectRegistry;

		/// <summary>
		/// このオブジェクトのタイプを表す文字列。
		/// インスペクターから設定します。
		/// </summary>
		[SerializeField] private CompositeObjectIdTypeEntity _objectType;
	
		/// <summary>
		/// このオブジェクトのタイプを表す文字列。
		/// インスペクターから設定します。
		/// </summary>
		[SerializeField] private string _displayCompositeObjectId;

		/// <summary>
		/// このMonoBehaviourに割り当てられたCompositeObjectId。
		/// </summary>
		public CompositeObjectId ObjectId { get; private set; }

		[Inject]
		public void Construct(CompositeObjectIdManager idManager, CompositeObjectRegistry compositeObjectRegistry)
		{
			_idManager = idManager;
			_compositeObjectRegistry = compositeObjectRegistry;
        }

		/// <summary>
		/// コンポーネントの初期化を行います。
		/// </summary>
		public void OnAwake()
		{
			ObjectId = _idManager.CreateId(_objectType);
			_displayCompositeObjectId = ObjectId.ToString();
			_compositeObjectRegistry.Register(ObjectId);
		}

		public void OnStart()
		{
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